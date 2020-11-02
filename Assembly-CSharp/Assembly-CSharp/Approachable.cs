// Decompiled with JetBrains decompiler
// Type: Approachable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
  public CellOffset[] GetOffsets() => OffsetGroups.Use;

  public int GetCell() => Grid.PosToCell((KMonoBehaviour) this);
}
