﻿// Decompiled with JetBrains decompiler
// Type: RangedAttackable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RangedAttackable : AttackableBase
{
  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.preferUnreservedCell = true;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  public new int GetCell() => Grid.PosToCell((KMonoBehaviour) this);

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0.0f, 0.5f, 0.5f, 0.15f);
    foreach (CellOffset offset in this.GetOffsets())
      Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0.0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(this.gameObject), offset)), Vector3.one);
  }
}
