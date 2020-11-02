// Decompiled with JetBrains decompiler
// Type: UprootedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
  private int position;
  private int ground;
  [Serialize]
  public bool canBeUprooted = true;
  [Serialize]
  private bool uprooted;
  public CellOffset monitorCell = new CellOffset(0, -1);
  private HandleVector<int>.Handle partitionerEntry;
  private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>((System.Action<UprootedMonitor, object>) ((component, data) =>
  {
    if (component.uprooted)
      return;
    component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    component.uprooted = true;
    component.Trigger(-216549700, (object) null);
  }));

  public bool IsUprooted => this.uprooted || this.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
    this.position = Grid.PosToCell(this.gameObject);
    this.ground = Grid.OffsetCell(this.position, this.monitorCell);
    if (Grid.IsValidCell(this.position) && Grid.IsValidCell(this.ground))
      this.partitionerEntry = GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", (object) this.gameObject, this.ground, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnGroundChanged));
    this.OnGroundChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public bool CheckTileGrowable() => !this.canBeUprooted || !this.uprooted && this.IsCellSafe(this.position);

  public bool IsCellSafe(int cell)
  {
    if (!Grid.IsCellOffsetValid(cell, this.monitorCell))
      return false;
    int i = Grid.OffsetCell(cell, this.monitorCell);
    return Grid.Solid[i];
  }

  public void OnGroundChanged(object callbackData)
  {
    if (this.CheckTileGrowable())
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    this.uprooted = true;
    this.Trigger(-216549700, (object) null);
  }

  public static bool IsObjectUprooted(GameObject plant)
  {
    UprootedMonitor component = plant.GetComponent<UprootedMonitor>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsUprooted;
  }
}
