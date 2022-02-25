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
        //private string lastVol = "1";
        public Talk Talk;

        public TTS()
        {
            InitializeComponent();

            Disposed += TTS_Disposed;

            if (Talk == null)
                Talk = Program.Talk;

            if (Talk == null)
                throw new Exception("TTS App not found");

            VoiceGender.Items.Add("NotSet");
            VoiceGender.Items.Add("Male");
            VoiceGender.Items.Add("Female");
            VoiceGender.Items.Add("Neutral");

            VoiceAge.Items.Add("NotSet");
            VoiceAge.Items.Add("Child");
            VoiceAge.Items.Add("Teen");
            VoiceAge.Items.Add("Adult");
            VoiceAge.Items.Add("Senior");

            VoiceGender.SelectedItem = Talk.voiceGender.ToString();
            VoiceAge.SelectedItem = Talk.voiceAge.ToString();
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

        private void VoiceGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            var voiceGender = VoiceGender.SelectedItem.ToString();
            if (Talk == null) return;
            Program.Debugger.Log($"Swtiching voice from {Talk.voiceGender} to {voiceGender}");
            Talk.SwitchVoiceGender(voiceGender);
        }

        private void VoiceAge_SelectedIndexChanged(object sender, EventArgs e)
        {
            var voiceAge = VoiceAge.SelectedItem.ToString();
            if (Talk == null) return;
            Program.Debugger.Log($"Swtiching voice from {Talk.voiceAge} to {voiceAge}");
            Talk.SwitchVoiceAge(voiceAge);
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
