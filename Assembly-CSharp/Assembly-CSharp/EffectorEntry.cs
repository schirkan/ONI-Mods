// Decompiled with JetBrains decompiler
// Type: EffectorEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

internal struct EffectorEntry
{
  public string name;
  public int count;
  public float value;

  public EffectorEntry(string name, float value)
  {
    this.name = name;
    this.value = value;
    this.count = 1;
  }

  public override string ToString()
  {
    string str = "";
    if (this.count > 1)
      str = string.Format((string) UI.OVERLAYS.DECOR.COUNT, (object) this.count);
    return string.Format((string) UI.OVERLAYS.DECOR.ENTRY, (object) GameUtil.GetFormattedDecor(this.value), (object) this.name, (object) str);
  }
}
