// Decompiled with JetBrains decompiler
// Type: HashCache
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class HashCache
{
  private Dictionary<int, string> hashes = new Dictionary<int, string>();
  private static HashCache instance;

  public static HashCache Get()
  {
    if (HashCache.instance == null)
      HashCache.instance = new HashCache();
    return HashCache.instance;
  }

  public string Get(int hash)
  {
    string str = "";
    this.hashes.TryGetValue(hash, out str);
    return str;
  }

  public string Get(HashedString hash) => this.Get(hash.HashValue);

  public string Get(KAnimHashedString hash) => this.Get(hash.HashValue);

  public HashedString Add(string text)
  {
    HashedString hashedString = new HashedString(text);
    this.Add(hashedString.HashValue, text);
    return hashedString;
  }

  public void Add(int hash, string text)
  {
    string str = (string) null;
    if (this.hashes.TryGetValue(hash, out str))
      return;
    this.hashes[hash] = text.ToLower();
  }
}
