// Decompiled with JetBrains decompiler
// Type: Placeable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
  [MyCmpReq]
  private KPrefabID prefabId;
  [Serialize]
  private int targetCell = -1;
  public Tag previewTag;
  public Tag spawnOnPlaceTag;
  private GameObject preview;
  private FetchChore chore;
  private static readonly EventSystem.IntraObjectHandler<Placeable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Placeable>((System.Action<Placeable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Placeable>(493375141, Placeable.OnRefreshUserMenuDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.prefabId.AddTag(new Tag(this.prefabId.InstanceID.ToString()));
    if (this.targetCell == -1)
      return;
    this.QueuePlacement(this.targetCell);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.preview != (UnityEngine.Object) null)
      this.preview.DeleteObject();
    base.OnCleanUp();
  }

  public void QueuePlacement(int target)
  {
    this.targetCell = target;
    Vector3 posCbc = Grid.CellToPosCBC(this.targetCell, Grid.SceneLayer.Front);
    if ((UnityEngine.Object) this.preview == (UnityEngine.Object) null)
    {
      this.preview = GameUtil.KInstantiate(Assets.GetPrefab(this.previewTag), posCbc, Grid.SceneLayer.Front);
      this.preview.SetActive(true);
    }
    else
      this.preview.transform.SetPosition(posCbc);
    if (this.chore != null)
      this.chore.Cancel("new target");
    this.chore = new FetchChore(Db.Get().ChoreTypes.Fetch, this.preview.GetComponent<Storage>(), 1f, new Tag[1]
    {
      new Tag(this.prefabId.InstanceID.ToString())
    }, on_complete: new System.Action<Chore>(this.OnChoreComplete), operational_requirement: FetchOrder2.OperationalRequirement.None);
  }

  private void OnChoreComplete(Chore completed_chore) => this.Place(this.targetCell);

  public void Place(int target)
  {
    Vector3 posCbc = Grid.CellToPosCBC(target, Grid.SceneLayer.Front);
    GameUtil.KInstantiate(Assets.GetPrefab(this.spawnOnPlaceTag), posCbc, Grid.SceneLayer.Front).SetActive(true);
    this.DeleteObject();
  }

  private void OpenPlaceTool() => PlaceTool.Instance.Activate(this, this.previewTag);

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, this.targetCell == -1 ? new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELOCATE.NAME, new System.Action(this.OpenPlaceTool), tooltipText: ((string) UI.USERMENUACTIONS.RELOCATE.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELOCATE.NAME_OFF, new System.Action(this.CancelRelocation), tooltipText: ((string) UI.USERMENUACTIONS.RELOCATE.TOOLTIP_OFF)));

  private void CancelRelocation()
  {
    if ((UnityEngine.Object) this.preview != (UnityEngine.Object) null)
    {
      this.preview.DeleteObject();
      this.preview = (GameObject) null;
    }
    this.targetCell = -1;
  }
}
