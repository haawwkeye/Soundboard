using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soundboard
{
    public partial class TTS : Form
    {
        public Talk Talk;

        public TTS()
        {
            InitializeComponent();
            Disposed += TTS_Disposed;
        }

        private void TTS_Disposed(object sender, EventArgs e)
        {
            //Stop_Click(sender, e);

            if (Program.MainForm != null)
            {
                Program.MainForm.Show();

                Program.TTS = new TTS
                {
                    Talk = Talk
                };

                Program.MainForm.TTS = Program.TTS;
                Talk.TTS = Program.TTS;
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Talk.Stop();
        }

        private void Speak_Click(object sender, EventArgs e)
        {
            string sayThis = TTSBox.Text;
            TTSBox.Text = "";
            Talk.Speak(sayThis);
        }
    }
}
