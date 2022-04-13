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
        public unsafe static void WriteInt64BE(this Span<byte> buff, int offset, Int64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int64>());

            fixed (byte* bp = buff)
            {
                WriteInt64BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt64LE(this Span<byte> buff, int offset, Int64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int64>());

            fixed (byte* bp = buff)
            {
                WriteInt64LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32BE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int32>());

            fixed (byte* bp = buff)
            {
                WriteInt32BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt32LE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int32>());

            fixed (byte* bp = buff)
            {
                WriteInt32LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24BE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                WriteInt24BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt24LE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                WriteInt24LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16BE(this Span<byte> buff, int offset, Int16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int16>());

            fixed (byte* bp = buff)
            {
                WriteInt16BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteInt16LE(this Span<byte> buff, int offset, Int16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int16>());

            fixed (byte* bp = buff)
            {
                WriteInt16LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteSByte(this Span<byte> buff, int offset, sbyte value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<sbyte>());

            fixed (byte* bp = buff)
            {
                WriteSByte(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64BE(this Span<byte> buff, int offset, UInt64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());

            fixed (byte* bp = buff)
            {
                WriteUInt64BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt64LE(this Span<byte> buff, int offset, UInt64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());

            fixed (byte* bp = buff)
            {
                WriteUInt64LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32BE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());

            fixed (byte* bp = buff)
            {
                WriteUInt32BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt32LE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());

            fixed (byte* bp = buff)
            {
                WriteUInt32LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24BE(this Span<byte> buff, int offset, UInt32 value)
        {

            Debug.Assert(value <= 0xFFFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                WriteUInt24BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt24LE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                WriteUInt24LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16BE(this Span<byte> buff, int offset, UInt16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());

            fixed (byte* bp = buff)
            {
                WriteUInt16BE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUInt16LE(this Span<byte> buff, int offset, UInt16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());

            fixed (byte* bp = buff)
            {
                WriteUInt16LE(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteByte(this Span<byte> buff, int offset, byte value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<byte>());

            fixed (byte* bp = buff)
            {
                WriteByte(bp, offset, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int WriteBytes(this Span<byte> buff, int offset, byte[] input, UInt32 count)
        {
            Debug.Assert(buff.Length >= offset + count);

            fixed (byte* bp = buff)
            {
                return WriteBytes(bp, offset, input, count);
            }
        }

    }
}