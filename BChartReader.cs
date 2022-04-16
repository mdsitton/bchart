using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinaryEx;
using MoonscraperChartEditor.Song;
using MoonscraperChartEditor.Song.IO;
using static MoonscraperChartEditor.Song.Song;

namespace BChart;

public static class BChartReader
{
    public static string ReadTextEventData(Span<byte> data)
    {
        return Encoding.UTF8.GetString(data);
    }

    public static uint ReadPhraseLength(Span<byte> data)
    {
        return data.ReadUInt32LE(0);
    }

    public static Note ReadNoteData(Span<byte> data, uint tick)
    {
        int pos = 0;
        byte noteValue = data.ReadByte(ref pos);
        uint tickLength = data.ReadUInt32LE(ref pos);
        byte modifierCount = data.ReadByte(ref pos);

        if (modifierCount > pos + data.Length)
        {
            modifierCount = (byte)(data.Length - pos);
        }

        Note note = new Note(tick, noteValue, tickLength);

        for (int i = 0; i < modifierCount; ++i)
        {
            byte modifier = data.ReadByte(ref pos);

            switch (modifier)
            {
                case BChartConsts.MODIFIER_FORCED:
                    note.forced = true;
                    break;
                case BChartConsts.MODIFIER_TAP:
                    note.flags = Note.Flags.Tap;
                    break;
                case BChartConsts.MODIFIER_DRUMS_ACCENT:
                    note.flags |= Note.Flags.ProDrums_Accent;
                    break;
                case BChartConsts.MODIFIER_DRUMS_GHOST:
                    note.flags |= Note.Flags.ProDrums_Ghost;
                    break;
            }
        }
        return note;
    }

    public static void ReadDifficulty(Span<byte> data, Song song, Instrument inst)
    {
        int pos = 0;
        int eventCount = data.ReadInt32LE(ref pos);
        Difficulty diff = (Difficulty)data.ReadByte(ref pos);
        Console.WriteLine($"{inst} {diff}");
        var chart = song.GetChart(inst, diff);
        for (int i = 0; i < eventCount; ++i)
        {
            uint tickPos = data.ReadUInt32LE(ref pos);
            byte eventType = data.ReadByte(ref pos);
            byte eventLength = data.ReadByte(ref pos);

            Span<byte> dataSpan = data.Slice(pos, eventLength);
            pos += eventLength;

            switch (eventType)
            {
                case BChartConsts.EVENT_NOTE:
                    Note note = ReadNoteData(dataSpan, tickPos);
                    chart.Add(note, false);
                    // Console.WriteLine(note);
                    break;
                case BChartConsts.EVENT_PHRASE:
                    {
                        var type = dataSpan.ReadByte(0);

                        if (type == BChartConsts.PHRASE_STARPOWER)
                        {

                            uint length = ReadPhraseLength(dataSpan);
                            chart.Add(new Starpower(tickPos, length), false);
                        }
                        else if (type == BChartConsts.PHRASE_SOLO)
                        {

                            uint length = ReadPhraseLength(dataSpan);
                            var start = new ChartEvent(tickPos, MidIOHelper.SoloEventText);
                            var end = new ChartEvent(tickPos + length, MidIOHelper.SoloEndEventText);
                            chart.Add(start, false);
                            chart.Add(end, false);
                        }
                        break;
                    }
                case BChartConsts.EVENT_TEXT:
                    string txt = ReadTextEventData(dataSpan);
                    chart.Add(new ChartEvent(tickPos, txt), false);
                    break;
                default:
                    // Skip any unknown event types
                    break;
            }
        }
        chart.UpdateCache();
        Console.WriteLine(chart.note_count);
    }

    public static (Instrument inst, byte count) ReadInstrument(Span<byte> data)
    {
        int pos = 0;
        return ((Instrument)data.ReadUInt32LE(ref pos), data.ReadByte(ref pos));
    }

    private class BChartChunk
    {
        public uint ChunkID;
        public ReadOnlyMemory<byte> data;
    }

    public static Song ReadBChart(string path)
    {
        Song song = new Song();
        string directory = Path.GetDirectoryName(path);

        MsceIOHelper.DiscoverAudio(directory, song);

        var chunks = new List<BChartChunk>(10);

        byte[] fileData = File.ReadAllBytes(path);
        Span<byte> data = fileData;
        int pos = 0;
        Instrument currentInstrument = Instrument.Unrecognised;
        byte diffs = 0;

        while (pos < data.Length)
        {
            // Console.WriteLine(pos);
            var chunkID = data.ReadUInt32LE(ref pos);
            var chunkLength = data.ReadInt32LE(ref pos);
            // Console.WriteLine(BChartUtils.GetChunkNameFromInt(chunkID));
            // Console.WriteLine(chunkLength);
            // Console.WriteLine(pos);
            var chunkData = data.Slice(pos, chunkLength);
            pos += chunkLength;
            // Console.WriteLine(pos);

            if (chunkID == BChartConsts.HeaderChunkName)
            {

            }
            else if (chunkID == BChartConsts.TempoChunkName)
            {

            }
            else if (chunkID == BChartConsts.GlobalEventsChunkName)
            {

            }
            else if (chunkID == BChartConsts.InstrumentChunkName)
            {
                (currentInstrument, diffs) = ReadInstrument(chunkData);
            }
            else if (chunkID == BChartConsts.DifficultyChunkName)
            {
                ReadDifficulty(chunkData, song, currentInstrument);
            }

        }





        return song;
    }
}
