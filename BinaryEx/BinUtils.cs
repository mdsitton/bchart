﻿// Copyright (c) 2019-2022 Matthew Sitton <matthewsitton@gmail.com>
// BSD License - See LICENSE in the project root for license information.
using System;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinUtils
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 SwapEndianess(Int16 val)
        {
            return (Int16)SwapEndianess((UInt16)val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SwapEndianess(Int32 val)
        {
            return (Int32)SwapEndianess((UInt32)val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 SwapEndianess(Int64 val)
        {
            return (Int64)SwapEndianess((UInt64)val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 SwapEndianess(UInt16 val)
        {
            return (UInt16)((val << 8) | (val >> 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SwapEndianess(UInt32 val)
        {
            val = ((val << 8) & 0xFF00FF00) | ((val >> 8) & 0xFF00FF);
            return (val << 16) | (val >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 SwapEndianess(UInt64 val)
        {
            return (((UInt64)SwapEndianess((UInt32)val)) << 32) | (UInt64)SwapEndianess((UInt32)(val >> 32));
        }
    }
}
