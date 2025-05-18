using System.Media;
using System.Reflection;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    
    public static int PlayerPosition = 9;
    public static int OpponentPosition = 0;
    public static List<ICard> Inventory = new List<ICard>();
    public static Room CurrentRoom => Rooms[_currentRoom];
    
    public static List<Room> Rooms = new List<Room> // Raum-Liste wird erstellt
    {
        new Room(Room.RoomName.HotelZimmer),
        new Room(Room.RoomName.Hafen),
        new Room(Room.RoomName.Stadt),
        new Room(Room.RoomName.Wald),
        new Room(Room.RoomName.SafeHouse),
    };

    private static Stream MusicStream = Assembly.GetExecutingAssembly()
        .GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Musik.wav");

    private static void StartOpponent()
    {
        Task.Run(() =>
        {
            var sound = new SoundPlayer(MusicStream);
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
        // TODO: Check if field is opponent field
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

        //Spieler startet immer an StartField des neuen Raumes
        PlayerPosition = CurrentRoom.StartField;
        
        int levelOffset = -12;

        //Gegner neue Position berechnen
        int newOpponentPosition = levelOffset + OpponentPosition;

        //Absichern: Wenn neue Position außerhalb liegt, korrigieren
        if (newOpponentPosition < 0) newOpponentPosition = 0;
        if (newOpponentPosition >= CurrentRoom.Steps) newOpponentPosition = CurrentRoom.Steps - 1;

        OpponentPosition = newOpponentPosition;

        _gameForm.RenderBoard();
        _gameForm.UpdatePositions();
    }


}