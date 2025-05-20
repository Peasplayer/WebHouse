using System.Diagnostics;
using System.Media;
using System.Reflection;
using WebHouse_Client.Components;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    
    public static int PlayerPosition = 9;
    public static int OpponentPosition = 0;
    public static int playTime { get; set; }= 30;
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

    private static void StartOpponent()
    {
        Task.Run(() =>
        {
            var sound = new SoundPlayer(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Musik.wav"));
            sound.Load();
            sound.PlaySync();
            _gameForm.BeginInvoke(LowerTimer);
            _gameForm.BeginInvoke(() => MoveOpponent(1));
            StartOpponent();
        });
    }

    public static void Start(GameForm form)
    {
        _gameForm = form;
        CurrentChapterCards = CurrentRoom.ChapterCards.OrderBy(_ => Random.Shared.Next()).ToList();
        
        CreateEscapeCardList();
        
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
            var card = CurrentChapterCards.First();
            CurrentChapterCards.Remove(card);
            Inventory.Add(card);
            card.CreateComponent();
            _gameForm.Controls.Add(card.Component.Panel);
        }

        //Spieler startet immer an StartField des neuen Raumes
        PlayerPosition = CurrentRoom.StartField;
        // Neue Gegner Position berechnen
        OpponentPosition = Math.Max(0, OpponentPosition - 12);

        _gameForm.RenderBoard();
        _gameForm.UpdatePositions();
    }

    public static void CreateEscapeCardList()
    {
        var list = new List<EscapeCard>();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 15; i++)
            {
                var escapeCard = new EscapeCard(i +1, ((i + j) % 5) switch
                {
                    0 => "Hotel",
                    1 => "Hafen",
                    2 => "Stadt",
                    3 => "Wald",
                    4 => "SafeHouse",
                    _ => "Hotel"
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
    

    //Verringert den Timer um 2 Minuten
    public static void LowerTimer()
    {
        if (playTime > 0)
        {
            playTime -= 2;
            _gameForm.UpdateTimerLabel();
        }
    }
}