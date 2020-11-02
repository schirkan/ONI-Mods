// Decompiled with JetBrains decompiler
// Type: StringEntry
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class StringEntry
{
  public string String;

  public StringEntry(string str) => this.String = str;

  public override string ToString() => this.String;

  public static implicit operator string(StringEntry entry) => entry.String;
}
