﻿// Decompiled with JetBrains decompiler
// Type: AntiCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class AntiCluster : KMonoBehaviour, ISim200ms
{
  private int previousCell = Grid.InvalidCell;

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    this.UpdateCell(this.previousCell, cell);
    if (this.previousCell != Grid.InvalidCell)
      this.UpdateCell(Grid.CellAbove(this.previousCell), Grid.CellAbove(cell));
    else
      this.UpdateCell(this.previousCell, Grid.CellAbove(cell));
    this.previousCell = cell;
  }

  private void UpdateCell(int previous_cell, int current_cell)
  {
    if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && (Object) Grid.Objects[previous_cell, 0] == (Object) this.gameObject)
      Grid.Objects[previous_cell, 0] = (GameObject) null;
    GameObject gameObject = Grid.Objects[current_cell, 0];
    if ((Object) gameObject == (Object) null)
    {
      Grid.Objects[current_cell, 0] = this.gameObject;
    }
    else
    {
      if (this.GetComponent<KPrefabID>().InstanceID <= gameObject.GetComponent<KPrefabID>().InstanceID)
        return;
      Grid.Objects[current_cell, 0] = this.gameObject;
    }
  }
}
