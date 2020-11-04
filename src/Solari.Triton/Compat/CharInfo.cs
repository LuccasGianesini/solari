using System.Globalization;

namespace Solari.Triton.Compat
{
    internal static class CharInfo
    {
        public static UnicodeCategory GetUnicodeCategory(this char ch)
        {
            return char.GetUnicodeCategory(ch);
        }
    }
}
