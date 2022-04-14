// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;

static class Utility
{
    public const int NOTFOUND = -1;
    static System.Text.StringBuilder timeFormatter = new System.Text.StringBuilder(32, 32);
    static readonly char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public static string timeConvertion(float time)
    {
        timeFormatter.Remove(0, timeFormatter.Length);
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        if (timeSpan.Hours > 0)
        {
            return String.Format(@"{0:hh\:mm\:ss\.ff}", timeSpan);
        }
        else
        {
            return String.Format(@"{0:mm\:ss\.ff}", timeSpan);
        }
    }

    static void AppendDigit(System.Text.StringBuilder timeFormatter, int digit)
    {
        // Append the first digit
        int firstDigit = digit / 10;
        if (digit < 10)
            timeFormatter.Append('0');
        else
            timeFormatter.Append(numbers[firstDigit]);
        // Append second digit
        timeFormatter.Append(numbers[digit - firstDigit * 10]);
    }

    static int millisecondRounding(int value, int roundPlaces)
    {
        string sVal = value.ToString();

        if (sVal.Length > 0 && sVal[0] == '-')
            ++roundPlaces;

        if (sVal.Length > roundPlaces)
            sVal = sVal.Remove(roundPlaces);

        return int.Parse(sVal);
    }

    public static bool validateExtension(string filepath, string[] validExtensions)
    {
        // Need to check extension
        string extension = System.IO.Path.GetExtension(filepath);

        foreach (string validExtension in validExtensions)
        {
            if (extension == validExtension)
                return true;
        }
        return false;
    }

    public struct IntVector2
    {
        public int x, y;
        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

public static class floatExtension
{
    public static float Round(this float sourceFloat, int decimalPlaces)
    {
        return (float)Math.Round(sourceFloat, decimalPlaces);
    }
}
