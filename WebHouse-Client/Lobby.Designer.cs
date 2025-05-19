using System.ComponentModel;

namespace WebHouse_Client;

partial class Lobby
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
        Startbtn = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // Startbtn
        // 
        Startbtn.BackColor = System.Drawing.Color.DimGray;
        Startbtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        Startbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        Startbtn.Location = new System.Drawing.Point(516, 850);
        Startbtn.Name = "Startbtn";
        Startbtn.Size = new System.Drawing.Size(375, 105);
        Startbtn.TabIndex = 0;
        Startbtn.UseVisualStyleBackColor = false;
        Startbtn.Click += button1_Click;
        // 
        // Lobby
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1387, 1055);
        Controls.Add(Startbtn);
        Text = "Lobby";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button Startbtn;

    #endregion
}