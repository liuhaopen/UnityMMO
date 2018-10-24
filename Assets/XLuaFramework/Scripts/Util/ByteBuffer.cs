using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

namespace XLuaFramework {
    public class ByteBuffer {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        public ByteBuffer() {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public ByteBuffer(byte[] data) {
            if (data != null) {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            } else {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }

        public void Close() {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;
        }

        public void WriteByte(byte v) {
            writer.Write(v);
        }

        public void WriteShort(short v) {
            v = Util.SwapInt16(v);
            writer.Write((short)v);
        }

        public void WriteUshort(ushort v)
        {
            v = Util.SwapUInt16(v);
            writer.Write((ushort)v);
        }

        public void WriteInt(int v)
        {
            v = Util.SwapInt32(v);
            writer.Write((int)v);
        }


        public void WriteUint(uint v)
        {
            v = Util.SwapUInt32(v);
            writer.Write((uint)v);
        }




        public void WriteLong(long v)
        {
            v = Util.SwapInt64(v);
            writer.Write((long)v);
        }


        public void WriteUlong(ulong v) {
            v = Util.SwapUInt64(v);
            writer.Write((ulong)v);
        }

        public void WriteFloat(float v) {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double v) {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string v) {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            ushort len = Util.SwapUInt16((ushort)bytes.Length);
            writer.Write(len);
            writer.Write(bytes);
//          Util.LogWarning(
        }

        public void WriteText(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write(bytes);
            //          Util.LogWarning(
        }

        public void WriteBytes(byte[] v) {
            int len = Util.SwapInt32(v.Length);
            writer.Write(len);
            writer.Write(v);
        }

        public byte ReadByte() {
            return reader.ReadByte();
        }

        public short ReadShort()
        {
            return (short)Util.SwapInt16((short)reader.ReadInt16());
        }

        public ushort ReadUshort() {
            return (ushort)Util.SwapUInt16((ushort)reader.ReadUInt16());
        }

        public int ReadInt()
        {
            return (int)Util.SwapInt32(reader.ReadInt32());
        }

        public uint ReadUint()
        {
            return (uint)Util.SwapUInt32(reader.ReadUInt32());
        }

        public long ReadLong()
        {
            return (long)Util.SwapInt64(reader.ReadInt64());
        }

        public ulong ReadUlong() {
            return (ulong)Util.SwapUInt64(reader.ReadUInt64());
        }

        public float ReadFloat() {
            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble() {
            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString() {
            ushort len = ReadUshort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes() {
            uint len = ReadUint();
            return reader.ReadBytes((int)len);
        }

        public byte[] ToBytes() {
            writer.Flush();
            return stream.ToArray();
        }

        public void Flush() {
            writer.Flush();
        }
    }
}