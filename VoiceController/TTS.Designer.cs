
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
            this.SuspendLayout();
            // 
            // Speak
            // 
            this.Speak.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Speak.Location = new System.Drawing.Point(12, 415);
            this.Speak.Name = "Speak";
            this.Speak.Size = new System.Drawing.Size(388, 23);
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
            this.TTSBox.Location = new System.Drawing.Point(12, 12);
            this.TTSBox.MaxLength = 2147483647;
            this.TTSBox.Multiline = true;
            this.TTSBox.Name = "TTSBox";
            this.TTSBox.Size = new System.Drawing.Size(776, 397);
            this.TTSBox.TabIndex = 1;
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Stop.Location = new System.Drawing.Point(400, 415);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(388, 23);
            this.Stop.TabIndex = 2;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // TTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

