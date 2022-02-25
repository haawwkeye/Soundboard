using System;
using System.Collections.Generic;
using System.Text;
using System.Json;
using System.IO;
using JsonFormat;
using System.Speech.Synthesis;

namespace Soundboard
{
    public static class SettingsHandler
    {
        public static string settingsPath = $"{Directory.GetCurrentDirectory()}\\settings.json";

        public static JsonValue settings;
        public static JsonValue defaultSettings = new JsonObject(Array.Empty<KeyValuePair<string, JsonValue>>())
        {
            {
                "AutoUpdate",
                false
            },
            {
                "TTS",
                new JsonObject(Array.Empty<KeyValuePair<string, JsonValue>>())
                {
                    {
                        "VoiceGender",
                        "NotSet"
                    },
                    {
                        "VoiceAge",
                        "NotSet"
                    }
                }
            }
        };

        public static void Save()
        {
            bool useFile = true;

            if (!File.Exists(settingsPath))
            {
                useFile = false;
            }

            if (useFile)
            {
                string json = File.ReadAllText(settingsPath);
                Program.Debugger.Log(json);
                settings = JsonValue.Parse(json);
            }
            else
            {
                settings = defaultSettings;
            }

            File.WriteAllText(settingsPath, JsonFormatter.PrettyJson(settings.ToString()));
        }

        public static void Start()
        {
            Save();

            VoiceGender voiceGender = settings["TTS"]["VoiceGender"].ToString() switch
            {
                "NotSet" => VoiceGender.NotSet,
                "Male" => VoiceGender.Male,
                "Female" => VoiceGender.Female,
                "Neutral" => VoiceGender.Neutral,
                _ => VoiceGender.NotSet,
            };

            VoiceAge voiceAge = settings["TTS"]["VoiceAge"].ToString() switch
            {
                "NotSet" => VoiceAge.NotSet,
                "Child" => VoiceAge.Child,
                "Teen" => VoiceAge.Teen,
                "Adult" => VoiceAge.Adult,
                "Senior" => VoiceAge.Senior,
                _ => VoiceAge.NotSet,
            };

            Talk.voiceGender = voiceGender;
            Talk.voiceAge = voiceAge;
        }
    }
}
