// Decompiled with JetBrains decompiler
// Type: Capturable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
  [MyCmpAdd]
  private Baggable baggable;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  public bool allowCapture = true;
  [Serialize]
  private bool markedForCapture;
  private Chore chore;
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnTagsChanged(data)));

  public bool IsMarkedForCapture => this.markedForCapture;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Capturables.Add(this);
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.resetProgressOnStop = true;
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.multitoolContext = (HashedString) "capture";
    this.multitoolHitEffectTag = (Tag) "fx_capture_splash";
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
    this.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
    this.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
    if (this.markedForCapture)
      Prioritizable.AddRef(this.gameObject);
    this.UpdateStatusItem();
    this.UpdateChore();
    this.SetWorkTime(10f);
  }

  protected override void OnCleanUp()
  {
    Components.Capturables.Remove(this);
    base.OnCleanUp();
  }

  private void OnDeath(object data)
  {
    this.allowCapture = false;
    this.markedForCapture = false;
    this.UpdateChore();
  }

  private void OnTagsChanged(object data) => this.MarkForCapture(this.markedForCapture);

  public void MarkForCapture(bool mark)
  {
    PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
    this.MarkForCapture(mark, priority);
  }

  public void MarkForCapture(bool mark, PrioritySetting priority)
  {
    mark = mark && this.IsCapturable();
    if (this.markedForCapture && !mark)
      Prioritizable.RemoveRef(this.gameObject);
    else if (!this.markedForCapture & mark)
    {
      Prioritizable.AddRef(this.gameObject);
      Prioritizable component = this.GetComponent<Prioritizable>();
      if ((bool) (UnityEngine.Object) component)
        component.SetMasterPriority(priority);
    }
    this.markedForCapture = mark;
    this.UpdateStatusItem();
    this.UpdateChore();
  }

  public bool IsCapturable() => this.allowCapture && !this.gameObject.HasTag(GameTags.Trapped) && (!this.gameObject.HasTag(GameTags.Stored) && !this.gameObject.HasTag(GameTags.Creatures.Bagged));

  private void OnRefreshUserMenu(object data)
  {
    if (!this.IsCapturable())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.markedForCapture ? new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CAPTURE.NAME, (System.Action) (() => this.MarkForCapture(true)), tooltipText: ((string) UI.USERMENUACTIONS.CAPTURE.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CANCELCAPTURE.NAME, (System.Action) (() => this.MarkForCapture(false)), tooltipText: ((string) UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP)));
  }

  private void UpdateStatusItem()
  {
    this.shouldShowSkillPerkStatusItem = this.markedForCapture;
    this.UpdateStatusItem((object) null);
    if (this.markedForCapture)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, (object) this);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture);
  }

  private void UpdateChore()
  {
    if (this.markedForCapture && this.chore == null)
    {
      this.chore = (Chore) new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, (IStateMachineTarget) this, is_preemptable: true);
    }
    else
    {
      if (this.markedForCapture || this.chore == null)
        return;
      this.chore.Cancel("not marked for capture");
      this.chore = (Chore) null;
    }
  }

  protected override void OnStartWork(Worker worker) => this.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned);

  protected override void OnStopWork(Worker worker) => this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);

  protected override void OnCompleteWork(Worker worker)
  {
    int num1 = this.NaturalBuildingCell();
    if (Grid.Solid[num1])
    {
      int num2 = Grid.CellAbove(num1);
      if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
        num1 = num2;
    }
    this.MarkForCapture(false);
    this.baggable.SetWrangled();
    this.baggable.transform.SetPosition(Grid.CellToPosCCC(num1, Grid.SceneLayer.Ore));
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    if (this.allowCapture)
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE));
    return descriptors;
  }
}
