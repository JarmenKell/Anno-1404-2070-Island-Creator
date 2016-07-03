namespace Insel_Editor
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.insel = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.plateau = new System.Windows.Forms.Button();
            this.o1404 = new System.Windows.Forms.Button();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.tsize = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // insel
            // 
            this.insel.Location = new System.Drawing.Point(12, 38);
            this.insel.Name = "insel";
            this.insel.Size = new System.Drawing.Size(80, 24);
            this.insel.TabIndex = 0;
            this.insel.Text = "2070 Insel";
            this.insel.UseVisualStyleBackColor = true;
            this.insel.Click += new System.EventHandler(this.insel_Click);
            // 
            // plateau
            // 
            this.plateau.Location = new System.Drawing.Point(12, 68);
            this.plateau.Name = "plateau";
            this.plateau.Size = new System.Drawing.Size(80, 24);
            this.plateau.TabIndex = 2;
            this.plateau.Text = "2070 Plateau";
            this.plateau.UseVisualStyleBackColor = true;
            this.plateau.Click += new System.EventHandler(this.plateau_Click);
            // 
            // o1404
            // 
            this.o1404.Location = new System.Drawing.Point(107, 68);
            this.o1404.Name = "o1404";
            this.o1404.Size = new System.Drawing.Size(80, 24);
            this.o1404.TabIndex = 3;
            this.o1404.Text = "1404";
            this.o1404.UseVisualStyleBackColor = true;
            this.o1404.Click += new System.EventHandler(this.o1404_Click);
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(120, 33);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(67, 17);
            this.rb1.TabIndex = 4;
            this.rb1.TabStop = true;
            this.rb1.Text = "Okzident";
            this.rb1.UseVisualStyleBackColor = true;
            // 
            // rb2
            // 
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(120, 50);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(53, 17);
            this.rb2.TabIndex = 5;
            this.rb2.TabStop = true;
            this.rb2.Text = "Orient";
            this.rb2.UseVisualStyleBackColor = true;
            // 
            // tsize
            // 
            this.tsize.Location = new System.Drawing.Point(76, 12);
            this.tsize.Mask = "000";
            this.tsize.Name = "tsize";
            this.tsize.Size = new System.Drawing.Size(24, 20);
            this.tsize.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Inselgröße";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 99);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tsize);
            this.Controls.Add(this.rb2);
            this.Controls.Add(this.rb1);
            this.Controls.Add(this.o1404);
            this.Controls.Add(this.plateau);
            this.Controls.Add(this.insel);
            this.Name = "Form1";
            this.Text = "isd Creator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button insel;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.Button plateau;
        private System.Windows.Forms.Button o1404;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.MaskedTextBox tsize;
        private System.Windows.Forms.Label label1;
    }
}

