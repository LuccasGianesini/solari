using System;
using static System.Decimal;

namespace Solari.Sol.Extensions
{
  public static class DecimalExtensions
  {
    /// <summary>
    /// Truncate a decimal variable.
    /// </summary>
    /// <param name="decimal">The decimal value</param>
    /// <param name="places">Number of decimal places</param>
    /// <returns>Truncated decimal</returns>
    public static decimal TruncateDecimal(this decimal @decimal, int places)
    {
      var multiplier = (int)Math.Pow(10, places);

      return Truncate(@decimal * multiplier) / multiplier;
    }
  }
}