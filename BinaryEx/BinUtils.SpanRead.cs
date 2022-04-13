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
        public unsafe static UInt32 ReadUInt24LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                return ReadUInt24LE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt24BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                return ReadUInt24BE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt24LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                return ReadInt24LE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt24BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + 3);

            fixed (byte* bp = buff)
            {
                return ReadInt24BE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int64 ReadInt64LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int64>());

            fixed (byte* bp = buff)
            {
                return ReadInt64LE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int64 ReadInt64BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int64>());

            fixed (byte* bp = buff)
            {
                return ReadInt64BE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt32LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int32>());

            fixed (byte* bp = buff)
            {
                return ReadInt32LE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 ReadInt32BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int32>());

            fixed (byte* bp = buff)
            {
                return ReadInt32BE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int16 ReadInt16LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int16>());

            fixed (byte* bp = buff)
            {
                return ReadInt16LE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int16 ReadInt16BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<Int16>());

            fixed (byte* bp = buff)
            {
                return ReadInt16BE(bp, offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static sbyte ReadSByte(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<sbyte>());

            fixed (byte* bp = buff)
            {
                return ReadSByte(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt64 ReadUInt64LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());

            fixed (byte* bp = buff)
            {
                return ReadUInt64LE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt64 ReadUInt64BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());

            fixed (byte* bp = buff)
            {
                return ReadUInt64BE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt32LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());

            fixed (byte* bp = buff)
            {
                return ReadUInt32LE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt32 ReadUInt32BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());

            fixed (byte* bp = buff)
            {
                return ReadUInt32LE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt16 ReadUInt16LE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());

            fixed (byte* bp = buff)
            {
                return ReadUInt16LE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static UInt16 ReadUInt16BE(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());

            fixed (byte* bp = buff)
            {
                return ReadUInt16BE(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static byte ReadByte(this Span<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<byte>());

            fixed (byte* bp = buff)
            {
                return ReadByte(bp, offset);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ReadBytes(this Span<byte> buff, int offset, byte[] output, UInt32 count)
        {
            Debug.Assert(buff.Length >= offset + count);

            fixed (byte* bp = buff)
            {
                return ReadBytes(bp, offset, output, count);
            };
        }

        public unsafe static int ReadCountLE<T>(this Span<byte> buff, int offset, T[] output, UInt32 count) where T : unmanaged
        {
            Debug.Assert(buff.Length >= offset + (Unsafe.SizeOf<T>() * count));

            fixed (byte* bp = buff)
            {
                return ReadCountLE(bp, offset, output, count);
            };
        }
    }
}