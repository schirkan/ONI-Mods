// Decompiled with JetBrains decompiler
// Type: DebugHashes
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public static class DebugHashes
{
  private static Dictionary<int, string> hashMap = new Dictionary<int, string>();

  public static void Add(string name)
  {
    int key = Hash.SDBMLower(name);
    DebugHashes.hashMap[key] = name;
  }

  public static string GetName(int hash) => DebugHashes.hashMap.ContainsKey(hash) ? DebugHashes.hashMap[hash] : "Unknown HASH [0x" + hash.ToString("X") + "]";
}
