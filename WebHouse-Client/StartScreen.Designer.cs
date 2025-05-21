using System.ComponentModel;

namespace WebHouse_Client;

partial class StartScreen
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
        Start = new System.Windows.Forms.Button();
        Regeln = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // Start
        // 
        Start.Location = new System.Drawing.Point(308, 75);
        Start.Name = "Start";
        Start.Size = new System.Drawing.Size(83, 41);
        Start.TabIndex = 0;
        Start.UseVisualStyleBackColor = true;
        // 
        // Regeln
        // 
        Regeln.Location = new System.Drawing.Point(327, 167);
        Regeln.Name = "Regeln";
        Regeln.Size = new System.Drawing.Size(96, 60);
        Regeln.TabIndex = 1;
        Regeln.UseVisualStyleBackColor = true;
        // 
        // StartScreen
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(Regeln);
        Controls.Add(Start);
        Text = "StartScreen";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button Start;
    private System.Windows.Forms.Button Regeln;

    #endregion
}