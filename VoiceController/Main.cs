using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Soundboard
{
    public partial class Main : Form
    {
        public TTS TTS;

        public Main()
        {
            InitializeComponent();
        }

        private void GotoTTS_Click(object sender, EventArgs e)
        {
            if (TTS != null)
            {
                //TTS.StartPosition = StartPosition;
                TTS.Show();
            }
        }
    }
}
