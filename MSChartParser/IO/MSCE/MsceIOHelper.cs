using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoonscraperEngine;

namespace MoonscraperChartEditor.Song.IO
{
    // Stores space characters found in ChartEvent objects as Japanese full-width spaces. Need to convert this back when loading.
    public class MsceIOHelper
    {
        public const string FileExtention = ".msce";

        public static readonly Dictionary<char, char> LocalEventCharReplacementToMsce = new Dictionary<char, char>()
        {
            { ' ', '\u3000' }
        };

        public static readonly Dictionary<char, char> LyricEventCharReplacementToMsce = new Dictionary<char, char>()
        {
            { '\"', '`' }
        };

        public static readonly Dictionary<char, char> LocalEventCharReplacementFromMsce = LocalEventCharReplacementToMsce.ToDictionary((i) => i.Value, (i) => i.Key);
        public static readonly Dictionary<char, char> LyricEventCharReplacementFromMsce = LyricEventCharReplacementToMsce.ToDictionary((i) => i.Value, (i) => i.Key);



        static readonly Dictionary<Song.AudioInstrument, string[]> c_audioStreamLocationOverrideDict = new Dictionary<Song.AudioInstrument, string[]>()
        {
            // String list is ordered in priority. If it finds a file names with the first string it'll skip over the rest.
            // Otherwise just does a ToString on the AudioInstrument enum
            { Song.AudioInstrument.Drum, new string[] { "drums", "drums_1" } },
        };

        public static void DiscoverAudio(string directory, Song song)
        {

            foreach (Song.AudioInstrument audio in EnumX<Song.AudioInstrument>.Values)
            {
                // First try any specific filenames for the instrument, then try the instrument name
                List<string> filenamesToTry = new List<string>();

                if (c_audioStreamLocationOverrideDict.ContainsKey(audio))
                {
                    filenamesToTry.AddRange(c_audioStreamLocationOverrideDict[audio]);
                }

                filenamesToTry.Add(audio.ToString());

                // Search for each combination of filenamesToTry + audio extension until we find a file
                string audioFilepath = null;

                foreach (string testFilename in filenamesToTry)
                {
                    foreach (string extension in Globals.validAudioExtensions)
                    {
                        string testFilepath = Path.Combine(directory, testFilename.ToLower() + extension);

                        if (File.Exists(testFilepath))
                        {
                            audioFilepath = testFilepath;
                            break;
                        }
                    }
                }

                // If we didn't find a file, assign a default value to the audio path
                if (audioFilepath == null)
                {
                    audioFilepath = Path.Combine(directory, audio.ToString().ToLower() + ".ogg");
                }

                // Debug.Log(audioFilepath);
                song.SetAudioLocation(audio, audioFilepath);
            }
        }

    }
}
