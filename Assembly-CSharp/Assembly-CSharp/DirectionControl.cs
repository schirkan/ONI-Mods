﻿// Decompiled with JetBrains decompiler
// Type: DirectionControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DirectionControl")]
public class DirectionControl : KMonoBehaviour
{
  [Serialize]
  public WorkableReactable.AllowedDirection allowedDirection;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private DirectionControl.DirectionInfo[] directionInfos;
  public System.Action<WorkableReactable.AllowedDirection> onDirectionChanged;
  private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DirectionControl>((System.Action<DirectionControl, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DirectionControl>((System.Action<DirectionControl, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.allowedDirection = WorkableReactable.AllowedDirection.Any;
    this.directionInfos = new DirectionControl.DirectionInfo[3]
    {
      new DirectionControl.DirectionInfo()
      {
        allowLeft = true,
        allowRight = true,
        iconName = "action_direction_both",
        name = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.NAME,
        tooltip = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.TOOLTIP
      },
      new DirectionControl.DirectionInfo()
      {
        allowLeft = true,
        allowRight = false,
        iconName = "action_direction_left",
        name = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.NAME,
        tooltip = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.TOOLTIP
      },
      new DirectionControl.DirectionInfo()
      {
        allowLeft = false,
        allowRight = true,
        iconName = "action_direction_right",
        name = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.NAME,
        tooltip = (string) UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.TOOLTIP
      }
    };
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DirectionControl, (object) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetAllowedDirection(this.allowedDirection);
    this.Subscribe<DirectionControl>(493375141, DirectionControl.OnRefreshUserMenuDelegate);
    this.Subscribe<DirectionControl>(-905833192, DirectionControl.OnCopySettingsDelegate);
  }

  private void SetAllowedDirection(WorkableReactable.AllowedDirection new_direction)
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    DirectionControl.DirectionInfo directionInfo = this.directionInfos[(int) new_direction];
    bool is_visible1 = directionInfo.allowLeft && directionInfo.allowRight;
    bool is_visible2 = !is_visible1 && directionInfo.allowLeft;
    bool is_visible3 = !is_visible1 && directionInfo.allowRight;
    component.SetSymbolVisiblity((KAnimHashedString) "arrow2", is_visible1);
    component.SetSymbolVisiblity((KAnimHashedString) "arrow_left", is_visible2);
    component.SetSymbolVisiblity((KAnimHashedString) "arrow_right", is_visible3);
    if (new_direction == this.allowedDirection)
      return;
    this.allowedDirection = new_direction;
    if (this.onDirectionChanged == null)
      return;
    this.onDirectionChanged(this.allowedDirection);
  }

  private void OnChangeWorkableDirection() => this.SetAllowedDirection((WorkableReactable.AllowedDirection) ((int) (1 + this.allowedDirection) % this.directionInfos.Length));

  private void OnCopySettings(object data) => this.SetAllowedDirection(((GameObject) data).GetComponent<DirectionControl>().allowedDirection);

  private void OnRefreshUserMenu(object data)
  {
    DirectionControl.DirectionInfo directionInfo = this.directionInfos[(int) (1 + this.allowedDirection) % this.directionInfos.Length];
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo(directionInfo.iconName, directionInfo.name, new System.Action(this.OnChangeWorkableDirection), tooltipText: directionInfo.tooltip), 0.4f);
  }

  private struct DirectionInfo
  {
    public bool allowLeft;
    public bool allowRight;
    public string iconName;
    public string name;
    public string tooltip;
  }
}
