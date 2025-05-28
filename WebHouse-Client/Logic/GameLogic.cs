using System.Media;
using System.Reflection;
using WebHouse_Client.Networking;
using Timer = System.Timers.Timer;
using WebHouse_Client.Networking;
using System.Net.WebSockets;


namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0; //Aktueller Raum Index
    private static Timer _opponentTimer; //Timer für den Gegner
    
    public static int PlayerPosition = 9; //Aktuelle Position des Spielers
    public static int OpponentPosition = 0; //Aktuelle Position des Gegners
    public static int TurnState { get; private set; } //Aktueller Zustand des Zuges
    public static int PlayTime = 30; //Gesamte Spielzeit
    public static bool ChapterCardsEmpty = false; //Wird auf true gesetzt wenn keine ChapterCards für den Raum mehr vorhanden sind
    public static List<ILogicCard> Inventory = new (); //Das Inventar des Spielers
    public static List<ChapterCard> CurrentChapterCards = new (); //Kapitelkarten des aktuellen Raumes
    public static List<ChapterCard> PlacedChapterCards = new (); //Kapitelkarten die auf dem Ablagestapel liegen
    public static List<EscapeCard> CurrentEscapeCards = new (); //EscapeCards die gezogen werden können
    
    //legt die maximale Anzahl an Karten fest die eine Spieler auf der Hand haben kann. Dies ist abhängig von der Anzahl an Spieler
    public static int MaxCards = NetworkManager.Instance.Players.Count switch 
    {
        2 => 8, //Bei 2 Spielern kann jeder Spieler 8 Karten haben
        3 => 6, //Bei 3 Spielern kann jeder Spieler 6 Karten haben
        _ => 5 //Bei 4 oder mehr Spielern kann jeder Spieler 5 Karten haben
    };
    public static Room CurrentRoom => Rooms[_currentRoom]; //Aktueller Raum
    
    //Raum-Liste wird erstellt
    public static List<Room> Rooms = new List<Room> 
    {
        new Room(Room.RoomName.Hotelzimmer),
        new Room(Room.RoomName.Hafen),
        new Room(Room.RoomName.Stadt),
        new Room(Room.RoomName.Wald),
        new Room(Room.RoomName.Safehouse),
    };

    //Timer für den Gegner. Gegner wird bewegt wenn die Musik endet
    private static void StartOpponentTimer()
    {
        Task.Run(() =>
        {
            var sound = new SoundPlayer(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Musik.wav"));
            sound.Load();
            sound.PlaySync();
            if (NetworkManager.Instance.LocalPlayer.IsHost)
                NetworkManager.Rpc.MoveOpponent(1);
            GameForm.Instance.BeginInvoke(LowerTimer);
            StartOpponentTimer(); //Die Musik wird neu gestartet
        });
    }

    //Startet das Spiel
    public static void Start()
    {
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList(); //Mischt die Kapitelkarten des aktuellen Raumes
        
        CreateEscapeCardList(); //Erstellt die Liste der EscapeCards

        //Der Host verteilt die Karten
        Task.Run(() =>
        {
            Task.Delay(1000).Wait();
            
            if (NetworkManager.Instance.LocalPlayer.IsHost)
            {
                foreach (var player in NetworkManager.Instance.Players)
                {
                    for (int i = 0; i < MaxCards - 1; i++)
                    {
                        NetworkManager.Rpc.DrawEscapeCard(player.Id);
                    }
                    NetworkManager.Rpc.DrawChapterCard(player.Id);
                }
                
                NetworkManager.Rpc.SwitchTurn(NetworkManager.Instance.Id);
                ShuffleOpponentCardsIn(); //Die Verfolgerkarten werden zu den EscapeCards gemischt
            }
        });
        
        StartOpponentTimer(); //Startet den Timer für den Gegner
    }

    //Beendet das Spiel und zeigt den Endscreen an
    public static void Stop(bool win)
    {
        if (GameForm.Instance != null && !GameForm.Instance.IsDisposed)
        {
            //Der Host stoppt das Spiel und schließt die Verbindung zum Server
            if (NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.StopGame();
            }
            //Jenachdem ob das Spiel gewonnen oder verloren wurde, wird der Endscreen angezeigt
            var form = new EndScreen(win);
            form.Show();
            GameForm.Instance.Hide();
        }
    }

    //Bewegt den Spieler
    public static void MovePlayer(int steps)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            PlayerPosition += steps;
            
            //Überprüft ob der Spieler auf dem letzten Feld des letzten Raumes ist. Wenn ja wurde das Spiel gewonnen
            if (CurrentRoom.RoomType == Room.RoomName.Safehouse && PlayerPosition >= 28)
            {
                Stop(true);
                return;
            }
            //Wenn das Ende eines Raumes erreicht wu8rde wird der Raum gewechselt und die Speiler Position auf das Startfeld des neuen Raumes gesetzt
            if (PlayerPosition >= CurrentRoom.Steps)
            {
                PlayerPosition = 0;
                SwitchRoom();
            }
        
            //Verfolger bewegt sich wenn wenn der Spieler auf eine Verfolger Feld kommt
            if (CurrentRoom.OpponentMoveTriggerFields.Contains(PlayerPosition) && NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.MoveOpponent(1);
            }
        
            GameForm.Instance.UpdatePositions();
        });
    }

    public static void MoveOpponent(int steps)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            OpponentPosition += steps;
            if (OpponentPosition >= PlayerPosition || (OpponentPosition >= 16 && CurrentRoom.RoomType == Room.RoomName.Safehouse))
            {
                Stop(false);
                return;
            }
        
            GameForm.Instance.UpdatePositions();
        });
    }

    //Wechselt den Raum und aktualisiert die Kapitelkarten
    public static void SwitchRoom()
    {
        _currentRoom++; //Wechselt zum nächsten Raum
        
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList(); //Mischt die Kapitelkarten des neuen Raumes
        //Speichert die Kapitelkarten die gelöscht werden sollen in einer Liste
        var cardsToRemove = Inventory
            .OfType<ChapterCard>()
            .Where(c => c.Chapter != CurrentRoom.RoomType)
            .ToList();

        //Löscht die Liste der Kapitelkarten die nicht mehr gebraucht werden
        foreach (var card in cardsToRemove)
        {
            card.Component.Panel.Dispose();
            Inventory.Remove(card);
        }   
        //Ersetzt die gelöschten Karten duch neue
        for (int i = 0; i < cardsToRemove.Count; i++)
        {
            NetworkManager.Rpc.RequestChapterCard();
        }

        //Spieler startet immer an StartField des neuen Raumes
        PlayerPosition = CurrentRoom.StartField;
        //Neue Gegner Position berechnen
        OpponentPosition = Math.Max(0, OpponentPosition - 12);
        ChapterCardsEmpty = false;

        GameForm.Instance.specialChapterCard.Component.Panel.Dispose(); //Löscht die SpecialCard des alten Raumes
        GameForm.Instance.specialChapterCard = CurrentRoom.SpecialCard; //Setzt die SpecialCard des neuen Raumes
        GameForm.Instance.RenderBoard(); //Aktualisiert das Spielfeld
        GameForm.Instance.UpdatePositions(); //Aktualisiert die Positionen der Spieler
    }

    //Erstellt die EscapeCards
    private static void CreateEscapeCardList()
    {
        var list = new List<EscapeCard>();
        for (int j = 0; j < 5; j++) //Erstellt 5 verschiedene Farben von EscapeCards
        {
            // Für jede Farbe werden 15 EscapeCards erstellt
            for (int i = 0; i < 15; i++)
            {
                var escapeCard = new EscapeCard(EscapeCard.EscapeCardType.Normal, i +1, ((i + j) % 5) switch
                {
                    0 => Room.RoomName.Hotelzimmer,
                    1 => Room.RoomName.Hafen,
                    2 => Room.RoomName.Stadt,
                    3 => Room.RoomName.Wald,
                    4 => Room.RoomName.Safehouse
                }, j switch
                {
                    0 => CardColor.Red,
                    1 => CardColor.Green,
                    2 => CardColor.Blue,
                    3 => CardColor.Pink,
                    4 => CardColor.Yellow,
                    _ => CardColor.Red
                });
                list.Add(escapeCard);
            }
        }
        //Mischt die EscapeCards
        CurrentEscapeCards = list.OrderBy(x => Random.Shared.Next()).ToList();
    }
    
    //Die Verfolgerkarten werden gemischt und in die EscapeCards Liste eingefügt
    public static void ShuffleOpponentCardsIn()
    {
        var opponentCards = new List<EscapeCard>
        {
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 1),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 1),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 1),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 1),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 2),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 2),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 2),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 3),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 3),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 0),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 0),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 0),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 0),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 1),
            new EscapeCard(EscapeCard.EscapeCardType.OpponentCards, 1)
        }.OrderBy(x => Random.Shared.Next()).ToList();
        
        //Fügt die Verfolgerkarten zufällig in die EscapeCards Liste ein
        for (int i = 0; i < 10; i++)
        {
            var pos = Random.Shared.Next(13);
            var card = opponentCards[0];
            opponentCards.Remove(card);
            CurrentEscapeCards.Insert(Math.Min((i / 2) * 15 + i + pos, CurrentEscapeCards.Count - 1), card);
        }
    }

    public static void PlaceChapterCard(ChapterCard card, int pileIndex)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            //Hole den entsprechenden Ablagestapel
            var pile = GameForm.Instance.discardPiles[pileIndex];
            pile.Panel.Enabled = false;
            pile.Panel.Visible = false;
    
            //Erstelle das UI-Element für die Karte und platziere es auf dem Stapel
            card.CreateComponent();
            GameForm.Instance.Controls.Add(card.Component.Panel);
            card.Component.Panel.Location = pile.Panel.Location;
            card.Component.Panel.Size = pile.Panel.Size;
            card.Component.Panel.BringToFront();

            //Verknüpfe die Karte mit dem Stapel
            ((Components.ChapterCard)card.Component).Pile = pile;

            //Entferne die Karte aus dem Inventar und füge sie dem Ablagestapel hinzu
            Inventory.Remove(card);
            PlacedChapterCards.Add(card);

            //Aktualisiere das Spielfeld
            GameForm.Instance.RenderBoard();
        });
    }

    public static void PlaceEscapeCard(EscapeCard card, ChapterCard chapterCard)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            //Fügt die EscapeCard der ChapterCard hinzu
            chapterCard.AddEscapeCard(card);

            //Entfernt das Panel der EscapeCard aus dem UI
            card.Component?.Panel.Dispose();
            //Aktualisiert die Anzeige der ChapterCard
            chapterCard.Component.Panel.Invalidate();
            //Rendert das Spielfeld neu
            GameForm.Instance.RenderBoard();
        });
    }
    
    public static void DrawEscapeCard(EscapeCard escapeCard)
    {
        //Wenn der Spieler am Ende seines Zuges ist und das Inventar voll ist wird der Zug beendet
        if (TurnState == 2 && Inventory.Count >= MaxCards - 1)
        {
            SwitchTurnState();
        }
    
        
        GameForm.Instance.BeginInvoke(() =>
        {
            //Normale EscapeCard wird dem Inventar hinzugefügt und angezeigt
            if (escapeCard.Type == EscapeCard.EscapeCardType.Normal)
            {
                Inventory.Add(escapeCard);
            
                escapeCard.CreateComponent();
                GameForm.Instance.Controls.Add(escapeCard.Component?.Panel);
                escapeCard.Component?.Panel.BringToFront();
                GameForm.Instance.RenderBoard();
            }
            else
            {
                //Spezielle EscapeCard werden angezeigt
                if (NetworkManager.Instance.LocalPlayer.IsHost)
                    CurrentEscapeCards.Add(escapeCard);

                GameForm.Instance.blockDrawingEscapeCard = true;
        
                escapeCard.CreateComponent();
                escapeCard.Component.Panel.Location = GameForm.Instance.drawEscapeCardButton.Location;
                escapeCard.Component.Panel.Size = GameForm.Instance.drawEscapeCardButton.Size;
                GameForm.Instance.Controls.Add(escapeCard.Component?.Panel);

                //Gegner bewegt sich um die Anzahl an Schritten 
                NetworkManager.Rpc.MoveOpponent((escapeCard.Type == EscapeCard.EscapeCardType.OpponentSteps ? 0 : PlacedChapterCards.Count) + escapeCard.Number);
                GameForm.Instance.RenderBoard();

                //Die Verfolgerkarte wird in den Vordergrund gebracht
                GameForm.Instance.BeginInvoke(() =>
                {
                    escapeCard.Component.Panel.BringToFront();
                });

                //Nach kurzer Wartezeit wird die Verfolgerkarte entfernt und eine weiter EscapeCard gezogen
                Task.Run(() =>
                {
                    Task.Delay(2000).Wait();
                    GameForm.Instance.BeginInvoke(() =>
                    {
                        escapeCard.Component?.Panel.Dispose();
                        GameForm.Instance.RenderBoard();
                        GameForm.Instance.Invalidate(new Rectangle(escapeCard.Component.Panel.Location, escapeCard.Component.Panel.Size), true);
                    
                        Task.Delay(1000).Wait();
                        GameForm.Instance.blockDrawingEscapeCard = false;
                        NetworkManager.Rpc.RequestEscapeCard();
                        GameForm.Instance.RenderBoard();
                    });
                });
            }
        });
    }

    //Ziehr eine ChapterCard
    public static void DrawChapterCard(ChapterCard chapterCard)
    {
        if (TurnState == 2 && Inventory.Count >= MaxCards - 1)
        {
            SwitchTurnState();
        }
        
        Inventory.Add(chapterCard); //Fügt die ChapterCard dem Inventar hinzu

        GameForm.Instance.BeginInvoke(() =>
        {
            chapterCard.CreateComponent();
            GameForm.Instance.Controls.Add(chapterCard.Component.Panel);
            chapterCard.Component.Panel.BringToFront();
            GameForm.Instance.RenderBoard();
        });
    }

    //Wechselt zwischen den Zuständen den ein Zug haben kann
    public static void SwitchTurnState()
    {
        //Wird nur ausgeführt wenn der Spieler am Zug ist
        if (!NetworkManager.Instance.LocalPlayer.IsTurn)
            return;
        
        TurnState++;
        switch (TurnState)
        {
            case 1:
                GameForm.Instance.drawChapterCardButton.Visible = !ChapterCardsEmpty; //Zeigt den Button zum Ziehen einer ChapterCard an wenn noch ChapterCards vorhanden sind
                GameForm.Instance.drawEscapeCardButton.Visible = true; //Zeigt den Button zum Ziehen einer EscapeCard an
                break;
            case 3:
                GameForm.Instance.drawChapterCardButton.Visible = false; //Versteckt den Button zum Ziehen einer ChapterCard
                GameForm.Instance.drawEscapeCardButton.Visible = false; //Versteckt den Button zum Ziehen einer EscapeCard
                TurnState = 0;
                NetworkManager.Rpc.SwitchTurn(); //Wechselt den Zug zum nächsten Spieler
                break;
        }
    }

    //Verringert den Timer um 2 Minuten
    public static void LowerTimer()
    {
        if (PlayTime > 0)
        {
            PlayTime -= 2;
            GameForm.Instance.UpdateTimerLabel(PlayTime);
        }
        else
        {
            Stop(false);
        }
    }
}