// Decompiled with JetBrains decompiler
// Type: DebugCellDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
  public List<int> cells;

  private void Update()
  {
    for (int index = 0; index < this.cells.Count; ++index)
    {
      if (this.cells[index] != PathFinder.InvalidCell)
        DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[index], Grid.SceneLayer.Background));
    }
  }
}
