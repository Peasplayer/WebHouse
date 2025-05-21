using System.Media;
using System.Reflection;
using WebHouse_Client.Networking;
using Timer = System.Timers.Timer;
using WebHouse_Client.Networking;
using System.Net.WebSockets;


namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    
    public static int PlayerPosition = 9;
    public static int OpponentPosition = 0;
    public static int TurnState { get; private set; }
    public static int PlayTime = 30;
    public static List<ILogicCard> Inventory = new ();
    public static List<ChapterCard> CurrentChapterCards = new ();
    public static List<ChapterCard> PlacedChapterCards = new ();
    public static List<EscapeCard> CurrentEscapeCards = new ();
    
    public static int MaxCards = NetworkManager.Instance.Players.Count switch 
    {
        2 => 8,
        3 => 6,
        _ => 5
    };
    public static Room CurrentRoom => Rooms[_currentRoom];
    
    public static List<Room> Rooms = new List<Room> // Raum-Liste wird erstellt
    {
        new Room(Room.RoomName.HotelZimmer),
        new Room(Room.RoomName.Hafen),
        new Room(Room.RoomName.Stadt),
        new Room(Room.RoomName.Wald),
        new Room(Room.RoomName.SafeHouse),
    };

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
            StartOpponentTimer();
        });
    }

    public static void Start()
    {
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList();
        
        CreateEscapeCardList();

        Task.Run(() =>
        {
            Task.Delay(1000).Wait();
            for (int i = 0; i < MaxCards - 1; i++)
            {
                NetworkManager.Rpc.RequestEscapeCard();
            }
            NetworkManager.Rpc.RequestChapterCard();
            
            if (NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.SwitchTurn(NetworkManager.Instance.Id);
                ShuffleOpponentCardsIn();
            }
        });
        
        StartOpponentTimer();
    }

    public static void Stop(bool win)
    {
        if (GameForm.Instance != null && !GameForm.Instance.IsDisposed)
        {
            NetworkManager.Instance.Client.Stop(WebSocketCloseStatus.NormalClosure, "Client closed");
            var form = new EndScreen(win);
            form.Show();
            GameForm.Instance.Hide();
        }
    }

    public static void MovePlayer(int steps)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            PlayerPosition += steps;
            
            if (CurrentRoom.RoomType == Room.RoomName.SafeHouse && PlayerPosition >= 28)
            {
                Stop(true);
                return;
            }
            
            if (PlayerPosition >= CurrentRoom.Steps)
            {
                PlayerPosition = 0;
                SwitchRoom();
            }
        
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
            if (OpponentPosition >= PlayerPosition || (OpponentPosition >= 16 && CurrentRoom.RoomType == Room.RoomName.SafeHouse))
            {
                Stop(false);
                return;
            }
        
            GameForm.Instance.UpdatePositions();
        });
    }

    public static void SwitchRoom()
    {
        _currentRoom++;
        
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList();
        var cardsToRemove = Inventory
            .OfType<ChapterCard>()
            .Where(c => c.Chapter != CurrentRoom.RoomType)
            .ToList();

        foreach (var card in cardsToRemove)
        {
            card.Component.Panel.Dispose();
            Inventory.Remove(card);
        }

        for (int i = 0; i < cardsToRemove.Count; i++)
        {
            NetworkManager.Rpc.RequestChapterCard();
        }

        //Spieler startet immer an StartField des neuen Raumes
        PlayerPosition = CurrentRoom.StartField;
        // Neue Gegner Position berechnen
        OpponentPosition = Math.Max(0, OpponentPosition - 12);

        GameForm.Instance.RenderBoard();
        GameForm.Instance.UpdatePositions();
    }

    private static void CreateEscapeCardList()
    {
        var list = new List<EscapeCard>();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 15; i++)
            {
                var escapeCard = new EscapeCard(EscapeCard.EscapeCardType.Normal, i +1, ((i + j) % 5) switch
                {
                    0 => Room.RoomName.HotelZimmer,
                    1 => Room.RoomName.Hafen,
                    2 => Room.RoomName.Stadt,
                    3 => Room.RoomName.Wald,
                    4 => Room.RoomName.SafeHouse
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
        
        CurrentEscapeCards = list.OrderBy(x => Random.Shared.Next()).ToList();
    }

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
            var pile = GameForm.Instance.discardPiles[pileIndex];
            pile.Panel.Enabled = false;
            pile.Panel.Visible = false;
        
            card.CreateComponent();
            GameForm.Instance.Controls.Add(card.Component.Panel);
            card.Component.Panel.Location = pile.Panel.Location;
            card.Component.Panel.Size = pile.Panel.Size;
            card.Component.Panel.BringToFront();
            ((Components.ChapterCard)card.Component).Pile = pile;
            Inventory.Remove(card);
            PlacedChapterCards.Add(card);

            GameForm.Instance.RenderBoard();
        });
    }

    public static void PlaceEscapeCard(EscapeCard card, ChapterCard chapterCard)
    {
        GameForm.Instance.BeginInvoke(() =>
        {
            chapterCard.AddEscapeCard(card);

            card.Component?.Panel.Dispose();
            chapterCard.Component.Panel.Invalidate();
            GameForm.Instance.RenderBoard();
        });
    }
    
    public static void DrawEscapeCard(EscapeCard escapeCard)
    {
        if (TurnState == 2 && Inventory.Count >= MaxCards - 1)
        {
            SwitchTurnState();
        }
        
        GameForm.Instance.BeginInvoke(() =>
        {
            if (escapeCard.Type == EscapeCard.EscapeCardType.Normal)
            {;
                Inventory.Add(escapeCard);
                
                escapeCard.CreateComponent();
                GameForm.Instance.Controls.Add(escapeCard.Component?.Panel);
                escapeCard.Component?.Panel.BringToFront();
                GameForm.Instance.RenderBoard();
            }
            else {
                if (NetworkManager.Instance.LocalPlayer.IsHost)
                    CurrentEscapeCards.Add(escapeCard);
                GameForm.Instance.blockDrawingEscapeCard = true;
            
                escapeCard.CreateComponent();
                escapeCard.Component.Panel.Location = GameForm.Instance.drawEscapeCardButton.Location;
                escapeCard.Component.Panel.Size = GameForm.Instance.drawEscapeCardButton.Size;
                GameForm.Instance.Controls.Add(escapeCard.Component?.Panel);
                NetworkManager.Rpc.MoveOpponent((escapeCard.Type == EscapeCard.EscapeCardType.OpponentSteps ? 0 : PlacedChapterCards.Count) + escapeCard.Number);
                GameForm.Instance.RenderBoard();
                GameForm.Instance.BeginInvoke(() =>
                {
                    escapeCard.Component.Panel.BringToFront();
                });

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

    public static void DrawChapterCard(ChapterCard chapterCard)
    {
        if (TurnState == 2 && Inventory.Count >= MaxCards - 1)
        {
            SwitchTurnState();
        }
        
        Inventory.Add(chapterCard);

        GameForm.Instance.BeginInvoke(() =>
        {
            chapterCard.CreateComponent();
            GameForm.Instance.Controls.Add(chapterCard.Component.Panel);
            chapterCard.Component.Panel.BringToFront();
            GameForm.Instance.RenderBoard();
        });
    }

    public static void SwitchTurnState()
    {
        if (!NetworkManager.Instance.LocalPlayer.IsTurn)
            return;
        
        TurnState++;
        switch (TurnState)
        {
            case 1:
                GameForm.Instance.drawChapterCardButton.Visible = true;
                GameForm.Instance.drawEscapeCardButton.Visible = true;
                break;
            case 3:
                GameForm.Instance.drawChapterCardButton.Visible = false;
                GameForm.Instance.drawEscapeCardButton.Visible = false;
                TurnState = 0;
                NetworkManager.Rpc.SwitchTurn();
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