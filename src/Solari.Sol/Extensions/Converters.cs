using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Text;

namespace Solari.Sol.Extensions
{
    public static class Converters
    {
        public static bool IsValidPrimitive(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                case TypeCode.Object:
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Get Bytes from a single boolean object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this bool value) { return new[] {value ? (byte) 1 : (byte) 0}; }

        /// <summary>
        ///     Get Bytes from an array of boolean objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this bool[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (bool[]) object cannot be null.");
            var seed = new byte[0];
            return value.Aggregate(seed, (current, bl) => bl.GetBytes());
        }

        /// <summary>
        ///     Get Bytes from a single byte object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this byte value) { return new[] {value}; }

        /// <summary>
        ///     Get Bytes from a sbyte short object
        ///     (Converters.cs)
        /// </summary>
        [SecuritySafeCritical]
        public static unsafe byte[] GetBytes(this sbyte value)
        {
            var numArray = new byte[1];
            fixed (byte* ptr = numArray)
            {
                *(sbyte*) ptr = value;
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of sbyte objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this sbyte[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (sbyte[]) object cannot be null.");
            var numArray = new byte[value.Length];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single short object
        ///     (Converters.cs)
        /// </summary>
        [SecuritySafeCritical]
        public static unsafe byte[] GetBytes(this short value)
        {
            var numArray = new byte[2];
            fixed (byte* ptr = numArray)
            {
                *(short*) ptr = value;
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of short objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this short[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (short[]) object cannot be null.");
            var numArray = new byte[value.Length * 2];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single unsigned short object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this ushort value) { return ((short) value).GetBytes(); }

        /// <summary>
        ///     Get Bytes from an array of unsigned short objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this ushort[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (ushort[]) object cannot be null.");
            var numArray = new byte[value.Length * 2];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single character object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this char value) { return ((short) value).GetBytes(); }

        /// <summary>
        ///     Get Bytes from an array of character objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this char[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (char[]) object cannot be null.");
            var numArray = new byte[value.Length * 2];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single integer object
        ///     (Converters.cs)
        /// </summary>
        [SecuritySafeCritical]
        public static unsafe byte[] GetBytes(this int value)
        {
            var numArray = new byte[4];
            fixed (byte* ptr = numArray)
            {
                *(int*) ptr = value;
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single integer object with index and count
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this int value, int sIndex, int count)
        {
            if (count > 4)
                throw new Exception("Size cannot exceed 4 bytes.");
            return value.GetBytes().SubArray(sIndex, count);
        }

        /// <summary>
        ///     Get Bytes from an array of integer objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this int[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (int[]) object cannot be null.");
            var numArray = new byte[value.Length * 4];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single unsigned integer object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this uint value) { return ((int) value).GetBytes(); }

        public static byte[] GetBytes(this uint value, int sIndex = 0, int count = 4)
        {
            if (count > 4)
                throw new Exception("Size cannot exceed 4 bytes.");
            if (sIndex <= 0) throw new ArgumentOutOfRangeException(nameof(sIndex));
            return value.GetBytes().SubArray(sIndex, count);
        }

        /// <summary>
        ///     Get Bytes from an array of unsigned integer objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this uint[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (uint[]) object cannot be null.");
            var numArray = new byte[value.Length * 4];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single long object
        ///     (Converters.cs)
        /// </summary>
        public static unsafe byte[] GetBytes(this long value)
        {
            var numArray = new byte[8];
            fixed (byte* ptr = numArray)
            {
                *(long*) ptr = value;
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of long objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this long[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (long[]) object cannot be null.");
            var numArray = new byte[value.Length * 8];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of long objects with index and count
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this long value, int sIndex = 0, int count = 8)
        {
            if (count > 8)
                throw new Exception("Size cannot exceed 8 bytes.");
            return value.GetBytes().SubArray(sIndex, count);
        }

        /// <summary>
        ///     Get Bytes from a single unsigned long object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this ulong value) { return ((long) value).GetBytes(); }

        public static byte[] GetBytes(this ulong value, int sIndex = 0, int count = 8)
        {
            if (count > 8)
                throw new Exception("Size cannot exceed 8 bytes.");
            return ((long) value).GetBytes().SubArray(sIndex, count);
        }

        /// <summary>
        ///     Get Bytes from an array of unsigned long objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this ulong[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (ulong[]) object cannot be null.");
            var numArray = new byte[value.Length * 8];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single float object
        ///     (Converters.cs)
        /// </summary>
        [SecuritySafeCritical]
        public static unsafe byte[] GetBytes(this float value) { return (*(int*) &value).GetBytes(); }

        /// <summary>
        ///     Get Bytes from an array of float objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this float[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (float[]) object cannot be null.");
            var numArray = new byte[value.Length * 4];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single double object
        ///     (Converters.cs)
        /// </summary>
        public static unsafe byte[] GetBytes(this double value) { return (*(long*) &value).GetBytes(); }

        /// <summary>
        ///     Get Bytes from an array of double objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this double[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (double[]) object cannot be null.");
            var numArray = new byte[value.Length * 8];
            Buffer.BlockCopy(value, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single decimal object
        ///     (Converters.cs)
        /// </summary>
        public static unsafe byte[] GetBytes(this decimal value)
        {
            var array = new byte[16];
            fixed (byte* bp = array)
            {
                *(decimal*) bp = value;
            }

            return array;
        }

        /// <summary>
        ///     Get Bytes from a single DateTime object
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this DateTime value) { return value.Ticks.GetBytes(); }

        public static byte[] GetBytes(this DateTime[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (DateTime[]) object cannot be null.");
            var sodt = 0;
            unsafe
            {
                sodt = sizeof(DateTime);
            }

            var numArray = new byte[value.Length * sodt];
            for (var i = 0; i < value.Length; i++)
            {
                byte[] dba = value[i].GetBytes();
                Buffer.BlockCopy(dba, 0, numArray, i * sodt, sodt);
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of decimal objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this decimal[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (decimal[]) object cannot be null.");
            var numArray = new byte[value.Length * 16];
            for (var i = 0; i < value.Length; i++)
            {
                byte[] dba = value[i].GetBytes();
                Buffer.BlockCopy(dba, 0, numArray, i * 16, 16);
            }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from a single string object using a specified Encoding.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this string value, Encoding enc = null)
        {
            if (value == null)
                throw new Exception("GetBytes (string) object cannot be null.");
            if (enc == null)
                return Encoding.ASCII.GetBytes(value);
            switch (enc)
            {
                case ASCIIEncoding AsciiEncoding:
                {
                    return Encoding.ASCII.GetBytes(value);
                }
                case UnicodeEncoding UnicodeEncoding:
                {
                    byte[] ba = Encoding.Unicode.GetBytes(value);
                    var pre = new byte[] {0xff, 0xfe};
                    var ra = new byte[ba.Length + 2];
                    Array.Copy(pre, 0, ra, 0, 2);
                    Array.Copy(ba, 0, ra, 2, ba.Length);
                    return ra;
                }
                case UTF32Encoding Utf32Encoding:
                {
                    byte[] ba = Encoding.UTF32.GetBytes(value);
                    var pre = new byte[] {0xff, 0xfe, 0, 0};
                    var ra = new byte[ba.Length + 4];
                    Array.Copy(pre, 0, ra, 0, 4);
                    Array.Copy(ba, 0, ra, 4, ba.Length);
                    return ra;
                }
                case UTF7Encoding Utf7Encoding:
                {
                    byte[] ba = Encoding.UTF7.GetBytes(value);
                    var pre = new byte[] {0x2b, 0x2f, 0x76};
                    var ra = new byte[ba.Length + 3];
                    Array.Copy(pre, 0, ra, 0, 3);
                    Array.Copy(ba, 0, ra, 3, ba.Length);
                    return ra;
                }
                case UTF8Encoding Utf8Encoding:
                {
                    byte[] ba = Encoding.UTF8.GetBytes(value);
                    var pre = new byte[] {0xef, 0xbb, 0xbf};
                    var ra = new byte[ba.Length + 3];
                    Array.Copy(pre, 0, ra, 0, 3);
                    Array.Copy(ba, 0, ra, 3, ba.Length);
                    return ra;
                }
                default:
                    return Encoding.ASCII.GetBytes(value);
            }
        }

        /// <summary>
        ///     Get Bytes from a array of string objects.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this string[] value, Encoding enc = null)
        {
            if (value == null)
                throw new Exception("GetBytes (string[]) object cannot be null.");
            var numArray = new byte[value.Where(ss => ss != null).Sum(ss => ss.Length)];
            var dstOffset = 0;
            foreach (string str in value)
                if (str != null)
                {
                    Buffer.BlockCopy(str.GetBytes(enc), 0, numArray, dstOffset, str.Length);
                    dstOffset += str.Length;
                }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of string objects.??
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes1(this string[] value, Encoding enc = null)
        {
            if (value == null)
                throw new Exception("GetBytes (string[]) object cannot be null.");
            byte[] tb = value[0].GetBytes(enc);
            int cs = tb.Length / value[0].Length;
            var numArray = new byte[value.Where(ss => ss != null).Sum(ss => ss.Length) * cs];
            var dstOffset = 0;
            foreach (string str in value)
                if (str != null)
                {
                    byte[] buf = str.GetBytes(enc);
                    Buffer.BlockCopy(buf, 0, numArray, dstOffset, buf.Length);
                    dstOffset += buf.Length;
                }

            return numArray;
        }

        /// <summary>
        ///     Get Bytes from an array of secure string objects
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytes(this SecureString[] value)
        {
            if (value == null)
                throw new Exception("GetBytes (SecureString[]) object cannot be null.");
            List<byte[]> source = (from secureString in value where secureString != null select secureString.GetBytes()).ToList();

            var seed = new byte[source.Sum(ba => ba.Length)];
            return source.Aggregate(seed, (current, ba) => ba);
        }

        /// <summary>
        ///     Get Bytes from a single secure string object using a specified encoding
        ///     (Converters.cs)
        /// </summary>
        public static unsafe byte[] GetBytes(this SecureString value, Encoding enc = null)
        {
            if (value == null)
                throw new Exception("GetBytes (SecureString) object cannot be null.");
            if (enc == null)
                enc = Encoding.Default;
            int maxLength = enc.GetMaxByteCount(value.Length);
            IntPtr allocHGlobal = IntPtr.Zero;
            IntPtr str = IntPtr.Zero;
            try
            {
                allocHGlobal = Marshal.AllocHGlobal(maxLength);
                str = Marshal.SecureStringToBSTR(value);
                var chars = (char*) str.ToPointer();
                var pointer = (byte*) allocHGlobal.ToPointer();
                int len = enc.GetBytes(chars, value.Length, pointer, maxLength);
                var bytes = new byte[len];
                for (var i = 0; i < len; ++i)
                {
                    bytes[i] = *pointer;
                    pointer++;
                }

                return bytes;
            }
            finally
            {
                if (allocHGlobal != IntPtr.Zero)
                    Marshal.FreeHGlobal(allocHGlobal);
                if (str != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(str);
            }
        }

        /// <summary>
        ///     Get Bytes from a object array with an optional object type specified.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytesObjectGen(this object[] value, Type type = null) { return value.SelectMany(o => GetBytesObject(o, type)).ToArray(); }

        /// <summary>
        ///     Get Bytes from a single object with an optional object type specified.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytesObject(object obj, Type type = null)
        {
            type = type == null ? obj.GetType() : type;
            switch (type.Name)
            {
                case "Byte":
                    return !type.IsArray ? new byte[1] {(byte) obj} : (byte[]) obj;
                case "Boolean":
                    return !type.IsArray ? ((bool) obj).GetBytes() : ((bool[]) obj).GetBytes();
                case "SByte":
                    return !type.IsArray ? ((sbyte) obj).GetBytes() : ((sbyte[]) obj).GetBytes();
                case "Char":
                    return !type.IsArray ? ((char) obj).GetBytes() : ((char[]) obj).GetBytes();
                case "Int16":
                    return !type.IsArray ? ((short) obj).GetBytes() : ((short[]) obj).GetBytes();
                case "UInt16":
                    return !type.IsArray ? ((ushort) obj).GetBytes() : ((ushort[]) obj).GetBytes();
                case "Int32":
                    return !type.IsArray ? ((int) obj).GetBytes() : ((int[]) obj).GetBytes();
                case "UInt32":
                    return !type.IsArray ? ((uint) obj).GetBytes() : ((uint[]) obj).GetBytes();
                case "Int64":
                    return !type.IsArray ? ((long) obj).GetBytes() : ((long[]) obj).GetBytes();
                case "UInt64":
                    return !type.IsArray ? ((ulong) obj).GetBytes() : ((ulong[]) obj).GetBytes();
                case "Single":
                    return !type.IsArray ? ((float) obj).GetBytes() : ((float[]) obj).GetBytes();
                case "Double":
                    return !type.IsArray ? ((double) obj).GetBytes() : ((double[]) obj).GetBytes();
                case "String":
                    return !type.IsArray ? ((string) obj).GetBytes() : ((string[]) obj).GetBytes();
                case "Decimal":
                    return !type.IsArray ? ((decimal) obj).GetBytes() : ((decimal[]) obj).GetBytes();
                case "DateTime":
                    return !type.IsArray ? ((DateTime) obj).GetBytes() : ((DateTime[]) obj).GetBytes();
            }

            if (type == typeof(BigInteger))
                return ((BigInteger) obj).ToByteArray();
            if (type == typeof(SecureString))
                return ((SecureString) obj).GetBytes();
            if (type == typeof(SecureString[]))
                return ((SecureString[]) obj).GetBytes();
            if (!type.IsSerializable)
                throw new Exception("Error: Object is not Serializable.");
            var fs = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(fs, obj);
            return fs.GetBuffer().SubArray(0, (int) fs.Length);
        }

        /// <summary>
        ///     Gets list of byte arrays from a list of objects of type T.
        ///     (Converters.cs)
        /// </summary>
        public static List<byte[]> GetBytesObject<T>(this List<T> value) { return value.Select(o => o.GetBytesObject()).ToList(); }

        /// <summary>
        ///     Get Bytes from a single object with an optional object type specified.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytesObject(this object value)
        {
            Type type = value.GetType();
            if (!type.IsSerializable)
                throw new Exception("Error: Object is not Serializable.");
            using (var fs = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, value);
                return fs.ToArray().SubArray(0, (int) fs.Length);
            }
        }

        /// <summary>
        ///     Gets a single object of type T from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static T ToObject<T>(this byte[] value)
        {
            if (value == null)
                throw new Exception("value cannot be null.");
            using (var stream = new MemoryStream(value))
            {
                var formatter = new BinaryFormatter();
                var result = (T) formatter.Deserialize(stream);
                return result;
            }
        }

        /// <summary>
        ///     Gets an array of objects of type T from a list of byte arrays.
        ///     (Converters.cs)
        /// </summary>
        public static T[] ToObject<T>(this List<byte[]> value)
        {
            if (value == null)
                throw new Exception("value cannot be null.");
            if (value.Count == 0)
                throw new Exception("value is empty.");
            return value.Select(o => o.ToObject<T>()).ToArray();
        }

        /// <summary>
        ///     Converts a hex string to a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] HexToBytes(this string hex)
        {
            if (hex.Length % 2 != 0)
                throw new Exception($"Incomplete Hex string {hex}");
            if (!hex.ContainsOnly("0123456789abcdefABCDEFxX"))
                throw new Exception("Error: hexNumber cannot contain characters other than 0-9,a-f,A-F, or xX");
            hex = hex.ToUpper();
            if (hex.IndexOf("0X", StringComparison.OrdinalIgnoreCase) != -1)
                hex = hex.Substring(2);
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        /// <summary>
        ///     Converts a byte array to a hex string.
        ///     (Converters.cs)
        /// </summary>
        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        ///     Converts a hex string to an unsigned long.
        ///     (Converters.cs)
        /// </summary>
        public static unsafe ulong FromHexStringTo(this string value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            if (value.Length == 0)
                return 0;
            byte[] ba = value.HexToBytes();
            ba = ba.Invert();
            if (ba.Length > 8)
                throw new Exception("Maximum bit width is limited to 64 bits.");
            int len = ba.Length;
            switch (len)
            {
                case 1:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr;
                    }
                case 2:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8);
                    }
                case 3:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16);
                    }
                case 4:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16) | ((ulong) ptr[3] << 24);
                    }
                case 5:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16) | ((ulong) ptr[3] << 24) | ((ulong) ptr[4] << 32);
                    }
                case 6:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16) | ((ulong) ptr[3] << 24) | ((ptr[4] | ((ulong) ptr[5] << 8)) << 32);
                    }
                case 7:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16) | ((ulong) ptr[3] << 24) |
                               ((ptr[4] | ((ulong) ptr[5] << 8) | ((ulong) ptr[6] << 16)) << 32);
                    }
                case 8:
                    fixed (byte* ptr = &ba[0])
                    {
                        return *ptr | ((ulong) ptr[1] << 8) | ((ulong) ptr[2] << 16) | ((ulong) ptr[3] << 24) |
                               ((ptr[4] | ((ulong) ptr[5] << 8) | ((ulong) ptr[6] << 16) | ((ulong) ptr[7] << 24)) << 32);
                    }
                default:
                    return 0;
            }
        }

        /// <summary>
        ///     Converts a byte array to a hex string.
        ///     (Converters.cs)
        /// </summary>
        public static string ToBinaryString(this byte[] bytes)
        {
            int len = bytes.Length;
            var sb = new StringBuilder();
            for (var i = 0; i < len; i++)
                sb.Append(Convert.ToString(bytes[i], 2).PadLeft(8, '0'));
            return sb.ToString();
        }

        /// <summary>
        ///     Converts a binary string to an unsigned long.
        ///     (Converters.cs)
        /// </summary>
        public static ulong FromBinaryStringTo(this string value)
        {
            char[] reversed = value.Reverse().ToArray();
            var num = 0ul;
            for (var p = 0; p < reversed.Count(); p++)
            {
                if (reversed[p] != '1')
                    continue;
                num += (ulong) Math.Pow(2, p);
            }

            return num;
        }

        /// <summary>
        ///     Converts a binary string to a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static byte[] GetBytesFromBinaryString(this string value)
        {
            if (!value.ContainsOnly("01"))
                throw new Exception($"Error: Binary string can only contains 0's and 1's. Value:{value}");
            int len = value.Length;
            var bLen = (int) Math.Ceiling(len / 8d);
            var bytes = new byte[bLen];
            var size = 8;
            for (var i = 1; i <= bLen; i++)
            {
                int idx = len - 8 * i;
                if (idx < 0)
                {
                    size = 8 + idx;
                    idx = 0;
                }

                bytes[bLen - i] = Convert.ToByte(value.Substring(idx, size), 2);
            }

            return bytes;
        }

        /// <summary>
        ///     Converts a byte array to a octal string.
        ///     (Converters.cs)
        /// </summary>
        public static string ToOctalString(this byte[] value)
        {
            value = value.Invert();
            int index = value.Length - 1;
            var base8 = new StringBuilder();
            int rem = value.Length % 3;
            if (rem == 0)
                rem = 3;
            var @base = 0;
            while (rem != 0)
            {
                @base <<= 8;
                @base += value[index--];
                rem--;
            }

            base8.Append(Convert.ToString(@base, 8));
            while (index >= 0)
            {
                @base = (value[index] << 16) + (value[index - 1] << 8) + value[index - 2];
                base8.Append(Convert.ToString(@base, 8).PadLeft(8, '0'));
                index -= 3;
            }

            return base8.ToString();
        }

        /// <summary>
        ///     Returns a Boolean value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static bool ToBool(this byte[] value, int pos = 0) { return BitConverter.ToBoolean(value, pos); }

        /// <summary>
        ///     Returns a Character value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static char ToChar(this byte[] value, int pos = 0) { return BitConverter.ToChar(value, pos); }

        public static unsafe byte ToByte(this byte[] value, int pos = 0)
        {
            byte bv;
            fixed (byte* bp = value)
            {
                byte* bpp = bp + pos;
                bv = *bpp;
                return bv;
            }
        }

        public static unsafe sbyte ToSByte(this byte[] value, int pos = 0)
        {
            fixed (byte* bp = value)
            {
                byte* ptr = bp + pos;
                if (pos % 2 == 0)
                    return *(sbyte*) ptr;
                return (sbyte) *ptr;
            }
        }

        /// <summary>
        ///     Returns a Short value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static short ToShort(this byte[] value, int pos = 0) { return BitConverter.ToInt16(value, pos); }

        /// <summary>
        ///     Returns a Unsigned Short value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static ushort ToUShort(this byte[] value, int pos = 0) { return BitConverter.ToUInt16(value, pos); }

        /// <summary>
        ///     Returns a Integer value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static int ToInt(this byte[] value, int pos = 0) { return BitConverter.ToInt32(value, pos); }

        /// <summary>
        ///     Returns a Unsigned Integer value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static uint ToUInt(this byte[] value, int pos = 0) { return BitConverter.ToUInt32(value, pos); }

        /// <summary>
        ///     Returns a Long value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static long ToLong(this byte[] value, int pos = 0) { return BitConverter.ToInt64(value, pos); }

        /// <summary>
        ///     Returns a Unsigned Long value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static ulong ToULong(this byte[] value, int pos = 0) { return BitConverter.ToUInt64(value, pos); }

        /// <summary>
        ///     Returns a Float value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static float ToFloat(this byte[] value, int pos = 0) { return BitConverter.ToSingle(value, pos); }

        /// <summary>
        ///     Returns a Double value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static double ToDouble(this byte[] value, int pos = 0) { return BitConverter.ToDouble(value, pos); }

        /// <summary>
        ///     Returns a Decimal value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static unsafe decimal ToDecimal(this byte[] value, int pos = 0)
        {
            decimal dv;
            fixed (byte* bp = value)
            {
                byte* bpp = bp + pos;
                dv = *(decimal*) bpp;
            }

            return dv;
        }

        /// <summary>
        ///     Returns a String value converted from the byte at a specified position.
        ///     (Converters.cs)
        /// </summary>
        public static string ToString(this byte[] value, int pos = 0) { return BitConverter.ToString(value, pos); }

        /// <summary>
        ///     Returns a Secure String value converted from the byte array.
        ///     (Converters.cs)
        /// </summary>
        public static SecureString ToSecureString(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var securestring = new SecureString();
            char[] asCharA = value.ToCharArray();
            foreach (char c in asCharA)
                securestring.AppendChar(c);
            return securestring;
        }

        /// <summary>
        ///     Returns a Boolean array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static bool[] ToBooleanArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new bool[value.Length];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Character array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static char[] ToCharArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new char[value.Length];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        public static byte[] ToByteArray(this byte[] value, int index = 0, int length = -1)
        {
            if (length == -1)
                length = value.Length - index;
            var ba = new byte[length];
            Buffer.BlockCopy(value, index, ba, 0, length);
            return ba;
        }

        /// <summary>
        ///     Returns a SByte array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static sbyte[] ToSByteArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new sbyte[value.Length];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Short array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static short[] ToShortArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new short[value.Length / 2];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Unsigned Short array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static ushort[] ToUShortArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new ushort[value.Length / 2];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Integer array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static int[] ToIntArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new int[value.Length / 4];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Unsigned Integer array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static uint[] ToUIntArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new uint[value.Length / 4];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Long array converted from the byte array.
        ///     (Converters.cs)
        /// </summary>
        public static long[] ToLongArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new long[value.Length / 8];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Unsigned Long array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static ulong[] ToULongArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            var arr = new ulong[value.Length / 8];
            Buffer.BlockCopy(value, 0, arr, 0, value.Length);
            return arr;
        }

        /// <summary>
        ///     Returns a Float array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static float[] ToFloatArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            if (value.Length % 4 != 0)
                throw new Exception("Byte Object length must be a multiple of 4");
            var arr = new List<float>();
            for (var i = 0; i < value.Length; i += 4)
            {
                byte[] t = {value[i], value[i + 1], value[i + 2], value[i + 3]};
                arr.Add(t.ToFloat());
            }

            return arr.ToArray();
        }

        /// <summary>
        ///     Returns a Double array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static double[] ToDoubleArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            if (value.Length % 8 != 0)
                throw new Exception("Byte Object length must be a multiple of 8");
            var arr = new List<double>();
            for (var i = 0; i < value.Length; i += 8)
            {
                byte[] t = {value[i], value[i + 1], value[i + 2], value[i + 3], value[i + 4], value[i + 5], value[i + 6], value[i + 7]};
                arr.Add(t.ToDouble());
            }

            return arr.ToArray();
        }

        /// <summary>
        ///     Returns a decimal array converted from a byte array.
        ///     (Converters.cs)
        /// </summary>
        public static decimal[] ToDecimalArray(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            if (value.Length % 16 != 0)
                throw new Exception("Byte Object length must be a multiple of 16");
            var arr = new List<decimal>();
            for (var i = 0; i < value.Length; i += 16)
            {
                byte[] t =
                {
                    value[i], value[i + 1], value[i + 2], value[i + 3], value[i + 4], value[i + 5], value[i + 6], value[i + 7], value[i + 8], value[i + 9],
                    value[i + 10], value[i + 11], value[i + 12], value[i + 13], value[i + 14], value[i + 15]
                };
                arr.Add(t.ToDecimal());
            }

            return arr.ToArray();
        }

        /// <summary>
        ///     Returns a Single String converted from the byte array.
        ///     (Converters.cs)
        /// </summary>
        public static string ToSingleString(this byte[] value)
        {
            if (value == null)
                throw new Exception("Value cannot be null.");
            Encoding enc = GetEncoding(value);
            return enc switch
                   {
                       // ReSharper disable once InconsistentNaming
                       ASCIIEncoding AsciiEncoding => Encoding.ASCII.GetString(value),
                       // ReSharper disable once InconsistentNaming
                       UnicodeEncoding UnicodeEncoding => Encoding.Unicode.GetString(value),
                       // ReSharper disable once InconsistentNaming
                       UTF32Encoding Utf32Encoding => Encoding.UTF32.GetString(value),
                       // ReSharper disable once InconsistentNaming
                       UTF7Encoding Utf7Encoding => Encoding.UTF7.GetString(value),
                       // ReSharper disable once InconsistentNaming
                       UTF8Encoding Utf8Encoding => Encoding.UTF8.GetString(value),
                       _                         => Encoding.ASCII.GetString(value)
                   };
        }

        private static Encoding GetEncoding(IReadOnlyList<byte> data)
        {
            if (data == null)
                throw new Exception("Array cannot be null.");
            if (data.Count < 2)
                return Encoding.Default;
            if (data[0] == 0xff && data[1] == 0xfe)
                return Encoding.Unicode;
            if (data[0] == 0xfe && data[1] == 0xff)
                return Encoding.BigEndianUnicode;
            if (data.Count < 3)
                return Encoding.Default;
            if (data[0] == 0xef && data[1] == 0xbb && data[2] == 0xbf)
                return Encoding.UTF8;
            if (data[0] == 0x2b && data[1] == 0x2f && data[2] == 0x76)
                return Encoding.UTF7;
            if (data.Count < 4)
                return Encoding.Default;
            if (data[0] == 0xff && data[1] == 0xfe && data[2] == 0 && data[3] == 0)
                return Encoding.UTF32;
            return Encoding.Default;
        }
    }
}