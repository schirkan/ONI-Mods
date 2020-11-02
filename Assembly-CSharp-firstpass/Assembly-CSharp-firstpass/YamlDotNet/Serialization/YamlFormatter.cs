// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.YamlFormatter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace YamlDotNet.Serialization
{
  internal static class YamlFormatter
  {
    public static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo()
    {
      CurrencyDecimalSeparator = ".",
      CurrencyGroupSeparator = "_",
      CurrencyGroupSizes = new int[1]{ 3 },
      CurrencySymbol = string.Empty,
      CurrencyDecimalDigits = 99,
      NumberDecimalSeparator = ".",
      NumberGroupSeparator = "_",
      NumberGroupSizes = new int[1]{ 3 },
      NumberDecimalDigits = 99,
      NaNSymbol = ".nan",
      PositiveInfinitySymbol = ".inf",
      NegativeInfinitySymbol = "-.inf"
    };

    public static string FormatNumber(object number) => Convert.ToString(number, (IFormatProvider) YamlFormatter.NumberFormat);

    public static string FormatNumber(double number) => number.ToString("G17", (IFormatProvider) YamlFormatter.NumberFormat);

    public static string FormatNumber(float number) => number.ToString("G17", (IFormatProvider) YamlFormatter.NumberFormat);

    public static string FormatBoolean(object boolean) => !boolean.Equals((object) true) ? "false" : "true";

    public static string FormatDateTime(object dateTime) => ((DateTime) dateTime).ToString("o", (IFormatProvider) CultureInfo.InvariantCulture);

    public static string FormatTimeSpan(object timeSpan) => ((TimeSpan) timeSpan).ToString();
  }
}
