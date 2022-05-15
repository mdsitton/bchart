
using System;
using System.IO;
using MoonscraperChartEditor.Song;
using MoonscraperChartEditor.Song.IO;
using BinaryEx;
using System.Collections.Generic;
using static MoonscraperChartEditor.Song.Song;
using System.Text;
using System.Buffers;

namespace BChart;

public static class BChartWriter
{
    // Event serialization functions

    public static void WriteTextEvent(Stream stream, ChartEvent ev)
    {
        byte[] bytes = BChartUtils.GetStringAsBytes(ev.eventName, out var outSpan);

        // limit string size to byte max value
        if (outSpan.Length > 255)
        {
            outSpan = outSpan.Slice(0, 255);
        }
        WriteTicks(stream, ev.tick);
        stream.WriteByte(BChartConsts.EVENT_TEXT);
        stream.WriteByte((byte)outSpan.Length);
        stream.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteTextEvent(Stream stream, Event ev)
    {
        byte[] bytes = BChartUtils.GetStringAsBytes(ev.title, out var outSpan);

        // limit string size to byte max value
        if (outSpan.Length > 255)
        {
            outSpan = outSpan.Slice(0, 255);
        }
        WriteTicks(stream, ev.tick);
        stream.WriteByte(BChartConsts.EVENT_TEXT);
        stream.WriteByte((byte)outSpan.Length);
        stream.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteSection(Stream stream, Section section)
    {
        byte[] bytes = BChartUtils.GetStringAsBytes(section.title, out var outSpan);

        // limit string size to byte max value
        if (outSpan.Length > 255)
        {
            outSpan = outSpan.Slice(0, 255);
        }
        WriteTicks(stream, section.tick);
        stream.WriteByte(BChartConsts.EVENT_SECTION);
        stream.WriteByte((byte)outSpan.Length);
        stream.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WritePhrase(Stream stream, byte phraseType, uint tick, uint tickLength)
    {
        WriteTicks(stream, tick);
        stream.WriteByte(BChartConsts.EVENT_PHRASE);
        stream.WriteByte(5);
        stream.WriteByte(phraseType);
        stream.WriteUInt32LE(tickLength);
    }

    public static void WriteTimeSignature(Stream stream, TimeSignature ts)
    {
        WriteTicks(stream, ts.tick);
        stream.WriteByte(BChartConsts.EVENT_TIME_SIG);
        stream.WriteByte(2);
        stream.WriteByte((byte)ts.numerator);
        stream.WriteByte((byte)ts.denominator);
    }

    const long microsecondsPerMinute = 60000000;
    public static void WriteTempo(Stream stream, BPM bpm)
    {
        uint microSecondsPerQuarter = (uint)(microsecondsPerMinute * 1000 / bpm.value);
        WriteTicks(stream, bpm.tick);
        stream.WriteByte(BChartConsts.EVENT_TEMPO);
        stream.WriteByte(4);
        stream.WriteUInt32LE(microSecondsPerQuarter);
    }


    private static uint previousTickPos = 0;
    public static void WriteTicks(Stream stream, uint tickPos)
    {

        if (tickPos < previousTickPos)
        {
            previousTickPos = 0;
        }
        uint deltaTicks = tickPos - previousTickPos;
        if (deltaTicks > ushort.MaxValue)
        {
            stream.WriteByte(255);
            stream.WriteUInt32LE(deltaTicks);
        }
        else if (deltaTicks > 253)
        {
            stream.WriteByte(254);
            stream.WriteUInt16LE((ushort)deltaTicks);
        }
        else
        {
            stream.WriteByte((byte)deltaTicks);
        }
        previousTickPos = tickPos;
    }

    public static bool WriteNote(Stream stream, Note note, Instrument inst)
    {
        uint eventLength = 5; // Note event is atleast 6 bytes
        uint supplementalDataLength = 4; // Note modifiers supplemental data is 4 bytes

        uint modifiers = BChartUtils.MoonNoteToBChartMod(inst, note);

        byte noteOut = BChartUtils.MoonNoteToBChart(inst, note);

        // don't write out unknown note
        if (noteOut == BChartConsts.NOTE_UKN)
        {
            return false;
        }

        byte byteLength = (byte)eventLength;

        if (modifiers > 0)
        {
            byteLength++;
            byteLength += (byte)supplementalDataLength;
        }

        WriteTicks(stream, note.tick);
        stream.WriteByte(BChartConsts.EVENT_NOTE);
        stream.WriteByte(byteLength);
        stream.WriteByte(noteOut);
        stream.WriteUInt32LE(note.length);

        if (modifiers > 0)
        {
            stream.WriteUInt32LE(modifiers);
        }
        return true;
    }

    public static void WriteChunk(Stream stream, uint chunkId, Action<Stream> action, Action<Stream> preAction = null)
    {
        // serialize chunk data to memory stream 
        using (var ms = new MemoryStream())
        {
            action?.Invoke(ms);
            using (var ms2 = new MemoryStream())
            {
                preAction?.Invoke(ms2);
                stream.WriteUInt32LE(chunkId);
                stream.WriteInt32LE((int)(ms.Length + ms2.Length));
                stream.Write(ms2.GetBuffer(), 0, (int)ms2.Length);
                stream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
    }

    // Chunk serialization functions
    public static void WriteHeader(Stream stream, uint instrumentCount, uint resolution)
    {
        // header
        const int headerLength = 6;
        stream.WriteUInt32LE(BChartConsts.HeaderChunkName); // BCHF
        stream.WriteInt32LE(headerLength);
        stream.WriteUInt16LE(1); // version 1
        stream.WriteUInt16LE((ushort)resolution);
        stream.WriteUInt16LE((ushort)instrumentCount);
    }

    public static void WriteTempoMap(Stream stream, IList<SyncTrack> tempoMap)
    {
        int savedEvents = 0;
        void WriteData(Stream stre)
        {
            foreach (var tempo in tempoMap)
            {
                if (tempo is TimeSignature ts)
                {
                    savedEvents++;
                    WriteTimeSignature(stre, ts);
                }
                else if (tempo is BPM bpm)
                {
                    savedEvents++;
                    WriteTempo(stre, bpm);
                }
            }
        }

        void PreDataWrite(Stream stre)
        {
            // write this to the outstream directly before the data gets written
            stre.WriteInt32LE(savedEvents);
        }

        WriteChunk(stream, BChartConsts.TempoChunkName, WriteData, PreDataWrite); // SYNC

    }

    public static void WriteGlobalEvents(Stream stream, IList<Event> events)
    {
        int savedEvents = 0;
        void WriteData(Stream stre)
        {
            foreach (var ev in events)
            {
                if (ev is Section section)
                {
                    savedEvents++;
                    WriteSection(stre, section);
                }
                else
                {
                    savedEvents++;
                    WriteTextEvent(stre, ev);
                }
            }
        }

        void PreDataWrite(Stream stre)
        {
            // write this to the outstream directly before the data gets written
            stre.WriteInt32LE(savedEvents);
        }
        WriteChunk(stream, BChartConsts.GlobalEventsChunkName, WriteData, PreDataWrite); // EVTS

    }

    public static void WriteInstrument(Stream stream, Instrument inst, Dictionary<Difficulty, Chart> diffs)
    {
        void WriteData(Stream stre)
        {
            stre.WriteUInt32LE(BChartUtils.MoonInstrumentToBChart(inst));
            stre.WriteByte((byte)diffs.Count);
            // TODO per instrument events track?
        }
        WriteChunk(stream, BChartConsts.InstrumentChunkName, WriteData); // INST
        foreach (var data in diffs)
        {
            WriteDifficulty(stream, inst, data.Key, data.Value);
        }
    }

    public static void WriteDifficulty(Stream stream, Instrument inst, Difficulty diff, Chart chart)
    {
        int savedEvents = 0;
        void WriteData(Stream stre)
        {
            stre.WriteByte((byte)diff);
            int i = 0;
            foreach (var ev in chart.chartObjects)
            {
                ++i;
                switch (ev)
                {
                    case Note note:
                        if (WriteNote(stre, note, inst))
                        {
                            savedEvents++;
                        }
                        break;
                    case Starpower sp:
                        savedEvents++;
                        WritePhrase(stre, BChartConsts.PHRASE_STARPOWER, sp.tick, sp.length);
                        break;
                    case ChartEvent chEv:
                        {
                            if (chEv.eventName == MidIOHelper.SoloEventText)
                            {
                                bool foundMatch = false;
                                // Find matching soloEnd
                                for (int j = i; j < chart.chartObjects.Count; ++i)
                                {
                                    if (chart.chartObjects[i] is ChartEvent chNext)
                                    {
                                        if (chNext.eventName == MidIOHelper.SoloEndEventText)
                                        {
                                            foundMatch = true;
                                            savedEvents++;
                                            WritePhrase(stre, BChartConsts.PHRASE_SOLO, chEv.tick, chNext.tick - chEv.tick);
                                            break;
                                        }
                                    }
                                }
                                if (!foundMatch)
                                {
                                    savedEvents++;
                                    // Make length match until the last chart event
                                    var last = chart.chartObjects[chart.chartObjects.Count - 1];
                                    WritePhrase(stre, BChartConsts.PHRASE_SOLO, chEv.tick, last.tick - chEv.tick);
                                }
                            }
                            else if (chEv.eventName == MidIOHelper.SoloEndEventText)
                            {
                                continue;
                            }
                            else
                            {
                                savedEvents++;
                                WriteTextEvent(stre, chEv);
                            }
                        }
                        break;
                }
            }
        }
        void PreDataWrite(Stream stre)
        {
            // write this to the outstream directly before the data gets written
            stre.WriteInt32LE(savedEvents);
        }
        WriteChunk(stream, BChartConsts.DifficultyChunkName, WriteData, PreDataWrite); // DIFF
    }

    /// <summary>
    /// Serialize and save <see cref="Song"/> instance as BChart file format
    /// </summary>
    /// <param name="outputPath">File path to save to</param>
    /// <param name="song">Instance to serialize</param>
    public static void WriteToFile(string outputPath, Song song)
    {
        Dictionary<Instrument, Dictionary<Difficulty, Chart>> charts = new Dictionary<Instrument, Dictionary<Difficulty, Chart>>();
        var instruments = Enum.GetValues<Instrument>();
        var diffs = Enum.GetValues<Difficulty>();

        foreach (var inst in instruments)
        {
            foreach (var diff in diffs)
            {
                try
                {
                    var chart = song.GetChart(inst, diff);
                    if (chart != null)
                    {
                        // Skip any empty tracks
                        if (chart.note_count == 0)
                        {
                            continue;
                        }
                        if (!charts.ContainsKey(inst))
                        {
                            charts[inst] = new Dictionary<Difficulty, Chart>();
                        }
                        charts[inst][diff] = chart;

                    }
                }
                catch
                {
                }
            }
        }
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
        using (var fs = File.OpenWrite(outputPath))
        {
            WriteHeader(fs, (uint)charts.Values.Count, (uint)Math.Round(song.resolution));
            WriteTempoMap(fs, song.syncTrack);
            WriteGlobalEvents(fs, song.eventsAndSections);

            foreach (var inst in charts)
            {
                Console.WriteLine($"Writing {inst.Key}");
                WriteInstrument(fs, inst.Key, inst.Value);
            }
        }
    }

}
