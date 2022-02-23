using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceController
{
    public partial class Form1 : Form
    {
        public Talk Talk;

        public Form1()
        {
            InitializeComponent();
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
