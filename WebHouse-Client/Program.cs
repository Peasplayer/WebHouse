using WebHouse_Client.Networking;

namespace WebHouse_Client;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Der Client verbindet sich mit dem Server und gibt seinen Namen an
        // Als Task wird so nicht das öffnen der Forms blockiert
        Task.Run(() =>
        {
            var net = new NetworkManager();
            net.Connect("ws://127.0.0.1:8443", "DumDum");
        });

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Pursuer.StartPursuerTimer();
        Application.Run(new Form1());
    }
}