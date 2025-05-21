using System.Reflection;
using WebHouse_Client.Networking;
using WebHouse_Client.Logic;

namespace WebHouse_Client;

public partial class Rules : Form
{
    private int currentRuleChapter = 0;
    private Label contentTitleLabel;
    private Label contentLabel;
    private Label TitelLabel;
    private PictureBox nextPictureBox;
    private PictureBox backPictureBox;
    private readonly List<Control> previewControls = new();

     public Rules()
    {
        InitializeComponent();
        SetupUI();
        ShowChapter(currentRuleChapter);
        
        var titleLabel = new Label()
        {
            Text = "Spielregeln",
            AutoSize = true,
            BackColor = Color.Transparent,
            ForeColor = Color.Red,
            UseCompatibleTextRendering = true,
            Font = new Font(Program.Font, 70f * ClientSize.Height / 1080, FontStyle.Regular, GraphicsUnit.Pixel),
            Location = new Point((int)(ClientSize.Width / 1920f * 730f), (int)(ClientSize.Height / 1080f * 200f))
        };
        this.Controls.Add(titleLabel);
    }

    private void SetupUI()
    {
        this.DoubleBuffered = true;
        this.BackgroundImage = Image.FromStream(
            Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Regeln.png"));
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = Screen.PrimaryScreen.Bounds.Width / 2;
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        
        contentTitleLabel = new Label
        {
            UseCompatibleTextRendering = true,
            Font = new Font(Program.Font, 50f * ClientSize.Height / 1080, FontStyle.Regular, GraphicsUnit.Pixel),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 550f), (int)(ClientSize.Height / 1080f * 350f)),
            AutoSize = true
        };
        contentTitleLabel.MaximumSize = new Size((int)(ClientSize.Width / 1920f * 800f), 0); //maximale breite
        contentTitleLabel.AutoSize = true;
        this.Controls.Add(contentTitleLabel);

        contentLabel = new Label
        {
            UseCompatibleTextRendering = true, 
            Font = new Font(Program.Font, 30f * ClientSize.Height / 1080, FontStyle.Regular, GraphicsUnit.Pixel),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 550f), (int)(ClientSize.Height / 1080f * 450f)),
            AutoSize = true
        };
        contentLabel.MaximumSize = new Size((int)(ClientSize.Width / 1920f * 800f), 0); //maximale breite
        contentLabel.AutoSize = true;
        this.Controls.Add(contentLabel);

        backPictureBox = new PictureBox()
        {
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 510f), (int)(ClientSize.Height / 1080f * 840f)),
        };
        backPictureBox.Width = (int)(ClientSize.Width / 1920f * 200f);
        backPictureBox.Height = backPictureBox.Width / 23 * 10;
        backPictureBox.Image = Image.FromStream(
            Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Images.Zurück.png"));
        backPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        backPictureBox.BringToFront();
        
        backPictureBox.Click += BackButton_CLick;
        this.Controls.Add(backPictureBox);
        
        
        nextPictureBox = new PictureBox()
        { 
            Location = new Point((int)(ClientSize.Width / 1920f * 1180f), (int)(ClientSize.Height / 1080f * 840)),
            BackColor = Color.Transparent,
        };
        nextPictureBox.Width = backPictureBox.Width;
        nextPictureBox.Height = backPictureBox.Height;
        nextPictureBox.Image = Image.FromStream(
            Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Images.Weiter.png"));
        nextPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        nextPictureBox.BringToFront();
        
        nextPictureBox.Click += NextButton_Click;
        this.Controls.Add(nextPictureBox);
        
        var card = new EscapeCard(EscapeCard.EscapeCardType.Normal, 7, Room.RoomName.Wald, CardColor.Red); //Escapekarte
        card.CreateComponent();

        card.Component.Panel.Location = new Point(
            (int)(ClientSize.Width / 1920f * 150f),
            (int)(ClientSize.Height / 1080f * 400f)
        );
        card.Component.Panel.Size = new Size(
            (int)(ClientSize.Width / 1920f * 180f),
            (int)(ClientSize.Height / 1080f * 270)
        );
        card.Component.Panel.Visible = false;

        Controls.Add(card.Component.Panel);
        card.Component.Panel.BringToFront();
        previewControls.Add(card.Component.Panel);
        
        
        var chapterCard = new ChapterCard(Room.RoomName.Wald, 2, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Green } //ChapterCard
        );
        chapterCard.CreateComponent();

        chapterCard.Component.Panel.Location = new Point(
            (int)(ClientSize.Width / 1920f * 150f),
            (int)(ClientSize.Height / 1080f * 400f)
        );
        chapterCard.Component.Panel.Size = new Size(
            (int)(ClientSize.Width / 1920f * 180f),
            (int)(ClientSize.Height / 1080f * 270)
        );
        chapterCard.Component.Panel.Visible = false;

        Controls.Add(chapterCard.Component.Panel);
        chapterCard.Component.Panel.BringToFront();
        previewControls.Add(chapterCard.Component.Panel);

        
        var chaserCard = new EscapeCard(EscapeCard.EscapeCardType.OpponentSteps, 2); //Verfolgerkarte
        chaserCard.CreateComponent();

        chaserCard.Component.Panel.Location = new Point(
            (int)(ClientSize.Width / 1920f * 150f),
            (int)(ClientSize.Height / 1080f * 400f)
        );
        chaserCard.Component.Panel.Size = new Size(
            (int)(ClientSize.Width / 1920f * 180f),
            (int)(ClientSize.Height / 1080f * 270)
        );
        chaserCard.Component.Panel.Visible = false;

        Controls.Add(chaserCard.Component.Panel);
        chaserCard.Component.Panel.BringToFront();
        previewControls.Add(chaserCard.Component.Panel);
    }

    private void BackButton_CLick(object sender, EventArgs e)
    {
        currentRuleChapter--;
        ShowChapter(currentRuleChapter);
    }
    private void NextButton_Click(object sender, EventArgs e)
    {
        currentRuleChapter++;
        ShowChapter(currentRuleChapter);
    }

    private void ShowChapter(int chapter)
    {
        foreach (var ctrl in previewControls)
            ctrl.Visible = false;   
        switch (chapter)
        {
            case 0:
                contentTitleLabel.Text = "Einleitung";
                contentLabel.Text = "Ihr habt ein Verbrechen beobachtet und seid jetzt auf der Flucht. Der Täter ist euch dicht auf den Fersen. \n\nEure einzige Chance: Erreicht das Safehouse, bevor euch der Verfolger einholt. Ein gnadenloses Spiel auf Zeit beginnt!";
                break;
            case 1:
                contentTitleLabel.Text = "Spielziel und Ablauf";
                contentLabel.Text = "Ihr müsst gemeinsam dem Verfolger entkommen. \n\nStart im Hotelzimmer dann Hafen → Stadt → Wald → Safehouse. \n\nErreicht das Safehouse, ohne eingeholt zu werden, um zu gewinnen.";
                break;
            case 2:
                contentTitleLabel.Text = "Kapitelkarten";
                contentLabel.Text = "Kapitelkarten geben euch Bewegungen auf dem Spielfeld. Um sie zu erfüllen, müsst ihr passende Fluchtkarten in den geforderten Farben anlegen. \n\nJe mehr Fluchtkarten nötig sind, desto mehr Schritte erhaltet ihr.";
                previewControls[1].Visible = true;
                break;
            case 3:
                contentTitleLabel.Text = "Fluchtkarten & Aufträge";
                contentLabel.Text = "Fluchtkarten haben Werte 1–15 in 5 Farben. Zum Erfüllen von Kapitelkarten:\n• Farben müssen passen\n• Zahlen müssen aufsteigend gelegt werden\n\nLegt Fluchtkarten an Kapitelkarten an, um den Zeugen zu bewegen.";
                previewControls[0].Visible = true;
                break;
            case 4:
                contentTitleLabel.Text = "Kommunikation";
                contentLabel.Text = "Ihr dürft euch nicht direkt über Zahlen oder Farben auf euren Handkarten austauschen!\n\n Verboten: Ich habe eine rote 7.\n\n Erlaubt: Ich habe etwas Passendes für den roten Auftrag.";
                break;
            case 5:
                contentTitleLabel.Text = "Spezialkarten";
                contentLabel.Text = "Auf dem Spielplan gibt es Spezialkarten. Um sie zu erfüllen, benötigt ihr Fluchtkarten mit dem aktuellen Kapitel-Namen (z.B. „Wald“).\n\nErfüllte Spezialkarten bringen zusätzliche Schritte!";
                break;
            case 6:
                contentTitleLabel.Text = "Verfolgerkarten";
                contentLabel.Text = "Diese Karten bringen den Verfolger voran:\n• Karten mit 1, 2 oder 3 Schritten\n• Karten, die auf offene Kapitelkarten reagieren\n\nZieht ihr eine Verfolgerkarte, wird sie sofort ausgespielt!";
                previewControls[2].Visible = true;
                break;
            case 7:
                contentTitleLabel.Text = "Zugreihenfolge";
                contentLabel.Text = "• Spiele beliebig viele Flucht- und Kapitelkarten aus\n• Du MUSST mindestens eine Aktion pro Zug ausführen:\n   - Kapitelkarte ausspielen\n   - Fluchtkarten anlegen\n   - Spezialkarte erfüllen\n• Danach ziehst du Karten nach";
                break;
            case 8:
                contentTitleLabel.Text = "Nachziehen & Verwerfen";
                contentLabel.Text = "• Hast du keine sinnvollen Karten? Dann darfst du Karten abwerfen \n Du kann immer erst am Ende deines Zuges nachziehen";
                break;
            case 9:
                contentTitleLabel.Text = "Kapitelwechsel";
                contentLabel.Text = "• Erreicht ihr das Kapitelende, geht es ins nächste Kapitel\n• Der Verfolger behält seinen Abstand gleich\n• \n• Kapitelkarten vom alten Kapitel, welche sich noch auf der Hand befinden, werden Automatisch ausgetauscht mit den Karten des neuen Kapitels";
                break;
            case 10:
                contentTitleLabel.Text = "Wann läuft der Verfolger?";
                contentLabel.Text = "• Beim Ziehen einer Verfolgerkarte\n• Beim Verwerfern gespielter Kapitelkarten \n• Wenn ihr ein Verfolger-Feld betretet\n• Alle zwei Minuten beim ertönen des Horns";
                break;
            case 11:
                contentTitleLabel.Text = "Spielende";
                contentLabel.Text = "• GEWONNEN: Ihr erreicht das Safehouse vor dem Verfolger\n• VERLOREN: Der Verfolger holt euch ein oder die Zeit von 30 Minuten ist abgelaufen";
                break;
        }
    }

}