
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
        stream.WriteUInt32LE(ev.tick);
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
        stream.WriteUInt32LE(ev.tick);
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
        stream.WriteUInt32LE(section.tick);
        stream.WriteByte(BChartConsts.EVENT_SECTION);
        stream.WriteByte((byte)outSpan.Length);
        stream.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WritePhrase(Stream stream, byte phraseType, uint tick, uint tickLength)
    {
        stream.WriteUInt32LE(tick);
        stream.WriteByte(BChartConsts.EVENT_PHRASE);
        stream.WriteByte(5);
        stream.WriteByte(phraseType);
        stream.WriteUInt32LE(tickLength);
    }

    public static void WriteTimeSignature(Stream stream, TimeSignature ts)
    {
        stream.WriteUInt32LE(ts.tick);
        stream.WriteByte(BChartConsts.EVENT_TIME_SIG);
        stream.WriteByte(2);
        stream.WriteByte((byte)ts.numerator);
        stream.WriteByte((byte)ts.denominator);
    }

    const long microsecondsPerMinute = 60000000;
    public static void WriteTempo(Stream stream, BPM bpm)
    {
        uint microSecondsPerQuarter = (uint)(microsecondsPerMinute * 1000 / bpm.value);
        stream.WriteUInt32LE(bpm.tick);
        stream.WriteByte(BChartConsts.EVENT_TEMPO);
        stream.WriteByte(4);
        stream.WriteUInt32LE(microSecondsPerQuarter);
    }


    public static void WriteNote(Stream stream, Note note)
    {
        uint eventLength = 7; // Note event is atleast 7 bytes
        byte modifierLength = 0;

        if (note.forced || note.type == Note.NoteType.Tap ||
            note.flags.HasFlag(Note.Flags.ProDrums_Accent) ||
            note.flags.HasFlag(Note.Flags.ProDrums_Ghost))
        {
            modifierLength++;
        }

        byte byteLength = (byte)(eventLength + modifierLength);

        stream.WriteUInt32LE(note.tick);
        stream.WriteByte(BChartConsts.EVENT_NOTE);
        stream.WriteByte(byteLength);
        stream.WriteByte((byte)note.rawNote);
        stream.WriteUInt32LE(note.length);
        stream.WriteByte(modifierLength);

        if (note.forced)
        {
            stream.WriteByte(BChartConsts.MODIFIER_FORCED);
        }

        if (note.type == Note.NoteType.Tap)
        {
            stream.WriteByte(BChartConsts.MODIFIER_TAP);
        }

        if (note.flags.HasFlag(Note.Flags.ProDrums_Accent))
        {
            stream.WriteByte(BChartConsts.MODIFIER_DRUMS_ACCENT);
        }

        if (note.flags.HasFlag(Note.Flags.ProDrums_Ghost))
        {
            stream.WriteByte(BChartConsts.MODIFIER_DRUMS_GHOST);
        }
        // if (note.flags.HasFlag(Note.Flags.ProDrums_Cymbal))
        // {
        //     fs.WriteByte(BChartConsts.MODIFIER_DRUMS_GHOST);
        // }
    }

    public static void WriteChunk(Stream stream, uint chunkId, Action<Stream> action)
    {
        // serialize chunk data to memory stream 
        using (var ms = new MemoryStream())
        {
            action?.Invoke(ms);
            stream.WriteUInt32LE(chunkId);
            stream.WriteInt32LE((int)ms.Length);
            stream.Write(ms.GetBuffer(), 0, (int)ms.Length);
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
        void WriteData(Stream stre)
        {
            foreach (var tempo in tempoMap)
            {
                if (tempo is TimeSignature ts)
                {
                    WriteTimeSignature(stre, ts);
                }
                else if (tempo is BPM bpm)
                {
                    WriteTempo(stre, bpm);
                }
            }
        }

        WriteChunk(stream, BChartConsts.TempoChunkName, WriteData); // SYNC

    }

    public static void WriteGlobalEvents(Stream stream, IList<Event> events)
    {
        void WriteData(Stream stre)
        {
            foreach (var ev in events)
            {
                if (ev is Section section)
                {
                    WriteSection(stre, section);
                }
                else
                {
                    WriteTextEvent(stre, ev);
                }
            }
        }

        WriteChunk(stream, BChartConsts.GlobalEventsChunkName, WriteData); // EVTS

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
            WriteDifficulty(stream, data.Key, data.Value);
        }
    }

    public static void WriteDifficulty(Stream stream, Difficulty diff, Chart chart)
    {
        void WriteData(Stream stre)
        {
            stream.WriteByte((byte)diff);
            int i = 0;
            foreach (var ev in chart.chartObjects)
            {
                ++i;
                switch (ev)
                {
                    case Note note:
                        WriteNote(stre, note);
                        break;
                    case Starpower sp:
                        WritePhrase(stre, BChartConsts.PHRASE_SOLO, sp.tick, sp.length);
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
                                            WritePhrase(stre, BChartConsts.PHRASE_SOLO, chEv.tick, chNext.tick - chEv.tick);
                                            break;
                                        }
                                    }
                                }
                                if (!foundMatch)
                                {
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
                                WriteTextEvent(stre, chEv);
                            }
                        }
                        break;
                }
            }
        }
        WriteChunk(stream, BChartConsts.DifficultyChunkName, WriteData); // DIFF
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
