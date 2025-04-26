using System;
using System.Timers;
using System.Windows.Forms;
namespace WebHouse_Client;

public class Pursuer
{
    private static System.Timers.Timer PursuerTimer;
    private static int PhaseCounter;

    public static void StartPursuerTimer()
    {
        PursuerTimer = new System.Timers.Timer( 2 * 60 * 1000); //2 Minuten
        PursuerTimer.Elapsed += GameTimer;
        PursuerTimer.AutoReset = true;
        PursuerTimer.Enabled = true;
        PursuerTimer.Start();
    }
    private static void GameTimer(object sender, ElapsedEventArgs e)
    {
        PhaseCounter++;
        Console.WriteLine(PhaseCounter);
        if (PhaseCounter == 10)
        {
            GameEnd();
        }
    }

    private static void GameEnd()
    {
        Console.WriteLine("Game End");
        PursuerTimer.AutoReset = false;
        PursuerTimer.Enabled = false;
        PursuerTimer.Stop();
    }
}