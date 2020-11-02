// Decompiled with JetBrains decompiler
// Type: Chattable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
  public CellOffset[] GetOffsets() => OffsetGroups.Chat;

  public int GetCell() => Grid.PosToCell((KMonoBehaviour) this);
}
