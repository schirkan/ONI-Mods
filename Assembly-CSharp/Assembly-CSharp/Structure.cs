// Decompiled with JetBrains decompiler
// Type: Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private Operational operational;
  public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);
  private bool isEntombed;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsEntombed() => this.isEntombed;

  public static bool IsBuildingEntombed(Building building)
  {
    for (int index = 0; index < building.PlacementCells.Length; ++index)
    {
      int placementCell = building.PlacementCells[index];
      if (Grid.Element[placementCell].IsSolid && !Grid.Foundation[placementCell])
        return true;
    }
    return false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", (object) this.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.OnSolidChanged((object) null);
  }

  private void OnSolidChanged(object data)
  {
    bool flag = Structure.IsBuildingEntombed(this.building);
    if (flag == this.isEntombed)
      return;
    this.isEntombed = flag;
    this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, (object) this);
  }

  protected override void OnCleanUp() => GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
}
