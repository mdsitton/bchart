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
        Difficulty diff = (Difficulty)data.ReadByte(ref pos);
        Console.WriteLine($"{inst} {diff}");
        var chart = song.GetChart(inst, diff);
        for (int i = 0; i < 100; ++i)
        {
            uint tickPos = data.ReadUInt32LE(ref pos);
            byte eventType = data.ReadByte(ref pos);
            byte eventLength = data.ReadByte(ref pos);

            Span<byte> dataSpan = data.Slice(pos, eventLength);
            eventLength += eventLength;

            switch (eventType)
            {
                case BChartConsts.EVENT_NOTE:
                    Note note = ReadNoteData(dataSpan, tickPos);
                    chart.Add(note);
                    break;
                case BChartConsts.PHRASE_STARPOWER:
                    {

                        uint length = ReadPhraseLength(dataSpan);
                        chart.Add(new Starpower(tickPos, length));
                        break;
                    }
                case BChartConsts.PHRASE_SOLO:
                    {
                        uint length = ReadPhraseLength(dataSpan);
                        var start = new ChartEvent(tickPos, MidIOHelper.SoloEventText);
                        var end = new ChartEvent(tickPos + length, MidIOHelper.SoloEndEventText);
                        chart.Add(start);
                        chart.Add(end);
                        break;
                    }
                case BChartConsts.EVENT_TEXT:
                    string txt = ReadTextEventData(dataSpan);
                    chart.Add(new ChartEvent(tickPos, txt));
                    break;
                default:
                    // Skip any unknown event types
                    pos += eventLength;
                    break;
            }
        }
        chart.UpdateCache();
    }

    public static (Instrument inst, byte count) ReadInstrument(Span<byte> data)
    {
        int pos = 0;
        return ((Instrument)data.ReadUInt32LE(ref pos), data.ReadByte(ref pos));
    }

    private class BChartChunk
    {
        public uint ChunkID;
        public byte[] data;
    }

    public static Song ReadBChart(string path)
    {
        Song song = new Song();
        string directory = Path.GetDirectoryName(path);

        MsceIOHelper.DiscoverAudio(directory, song);

        var chunks = new List<BChartChunk>();

        using (FileStream fs = File.OpenRead(path))
        {
            while (true)
            {
                var chunkID = fs.ReadUInt32LE();
                var chunkLength = fs.ReadInt32LE();
                var bytes = new byte[chunkLength];
                int i = fs.Read(bytes, 0, chunkLength);

                var remain = fs.Length - fs.Position;
                chunks.Add(new BChartChunk
                {
                    ChunkID = chunkID,
                    data = bytes
                });
                if (remain == 0)
                {
                    break;
                }
            }

        }

        Instrument currentInstrument = Instrument.Unrecognised;
        byte diffs = 0;

        foreach (var chunk in chunks)
        {
            if (chunk.ChunkID == BChartConsts.HeaderChunkName)
            {

            }
            else if (chunk.ChunkID == BChartConsts.TempoChunkName)
            {

            }
            else if (chunk.ChunkID == BChartConsts.GlobalEventsChunkName)
            {

            }
            else if (chunk.ChunkID == BChartConsts.InstrumentChunkName)
            {
                (currentInstrument, diffs) = ReadInstrument(chunk.data);
            }
            else if (chunk.ChunkID == BChartConsts.DifficultyChunkName)
            {
                ReadDifficulty(chunk.data, song, currentInstrument);
            }
        }


        return song;
    }
}
