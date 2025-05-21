using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WebHouse_Client;

static class Program
{
    public static FontFamily Font { get; private set; }
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        using (Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.321impact.ttf"))
        {
            byte[] fontData = new byte[fontStream.Length];
            fontStream.Read(fontData, 0, (int)fontStream.Length);

            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            var font = new PrivateFontCollection();
            font.AddMemoryFont(fontPtr, fontData.Length);
            Font = font.Families[0];
            Marshal.FreeCoTaskMem(fontPtr);
        }
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new StartScreen());
    }
}