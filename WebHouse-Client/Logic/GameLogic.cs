using System.Media;
using System.Reflection;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    
    public static int PlayerPosition = 0;
    public static int OpponentPosition = 0;
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
                .GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Ikea.wav"));
            sound.PlaySync();
            MessageBox.Show("ALARM");
            StartOpponent();
        });
    }
    
    public static void Start(GameForm form)
    {
        _gameForm = form;
        
        StartOpponent();
        
        _opponentTimer = new Timer(1000 * 10);
        _opponentTimer.Elapsed += (s, e) =>
        {
            SoundPlayer test = new SoundPlayer(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Sounds.Test.wav"));
            test.Play();
        };
        _opponentTimer.Enabled = true;
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
        
        _gameForm.UpdatePositions();
    }
    
    public static void SwitchRoom()
    {
        _currentRoom++;
        _gameForm.RenderBoard();
        // TODO: Set positions of player and opponent
    }
}