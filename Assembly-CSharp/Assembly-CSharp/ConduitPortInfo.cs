// Decompiled with JetBrains decompiler
// Type: ConduitPortInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class ConduitPortInfo
{
  public ConduitType conduitType;
  public CellOffset offset;

  public ConduitPortInfo(ConduitType type, CellOffset offset)
  {
    this.conduitType = type;
    this.offset = offset;
  }
}
