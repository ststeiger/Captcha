﻿namespace Captcha
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
            this.btnVariant1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dgvPasswordDisplay = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasswordDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // btnVariant1
            // 
            this.btnVariant1.Location = new System.Drawing.Point(12, 576);
            this.btnVariant1.Name = "btnVariant1";
            this.btnVariant1.Size = new System.Drawing.Size(75, 23);
            this.btnVariant1.TabIndex = 0;
            this.btnVariant1.Text = "Variant 1";
            this.btnVariant1.UseVisualStyleBackColor = true;
            this.btnVariant1.Click += new System.EventHandler(this.btnVariant1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(659, 544);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dgvPasswordDisplay
            // 
            this.dgvPasswordDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPasswordDisplay.Location = new System.Drawing.Point(677, 12);
            this.dgvPasswordDisplay.Name = "dgvPasswordDisplay";
            this.dgvPasswordDisplay.Size = new System.Drawing.Size(280, 544);
            this.dgvPasswordDisplay.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(677, 576);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Variant 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 648);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvPasswordDisplay);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnVariant1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasswordDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVariant1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgvPasswordDisplay;
        private System.Windows.Forms.Button button1;
    }
}

