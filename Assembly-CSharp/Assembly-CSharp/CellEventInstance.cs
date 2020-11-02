// Decompiled with JetBrains decompiler
// Type: CellEventInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
  [Serialize]
  public int cell;
  [Serialize]
  public int data;
  [Serialize]
  public int data2;

  public CellEventInstance(int cell, int data, int data2, CellEvent ev)
    : base((EventBase) ev)
  {
    this.cell = cell;
    this.data = data;
    this.data2 = data2;
  }
}
