// Decompiled with JetBrains decompiler
// Type: StringFormatter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public static class StringFormatter
{
  private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> cachedReplacements = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
  private static Dictionary<string, Dictionary<string, string>> cachedCombines = new Dictionary<string, Dictionary<string, string>>();
  private static Dictionary<HashedString, string> cachedToUppers = new Dictionary<HashedString, string>();

  public static string Replace(string format, string token, string replacement)
  {
    Dictionary<string, Dictionary<string, string>> dictionary1 = (Dictionary<string, Dictionary<string, string>>) null;
    if (!StringFormatter.cachedReplacements.TryGetValue(format, out dictionary1))
    {
      dictionary1 = new Dictionary<string, Dictionary<string, string>>();
      StringFormatter.cachedReplacements[format] = dictionary1;
    }
    Dictionary<string, string> dictionary2 = (Dictionary<string, string>) null;
    if (!dictionary1.TryGetValue(token, out dictionary2))
    {
      dictionary2 = new Dictionary<string, string>();
      dictionary1[token] = dictionary2;
    }
    string str = (string) null;
    if (!dictionary2.TryGetValue(replacement, out str))
    {
      str = format.Replace(token, replacement);
      dictionary2[replacement] = str;
    }
    return str;
  }

  public static string Combine(string a, string b, string c) => StringFormatter.Combine(StringFormatter.Combine(a, b), c);

  public static string Combine(string a, string b, string c, string d) => StringFormatter.Combine(StringFormatter.Combine(StringFormatter.Combine(a, b), c), d);

  public static string Combine(string a, string b)
  {
    Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
    if (!StringFormatter.cachedCombines.TryGetValue(a, out dictionary))
    {
      dictionary = new Dictionary<string, string>();
      StringFormatter.cachedCombines[a] = dictionary;
    }
    string str = (string) null;
    if (!dictionary.TryGetValue(b, out str))
    {
      str = a + b;
      dictionary[b] = str;
    }
    return str;
  }

  public static string ToUpper(string a)
  {
    HashedString key = (HashedString) a;
    string str = (string) null;
    if (!StringFormatter.cachedToUppers.TryGetValue(key, out str))
    {
      str = a.ToUpper();
      StringFormatter.cachedToUppers[key] = str;
    }
    return str;
  }
}
