// Decompiled with JetBrains decompiler
// Type: StringKey
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public struct StringKey
{
  public string String;
  public int Hash;

  public StringKey(string str)
  {
    this.String = str;
    this.Hash = str.GetHashCode();
  }

  public override string ToString() => "S: [" + this.String + "] H: [" + (object) this.Hash + "] Value: [" + (string) Strings.Get(this) + "]";

  public bool IsValid() => (uint) this.Hash > 0U;
}
