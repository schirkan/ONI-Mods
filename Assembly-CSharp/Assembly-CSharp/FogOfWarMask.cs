// Decompiled with JetBrains decompiler
// Type: FogOfWarMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FogOfWarMask")]
public class FogOfWarMask : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    Grid.OnReveal += new System.Action<int>(this.OnReveal);
  }

  private void OnReveal(int cell)
  {
    if (Grid.PosToCell((KMonoBehaviour) this) != cell)
      return;
    Grid.OnReveal -= new System.Action<int>(this.OnReveal);
    this.gameObject.DeleteObject();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    GameUtil.FloodCollectCells(Grid.PosToCell((KMonoBehaviour) this), (Func<int, bool>) (cell =>
    {
      Grid.Visible[cell] = (byte) 0;
      Grid.PreventFogOfWarReveal[cell] = true;
      return !Grid.Solid[cell];
    }));
    GameUtil.FloodCollectCells(Grid.PosToCell((KMonoBehaviour) this), (Func<int, bool>) (cell =>
    {
      int num = Grid.PreventFogOfWarReveal[cell] ? 1 : 0;
      if (Grid.Solid[cell] && Grid.Foundation[cell])
      {
        Grid.PreventFogOfWarReveal[cell] = true;
        Grid.Visible[cell] = (byte) 0;
        GameObject gameObject = Grid.Objects[cell, 1];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.GetComponent<KPrefabID>().PrefabTag.ToString() == "POIBunkerExteriorDoor")
        {
          Grid.PreventFogOfWarReveal[cell] = false;
          Grid.Visible[cell] = byte.MaxValue;
        }
      }
      return num != 0 || Grid.Foundation[cell];
    }));
  }

  public static void ClearMask(int cell)
  {
    if (!Grid.PreventFogOfWarReveal[cell])
      return;
    GameUtil.FloodCollectCells(cell, new Func<int, bool>(FogOfWarMask.RevealFogOfWarMask));
  }

  public static bool RevealFogOfWarMask(int cell)
  {
    int num = Grid.PreventFogOfWarReveal[cell] ? 1 : 0;
    if (num == 0)
      return num != 0;
    Grid.PreventFogOfWarReveal[cell] = false;
    Grid.Reveal(cell);
    return num != 0;
  }
}
