using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.IO;
using System.Media;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Threading;
using System.Diagnostics;

namespace VoiceController
{
    internal class Debugger
    {
        private bool IsWritingLog = false;

        private string GetTimestamp(DateTime value)
        {
            return value.ToString("F");
        }

        internal void SaveToFile(string text, string fileName)
        {
            if (IsWritingLog)
            {
                SaveToFile(text, fileName);
                Thread.Sleep(50);
                return;
            }

            IsWritingLog = true;

            string current = Directory.GetCurrentDirectory();
            string logFile = current + "/" + fileName;
            string lastLogs = "";

            if (File.Exists(logFile))
            {
                lastLogs = File.ReadAllText(logFile);
            }

            string newLogs = text + "\n" + lastLogs;

            File.WriteAllText(logFile, newLogs);

            IsWritingLog = false;
        }
        public void Log(string log)
        {
            string timeStamp = GetTimestamp(DateTime.Now);
            string logTxt = $"{timeStamp} [DEBUG] {log}";

            Console.WriteLine(logTxt);
            SaveToFile(logTxt, "VoiceController.log");
        }
    }
}

namespace VoiceController
{
    public class Talk
    {
        public Form1 Form1;
        private Debugger Debugger = new Debugger();
        public WaveOutEvent outputDevice = null;
        public WaveOutEvent virtualDevice = null;

        private AudioFileReader outputAudioFile = null;
        private AudioFileReader virtualAudioFile = null;
        private SpeechSynthesizer synth_out;

        private bool outputStopped = false;
        private bool virtualStopped = false;

        internal void Init(int deviceNumberOut = -2, int virtualNumberOut = -2)
        {
            if (deviceNumberOut == -2) // -2 is the default so we don't want to accept that for this output device
                return;

            if (virtualDevice == null && virtualNumberOut != deviceNumberOut && virtualNumberOut != -2)
            {
                virtualDevice = new WaveOutEvent()
                {
                    DeviceNumber = virtualNumberOut
                };

                virtualDevice.PlaybackStopped += virtual_PlaybackStopped;
            }
            else
            {
                virtualStopped = true;
            }

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent()
                {
                    DeviceNumber = deviceNumberOut
                };

                //if (virtualDevice == null)
                //{
                    outputDevice.PlaybackStopped += output_PlaybackStopped;
                //}
            }
        }

        private void virtual_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (virtualAudioFile != null)
            {
                string file = virtualAudioFile.FileName;

                virtualAudioFile.Dispose();
                virtualAudioFile = null;

                if (outputStopped && virtualStopped)
                {
                    Debugger.Log("Playback Stop");
                    File.Delete(file);
                }
            }
        }

        private void output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (outputAudioFile != null)
            {
                string file = outputAudioFile.FileName;

                outputAudioFile.Dispose();
                outputAudioFile = null;

                if (outputStopped && virtualStopped)
                {
                    Debugger.Log("Playback Stop");
                    File.Delete(file);
                }
            }
        }

        internal void PlaySound(string rawDataPath, string text = null)
        {
            bool shouldAttemptToPlay = true;

            if (shouldAttemptToPlay)
            {
                outputDevice.Stop();

                if (virtualDevice != null)
                {
                    virtualDevice.Stop();
                }

                if (outputAudioFile != null)
                {
                    outputAudioFile.Dispose();
                    outputAudioFile = null;
                }

                if (virtualAudioFile != null)
                {
                    Debugger.Log("Playback Force Stop");

                    string file = virtualAudioFile.FileName;

                    virtualAudioFile.Dispose();
                    virtualAudioFile = null;

                    File.Delete(file);
                }

                outputAudioFile = new AudioFileReader(rawDataPath); //new RawSourceWaveStream(rawData, WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
                virtualAudioFile = new AudioFileReader(rawDataPath);

                try
                {
                    if (outputAudioFile.CanSeek)
                        outputAudioFile.Seek(0, SeekOrigin.Begin);
                    else
                        outputAudioFile.Position = 0;
                }
                catch (Exception) { }

                try
                {
                    if (virtualAudioFile.CanSeek)
                        virtualAudioFile.Seek(0, SeekOrigin.Begin);
                    else
                        virtualAudioFile.Position = 0;
                }
                catch (Exception) { }

                if (outputAudioFile == null || virtualAudioFile == null)
                    return;

                Debugger.Log("Playback Start");

                outputStopped = false;

                virtualAudioFile = outputAudioFile;
                if (virtualDevice != null)
                {
                    virtualStopped = false;
                    virtualDevice.Init(virtualAudioFile);
                    virtualDevice.Play();
                }

                if (text != null && !string.IsNullOrWhiteSpace(text))
                {
                    if (synth_out != null)
                        synth_out.Dispose();

                    synth_out = new SpeechSynthesizer();
                    synth_out.SetOutputToDefaultAudioDevice();

                    PromptBuilder builder = new PromptBuilder();
                    builder.AppendText(text);

                    synth_out.SpeakAsync(builder);
                }

                //outputDevice.Init(outputAudioFile);
                //outputDevice.Play();
            }
        }

        private int runtimes = 0;

        public void Stop()
        {
            if (synth_out != null)
                synth_out.Dispose();

            outputDevice.Stop();

            if (virtualDevice != null)
            {
                virtualDevice.Stop();
            }
        }

        public void Speak(string text)
        {
            string current = Directory.GetCurrentDirectory();
            if (virtualDevice != null && virtualDevice.PlaybackState == PlaybackState.Playing || outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Form1.TTSBox.Text = text;
                MessageBox.Show("Please wait for the text to stop speaking before trying to speak again\n\n(atleast until I add a overide for this)");
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
                return;

            Debugger.Log(text);

            // Initialize a new instance of the speech synthesizer.  
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            using (MemoryStream streamAudio = new MemoryStream())
            {
                if (runtimes == int.MaxValue)
                    runtimes = 0;

                runtimes += 1;
                // Set voice to using hints
                synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.NotSet);
                // Configure the synthesizer to output to an audio stream.  
                synth.SetOutputToWaveStream(streamAudio);

                // Speak a phrase.
                PromptBuilder builder = new PromptBuilder();
                builder.AppendText(text);

                synth.Speak(builder);

                // Set the synthesizer output to null to release the stream.   
                synth.SetOutputToNull();

                try
                {
                    string FileName = $"{current}/AudioFiles/DO_NOT_TOUCH{runtimes}.wav";

                    if (streamAudio.CanSeek)
                        streamAudio.Seek(0, SeekOrigin.Begin);
                    else
                        streamAudio.Position = 0;

                    byte[] buffer = streamAudio.GetBuffer();

                    File.WriteAllBytes(FileName, buffer);

                    PlaySound(FileName, text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debugger.Log(e.Message + e.StackTrace);
                }
            }
        }
    }
    class Program
    {
        private static Debugger Debugger = new Debugger();
        private static Talk Talk = new Talk();
        public static Form1 Form1;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ApplicationExit += Application_ApplicationExit;

            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/AudioFiles");
            int DeviceNumberVirtual = -2;
            int DeviceNumberOut = -2;

            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);

                //Debugger.Log(i + ": " + capabilities.ProductName);

                if (capabilities.ProductName.ToLower().StartsWith("cable input"))
                {
                    DeviceNumberVirtual = i;
                }
                /*
                else
                {
                    DeviceNumberOut = i;
                }
                */
            }

            DeviceNumberOut = 0;

            /*
            if (DeviceNumberVirtual != -2)
                Debugger.Log("Virtual Cable: " + WaveIn.GetCapabilities(DeviceNumberVirtual).ProductName);

            if (DeviceNumberOut != -2)
                Debugger.Log("Audio Device: " + WaveOut.GetCapabilities(DeviceNumberOut).ProductName);
            */
            

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 = new Form1
            {
                Talk = Talk
            };

            Talk.Form1 = Form1;
            Talk.Init(DeviceNumberOut, DeviceNumberVirtual);

            Application.Run(Form1);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Talk.virtualDevice != null)
            {
                Talk.virtualDevice.Stop();
                Talk.virtualDevice.Dispose();
                Talk.virtualDevice = null;
            }

            if (Talk.outputDevice != null)
            {
                Talk.outputDevice.Stop();
                Talk.outputDevice.Dispose();
                Talk.outputDevice = null;
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}/kill.bat", "RMDIR /S /Q .\\AudioFiles\nDEL \"%~f0\" & EXIT");

            ProcessStartInfo kill = new ProcessStartInfo
            {
                FileName = $"{Directory.GetCurrentDirectory()}/kill.bat",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(kill);
            //Process.GetCurrentProcess().Kill(true);
        }
    }
}
