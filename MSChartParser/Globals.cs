namespace MoonscraperChartEditor
{
    public static class Globals
    {
        public const string TABSPACE = "  ";
        public static readonly string LINE_ENDING = "\r\n";         
        public static readonly string[] validAudioExtensions = { ".ogg", ".wav", ".mp3" };

        public static bool validateExtension(string filepath, string[] validExtensions)
        {
            // Need to check extension
            string extension = System.IO.Path.GetExtension(filepath);

            foreach (string validExtension in validExtensions)
            {
                if (extension == validExtension)
                    return true;
            }
            return false;
        }
    }
}
