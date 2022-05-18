using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BChart;

public static class BChartConsts
{
    public const byte EVENT_TEMPO = 0x01;
    public const byte EVENT_TIME_SIG = 0x02;
    public const byte EVENT_TEXT = 0x03;
    public const byte EVENT_SECTION = 0x04;
    public const byte EVENT_PHRASE = 0x05;
    public const byte EVENT_NOTE = 0x06;

    public const byte PHRASE_STARPOWER = 0x01;
    public const byte PHRASE_SOLO = 0x02;
    public const byte PHRASE_LYRICS_LINE = 0x03;

    public const byte DIFFICULTY_EASY = 0x00;
    public const byte DIFFICULTY_MEDIUM = 0x01;
    public const byte DIFFICULTY_HARD = 0x02;
    public const byte DIFFICULTY_EXPERT = 0x03;

    public const byte NOTE_UKN = 0xFF;

    public static class GuitarNotes
    {
        public const byte NOTE_OPEN = 0x00;
        public const byte NOTE_GREEN = 0x01;
        public const byte NOTE_RED = 0x02;
        public const byte NOTE_YELLOW = 0x03;
        public const byte NOTE_BLUE = 0x04;
        public const byte NOTE_ORANGE = 0x05;

        public const uint NOTE_MOD_TOGGLE_FORCED = 1;
        public const uint NOTE_MOD_FORCE_HOPO = 2;
        public const uint NOTE_MOD_FORCE_STRUM = 4;
        public const uint NOTE_MOD_TAP = 8;
    }

    public static class SixFretGuitarNotes
    {
        public const byte NOTE_OPEN = 0x00;
        public const byte NOTE_B1 = 0x01;
        public const byte NOTE_B2 = 0x02;
        public const byte NOTE_B3 = 0x03;
        public const byte NOTE_W1 = 0x04;
        public const byte NOTE_W2 = 0x05;
        public const byte NOTE_W3 = 0x06;

        public const uint NOTE_MOD_TOGGLE_FORCED = 1;
        public const uint NOTE_MOD_FORCE_HOPO = 2;
        public const uint NOTE_MOD_FORCE_STRUM = 4;
        public const uint NOTE_MOD_TAP = 8;
    }

    public static class DrumNotes
    {
        public const byte NOTE_KICK = 0x00;
        public const byte NOTE_RED = 0x01;
        public const byte NOTE_YELLOW = 0x02;
        public const byte NOTE_BLUE = 0x03;
        public const byte NOTE_GREEN = 0x04;
        public const byte NOTE_FIVE_LANE_GREEN = 0x05;

        public const uint NOTE_MOD_ACCENT = 1;
        public const uint NOTE_MOD_GHOST = 2;
        public const uint NOTE_MOD_CYMBAL = 4;
        public const uint NOTE_MOD_KICK_2 = 8;
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

    public static uint HeaderChunkName = BChartUtils.GetChunkNameToInt("BCHF");
    public static uint TempoChunkName = BChartUtils.GetChunkNameToInt("SYNC");
    public static uint GlobalEventsChunkName = BChartUtils.GetChunkNameToInt("EVTS");
    public static uint InstrumentChunkName = BChartUtils.GetChunkNameToInt("INST");
    public static uint DifficultyChunkName = BChartUtils.GetChunkNameToInt("DIFF");

}