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
        public unsafe static void WriteInt64BE(byte* buff, int offset, Int64 value)
        {
            WriteUInt64BE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt64LE(byte* buff, int offset, Int64 value)
        {
            WriteUInt64LE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32BE(byte* buff, int offset, Int32 value)
        {
            WriteUInt32BE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32LE(byte* buff, int offset, Int32 value)
        {
            WriteUInt32LE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24BE(byte* buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            WriteUInt24BE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24LE(byte* buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            WriteUInt24LE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16BE(byte* buff, int offset, Int16 value)
        {
            WriteUInt16BE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16LE(byte* buff, int offset, Int16 value)
        {
            WriteUInt16LE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteSByte(byte* buff, int offset, sbyte value)
        {
            WriteByte(buff, offset, (byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64BE(byte* buff, int offset, UInt64 value)
        {
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64LE(byte* buff, int offset, UInt64 value)
        {
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32BE(byte* buff, int offset, UInt32 value)
        {
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32LE(byte* buff, int offset, UInt32 value)
        {
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24BE(byte* buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);

            value = SwapEndianess(value);

            byte* src = (byte*)Unsafe.AsPointer(ref value) + 1;
            byte* destStart = buff + offset;

            Unsafe.CopyBlockUnaligned(destStart, src, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24LE(byte* buff, int offset, UInt32 value)
        {
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref Unsafe.As<UInt32, byte>(ref value), 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16BE(byte* buff, int offset, UInt16 value)
        {
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16LE(byte* buff, int offset, UInt16 value)
        {
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteByte(byte* buff, int offset, byte value)
        {
            Unsafe.WriteUnaligned<byte>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int WriteBytes(byte* buff, int offset, byte[] input, UInt32 count)
        {
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref input[0], count);
            return (int)count;
        }

    }
}