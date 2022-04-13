// Copyright (c) 2019-2022 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinUtils
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt24LE(byte* buff, ref int offset)
        {
            Int32 val = ReadInt24LE(buff, offset);
            offset += 3;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt24BE(byte* buff, ref int offset)
        {
            Int32 val = ReadInt24BE(buff, offset);
            offset += 3;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt24LE(byte* buff, ref int offset)
        {
            UInt32 val = ReadUInt24LE(buff, offset);
            offset += 3;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt24BE(byte* buff, ref int offset)
        {
            UInt32 val = ReadUInt24BE(buff, offset);
            offset += 3;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int64 ReadInt64LE(byte* buff, ref int offset)
        {
            Int64 val = ReadInt64LE(buff, offset);
            offset += 8;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int64 ReadInt64BE(byte* buff, ref int offset)
        {
            Int64 val = ReadInt64BE(buff, offset);
            offset += 8;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt32LE(byte* buff, ref int offset)
        {
            Int32 val = ReadInt32LE(buff, offset);
            offset += 4;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt32BE(byte* buff, ref int offset)
        {
            Int32 val = ReadInt32BE(buff, offset);
            offset += 4;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int16 ReadInt16LE(byte* buff, ref int offset)
        {
            Int16 val = ReadInt16LE(buff, offset);
            offset += 2;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int16 ReadInt16BE(byte* buff, ref int offset)
        {
            Int16 val = ReadInt16BE(buff, offset);
            offset += 2;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static sbyte ReadSByte(byte* buff, ref int offset)
        {
            sbyte val = ReadSByte(buff, offset);
            offset += 1;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt64 ReadUInt64LE(byte* buff, ref int offset)
        {
            UInt64 val = ReadUInt64LE(buff, offset);
            offset += 8;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt64 ReadUInt64BE(byte* buff, ref int offset)
        {
            UInt64 val = ReadUInt64BE(buff, offset);
            offset += 8;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt32LE(byte* buff, ref int offset)
        {
            UInt32 val = ReadUInt32LE(buff, offset);
            offset += 4;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt32BE(byte* buff, ref int offset)
        {
            UInt32 val = ReadUInt32BE(buff, offset);
            offset += 4;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt16 ReadUInt16LE(byte* buff, ref int offset)
        {
            UInt16 val = ReadUInt16LE(buff, offset);
            offset += 2;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt16 ReadUInt16BE(byte* buff, ref int offset)
        {
            UInt16 val = ReadUInt16BE(buff, offset);
            offset += 2;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static byte ReadByte(byte* buff, ref int offset)
        {
            return buff[offset++];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ReadBytes(byte* buff, ref int offset, byte[] output, UInt32 count)
        {
            offset += ReadBytes(buff, offset, output, count);
        }

        public unsafe static void ReadCountLE<T>(byte* buff, ref int offset, T[] output, UInt32 count) where T : unmanaged
        {
            offset += ReadCountLE(buff, offset, output, count);
        }
    }
}