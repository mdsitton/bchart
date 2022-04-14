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

    public const byte MODIFIER_FORCED = 0x01;
    public const byte MODIFIER_TAP = 0x02;
    public const byte MODIFIER_DRUMS_ACCENT = 0x03;
    public const byte MODIFIER_DRUMS_GHOST = 0x04;

    public const byte DIFFICULTY_EASY = 0x00;
    public const byte DIFFICULTY_MEDIUM = 0x01;
    public const byte DIFFICULTY_HARD = 0x02;
    public const byte DIFFICULTY_EXPERT = 0x02;

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