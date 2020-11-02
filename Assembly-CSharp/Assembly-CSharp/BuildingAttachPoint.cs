// Decompiled with JetBrains decompiler
// Type: BuildingAttachPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
  public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.BuildingAttachPoints.Add(this);
    this.TryAttachEmptyHardpoints();
  }

  protected override void OnSpawn() => base.OnSpawn();

  private void TryAttachEmptyHardpoints()
  {
    for (int index = 0; index < this.points.Length; ++index)
    {
      if (!((UnityEngine.Object) this.points[index].attachedBuilding != (UnityEngine.Object) null))
      {
        bool flag = false;
        for (int idx = 0; idx < Components.AttachableBuildings.Count && !flag; ++idx)
        {
          if (Components.AttachableBuildings[idx].attachableToTag == this.points[index].attachableType && Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.points[index].position) == Grid.PosToCell((KMonoBehaviour) Components.AttachableBuildings[idx]))
          {
            this.points[index].attachedBuilding = Components.AttachableBuildings[idx];
            flag = true;
          }
        }
      }
    }
  }

  public bool AcceptsAttachment(Tag type, int cell)
  {
    int cell1 = Grid.PosToCell(this.gameObject);
    for (int index = 0; index < this.points.Length; ++index)
    {
      if (Grid.OffsetCell(cell1, this.points[index].position) == cell && this.points[index].attachableType == type)
        return true;
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.BuildingAttachPoints.Remove(this);
  }

  [Serializable]
  public struct HardPoint
  {
    public CellOffset position;
    public Tag attachableType;
    public AttachableBuilding attachedBuilding;

    public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
    {
      this.position = position;
      this.attachableType = attachableType;
      this.attachedBuilding = attachedBuilding;
    }
  }
}
