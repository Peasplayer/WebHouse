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
        Startbtn = new System.Windows.Forms.Button();
        textBox1 = new System.Windows.Forms.TextBox();
        textBox2 = new System.Windows.Forms.TextBox();
        SuspendLayout();
        // 
        // Startbtn
        // 
        Startbtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        Startbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        Startbtn.Location = new System.Drawing.Point(850, 700);
        Startbtn.Name = "Startbtn";
        Startbtn.Size = new System.Drawing.Size(584, 132);
        Startbtn.TabIndex = 0;
        Startbtn.UseVisualStyleBackColor = true;
        Startbtn.Click += Startbtn_Click;
        // 
        // textBox1
        // 
        textBox1.BackColor = System.Drawing.SystemColors.InfoText;
        textBox1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        textBox1.ForeColor = System.Drawing.SystemColors.HighlightText;
        textBox1.Location = new System.Drawing.Point(850, 205);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(474, 61);
        textBox1.TabIndex = 1;
        textBox1.Tag = "hallo";
        textBox1.Text = "Alex";
        // 
        // textBox2
        // 
        textBox2.BackColor = System.Drawing.SystemColors.InfoText;
        textBox2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        textBox2.ForeColor = System.Drawing.SystemColors.HighlightText;
        textBox2.Location = new System.Drawing.Point(850, 356);
        textBox2.Name = "textBox2";
        textBox2.Size = new System.Drawing.Size(474, 61);
        textBox2.TabIndex = 2;
        textBox2.Text = "0000000001000001";
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(1627, 1055);
        Controls.Add(textBox2);
        Controls.Add(textBox1);
        Controls.Add(Startbtn);
        Location = new System.Drawing.Point(19, 19);
        Margin = new System.Windows.Forms.Padding(2);
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;

    private System.Windows.Forms.Button Startbtn;

    #endregion
}