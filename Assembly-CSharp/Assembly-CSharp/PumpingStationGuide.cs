// Decompiled with JetBrains decompiler
// Type: PumpingStationGuide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
  private int previousDepthAvailable = -1;
  public GameObject parent;
  public bool occupyTiles;
  private KBatchedAnimController parentController;
  private KBatchedAnimController guideController;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.parentController = this.parent.GetComponent<KBatchedAnimController>();
    this.guideController = this.GetComponent<KBatchedAnimController>();
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  private void RefreshTint() => this.guideController.TintColour = this.parentController.TintColour;

  private void RefreshDepthAvailable()
  {
    int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell((KMonoBehaviour) this), this.parent);
    if (depthAvailable == this.previousDepthAvailable)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (depthAvailable == 0)
    {
      component.enabled = false;
    }
    else
    {
      component.enabled = true;
      component.Play(new HashedString("place_pipe" + depthAvailable.ToString()));
    }
    if (this.occupyTiles)
      PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
    this.previousDepthAvailable = depthAvailable;
  }

  public void RenderEveryTick(float dt)
  {
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  public static void OccupyArea(GameObject go, int depth_available)
  {
    int cell = Grid.PosToCell(go.transform.GetPosition());
    for (int index = 1; index <= depth_available; ++index)
    {
      int key1 = Grid.OffsetCell(cell, 0, -index);
      int key2 = Grid.OffsetCell(cell, 1, -index);
      Grid.ObjectLayers[1][key1] = go;
      Grid.ObjectLayers[1][key2] = go;
    }
  }

  public static int GetDepthAvailable(int root_cell, GameObject pump)
  {
    int num1 = 4;
    int num2 = 0;
    for (int index = 1; index <= num1; ++index)
    {
      int num3 = Grid.OffsetCell(root_cell, 0, -index);
      int num4 = Grid.OffsetCell(root_cell, 1, -index);
      if (Grid.IsValidCell(num3) && !Grid.Solid[num3] && (Grid.IsValidCell(num4) && !Grid.Solid[num4]) && (!Grid.ObjectLayers[1].ContainsKey(num3) || (Object) Grid.ObjectLayers[1][num3] == (Object) null || (Object) Grid.ObjectLayers[1][num3] == (Object) pump) && (!Grid.ObjectLayers[1].ContainsKey(num4) || (Object) Grid.ObjectLayers[1][num4] == (Object) null || (Object) Grid.ObjectLayers[1][num4] == (Object) pump))
        num2 = index;
      else
        break;
    }
    return num2;
  }
}
