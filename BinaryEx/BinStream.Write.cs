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
        [ThreadStatic] static byte[] scratchData;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] EnsureScratch()
        {
            if (scratchData == null)
            {
                scratchData = new byte[8];
            }

            return scratchData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt24BE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt24BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt24LE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt24LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64BE(this Stream data, Int64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt64BE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64LE(this Stream data, Int64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt64LE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32BE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt32BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32LE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt32LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16BE(this Stream data, Int16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt16BE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16LE(this Stream data, Int16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt16LE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSByte(this Stream data, sbyte value)
        {
            Debug.Assert(data.CanWrite);
            data.WriteByte((byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24BE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt24BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt24LE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt24LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64BE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64BE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64LE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64LE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32BE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32LE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16BE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16BE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16LE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16LE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByte(this Stream data, byte value)
        {
            Debug.Assert(data.CanWrite);
            data.WriteByte(value);
        }
    }
}