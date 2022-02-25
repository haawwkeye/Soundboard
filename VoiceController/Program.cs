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

namespace Soundboard
{
    internal class Debugger
    {
        private bool IsWritingLog = false;

        private string GetTimestamp(DateTime value)
        {
            return value.ToString("F");
        }

        internal void SaveToFile(string text, string fileName = "Debug.log")
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
            SaveToFile(logTxt);
        }
    }

    public class Talk
    {
        public TTS TTS;
        private Debugger Debugger = new Debugger();
        public WaveOutEvent outputDevice = null;
        public WaveOutEvent virtualDevice = null;

        private AudioFileReader outputAudioFile = null;
        private AudioFileReader virtualAudioFile = null;
        private SpeechSynthesizer synth_out;

        public static VoiceGender voiceGender = VoiceGender.NotSet;
        public static VoiceAge voiceAge = VoiceAge.NotSet;

        private bool outputStopped = false;
        private bool virtualStopped = false;

        public float volume = 1f;

        public void SwitchVoiceGender(string selectedItem)
        {
            voiceGender = selectedItem switch
            {
                "NotSet" => VoiceGender.NotSet,
                "Male" => VoiceGender.Male,
                "Female" => VoiceGender.Female,
                "Neutral" => VoiceGender.Neutral,
                _ => VoiceGender.NotSet,
            };
        }

        public void SwitchVoiceAge(string selectedItem)
        {
            voiceAge = selectedItem switch
            {
                "NotSet" => VoiceAge.NotSet,
                "Child" => VoiceAge.Child,
                "Teen" => VoiceAge.Teen,
                "Adult" => VoiceAge.Adult,
                "Senior" => VoiceAge.Senior,
                _ => VoiceAge.NotSet,
            };
        }

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
                    virtualDevice.Volume = volume;
                    virtualDevice.Play();
                }

                if (text != null && !string.IsNullOrWhiteSpace(text))
                {
                    if (synth_out != null)
                        synth_out.Dispose();

                    synth_out = new SpeechSynthesizer();
                    synth_out.SelectVoiceByHints(voiceGender, voiceAge);
                    synth_out.SetOutputToDefaultAudioDevice();

                    PromptBuilder builder = new PromptBuilder();
                    builder.AppendText(text);

                    //synth_out.Volume = (int)(volume*100);
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

        public MemoryStream? MakeAudio(string text)
        {
            MemoryStream streamAudio = new MemoryStream();

            string current = Directory.GetCurrentDirectory();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            Debugger.Log(text);

            // Initialize a new instance of the speech synthesizer.  
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                if (runtimes == int.MaxValue)
                    runtimes = 0;

                runtimes += 1;
                // Set voice to using hints
                synth.SelectVoiceByHints(voiceGender, voiceAge);
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
                    if (streamAudio.CanSeek)
                        streamAudio.Seek(0, SeekOrigin.Begin);
                    else
                        streamAudio.Position = 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debugger.Log(e.Message + e.StackTrace);
                }

                return streamAudio;
            }
        }

        public void Speak(string text)
        {
            string current = Directory.GetCurrentDirectory();
            if (virtualDevice != null && virtualDevice.PlaybackState == PlaybackState.Playing || outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                TTS.TTSBox.Text = text;
                MessageBox.Show("Please wait for the text to stop speaking before trying to speak again\n\n(atleast until I add a overide for this)");
                return;
            }

            MemoryStream streamAudio = MakeAudio(text);

            if (streamAudio == null)
            {
                return;
            }

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

        // Do I really need to explain this? it just replaces a string from a string with a string
        private string ReplaceString(string sourceString, string removeString, string replace = "")
        {
            return sourceString.Replace(removeString, replace);
        }

        private DialogResult PromptFileDialog(string current, byte[] buffer)
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Audio File|*.wav",
                Title = "Save Voice",
                InitialDirectory = $"{current}\\Sounds",
                FileName = "AudioName",
                DefaultExt = ".wav"
            };

            DialogResult dialogResult = saveFile.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string fileName = ReplaceString(saveFile.FileName, Path.GetDirectoryName(saveFile.FileName)+"\\");
                fileName = ReplaceString(fileName, ".wav");

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Debugger.Log($"Failed to save file '{fileName}.wav'");
                    DialogResult dialog = MessageBox.Show($"Cannot save file if name is null or whitespace", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    
                    if (dialog == DialogResult.Retry)
                    {
                        return PromptFileDialog(current, buffer);
                    }

                    return DialogResult.Abort;
                }
                else
                {
                    Debugger.Log($"Saved file '{fileName}'");
                    File.WriteAllBytes(saveFile.FileName, buffer);
                }
            }
            else
            {
                string fileName = ReplaceString(saveFile.FileName, Path.GetDirectoryName(saveFile.FileName) + "\\");
                fileName = ReplaceString(fileName, ".wav");

                Debugger.Log($"Failed to save file '{fileName}.wav' :: {dialogResult}");
            }

            return dialogResult;
        }

        public void PromptSave(string text)
        {
            MemoryStream streamAudio = MakeAudio(text);

            if (streamAudio == null)
            {
                return;
            }

            string current = Directory.GetCurrentDirectory();
            byte[] buffer = streamAudio.GetBuffer();

            PromptFileDialog(current, buffer);
        }
    }
    class Program
    {
        internal static Debugger Debugger = new Debugger();
        internal static Talk Talk = new Talk();
        public static Main MainForm;
        public static TTS TTS;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SettingsHandler.Start();

            bool hasUpdate = AutoUpdate.CheckForUpdate();
            Application.ApplicationExit += ApplicationExit;

            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/AudioFiles");
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/Sounds");

            if (hasUpdate && AutoUpdate.shouldUpdate)
            {
                AutoUpdate.Update();
                return;
            }

            int DeviceNumberVirtual = -2;
            int DeviceNumberOut = -2;

            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities capabilities = WaveOut.GetCapabilities(i);

                if (capabilities.ProductName.ToLower().StartsWith("cable input"))
                {
                    DeviceNumberVirtual = i;
                }
            }

            DeviceNumberOut = 0;

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TTS = new TTS
            {
                Talk = Talk
            };

            MainForm = new Main
            {
                TTS = TTS
            };

            Talk.TTS = TTS;
            Talk.Init(DeviceNumberOut, DeviceNumberVirtual);

            Application.Run(MainForm);
        }

        private static void ApplicationExit(object sender, EventArgs e)
        {
            SettingsHandler.settings["TTS"]["VoiceGender"] = Talk.voiceGender.ToString();
            SettingsHandler.settings["TTS"]["VoiceAge"] = Talk.voiceAge.ToString();

            SettingsHandler.Save();

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
        }
    }
}
