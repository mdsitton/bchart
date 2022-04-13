

using System;
using System.IO;
using MoonscraperChartEditor.Song;
using MoonscraperChartEditor.Song.IO;

public static class Program
{
    public static Song LoadSong(string chartPath)
    {
        var ext = Path.GetExtension(chartPath);
        Song song = null;
        if (ext.Equals(".mid", StringComparison.OrdinalIgnoreCase))
        {
            MidReader.CallbackState callBackState = default;
            song = MidReader.ReadMidi(chartPath, ref callBackState);
        }
        else if (ext.Equals(".chart", StringComparison.OrdinalIgnoreCase))
        {
            song = ChartReader.ReadChart(chartPath);
        }
        return song;
    }

    public static void Main(string[] args)
    {
        var chartPath = args[0];
        var outputFolder = Path.GetDirectoryName(chartPath);
        var outputFile = Path.Combine(outputFolder, "notes.bch");

        Song song = LoadSong(chartPath);
        BChartWriter.WriteToFile(outputFile, song);
    }
}
