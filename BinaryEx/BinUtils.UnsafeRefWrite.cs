// Copyright (c) 2019-2022 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt64BE(byte* buff, ref int offset, Int64 value)
        {
            WriteInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt64LE(byte* buff, ref int offset, Int64 value)
        {
            WriteInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32BE(byte* buff, ref int offset, Int32 value)
        {
            WriteInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32LE(byte* buff, ref int offset, Int32 value)
        {
            WriteInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24BE(byte* buff, ref int offset, Int32 value)
        {
            WriteInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24LE(byte* buff, ref int offset, Int32 value)
        {
            WriteInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16BE(byte* buff, ref int offset, Int16 value)
        {
            WriteInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16LE(byte* buff, ref int offset, Int16 value)
        {
            WriteInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteSByte(byte* buff, ref int offset, sbyte value)
        {
            WriteSByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64BE(byte* buff, ref int offset, UInt64 value)
        {
            WriteUInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64LE(byte* buff, ref int offset, UInt64 value)
        {
            WriteUInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32BE(byte* buff, ref int offset, UInt32 value)
        {
            WriteUInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32LE(byte* buff, ref int offset, UInt32 value)
        {
            WriteUInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24BE(byte* buff, ref int offset, UInt32 value)
        {
            WriteUInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24LE(byte* buff, ref int offset, UInt32 value)
        {
            WriteUInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16BE(byte* buff, ref int offset, UInt16 value)
        {
            WriteUInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16LE(byte* buff, ref int offset, UInt16 value)
        {
            WriteUInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteByte(byte* buff, ref int offset, byte value)
        {
            WriteByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteBytes(byte* buff, ref int offset, byte[] input, UInt32 count)
        {
            offset += WriteBytes(buff, offset, input, count);
        }

    }
}