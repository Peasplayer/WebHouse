using System.Media;
using System.Reflection;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    public static List<ChapterCard> ChapterDeck { get; private set; } = new List<ChapterCard>();

    public static int PlayerPosition = 9;
    public static int OpponentPosition = 0;
    public static List<ICard> Inventory = new List<ICard>();
    public static List<ChapterCard> PlacedChapterCards = new List<ChapterCard>();
    public static Room CurrentRoom => Rooms[_currentRoom];

    public static List<Room> Rooms = new List<Room> //Raum-Liste wird erstellt
    {
        new Room(Room.RoomName.HotelZimmer),
        new Room(Room.RoomName.Hafen),
        new Room(Room.RoomName.Stadt),
        new Room(Room.RoomName.Wald),
        new Room(Room.RoomName.SafeHouse),
    };

    public static void InitializeDeck() //Liste mit alle 40 Chapterkraten
    {
        ChapterDeck = new List<ChapterCard>
        {
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 1, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 2, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.HotelZimmer.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Hafen.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Stadt.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.Wald.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            new ChapterCard(Room.RoomName.SafeHouse.ToString(), 3, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green }),
            
            
        };
    }

    private static void StartOpponent()
    {
        Task.Run(() =>
        {
            var sound = new SoundPlayer(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Musik.wav"));
            sound.Load();
            sound.PlaySync();
            _gameForm.BeginInvoke(() => MoveOpponent(1));
            StartOpponent();
        });
    }

    public static void Start(GameForm form)
    {
        _gameForm = form;

        StartOpponent();
    }

    public static void Stop()
    {
        // TODO: Game over
    }

    public static void MovePlayer(int steps)
    {
        PlayerPosition += steps;
        // TODO: Check if field is opponent field
        if (PlayerPosition >= CurrentRoom.Steps)
        {
            PlayerPosition = 0;
            SwitchRoom();
        }

        if (CurrentRoom.OpponentMoveTriggerFields.Contains(PlayerPosition))
        {
            MoveOpponent(1);
        }

        _gameForm.UpdatePositions();
    }

    public static void MoveOpponent(int steps)
    {
        OpponentPosition += steps;
        if (OpponentPosition >= PlayerPosition)
        {
            Stop();
            return;
        }

        _gameForm.UpdatePositions();
    }

    public static void SwitchRoom()
    {
        _currentRoom++;
        PlayerPosition = CurrentRoom.StartField;
        OpponentPosition = Math.Max(0, OpponentPosition - 12); //  Gegner Position berechnen

        InitializeDeck();
        ChapterDeck = ChapterDeck
            .Where(card => card.Chapter == CurrentRoom.RoomType.ToString())
            .ToList();

        var cardsToRemove = Inventory
            .OfType<ChapterCard>()
            .Where(c => c.Chapter != CurrentRoom.RoomType.ToString())
            .ToList();

        foreach (var card in cardsToRemove)
        {
            card.Component.Panel.Dispose();
            Inventory.Remove(card);
        }
        

        _gameForm.RenderBoard();
        _gameForm.UpdatePositions();
    }


    public static void PlaceChapterCard(ChapterCard card)
    {
        Inventory.Remove(card);
        PlacedChapterCards.Add(card);

        _gameForm.RenderBoard();
    }

    public static void PlaceEscapeCard(EscapeCard card, ChapterCard chapterCard)
    {
        Inventory.Remove(card);
        chapterCard.AddEscapeCard(card);

        card.Component.Panel.Dispose();
        chapterCard.Component.Panel.Invalidate();
        _gameForm.RenderBoard();
    }
}