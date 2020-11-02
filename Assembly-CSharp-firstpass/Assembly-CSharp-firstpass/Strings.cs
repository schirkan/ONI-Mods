// Decompiled with JetBrains decompiler
// Type: Strings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public static class Strings
{
  private static StringTable RootTable = new StringTable();
  private static HashSet<string> invalidKeys = new HashSet<string>();

  private static StringEntry GetInvalidString(params StringKey[] keys)
  {
    string str = "MISSING";
    foreach (StringKey key in keys)
    {
      if (str != "")
        str += ".";
      str += key.String;
    }
    Strings.invalidKeys.Add(str);
    return new StringEntry(str);
  }

  public static StringEntry Get(StringKey key0)
  {
    StringEntry invalidString = Strings.RootTable.Get(key0);
    if (invalidString == null)
      invalidString = Strings.GetInvalidString(key0);
    return invalidString;
  }

  public static StringEntry Get(string key)
  {
    StringKey key0 = new StringKey(key);
    StringEntry invalidString = Strings.RootTable.Get(key0);
    if (invalidString == null)
      invalidString = Strings.GetInvalidString(key0);
    return invalidString;
  }

  public static bool TryGet(StringKey key, out StringEntry result)
  {
    result = Strings.RootTable.Get(key);
    return result != null;
  }

  public static bool TryGet(string key, out StringEntry result) => Strings.TryGet(new StringKey(key), out result);

  public static void Add(params string[] value) => Strings.RootTable.Add(0, value);

  public static void PrintTable() => Strings.RootTable.Print("");
}
