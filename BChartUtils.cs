
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using MoonscraperChartEditor.Song;
using static MoonscraperChartEditor.Song.Note;
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
        var count = Encoding.UTF8.GetBytes(chars, outSpan);
        spanOut = outSpan.Slice(0, count);
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

    // Internal to external conversion
    private static byte MoonSixFretToBChart(GHLiveGuitarFret note)
    {
        switch (note)
        {
            case GHLiveGuitarFret.Open:
                return BChartConsts.SixFretGuitarNotes.NOTE_OPEN;
            case GHLiveGuitarFret.Black1:
                return BChartConsts.SixFretGuitarNotes.NOTE_B1;
            case GHLiveGuitarFret.Black2:
                return BChartConsts.SixFretGuitarNotes.NOTE_B2;
            case GHLiveGuitarFret.Black3:
                return BChartConsts.SixFretGuitarNotes.NOTE_B3;
            case GHLiveGuitarFret.White1:
                return BChartConsts.SixFretGuitarNotes.NOTE_W1;
            case GHLiveGuitarFret.White2:
                return BChartConsts.SixFretGuitarNotes.NOTE_W2;
            case GHLiveGuitarFret.White3:
                return BChartConsts.SixFretGuitarNotes.NOTE_W3;
            default:
                return BChartConsts.NOTE_UKN;
        }
    }


    private static byte MoonDrumsToBChart(DrumPad note)
    {
        switch (note)
        {
            case DrumPad.Kick:
                return BChartConsts.DrumNotes.NOTE_KICK;
            case DrumPad.Red:
                return BChartConsts.DrumNotes.NOTE_RED;
            case DrumPad.Yellow:
                return BChartConsts.DrumNotes.NOTE_YELLOW;
            case DrumPad.Blue:
                return BChartConsts.DrumNotes.NOTE_BLUE;
            case DrumPad.Green:
                return BChartConsts.DrumNotes.NOTE_GREEN;
            // case DrumPad.GreenFiveLane:
            //     return BChartConsts.DrumNotes.NOTE_FIVE_LANE_GREEN;
            default:
                return BChartConsts.NOTE_UKN;
        }
    }

    private static byte MoonGuitarToBChart(GuitarFret note)
    {
        switch (note)
        {
            case GuitarFret.Open:
                return BChartConsts.GuitarNotes.NOTE_OPEN;
            case GuitarFret.Green:
                return BChartConsts.GuitarNotes.NOTE_GREEN;
            case GuitarFret.Red:
                return BChartConsts.GuitarNotes.NOTE_RED;
            case GuitarFret.Yellow:
                return BChartConsts.GuitarNotes.NOTE_YELLOW;
            case GuitarFret.Blue:
                return BChartConsts.GuitarNotes.NOTE_BLUE;
            case GuitarFret.Orange:
                return BChartConsts.GuitarNotes.NOTE_ORANGE;
            default:
                return BChartConsts.NOTE_UKN;
        }
    }

    /// <summary>
    /// Convert raw MS noteid to the bchart format id
    /// </summary>
    /// <param name="instrument"></param>
    /// <returns>BChart output instrument id</returns>
    public static byte MoonNoteToBChart(Instrument instrument, Note note)
    {
        switch (instrument)
        {
            case Instrument.Guitar:
            case Instrument.GuitarCoop:
            case Instrument.Bass:
            case Instrument.Rhythm:
            case Instrument.Keys:
                return MoonGuitarToBChart((GuitarFret)note.rawNote);
            case Instrument.Drums:
                return MoonDrumsToBChart((DrumPad)note.rawNote);
            case Instrument.GHLiveGuitar:
            case Instrument.GHLiveBass:
                return MoonSixFretToBChart((GHLiveGuitarFret)note.rawNote);
            default:
                return MoonGuitarToBChart((GuitarFret)note.rawNote);
        };
    }

    // External to internal conversion

    private static GHLiveGuitarFret BChartToMoonSixFret(byte note)
    {
        switch (note)
        {
            case BChartConsts.SixFretGuitarNotes.NOTE_OPEN:
                return GHLiveGuitarFret.Open;
            case BChartConsts.SixFretGuitarNotes.NOTE_B1:
                return GHLiveGuitarFret.Black1;
            case BChartConsts.SixFretGuitarNotes.NOTE_B2:
                return GHLiveGuitarFret.Black2;
            case BChartConsts.SixFretGuitarNotes.NOTE_B3:
                return GHLiveGuitarFret.Black3;
            case BChartConsts.SixFretGuitarNotes.NOTE_W1:
                return GHLiveGuitarFret.White1;
            case BChartConsts.SixFretGuitarNotes.NOTE_W2:
                return GHLiveGuitarFret.White2;
            case BChartConsts.SixFretGuitarNotes.NOTE_W3:
                return GHLiveGuitarFret.White3;
            default:
                return GHLiveGuitarFret.Open;
        }
    }


    private static DrumPad BChartToMoonDrums(byte note)
    {
        switch (note)
        {
            case BChartConsts.DrumNotes.NOTE_KICK:
                return DrumPad.Kick;
            case BChartConsts.DrumNotes.NOTE_RED:
                return DrumPad.Red;
            case BChartConsts.DrumNotes.NOTE_YELLOW:
                return DrumPad.Yellow;
            case BChartConsts.DrumNotes.NOTE_BLUE:
                return DrumPad.Blue;
            case BChartConsts.DrumNotes.NOTE_GREEN:
                return DrumPad.Green;
            // case BChartConsts.DrumNotes.NOTE_FIVE_LANE_GREEN:
            //     return DrumPad.GreenFiveLane;
            default:
                return DrumPad.Kick;
        }
    }

    private static GuitarFret BChartToMoonGuitar(byte note)
    {
        switch (note)
        {
            case BChartConsts.GuitarNotes.NOTE_OPEN:
                return GuitarFret.Open;
            case BChartConsts.GuitarNotes.NOTE_GREEN:
                return GuitarFret.Green;
            case BChartConsts.GuitarNotes.NOTE_RED:
                return GuitarFret.Red;
            case BChartConsts.GuitarNotes.NOTE_YELLOW:
                return GuitarFret.Yellow;
            case BChartConsts.GuitarNotes.NOTE_BLUE:
                return GuitarFret.Blue;
            case BChartConsts.GuitarNotes.NOTE_ORANGE:
                return GuitarFret.Orange;
            default:
                return GuitarFret.Open;
        }
    }

    /// <summary>
    /// Convert raw BChart noteid to the internal MS id
    /// </summary>
    /// <param name="instrument"></param>
    /// <returns>MS instrument id</returns>
    public static int BChartToMoonNote(Instrument instrument, byte note)
    {
        switch (instrument)
        {
            case Instrument.Guitar:
            case Instrument.GuitarCoop:
            case Instrument.Bass:
            case Instrument.Rhythm:
            case Instrument.Keys:
                return (int)BChartToMoonGuitar(note);
            case Instrument.Drums:
                return (int)BChartToMoonDrums(note);
            case Instrument.GHLiveGuitar:
            case Instrument.GHLiveBass:
                return (int)BChartToMoonSixFret(note);
            default:
                return (int)BChartToMoonGuitar(note);
        };
    }

    private static uint SixFretNoteToMod(Note note)
    {
        uint modifiers = 0;
        if (note.forced)
        {
            modifiers |= BChartConsts.SixFretGuitarNotes.NOTE_MOD_FORCE_STRUM;
        }
        if (note.type == Note.NoteType.Tap)
        {
            modifiers |= BChartConsts.SixFretGuitarNotes.NOTE_MOD_TAP;
        }
        return modifiers;
    }

    private static uint GuitarNoteToMod(Note note)
    {
        uint modifiers = 0;
        if (note.forced)
        {
            modifiers |= BChartConsts.GuitarNotes.NOTE_MOD_FORCE_STRUM;
        }
        if (note.type == Note.NoteType.Tap)
        {
            modifiers |= BChartConsts.GuitarNotes.NOTE_MOD_TAP;
        }
        return modifiers;
    }

    private static uint DrumNoteToMod(Note note)
    {
        uint modifiers = 0;

        if (note.flags.HasFlag(Note.Flags.ProDrums_Accent))
        {
            modifiers |= BChartConsts.DrumNotes.NOTE_MOD_ACCENT;
        }
        if (note.flags.HasFlag(Note.Flags.ProDrums_Ghost))
        {
            modifiers |= BChartConsts.DrumNotes.NOTE_MOD_GHOST;
        }
        if (note.flags.HasFlag(Note.Flags.ProDrums_Cymbal))
        {
            modifiers |= BChartConsts.DrumNotes.NOTE_MOD_CYMBAL;
        }
        if (note.flags.HasFlag(Note.Flags.DoubleKick))
        {
            modifiers |= BChartConsts.DrumNotes.NOTE_MOD_KICK_2;
        }
        return modifiers;
    }

    public static uint MoonNoteToBChartMod(Instrument instrument, Note note)
    {
        switch (instrument)
        {
            case Instrument.Guitar:
            case Instrument.GuitarCoop:
            case Instrument.Bass:
            case Instrument.Rhythm:
            case Instrument.Keys:
                return GuitarNoteToMod(note);
            case Instrument.Drums:
                return DrumNoteToMod(note);
            case Instrument.GHLiveGuitar:
            case Instrument.GHLiveBass:
                return SixFretNoteToMod(note);
            default:
                return GuitarNoteToMod(note);
        };
    }


    private static void ModToSixFretNote(Note note, uint modifiers)
    {
        if ((modifiers & BChartConsts.SixFretGuitarNotes.NOTE_MOD_FORCE_STRUM) != 0)
        {
            note.flags &= Note.Flags.Forced;
        }
        if ((modifiers & BChartConsts.SixFretGuitarNotes.NOTE_MOD_TAP) != 0)
        {
            note.flags &= Note.Flags.Tap;
        }
    }

    private static void ModToGuitarNote(Note note, uint modifiers)
    {
        if ((modifiers & BChartConsts.GuitarNotes.NOTE_MOD_FORCE_STRUM) != 0)
        {
            note.flags &= Note.Flags.Forced;
        }
        if ((modifiers & BChartConsts.GuitarNotes.NOTE_MOD_TAP) != 0)
        {
            note.flags &= Note.Flags.Tap;
        }
    }

    private static void ModToDrumNote(Note note, uint modifiers)
    {
        if ((modifiers & BChartConsts.DrumNotes.NOTE_MOD_ACCENT) != 0)
        {
            note.flags &= Note.Flags.ProDrums_Accent;
        }
        if ((modifiers & BChartConsts.DrumNotes.NOTE_MOD_GHOST) != 0)
        {
            note.flags &= Note.Flags.ProDrums_Ghost;
        }
        if ((modifiers & BChartConsts.DrumNotes.NOTE_MOD_CYMBAL) != 0)
        {
            note.flags &= Note.Flags.ProDrums_Cymbal;
        }
        if ((modifiers & BChartConsts.DrumNotes.NOTE_MOD_KICK_2) != 0)
        {
            note.flags &= Note.Flags.DoubleKick;
        }
    }

    public static void ApplyBChartModToNote(Instrument instrument, Note note, uint modifierData)
    {
        switch (instrument)
        {
            case Instrument.Guitar:
            case Instrument.GuitarCoop:
            case Instrument.Bass:
            case Instrument.Rhythm:
            case Instrument.Keys:
                ModToGuitarNote(note, modifierData);
                break;
            case Instrument.Drums:
                ModToDrumNote(note, modifierData);
                break;
            case Instrument.GHLiveGuitar:
            case Instrument.GHLiveBass:
                ModToSixFretNote(note, modifierData);
                break;
            default:
                ModToGuitarNote(note, modifierData);
                break;
        };
    }

}