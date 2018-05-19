namespace Client
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butsend = new System.Windows.Forms.Button();
            this.txtmess = new System.Windows.Forms.TextBox();
            this.listBoxclient = new System.Windows.Forms.ListBox();
            this.butsendnoise = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtmessnoise = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtkey = new System.Windows.Forms.TextBox();
            this.txtiv = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DodgerBlue;
            this.groupBox1.Controls.Add(this.butsend);
            this.groupBox1.Controls.Add(this.txtmess);
            this.groupBox1.Controls.Add(this.listBoxclient);
            this.groupBox1.Controls.Add(this.butsendnoise);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtmessnoise);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtkey);
            this.groupBox1.Controls.Add(this.txtiv);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 229);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat";
            // 
            // butsend
            // 
            this.butsend.Location = new System.Drawing.Point(278, 87);
            this.butsend.Name = "butsend";
            this.butsend.Size = new System.Drawing.Size(75, 27);
            this.butsend.TabIndex = 6;
            this.butsend.Text = "Send";
            this.butsend.UseVisualStyleBackColor = true;
            this.butsend.Click += new System.EventHandler(this.butsend_Click);
            // 
            // txtmess
            // 
            this.txtmess.Location = new System.Drawing.Point(19, 87);
            this.txtmess.Multiline = true;
            this.txtmess.Name = "txtmess";
            this.txtmess.Size = new System.Drawing.Size(245, 27);
            this.txtmess.TabIndex = 5;
            this.txtmess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtmess_KeyDown);
            // 
            // listBoxclient
            // 
            this.listBoxclient.BackColor = System.Drawing.Color.LightSteelBlue;
            this.listBoxclient.FormattingEnabled = true;
            this.listBoxclient.Location = new System.Drawing.Point(374, 19);
            this.listBoxclient.Name = "listBoxclient";
            this.listBoxclient.Size = new System.Drawing.Size(281, 186);
            this.listBoxclient.TabIndex = 4;
            // 
            // butsendnoise
            // 
            this.butsendnoise.Location = new System.Drawing.Point(278, 133);
            this.butsendnoise.Name = "butsendnoise";
            this.butsendnoise.Size = new System.Drawing.Size(75, 28);
            this.butsendnoise.TabIndex = 7;
            this.butsendnoise.Text = "Send Noise";
            this.butsendnoise.UseVisualStyleBackColor = true;
            this.butsendnoise.Click += new System.EventHandler(this.butsendnoise_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Key Chung:";
            // 
            // txtmessnoise
            // 
            this.txtmessnoise.BackColor = System.Drawing.SystemColors.Control;
            this.txtmessnoise.Location = new System.Drawing.Point(19, 133);
            this.txtmessnoise.Multiline = true;
            this.txtmessnoise.Name = "txtmessnoise";
            this.txtmessnoise.Size = new System.Drawing.Size(245, 72);
            this.txtmessnoise.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "IV Time:";
            // 
            // txtkey
            // 
            this.txtkey.Location = new System.Drawing.Point(84, 19);
            this.txtkey.Name = "txtkey";
            this.txtkey.Size = new System.Drawing.Size(269, 20);
            this.txtkey.TabIndex = 17;
            // 
            // txtiv
            // 
            this.txtiv.Location = new System.Drawing.Point(84, 45);
            this.txtiv.Name = "txtiv";
            this.txtiv.Size = new System.Drawing.Size(269, 20);
            this.txtiv.TabIndex = 19;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 252);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Client2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butsend;
        private System.Windows.Forms.TextBox txtmess;
        private System.Windows.Forms.ListBox listBoxclient;
        private System.Windows.Forms.Button butsendnoise;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtmessnoise;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtkey;
        private System.Windows.Forms.TextBox txtiv;
        private System.Windows.Forms.Timer timer1;
    }
}

