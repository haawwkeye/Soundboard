
namespace Soundboard
{
    partial class Main
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
            this.GotoTTS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GotoTTS
            // 
            this.GotoTTS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.GotoTTS.Location = new System.Drawing.Point(550, 408);
            this.GotoTTS.Name = "GotoTTS";
            this.GotoTTS.Size = new System.Drawing.Size(238, 30);
            this.GotoTTS.TabIndex = 0;
            this.GotoTTS.Text = "Open TTS Panel";
            this.GotoTTS.UseVisualStyleBackColor = true;
            this.GotoTTS.Click += new System.EventHandler(this.GotoTTS_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GotoTTS);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GotoTTS;
    }
}