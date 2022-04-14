
using System;

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
}