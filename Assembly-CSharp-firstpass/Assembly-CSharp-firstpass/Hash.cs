// Decompiled with JetBrains decompiler
// Type: Hash
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public static class Hash
{
  public static int SDBMLower(string s)
  {
    if (s == null)
      return 0;
    uint num = 0;
    for (int index = 0; index < s.Length; ++index)
      num = (uint) ((int) char.ToLower(s[index]) + ((int) num << 6) + ((int) num << 16)) - num;
    return (int) num;
  }

  public static int[] SDBMLower(IList<string> strings)
  {
    int[] numArray = new int[strings.Count];
    for (int index = 0; index < strings.Count; ++index)
      numArray[index] = Hash.SDBMLower(strings[index]);
    return numArray;
  }
}
