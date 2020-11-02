// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Utilities.StringExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Text.RegularExpressions;

namespace YamlDotNet.Serialization.Utilities
{
  internal static class StringExtensions
  {
    private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
    {
      string str1 = Regex.Replace(str, "([_\\-])(?<char>[a-z])", (MatchEvaluator) (match => match.Groups["char"].Value.ToUpperInvariant()), RegexOptions.IgnoreCase);
      return firstLetterTransform(str1[0]).ToString() + str1.Substring(1);
    }

    public static string ToCamelCase(this string str) => StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToLowerInvariant));

    public static string ToPascalCase(this string str) => StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToUpperInvariant));

    public static string FromCamelCase(this string str, string separator)
    {
      str = char.ToLower(str[0]).ToString() + str.Substring(1);
      str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", (MatchEvaluator) (match => separator + match.Groups["char"].Value.ToLowerInvariant()));
      return str;
    }
  }
}
