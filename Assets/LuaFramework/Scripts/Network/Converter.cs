/*
*  Copyright (c) 2008 Jonathan Wagner
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;

namespace LuaFramework {
    public class Converter {
        public static Int32 GetBigEndian(Int32 value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder(value);
            } else {
                return value;
            }
        }

        public static UInt16 GetBigEndian(UInt16 value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder(value);
            } else {
                return value;
            }
        }

        public static UInt32 GetBigEndian(UInt32 value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder(value);
            } else {
                return value;
            }
        }

        public static Int64 GetBigEndian(Int64 value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder(value);
            } else {
                return value;
            }
        }

        public static Double GetBigEndian(Double value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder(value);
            } else {
                return value;
            }
        }

        public static float GetBigEndian(float value) {
            if (BitConverter.IsLittleEndian) {
                return swapByteOrder((int)value);

            } else {
                return value;
            }
        }

        public static Int32 GetLittleEndian(Int32 value) {
            if (BitConverter.IsLittleEndian) {
                return value;
            } else {
                return swapByteOrder(value);
            }
        }

        public static UInt32 GetLittleEndian(UInt32 value) {
            if (BitConverter.IsLittleEndian) {
                return value;
            } else {
                return swapByteOrder(value);
            }
        }

        public static UInt16 GetLittleEndian(UInt16 value) {
            if (BitConverter.IsLittleEndian) {
                return value;
            } else {
                return swapByteOrder(value);
            }
        }

        public static Double GetLittleEndian(Double value) {
            if (BitConverter.IsLittleEndian) {
                return value;
            } else {
                return swapByteOrder(value);
            }
        }

        private static Int32 swapByteOrder(Int32 value) {
            Int32 swap = (Int32)((0x000000FF) & (value >> 24)
                | (0x0000FF00) & (value >> 8)
                | (0x00FF0000) & (value << 8)
                | (0xFF000000) & (value << 24));
            return swap;
        }

        private static Int64 swapByteOrder(Int64 value) {
            UInt64 uvalue = (UInt64)value;
            UInt64 swap = ((0x00000000000000FF) & (uvalue >> 56)
            | (0x000000000000FF00) & (uvalue >> 40)
            | (0x0000000000FF0000) & (uvalue >> 24)
            | (0x00000000FF000000) & (uvalue >> 8)
            | (0x000000FF00000000) & (uvalue << 8)
            | (0x0000FF0000000000) & (uvalue << 24)
            | (0x00FF000000000000) & (uvalue << 40)
            | (0xFF00000000000000) & (uvalue << 56));

            return (Int64)swap;
        }

        private static UInt16 swapByteOrder(UInt16 value) {
            return (UInt16)((0x00FF & (value >> 8))
                | (0xFF00 & (value << 8)));
        }

        private static UInt32 swapByteOrder(UInt32 value) {
            UInt32 swap = ((0x000000FF) & (value >> 24)
                | (0x0000FF00) & (value >> 8)
                | (0x00FF0000) & (value << 8)
                | (0xFF000000) & (value << 24));
            return swap;
        }

        private static Double swapByteOrder(Double value) {
            Byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer, 0, buffer.Length);
            return BitConverter.ToDouble(buffer, 0);
        }
    }
}