// Decompiled with JetBrains decompiler
// Type: RequiresFoundation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RequiresFoundation : KGameObjectComponentManager<RequiresFoundation.Data>, IKComponentManager
{
  public static readonly Operational.Flag solidFoundation = new Operational.Flag("solid_foundation", Operational.Flag.Type.Functional);

  public HandleVector<int>.Handle Add(GameObject go)
  {
    BuildingDef def = go.GetComponent<Building>().Def;
    int cell = Grid.PosToCell(go.transform.GetPosition());
    RequiresFoundation.Data data = new RequiresFoundation.Data()
    {
      cell = cell,
      width = def.WidthInCells,
      height = def.HeightInCells,
      buildRule = def.BuildLocationRule,
      solid = true,
      go = go
    };
    HandleVector<int>.Handle h = this.Add(go, data);
    if (def.ContinuouslyCheckFoundation)
    {
      System.Action<object> event_callback = (System.Action<object>) (d => this.OnSolidChanged(h));
      Rotatable component = data.go.GetComponent<Rotatable>();
      Orientation orientation = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetOrientation() : Orientation.Neutral;
      int num1 = -(def.WidthInCells - 1) / 2;
      int num2 = def.WidthInCells / 2;
      List<int> intList = new List<int>();
      for (int x = num1; x <= num2; ++x)
      {
        CellOffset offset = new CellOffset(x, -1);
        if (def.BuildLocationRule == BuildLocationRule.OnWall)
          offset = new CellOffset(x - 1, 0);
        else if (def.BuildLocationRule == BuildLocationRule.OnCeiling || def.BuildLocationRule == BuildLocationRule.InCorner)
          offset = new CellOffset(x, def.HeightInCells);
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
        int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
        intList.Add(num3);
      }
      Vector2I xy1 = Grid.CellToXY(intList[0]);
      Vector2I xy2 = Grid.CellToXY(intList[intList.Count - 1]);
      double num4 = xy1.x > xy2.x ? (double) xy2.x : (double) xy1.x;
      float num5 = xy1.x < xy2.x ? (float) xy2.x : (float) xy1.x;
      float num6 = xy1.y > xy2.y ? (float) xy2.y : (float) xy1.y;
      float num7 = xy1.y < xy2.y ? (float) xy2.y : (float) xy1.y;
      double num8 = (double) num6;
      double num9 = (double) num5;
      double num10 = (double) num7;
      Rect rect = Rect.MinMaxRect((float) num4, (float) num8, (float) num9, (float) num10);
      data.solidPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", (object) go, (int) rect.x, (int) rect.y, (int) rect.width + 1, (int) rect.height + 1, GameScenePartitioner.Instance.solidChangedLayer, event_callback);
      data.buildingPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", (object) go, (int) rect.x, (int) rect.y, (int) rect.width + 1, (int) rect.height + 1, GameScenePartitioner.Instance.objectLayers[1], event_callback);
      this.SetData(h, data);
      this.OnSolidChanged(h);
    }
    return h;
  }

  protected override void OnCleanUp(HandleVector<int>.Handle h)
  {
    RequiresFoundation.Data data = this.GetData(h);
    GameScenePartitioner.Instance.Free(ref data.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref data.buildingPartitionerEntry);
    this.SetData(h, data);
  }

  private void OnSolidChanged(HandleVector<int>.Handle h)
  {
    RequiresFoundation.Data data = this.GetData(h);
    SimCellOccupier component1 = data.go.GetComponent<SimCellOccupier>();
    if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && !component1.IsReady())
      return;
    Rotatable component2 = data.go.GetComponent<Rotatable>();
    Orientation orientation = (UnityEngine.Object) component2 != (UnityEngine.Object) null ? component2.GetOrientation() : Orientation.Neutral;
    this.UpdateSolidState(BuildingDef.CheckFoundation(data.cell, orientation, data.buildRule, data.width, data.height), ref data);
    this.SetData(h, data);
  }

  private void UpdateSolidState(bool is_solid, ref RequiresFoundation.Data data)
  {
    if (data.solid == is_solid)
      return;
    data.solid = is_solid;
    Operational component = data.go.GetComponent<Operational>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetFlag(RequiresFoundation.solidFoundation, is_solid);
    data.go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.MissingFoundation, !is_solid, (object) this);
  }

  public struct Data
  {
    public int cell;
    public int width;
    public int height;
    public BuildLocationRule buildRule;
    public HandleVector<int>.Handle solidPartitionerEntry;
    public HandleVector<int>.Handle buildingPartitionerEntry;
    public bool solid;
    public GameObject go;
  }
}
