using System.Media;
using System.Reflection;
using WebHouse_Client.Networking;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    
    public static int PlayerPosition = 9;
    public static int OpponentPosition = 0;
    public static List<ILogicCard> Inventory = new ();
    public static List<ChapterCard> CurrentChapterCards = new ();
    public static List<ChapterCard> PlacedChapterCards = new ();
    public static List<EscapeCard> CurrentEscapeCards = new ();
    
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
            StartOpponentTimer();
        });
    }

    public static void Start(GameForm form)
    {
        _gameForm = form;
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList();
        
        CreateEscapeCardList();
        
        StartOpponentTimer();
    }

    public static void Stop()
    {
        // TODO: Game over
    }

    public static void MovePlayer(int steps)
    {
        _gameForm.BeginInvoke(() =>
        {
            PlayerPosition += steps;
            // TODO: Check if field is opponent field
            if (PlayerPosition >= CurrentRoom.Steps)
            {
                PlayerPosition = 0;
                SwitchRoom();
            }
        
            if (CurrentRoom.OpponentMoveTriggerFields.Contains(PlayerPosition) && NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.MoveOpponent(1);
            }
        
            _gameForm.UpdatePositions();
        });
    }

    public static void MoveOpponent(int steps)
    {
        _gameForm.BeginInvoke(() =>
        {
            OpponentPosition += steps;
            if (OpponentPosition >= PlayerPosition)
            {
                Stop();
                return;
            }
        
            _gameForm.UpdatePositions();
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

        _gameForm.RenderBoard();
        _gameForm.UpdatePositions();
    }

    private static void CreateEscapeCardList()
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
        
        list = list.OrderBy(x => Random.Shared.Next()).ToList();
        
        for (int i = 0; i < 10; i++)
        {
            var pos = Random.Shared.Next(15);
            var card = opponentCards[0];
            opponentCards.Remove(card);
            list.Insert((i / 2) * 15 + i + pos, card);
        }

        CurrentEscapeCards = list;
    }

    public static void PlaceChapterCard(ChapterCard card, int pileIndex)
    {
        _gameForm.BeginInvoke(() =>
        {
            var pile = _gameForm.discardPiles[pileIndex];
            pile.Panel.Enabled = false;
            pile.Panel.Visible = false;
        
            card.CreateComponent();
            _gameForm.Controls.Add(card.Component.Panel);
            card.Component.Panel.Location = pile.Panel.Location;
            card.Component.Panel.Size = pile.Panel.Size;
            card.Component.Panel.BringToFront();
            ((Components.ChapterCard)card.Component).Pile = pile;
            Inventory.Remove(card);
            PlacedChapterCards.Add(card);

            _gameForm.RenderBoard();
        });
    }

    public static void PlaceEscapeCard(EscapeCard card, ChapterCard chapterCard)
    {
        _gameForm.BeginInvoke(() =>
        {
            chapterCard.AddEscapeCard(card);

            card.Component?.Panel.Dispose();
            chapterCard.Component.Panel.Invalidate();
            _gameForm.RenderBoard();
        });
    }

    private static bool _blockDrawingEscapeCard = false;
    
    public static void DrawEscapeCard(EscapeCard escapeCard)
    {
        _gameForm.BeginInvoke(() =>
        {
            if (escapeCard.Type == EscapeCard.EscapeCardType.Normal)
            {
                Inventory.Add(escapeCard);
                CurrentEscapeCards.Remove(escapeCard);
                
                escapeCard.CreateComponent();
                _gameForm.Controls.Add(escapeCard.Component?.Panel);
                escapeCard.Component?.Panel.BringToFront();
                _gameForm.RenderBoard();
            }
            else {
                CurrentEscapeCards.Remove(escapeCard);
                CurrentEscapeCards.Add(escapeCard);
                _blockDrawingEscapeCard = true;
            
                escapeCard.CreateComponent();
                escapeCard.Component.Panel.Location = _gameForm.drawEscapeCardButton.Location;
                escapeCard.Component.Panel.Size = _gameForm.drawEscapeCardButton.Size;
                _gameForm.Controls.Add(escapeCard.Component?.Panel);
                NetworkManager.Rpc.MoveOpponent((escapeCard.Type == EscapeCard.EscapeCardType.OpponentSteps ? 0 : PlacedChapterCards.Count) + escapeCard.Number);
                _gameForm.RenderBoard();
                _gameForm.BeginInvoke(() =>
                {
                    escapeCard.Component.Panel.BringToFront();
                });

                Task.Run(() =>
                {
                    Task.Delay(2000).Wait();
                    _gameForm.BeginInvoke(() =>
                    {
                        escapeCard.Component?.Panel.Dispose();
                        _gameForm.RenderBoard();
                        _gameForm.Invalidate(new Rectangle(escapeCard.Component.Panel.Location, escapeCard.Component.Panel.Size), true);
                    
                        Task.Delay(1000).Wait();
                        _blockDrawingEscapeCard = false;
                        //DrawEscapeCard();
                        NetworkManager.Rpc.RequestEscapeCard();
                        _gameForm.RenderBoard();
                    });
                });
            }
        });
    }

    public static void DrawChapterCard(ChapterCard chapterCard)
    {
        Inventory.Add(chapterCard);

        _gameForm.BeginInvoke(() =>
        {
            chapterCard.CreateComponent();
            _gameForm.Controls.Add(chapterCard.Component.Panel);
            chapterCard.Component.Panel.BringToFront();
            _gameForm.RenderBoard();
        });
    }
}