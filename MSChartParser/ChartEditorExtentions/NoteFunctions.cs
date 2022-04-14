// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

//#define OPEN_NOTES_BLOCK_EXTENDED_SUSTAINS

using System;
using System.Collections;
using System.Collections.Generic;
using MoonscraperChartEditor.Song;

public static class NoteFunctions
{

    public static bool AllowedToBeDoubleKick(Note note, Song.Difficulty difficulty)
    {
        return note.IsOpenNote() && difficulty == Song.Difficulty.Expert;
    }

}
