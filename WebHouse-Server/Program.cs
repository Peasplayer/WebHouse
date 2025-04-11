namespace WebHouse_Server;

class Program
{
    static void Main(string[] args)
    {
        // Der Server wird gestartet
        var net = new NetworkManager();
        net.StartWebsocket(8443);
        
        Console.WriteLine("Press any key to exit the program");
        Console.ReadKey(true);
    }
}