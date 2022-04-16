
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using static MoonscraperChartEditor.Song.Song;

namespace BChart;

public static class BChartUtils
{
    /// <summary>
    /// Convert 4 length ascii char to LE uint value
    /// </summary>
    /// <param name="chars"></param>
    /// <returns>uint encoded string</returns>
    public static uint GetChunkNameToInt(ReadOnlySpan<char> chars)
    {
        if (chars.Length != 4)
            return 0;

        uint val = 0;
        for (int i = chars.Length - 1; i >= 0; --i)
        {
            byte text = (byte)chars[i];
            val <<= 8;
            val |= text;
        }
        return val;
    }

    public static string GetChunkNameFromInt(uint val)
    {

        List<byte> data = new List<byte>();

        for (int i = 0; i < 4; ++i)
        {
            byte text = (byte)(val & 0xFF);
            data.Add(text);
            val >>= 8;
        }
        return Encoding.ASCII.GetString(data.ToArray()); ;
    }

    /// <summary>
    /// Convert string to UTF-8 bytes, note must return byte array to array pool
    /// </summary>
    /// <param name="chars">ReadOnlySpan<char> string data</param>
    /// <param name="spanOut">Output location</param>
    /// <returns>ArrayPool allocated byte array utf-8 encoded string</returns>
    public static byte[] GetStringAsBytes(ReadOnlySpan<char> chars, out Span<byte> spanOut)
    {
        int estCharCount = Encoding.UTF8.GetByteCount(chars);
        byte[] bytes = ArrayPool<byte>.Shared.Rent(estCharCount);
        Span<byte> outSpan = bytes;
        Encoding.UTF8.GetBytes(chars, outSpan);
        spanOut = outSpan;
        return bytes;
    }

    /// <summary>
    /// Convert Internal instrument track id to the bchart format id
    /// </summary>
    /// <param name="instrument"></param>
    /// <returns>BChart output instrument id</returns>
    public static uint MoonInstrumentToBChart(Instrument instrument)
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
}