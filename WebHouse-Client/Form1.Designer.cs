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
        SuspendLayout();
        // 
        // GameFormBTN
        // 
        GameFormBTN.Location = new System.Drawing.Point(226, 68);
        GameFormBTN.Name = "GameFormBTN";
        GameFormBTN.Size = new System.Drawing.Size(173, 270);
        GameFormBTN.TabIndex = 0;
        GameFormBTN.Text = "button1";
        GameFormBTN.UseVisualStyleBackColor = true;
        GameFormBTN.Click += GameFormBTN_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1316, 859);
        Controls.Add(GameFormBTN);
        Text = "Form1";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button GameFormBTN;

    #endregion
}