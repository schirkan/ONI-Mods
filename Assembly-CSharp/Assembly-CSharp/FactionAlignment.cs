// Decompiled with JetBrains decompiler
// Type: FactionAlignment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
  [Serialize]
  private bool alignmentActive = true;
  public FactionManager.FactionID Alignment;
  [Serialize]
  public bool targeted;
  [Serialize]
  public bool targetable = true;
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagChangedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, (System.Action<FactionAlignment, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((System.Action<FactionAlignment, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((System.Action<FactionAlignment, object>) ((component, data) => component.SetPlayerTargeted(false)));

  [MyCmpAdd]
  public Health health { get; private set; }

  public AttackableBase attackable { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.health = this.GetComponent<Health>();
    this.attackable = this.GetComponent<AttackableBase>();
    Components.FactionAlignments.Add(this);
    this.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
    this.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
    if (this.alignmentActive)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagChangedDelegate);
    this.UpdateStatusItem();
  }

  protected override void OnPrefabInit()
  {
  }

  private void OnDeath(object data) => this.SetAlignmentActive(false);

  public void SetAlignmentActive(bool active)
  {
    this.SetPlayerTargetable(active);
    this.alignmentActive = active;
    if (active)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    else
      FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
  }

  public bool IsAlignmentActive() => FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);

  public void SetPlayerTargetable(bool state)
  {
    this.targetable = state;
    if (state)
      return;
    this.SetPlayerTargeted(false);
  }

  public void SetPlayerTargeted(bool state)
  {
    this.targeted = state && this.targetable;
    this.UpdateStatusItem();
  }

  private void UpdateStatusItem()
  {
    if (this.targeted)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack);
  }

  public void SwitchAlignment(FactionManager.FactionID newAlignment)
  {
    this.SetAlignmentActive(false);
    this.Alignment = newAlignment;
    this.SetAlignmentActive(true);
  }

  protected override void OnCleanUp()
  {
    Components.FactionAlignments.Remove(this);
    FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
    base.OnCleanUp();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.Alignment == FactionManager.FactionID.Duplicant || !this.IsAlignmentActive())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.targeted ? new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.ATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(true)), tooltipText: ((string) UI.USERMENUACTIONS.ATTACK.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.CANCELATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(false)), tooltipText: ((string) UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP)));
  }
}
