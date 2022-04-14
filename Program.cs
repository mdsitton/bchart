

using System;
using System.Diagnostics;
using System.IO;
using BChart;
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
        var outputMidi = Path.Combine(outputFolder, "notes.mid");
        var outputChart = Path.Combine(outputFolder, "notes2.chart");

        Stopwatch sw = new Stopwatch();
        sw.Start();
        Song song = LoadSong(chartPath);
        sw.Stop();
        Console.WriteLine($"MSCP: Load took {sw.Elapsed.Milliseconds} ms");
        sw.Restart();
        // MidWriter.WriteToFile(outputMidi, song, song.defaultExportOptions);
        // sw.Stop();
        // Console.WriteLine($"midi: Took {sw.Elapsed.Milliseconds} ms");
        // sw.Restart();
        // ChartWriter.ErrorReport errorReport;
        // new ChartWriter(outputChart).Write(song, song.defaultExportOptions, out errorReport);
        // sw.Stop();
        // Console.WriteLine($"chart: Took {sw.Elapsed.Milliseconds} ms");
        // sw.Restart();
        BChartWriter.WriteToFile(outputFile, song);
        sw.Stop();
        Console.WriteLine($"bch: Save took {sw.Elapsed.Milliseconds} ms");
        sw.Restart();
    }
}
