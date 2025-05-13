using System.Media;
using System.Reflection;
using Timer = System.Timers.Timer;

namespace WebHouse_Client.Logic;

public class GameLogic
{
    private static int _currentRoom = 0;
    private static Timer _opponentTimer;
    private static GameForm _gameForm;
    
    public static Room CurrentRoom => Rooms[_currentRoom];
    public static List<Room> Rooms = new List<Room>
    {
        new Room(Room.RoomName.HotelZimmer, "Hotel.jpg"), //Raum wird mit den Daten gefüllt
        new Room(Room.RoomName.Hafen, "Hafen.jpg"), //Raum wird mit den Daten gefüllt
        new Room(Room.RoomName.Stadt, "Stadt.jpg"), //Raum wird mit den Daten gefüllt
        new Room(Room.RoomName.Wald, "Wald.jpg"), //Raum wird mit den Daten gefüllt
        new Room(Room.RoomName.SafeHouse, "Safehouse.jpg"), //Raum wird mit den Daten gefüllt
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

    public static void SwitchRoom()
    {
        _currentRoom++;
    }
}