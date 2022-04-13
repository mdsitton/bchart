// Copyright (c) 2019-2022 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinStream
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ReadUInt24LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadUInt24LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ReadUInt24BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadUInt24BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ReadInt64LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadInt64LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ReadInt64BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadInt64BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ReadInt32LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadInt32LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ReadInt32BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadInt32BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ReadInt16LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadInt16LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ReadInt16BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadInt16BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByte(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 1);

            return scratch.ReadSByte(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ReadUInt64LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadUInt64LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ReadUInt64BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadUInt64LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ReadUInt32LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadUInt32LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ReadUInt32BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadUInt32BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ReadUInt16LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadUInt16LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ReadUInt16BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadUInt16BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadByte(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 1);

            return scratch.ReadByte(0);
        }
    }
}