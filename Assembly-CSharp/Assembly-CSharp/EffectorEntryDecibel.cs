// Decompiled with JetBrains decompiler
// Type: EffectorEntryDecibel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal struct EffectorEntryDecibel
{
  public string name;
  public int count;
  public float value;

  public EffectorEntryDecibel(string name, float value)
  {
    this.name = name;
    this.value = value;
    this.count = 1;
  }
}
