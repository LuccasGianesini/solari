using System;

namespace Solari.Sol.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        ///     Copies a total or portion of a source array of type T starting at offset
        ///     and copying count bytes.  (ArrayHelper.cs)
        /// </summary>
        public static T[] SubArray<T>(this T[] array, int offset, int count)
        {
            if (array == null)
                throw new Exception("The array can be null.");
            if (count - offset > array.Length)
                throw new Exception($"Count {count} + Offset {offset} must be less than the length of the array {array.Length}");
            var result = new T[count];
            Array.Copy(array, offset, result, 0, count);
            return result;
        }

        /// <summary>
        ///     Copies the source array of type T into the target array.
        ///     (ArrayHelper.cs)
        /// </summary>
        public static void Copy<T>(this T[] array, T[] array1)
        {
            if (array == null || array1 == null)
                throw new Exception("Neither the source or target array can be null.");

            if (array == null || array1 == null)
                throw new Exception("Neither the source or target array can be null.");

            int len = array.Length;
            int len1 = array1.Length;
            if (len > len1)
                throw new Exception("The target array must be the same length.");
            Array.Copy(array, 0, array1, 0, len);
        }

        /// <summary>
        ///     Inverts the order of the array.
        ///     (ArrayHelper.cs)
        /// </summary>
        public static T[] Invert<T>(this T[] array)
        {
            if (array == null)
                throw new Exception("The array can be null.");
            int len = array.Length;
            var t = new T[len];
            for (int i = len - 1, j = 0; i >= 0; t[j++] = array[i--]) ;
            t.CopyTo(array, 0);
            return t;
        }
    }
}