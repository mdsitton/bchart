
using System;
using System.IO;
using MoonscraperChartEditor.Song;
using MoonscraperChartEditor.Song.IO;
using BinaryEx;
using System.Collections.Generic;
using static MoonscraperChartEditor.Song.Song;
using System.Text;
using System.Buffers;

public static class BChartWriter
{

    public static void WriteHeader(FileStream fs, uint instrumentCount, uint resolution)
    {
        // header
        const int headerLength = 6;
        fs.WriteUInt32LE(BChartConsts.HeaderChunkName); // BCHF
        fs.WriteUInt32LE(headerLength);
        fs.WriteUInt16LE(1); // version 1
        fs.WriteUInt16LE((ushort)resolution);
        fs.WriteUInt16LE((ushort)instrumentCount);
    }

    public static byte[] GetStringAsBytes(ReadOnlySpan<char> chars, out Span<byte> spanOut)
    {
        int estCharCount = Encoding.UTF8.GetByteCount(chars);
        byte[] bytes = ArrayPool<byte>.Shared.Rent(estCharCount);
        Span<byte> outSpan = bytes;
        Encoding.UTF8.GetBytes(chars, outSpan);
        spanOut = outSpan;
        return bytes;
    }

    public static void WriteTextEvent(FileStream fs, ChartEvent ev)
    {
        byte[] bytes = GetStringAsBytes(ev.eventName, out var outSpan);

        // limit string size to ushort max value
        if (outSpan.Length > ushort.MaxValue)
        {
            outSpan = outSpan.Slice(0, ushort.MaxValue);
        }
        fs.WriteUInt32LE(ev.tick);
        fs.WriteByte(BChartConsts.EVENT_TEXT);
        fs.WriteUInt16LE((ushort)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteTextEvent(FileStream fs, Event ev)
    {
        byte[] bytes = GetStringAsBytes(ev.title, out var outSpan);

        // limit string size to ushort max value
        if (outSpan.Length > ushort.MaxValue)
        {
            outSpan = outSpan.Slice(0, ushort.MaxValue);
        }
        fs.WriteUInt32LE(ev.tick);
        fs.WriteByte(BChartConsts.EVENT_TEXT);
        fs.WriteUInt16LE((ushort)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteSection(FileStream fs, Section section)
    {
        byte[] bytes = GetStringAsBytes(section.title, out var outSpan);

        // limit string size to ushort max value
        if (outSpan.Length > ushort.MaxValue)
        {
            outSpan = outSpan.Slice(0, ushort.MaxValue);
        }
        fs.WriteUInt32LE(section.tick);
        fs.WriteByte(BChartConsts.EVENT_SECTION);
        fs.WriteUInt16LE((ushort)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteGlobalEvents(FileStream fs, IList<Event> events)
    {
        fs.WriteUInt32LE(BChartConsts.GlobalEventsChunkName); // EVTS
        foreach (var ev in events)
        {
            if (ev is Section section)
            {
                WriteSection(fs, section);
            }
            else
            {
                WriteTextEvent(fs, ev);
            }
        }
    }

    public static void WritePhrase(FileStream fs, byte phraseType, uint tick, uint tickLength)
    {
        fs.WriteUInt32LE(tick);
        fs.WriteByte(BChartConsts.EVENT_PHRASE);
        fs.WriteUInt16LE(5);
        fs.WriteByte(phraseType);
        fs.WriteUInt32LE(tickLength);
    }

    public static void WriteNote(FileStream fs, Note note)
    {
        uint eventLength = 7; // Note event is atleast 7 bytes
        byte modifierLength = 0;

        if (note.forced)
        {
            modifierLength++;
        }

        if (note.type == Note.NoteType.Tap)
        {
            modifierLength++;
        }

        ushort byteLength = (ushort)(eventLength + modifierLength);

        fs.WriteUInt32LE(note.tick);
        fs.WriteByte(BChartConsts.EVENT_NOTE);
        fs.WriteUInt16LE(byteLength);
        fs.WriteUInt16LE((ushort)note.rawNote);
        fs.WriteUInt32LE(note.length);
        fs.WriteByte(modifierLength);

        if (note.forced)
        {
            fs.WriteByte(BChartConsts.MODIFIER_FORCED);
        }

        if (note.type == Note.NoteType.Tap)
        {
            fs.WriteByte(BChartConsts.MODIFIER_TAP);
        }

        if (note.flags.HasFlag(Note.Flags.ProDrums_Accent))
        {
            fs.WriteByte(BChartConsts.MODIFIER_DRUMS_ACCENT);
        }

        if (note.flags.HasFlag(Note.Flags.ProDrums_Ghost))
        {
            fs.WriteByte(BChartConsts.MODIFIER_DRUMS_GHOST);
        }
        // if (note.flags.HasFlag(Note.Flags.ProDrums_Cymbal))
        // {
        //     fs.WriteByte(BChartConsts.MODIFIER_DRUMS_GHOST);
        // }
    }

    public static void WriteDifficulty(FileStream fs, Difficulty diff, Chart chart)
    {
        fs.WriteUInt32LE(BChartConsts.DifficultyChunkName); // DIFF
        int i = 0;
        foreach (var ev in chart.chartObjects)
        {
            ++i;
            switch (ev)
            {
                case Note note:
                    WriteNote(fs, note);
                    break;
                case Starpower sp:
                    WritePhrase(fs, BChartConsts.PHRASE_SOLO, sp.tick, sp.length);
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
                                        WritePhrase(fs, BChartConsts.PHRASE_SOLO, chEv.tick, chNext.tick - chEv.tick);
                                        break;
                                    }
                                }
                            }
                            if (!foundMatch)
                            {
                                // Make length match until the last chart event
                                var last = chart.chartObjects[chart.chartObjects.Count - 1];
                                WritePhrase(fs, BChartConsts.PHRASE_SOLO, chEv.tick, last.tick - chEv.tick);
                            }
                        }
                        else if (chEv.eventName == MidIOHelper.SoloEndEventText)
                        {
                            continue;
                        }
                        else
                        {
                            WriteTextEvent(fs, chEv);
                        }
                    }
                    break;
            }
        }
    }


    public static uint MoonInstrumentToOutput(Instrument instrument)
    {
        return instrument switch
        {
            Instrument.Guitar => BChartConsts.INSTRUMENT_GUITAR,
            Instrument.GuitarCoop => BChartConsts.INSTRUMENT_COOP,
            Instrument.Bass => BChartConsts.INSTRUMENT_BASS,
            Instrument.Rhythm => BChartConsts.INSTRUMENT_RHYTHM,
            Instrument.Keys => BChartConsts.INSTRUMENT_KEYS,
            Instrument.Drums => BChartConsts.INSTRUMENT_DRUMS,
            Instrument.GHLiveGuitar => BChartConsts.INSTRUMENT_GUITAR_SIX,
            Instrument.GHLiveBass => BChartConsts.INSTRUMENT_BASS_SIX,
            _ => BChartConsts.INSTRUMENT_UKN,
        };
    }

    public static void WriteInstrument(FileStream fs, Instrument inst, Dictionary<Difficulty, Chart> diffs)
    {
        fs.WriteUInt32LE(BChartConsts.InstrumentChunkName); // INST
        fs.WriteUInt32LE(MoonInstrumentToOutput(inst));
        fs.WriteUInt32LE((uint)diffs.Count);
        foreach (var data in diffs)
        {
            WriteDifficulty(fs, data.Key, data.Value);
        }
    }

    public static void WriteTimeSignature(FileStream fs, TimeSignature ts)
    {
        fs.WriteUInt32LE(ts.tick);
        fs.WriteByte(BChartConsts.EVENT_TIME_SIG);
        fs.WriteUInt16LE(2);
        fs.WriteByte((byte)ts.numerator);
        fs.WriteByte((byte)ts.denominator);
    }

    const long microsecondsPerMinute = 60000000;
    public static void WriteTempo(FileStream fs, BPM bpm)
    {
        uint microSecondsPerQuarter = (uint)(microsecondsPerMinute * 1000 / bpm.value);
        fs.WriteUInt32LE(bpm.tick);
        fs.WriteByte(BChartConsts.EVENT_TEMPO);
        fs.WriteUInt16LE(4);
        fs.WriteUInt32LE(microSecondsPerQuarter);
    }

    public static void WriteTempoMap(FileStream fs, IList<SyncTrack> tempoMap)
    {
        fs.WriteUInt32LE(BChartConsts.TempoChunkName); // SYNC
        foreach (var tempo in tempoMap)
        {
            if (tempo is TimeSignature ts)
            {
                WriteTimeSignature(fs, ts);
            }
            else if (tempo is BPM bpm)
            {
                WriteTempo(fs, bpm);
            }
        }
    }

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
        using (var fs = File.OpenWrite(outputPath))
        {
            WriteHeader(fs, (uint)charts.Values.Count, (uint)Math.Round(song.resolution));
            WriteTempoMap(fs, song.syncTrack);
            WriteGlobalEvents(fs, song.eventsAndSections);

            foreach (var inst in charts)
            {
                // Console.WriteLine($"Writing {inst.Key}");
                WriteInstrument(fs, inst.Key, inst.Value);
            }
        }
    }

}
