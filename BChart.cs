
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
    public const byte EVENT_TEMPO = 0x01;
    public const byte EVENT_TIME_SIG = 0x02;
    public const byte EVENT_TEXT = 0x03;
    public const byte EVENT_SECTION = 0x04;
    public const byte EVENT_PHRASE = 0x05;
    public const byte EVENT_NOTE = 0x06;

    public static void WriteHeader(FileStream fs, uint instrumentCount, uint resolution)
    {
        // header
        fs.WriteUInt16LE(0xBCAF);
        fs.WriteUInt16LE(1); // version 1
        fs.WriteUInt16LE((ushort)resolution);
        fs.WriteUInt16LE((ushort)instrumentCount);
    }

    public static void WriteTextEvent(FileStream fs, ChartEvent ev)
    {
        ReadOnlySpan<char> textSpan = ev.eventName;

        int estCharCount = Encoding.UTF8.GetByteCount(textSpan);
        byte[] bytes = ArrayPool<byte>.Shared.Rent(estCharCount);
        Span<byte> outSpan = bytes;
        Encoding.UTF8.GetBytes(textSpan, outSpan);

        fs.WriteUInt32LE(ev.tick);
        fs.WriteByte(EVENT_TEXT);
        fs.WriteUInt32LE((uint)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteTextEvent(FileStream fs, Event ev)
    {
        ReadOnlySpan<char> textSpan = ev.title;

        int estCharCount = Encoding.UTF8.GetByteCount(textSpan);
        byte[] bytes = ArrayPool<byte>.Shared.Rent(estCharCount);
        Span<byte> outSpan = bytes;
        Encoding.UTF8.GetBytes(textSpan, outSpan);

        fs.WriteUInt32LE(ev.tick);
        fs.WriteByte(EVENT_TEXT);
        fs.WriteUInt32LE((uint)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteSection(FileStream fs, Section section)
    {
        ReadOnlySpan<char> textSpan = section.title;

        int estCharCount = Encoding.UTF8.GetByteCount(textSpan);
        byte[] bytes = ArrayPool<byte>.Shared.Rent(estCharCount);
        Span<byte> outSpan = bytes;
        Encoding.UTF8.GetBytes(textSpan, outSpan);

        fs.WriteUInt32LE(section.tick);
        fs.WriteByte(EVENT_SECTION);
        fs.WriteUInt32LE((uint)outSpan.Length);
        fs.Write(outSpan);

        ArrayPool<byte>.Shared.Return(bytes);
    }

    public static void WriteGlobalEvents(FileStream fs, IList<Event> events)
    {
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

    public const byte PHRASE_STARPOWER = 0x01;
    public const byte PHRASE_SOLO = 0x02;

    public static void WritePhrase(FileStream fs, byte phraseType, uint tick, uint tickLength)
    {
        fs.WriteUInt32LE(tick);
        fs.WriteByte(EVENT_PHRASE);
        fs.WriteUInt32LE(5);
        fs.WriteByte(phraseType);
        fs.WriteUInt32LE(tickLength);
    }

    public const byte MODIFIER_FORCED = 0x01;
    public const byte MODIFIER_TAP = 0x02;

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

        fs.WriteUInt32LE(note.tick);
        fs.WriteByte(EVENT_NOTE);
        fs.WriteUInt32LE(eventLength + modifierLength);
        fs.WriteUInt16LE((ushort)note.rawNote);
        fs.WriteUInt32LE(note.length);
        fs.WriteByte(modifierLength);

        if (note.forced)
        {
            fs.WriteByte(MODIFIER_FORCED);
        }

        if (note.type == Note.NoteType.Tap)
        {
            fs.WriteByte(MODIFIER_TAP);
        }
    }

    public const byte DIFFICULTY_EASY = 0x00;
    public const byte DIFFICULTY_MEDIUM = 0x01;
    public const byte DIFFICULTY_HARD = 0x02;
    public const byte DIFFICULTY_EXPERT = 0x02;

    public static void WriteDifficulty(FileStream fs, Difficulty diff, Chart chart)
    {
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
                    WritePhrase(fs, PHRASE_SOLO, sp.tick, sp.length);
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
                                        WritePhrase(fs, PHRASE_SOLO, chEv.tick, chNext.tick - chEv.tick);
                                        break;
                                    }
                                }
                            }
                            if (!foundMatch)
                            {
                                // Make length match until the last chart event
                                var last = chart.chartObjects[chart.chartObjects.Count - 1];
                                WritePhrase(fs, PHRASE_SOLO, chEv.tick, last.tick - chEv.tick);
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

    public const uint INSTRUMENT_GUITAR = 0;
    public const uint INSTRUMENT_GUITAR_SIX = 1;
    public const uint INSTRUMENT_BASS = 2;
    public const uint INSTRUMENT_BASS_SIX = 3;
    public const uint INSTRUMENT_RHYTHM = 4;
    public const uint INSTRUMENT_COOP = 5;
    public const uint INSTRUMENT_KEYS = 6;
    public const uint INSTRUMENT_DRUMS = 7;
    public const uint INSTRUMENT_VOCALS = 8;
    public const uint INSTRUMENT_UKN = 0xFFFF;

    public static uint MoonInstrumentToOutput(Instrument instrument)
    {
        return instrument switch
        {
            Instrument.Guitar => INSTRUMENT_GUITAR,
            Instrument.GuitarCoop => INSTRUMENT_COOP,
            Instrument.Bass => INSTRUMENT_BASS,
            Instrument.Rhythm => INSTRUMENT_RHYTHM,
            Instrument.Keys => INSTRUMENT_KEYS,
            Instrument.Drums => INSTRUMENT_DRUMS,
            Instrument.GHLiveGuitar => INSTRUMENT_GUITAR_SIX,
            Instrument.GHLiveBass => INSTRUMENT_BASS_SIX,
            _ => INSTRUMENT_UKN,
        };
    }

    public static void WriteInstrument(FileStream fs, Instrument inst, Dictionary<Difficulty, Chart> diffs)
    {
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
        fs.WriteByte(EVENT_TIME_SIG);
        fs.WriteUInt32LE(2);
        fs.WriteByte((byte)ts.numerator);
        fs.WriteByte((byte)ts.denominator);
    }

    const long microsecondsPerMinute = 60000000;
    public static void WriteTempo(FileStream fs, BPM bpm)
    {
        uint microSecondsPerQuarter = (uint)(microsecondsPerMinute * 1000 / bpm.value);
        fs.WriteUInt32LE(bpm.tick);
        fs.WriteByte(EVENT_TEMPO);
        fs.WriteUInt32LE(4);
        fs.WriteUInt32LE(microSecondsPerQuarter);
    }

    public static void WriteTempoMap(FileStream fs, IList<SyncTrack> tempoMap)
    {
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
                        Console.WriteLine(inst);

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
                Console.WriteLine(inst.Key);
                WriteInstrument(fs, inst.Key, inst.Value);
            }
        }
    }

}
