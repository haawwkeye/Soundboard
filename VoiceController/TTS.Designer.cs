
namespace Soundboard
{
    partial class TTS
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Speak = new System.Windows.Forms.Button();
            this.TTSBox = new System.Windows.Forms.TextBox();
            this.Stop = new System.Windows.Forms.Button();
            this.SaveTTS = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.TTSControlPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Speak
            // 
            this.Speak.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Speak.Location = new System.Drawing.Point(12, 415);
            this.Speak.Name = "Speak";
            this.Speak.Size = new System.Drawing.Size(194, 23);
            this.Speak.TabIndex = 0;
            this.Speak.Text = "Speak";
            this.Speak.UseVisualStyleBackColor = true;
            this.Speak.Click += new System.EventHandler(this.Speak_Click);
            // 
            // TTSBox
            // 
            this.TTSBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TTSBox.Location = new System.Drawing.Point(12, 54);
            this.TTSBox.MaxLength = 2147483647;
            this.TTSBox.Multiline = true;
            this.TTSBox.Name = "TTSBox";
            this.TTSBox.Size = new System.Drawing.Size(577, 355);
            this.TTSBox.TabIndex = 1;
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Stop.Location = new System.Drawing.Point(595, 415);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(194, 23);
            this.Stop.TabIndex = 2;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // SaveTTS
            // 
            this.SaveTTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveTTS.Location = new System.Drawing.Point(212, 415);
            this.SaveTTS.Name = "SaveTTS";
            this.SaveTTS.Size = new System.Drawing.Size(377, 23);
            this.SaveTTS.TabIndex = 3;
            this.SaveTTS.Text = "Save";
            this.SaveTTS.UseVisualStyleBackColor = true;
            this.SaveTTS.Click += new System.EventHandler(this.SaveTTS_Click);
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.Location = new System.Drawing.Point(12, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(776, 42);
            this.Title.TabIndex = 4;
            this.Title.Text = "TTS Panel";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TTSControlPanel
            // 
            this.TTSControlPanel.BackColor = System.Drawing.SystemColors.Window;
            this.TTSControlPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.TTSControlPanel.Location = new System.Drawing.Point(595, 54);
            this.TTSControlPanel.Name = "TTSControlPanel";
            this.TTSControlPanel.Size = new System.Drawing.Size(194, 355);
            this.TTSControlPanel.TabIndex = 5;
            // 
            // TTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TTSControlPanel);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.SaveTTS);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.TTSBox);
            this.Controls.Add(this.Speak);
            this.Name = "TTS";
            this.Text = "TTS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Speak;
        public System.Windows.Forms.TextBox TTSBox;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button SaveTTS;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Panel TTSControlPanel;
    }
}

