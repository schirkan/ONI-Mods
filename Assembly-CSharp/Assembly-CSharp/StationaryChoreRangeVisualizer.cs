﻿// Decompiled with JetBrains decompiler
// Type: StationaryChoreRangeVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Rotatable rotatable;
  public int x;
  public int y;
  public int width;
  public int height;
  public bool movable;
  public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;
  public CellOffset vision_offset;
  public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);
  public bool blocking_tile_visible = true;
  private static readonly string AnimName = "transferarmgrid_kanim";
  private static readonly HashedString[] PreAnims = new HashedString[2]
  {
    (HashedString) "grid_pre",
    (HashedString) "grid_loop"
  };
  private static readonly HashedString PostAnim = (HashedString) "grid_pst";
  private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();
  private List<int> newCells = new List<int>();
  private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>((System.Action<StationaryChoreRangeVisualizer, object>) ((component, data) => component.OnSelect(data)));
  private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>((System.Action<StationaryChoreRangeVisualizer, object>) ((component, data) => component.OnRotated(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
    if (!this.movable)
      return;
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
    this.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    this.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
    this.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
    this.ClearVisualizers();
    base.OnCleanUp();
  }

  private void OnSelect(object data)
  {
    if ((bool) data)
    {
      SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form"), this.transform.position);
      this.UpdateVisualizers();
    }
    else
    {
      SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear"), this.transform.position);
      this.ClearVisualizers();
    }
  }

  private void OnRotated(object data) => this.UpdateVisualizers();

  private void OnCellChange() => this.UpdateVisualizers();

  private void UpdateVisualizers()
  {
    this.newCells.Clear();
    CellOffset offset1 = this.vision_offset;
    if ((bool) (UnityEngine.Object) this.rotatable)
      offset1 = this.rotatable.GetRotatedCellOffset(this.vision_offset);
    int cell1 = Grid.PosToCell(this.transform.gameObject);
    int x1;
    int y1;
    Grid.CellToXY(Grid.OffsetCell(cell1, offset1), out x1, out y1);
    for (int index1 = 0; index1 < this.height; ++index1)
    {
      for (int index2 = 0; index2 < this.width; ++index2)
      {
        CellOffset offset2 = new CellOffset(this.x + index2, this.y + index1);
        if ((bool) (UnityEngine.Object) this.rotatable)
          offset2 = this.rotatable.GetRotatedCellOffset(offset2);
        int cell2 = Grid.OffsetCell(cell1, offset2);
        if (Grid.IsValidCell(cell2))
        {
          int x2;
          int y2;
          Grid.CellToXY(cell2, out x2, out y2);
          if (Grid.TestLineOfSight(x1, y1, x2, y2, this.blocking_cb, this.blocking_tile_visible))
            this.newCells.Add(cell2);
        }
      }
    }
    for (int index = this.visualizers.Count - 1; index >= 0; --index)
    {
      if (this.newCells.Contains(this.visualizers[index].cell))
      {
        this.newCells.Remove(this.visualizers[index].cell);
      }
      else
      {
        this.DestroyEffect(this.visualizers[index].controller);
        this.visualizers.RemoveAt(index);
      }
    }
    for (int index = 0; index < this.newCells.Count; ++index)
    {
      KBatchedAnimController effect = this.CreateEffect(this.newCells[index]);
      this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData()
      {
        cell = this.newCells[index],
        controller = effect
      });
    }
  }

  private void ClearVisualizers()
  {
    for (int index = 0; index < this.visualizers.Count; ++index)
      this.DestroyEffect(this.visualizers[index].controller);
    this.visualizers.Clear();
  }

  private KBatchedAnimController CreateEffect(int cell)
  {
    KBatchedAnimController effect = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), layer: this.sceneLayer, set_inactive: true);
    effect.destroyOnAnimComplete = false;
    effect.visibilityType = KAnimControllerBase.VisibilityType.Always;
    effect.gameObject.SetActive(true);
    effect.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
    return effect;
  }

  private void DestroyEffect(KBatchedAnimController controller)
  {
    controller.destroyOnAnimComplete = true;
    controller.Play(StationaryChoreRangeVisualizer.PostAnim);
  }

  private struct VisData
  {
    public int cell;
    public KBatchedAnimController controller;
  }
}
