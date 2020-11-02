// Decompiled with JetBrains decompiler
// Type: EntityPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
  [MyCmpReq]
  private OccupyArea occupyArea;
  [MyCmpReq]
  private KBatchedAnimController animController;
  [MyCmpGet]
  private Storage storage;
  public ObjectLayer objectLayer = ObjectLayer.NumLayers;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle objectPartitionerEntry;
  private static readonly Func<int, object, bool> ValidTestDelegate = (Func<int, object, bool>) ((cell, data) => EntityPreview.ValidTest(cell, data));

  public bool Valid { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.solidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnAreaChanged));
    if (this.objectLayer != ObjectLayer.NumLayers)
      this.objectPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new System.Action<object>(this.OnAreaChanged));
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
    this.OnAreaChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, cell);
    GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, cell);
    this.OnAreaChanged((object) null);
  }

  public void SetSolid() => this.occupyArea.ApplyToCells = true;

  private void OnAreaChanged(object obj) => this.UpdateValidity();

  public void UpdateValidity()
  {
    int num1 = this.Valid ? 1 : 0;
    this.Valid = this.occupyArea.TestArea(Grid.PosToCell((KMonoBehaviour) this), (object) this, EntityPreview.ValidTestDelegate);
    if (this.Valid)
      this.animController.TintColour = (Color32) Color.white;
    else
      this.animController.TintColour = (Color32) Color.red;
    int num2 = this.Valid ? 1 : 0;
    if (num1 == num2)
      return;
    this.Trigger(-1820564715, (object) this.Valid);
  }

  private static bool ValidTest(int cell, object data)
  {
    EntityPreview entityPreview = (EntityPreview) data;
    if (Grid.Solid[cell])
      return false;
    return entityPreview.objectLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) entityPreview.gameObject || (UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) null;
  }
}
