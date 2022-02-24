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
        private string lastVol = "1";
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
                Program.MainForm.StartPosition = FormStartPosition.Manual;
                Program.MainForm.Location = Location;
                Program.MainForm.Size = Size;

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

        private void SaveTTS_Click(object sender, EventArgs e)
        {
            Talk.PromptSave(TTSBox.Text);
        }

        /*
        private void Volume_TextChanged(object sender, EventArgs e)
        {
            string vol = Volume.Text;
            float volume = Talk.volume;

            if (float.TryParse(vol, out volume))
            {
                if (volume > 2)
                {
                    volume = 2;
                    Volume.Text = volume.ToString();
                }
                else if (volume < 0)
                {
                    volume = 0;
                    Volume.Text = volume.ToString();
                }

                lastVol = volume.ToString();
                Talk.volume = volume;
            }
            else if (vol == "" || vol == "-")
            {
                return;
            }
            else
            {
                Volume.Text = lastVol;
            }
        }
        */
    }
}
