// Decompiled with JetBrains decompiler
// Type: EntombVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
  [MyCmpReq]
  private KSelectable selectable;
  private OccupyArea _occupyArea;
  [Serialize]
  private bool isEntombed;
  private HandleVector<int>.Handle partitionerEntry;
  private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (Func<int, object, bool>) ((cell, data) => EntombVulnerable.IsCellSafeCB(cell, data));

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public bool GetEntombed => this.isEntombed;

  public string WiltStateString => Db.Get().CreatureStatusItems.Entombed.resolveStringCallback((string) CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, (object) this.gameObject);

  public WiltCondition.Condition[] Conditions => new WiltCondition.Condition[1]
  {
    WiltCondition.Condition.Entombed
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntombVulnerable), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.CheckEntombed();
    if (!this.isEntombed)
      return;
    this.Trigger(-1089732772, (object) true);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnSolidChanged(object data) => this.CheckEntombed();

  private void CheckEntombed()
  {
    int cell = Grid.PosToCell(this.gameObject.transform.GetPosition());
    if (!Grid.IsValidCell(cell))
      return;
    if (!this.IsCellSafe(cell))
    {
      if (this.isEntombed)
        return;
      this.isEntombed = true;
      this.selectable.AddStatusItem(Db.Get().CreatureStatusItems.Entombed, (object) this.gameObject);
      this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) true);
    }
    else
    {
      if (!this.isEntombed)
        return;
      this.isEntombed = false;
      this.selectable.RemoveStatusItem(Db.Get().CreatureStatusItems.Entombed);
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) false);
    }
  }

  public bool IsCellSafe(int cell) => this.occupyArea.TestArea(cell, (object) null, EntombVulnerable.IsCellSafeCBDelegate);

  private static bool IsCellSafeCB(int cell, object data) => Grid.IsValidCell(cell) && !Grid.Solid[cell];
}
