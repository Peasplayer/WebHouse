namespace WebHouse_Client;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        GameFormBTN = new System.Windows.Forms.Button();
        BenutzterNameTBox = new System.Windows.Forms.TextBox();
        ServerIPTBox = new System.Windows.Forms.TextBox();
        Spielerstellenbtn = new System.Windows.Forms.Button();
        Spielbeitretenbtn = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // GameFormBTN
        // 
        GameFormBTN.Location = new System.Drawing.Point(226, 68);
        GameFormBTN.Name = "GameFormBTN";
        GameFormBTN.Size = new System.Drawing.Size(173, 270);
        GameFormBTN.TabIndex = 0;
        GameFormBTN.Text = "Test";
        GameFormBTN.UseVisualStyleBackColor = true;
        GameFormBTN.Click += GameFormBTN_Click;
        // 
        // BenutzterNameTBox
        // 
        BenutzterNameTBox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
        BenutzterNameTBox.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        BenutzterNameTBox.ForeColor = System.Drawing.SystemColors.Window;
        BenutzterNameTBox.Location = new System.Drawing.Point(711, 184);
        BenutzterNameTBox.Name = "BenutzterNameTBox";
        BenutzterNameTBox.Size = new System.Drawing.Size(701, 87);
        BenutzterNameTBox.TabIndex = 1;
        BenutzterNameTBox.Text = "Alex";
        // 
        // ServerIPTBox
        // 
        ServerIPTBox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
        ServerIPTBox.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        ServerIPTBox.ForeColor = System.Drawing.Color.Transparent;
        ServerIPTBox.Location = new System.Drawing.Point(875, 344);
        ServerIPTBox.Name = "ServerIPTBox";
        ServerIPTBox.Size = new System.Drawing.Size(537, 87);
        ServerIPTBox.TabIndex = 2;
        ServerIPTBox.Text = "128.90.128.20";
        // 
        // Spielerstellenbtn
        // 
        Spielerstellenbtn.BackColor = System.Drawing.Color.Transparent;
        Spielerstellenbtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        Spielerstellenbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        Spielerstellenbtn.Location = new System.Drawing.Point(482, 549);
        Spielerstellenbtn.Name = "Spielerstellenbtn";
        Spielerstellenbtn.Size = new System.Drawing.Size(913, 125);
        Spielerstellenbtn.TabIndex = 3;
        Spielerstellenbtn.UseVisualStyleBackColor = false;
        // 
        // Spielbeitretenbtn
        // 
        Spielbeitretenbtn.BackColor = System.Drawing.Color.Transparent;
        Spielbeitretenbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        Spielbeitretenbtn.Location = new System.Drawing.Point(482, 758);
        Spielbeitretenbtn.Name = "Spielbeitretenbtn";
        Spielbeitretenbtn.Size = new System.Drawing.Size(913, 125);
        Spielbeitretenbtn.TabIndex = 4;
        Spielbeitretenbtn.UseVisualStyleBackColor = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1597, 942);
        Controls.Add(Spielbeitretenbtn);
        Controls.Add(Spielerstellenbtn);
        Controls.Add(ServerIPTBox);
        Controls.Add(BenutzterNameTBox);
        Controls.Add(GameFormBTN);
        Margin = new System.Windows.Forms.Padding(2);
        Text = "Form1";
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button Spielerstellenbtn;
    private System.Windows.Forms.Button Spielbeitretenbtn;

    private System.Windows.Forms.TextBox ServerIPTBox;

    private System.Windows.Forms.TextBox BenutzterNameTBox;

    private System.Windows.Forms.Button GameFormBTN;

    #endregion
}