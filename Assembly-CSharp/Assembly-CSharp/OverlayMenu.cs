// Decompiled with JetBrains decompiler
// Type: OverlayMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class OverlayMenu : KIconToggleMenu
{
  public static OverlayMenu Instance;
  private List<KIconToggleMenu.ToggleInfo> overlayToggleInfos;

  public static void DestroyInstance() => OverlayMenu.Instance = (OverlayMenu) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    OverlayMenu.Instance = this;
    this.InitializeToggles();
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) this.overlayToggleInfos);
    Game.Instance.Subscribe(1798162660, new System.Action<object>(this.OnOverlayChanged));
    Game.Instance.Subscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
    this.onSelect += new KIconToggleMenu.OnSelect(this.OnToggleSelect);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshButtons();
  }

  public void Refresh() => this.RefreshButtons();

  protected override void RefreshButtons()
  {
    base.RefreshButtons();
    if ((UnityEngine.Object) Research.Instance == (UnityEngine.Object) null)
      return;
    foreach (OverlayMenu.OverlayToggleInfo overlayToggleInfo1 in this.overlayToggleInfos)
    {
      OverlayMenu.OverlayToggleInfo overlayToggleInfo2;
      (overlayToggleInfo2 = overlayToggleInfo1).toggle.gameObject.SetActive(overlayToggleInfo2.IsUnlocked());
    }
  }

  private void OnResearchComplete(object data) => this.RefreshButtons();

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.Unsubscribe(1798162660, new System.Action<object>(this.OnOverlayChanged));
  }

  private void InitializeToggleGroups()
  {
  }

  private void InitializeToggles() => this.overlayToggleInfos = new List<KIconToggleMenu.ToggleInfo>()
  {
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.OXYGEN.BUTTON, "overlay_oxygen", OverlayModes.Oxygen.ID, hotKey: Action.Overlay1, tooltip: ((string) UI.TOOLTIPS.OXYGENOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.OXYGEN.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.ELECTRICAL.BUTTON, "overlay_power", OverlayModes.Power.ID, hotKey: Action.Overlay2, tooltip: ((string) UI.TOOLTIPS.POWEROVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.ELECTRICAL.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.TEMPERATURE.BUTTON, "overlay_temperature", OverlayModes.Temperature.ID, hotKey: Action.Overlay3, tooltip: ((string) UI.TOOLTIPS.TEMPERATUREOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.TEMPERATURE.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.TILEMODE.BUTTON, "overlay_materials", OverlayModes.TileMode.ID, hotKey: Action.Overlay4, tooltip: ((string) UI.TOOLTIPS.TILEMODE_OVERLAY_STRING), tooltip_header: ((string) UI.OVERLAYS.TILEMODE.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.LIGHTING.BUTTON, "overlay_lights", OverlayModes.Light.ID, hotKey: Action.Overlay5, tooltip: ((string) UI.TOOLTIPS.LIGHTSOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.LIGHTING.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.LIQUIDPLUMBING.BUTTON, "overlay_liquidvent", OverlayModes.LiquidConduits.ID, hotKey: Action.Overlay6, tooltip: ((string) UI.TOOLTIPS.LIQUIDVENTOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.LIQUIDPLUMBING.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.GASPLUMBING.BUTTON, "overlay_gasvent", OverlayModes.GasConduits.ID, hotKey: Action.Overlay7, tooltip: ((string) UI.TOOLTIPS.GASVENTOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.GASPLUMBING.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.DECOR.BUTTON, "overlay_decor", OverlayModes.Decor.ID, hotKey: Action.Overlay8, tooltip: ((string) UI.TOOLTIPS.DECOROVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.DECOR.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.DISEASE.BUTTON, "overlay_disease", OverlayModes.Disease.ID, hotKey: Action.Overlay9, tooltip: ((string) UI.TOOLTIPS.DISEASEOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.DISEASE.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.CROPS.BUTTON, "overlay_farming", OverlayModes.Crop.ID, hotKey: Action.Overlay10, tooltip: ((string) UI.TOOLTIPS.CROPS_OVERLAY_STRING), tooltip_header: ((string) UI.OVERLAYS.CROPS.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.ROOMS.BUTTON, "overlay_rooms", OverlayModes.Rooms.ID, hotKey: Action.Overlay11, tooltip: ((string) UI.TOOLTIPS.ROOMSOVERLAYSTRING), tooltip_header: ((string) UI.OVERLAYS.ROOMS.BUTTON)),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.SUIT.BUTTON, "overlay_suit", OverlayModes.Suit.ID, "SuitsOverlay", Action.Overlay12, (string) UI.TOOLTIPS.SUITOVERLAYSTRING, (string) UI.OVERLAYS.SUIT.BUTTON),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.LOGIC.BUTTON, "overlay_logic", OverlayModes.Logic.ID, "AutomationOverlay", Action.Overlay13, (string) UI.TOOLTIPS.LOGICOVERLAYSTRING, (string) UI.OVERLAYS.LOGIC.BUTTON),
    (KIconToggleMenu.ToggleInfo) new OverlayMenu.OverlayToggleInfo((string) UI.OVERLAYS.CONVEYOR.BUTTON, "overlay_conveyor", OverlayModes.SolidConveyor.ID, "ConveyorOverlay", Action.Overlay14, (string) UI.TOOLTIPS.CONVEYOR_OVERLAY_STRING, (string) UI.OVERLAYS.CONVEYOR.BUTTON)
  };

  private void OnToggleSelect(KIconToggleMenu.ToggleInfo toggle_info)
  {
    if (SimDebugView.Instance.GetMode() == ((OverlayMenu.OverlayToggleInfo) toggle_info).simView)
    {
      OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID);
    }
    else
    {
      if (!((OverlayMenu.OverlayToggleInfo) toggle_info).IsUnlocked())
        return;
      OverlayScreen.Instance.ToggleOverlay(((OverlayMenu.OverlayToggleInfo) toggle_info).simView);
    }
  }

  private void OnOverlayChanged(object overlay_data)
  {
    HashedString hashedString = (HashedString) overlay_data;
    for (int index = 0; index < this.overlayToggleInfos.Count; ++index)
      this.overlayToggleInfos[index].toggle.isOn = ((OverlayMenu.OverlayToggleInfo) this.overlayToggleInfos[index]).simView == hashedString;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && e.TryConsume(Action.Escape))
      OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private class OverlayToggleGroup : KIconToggleMenu.ToggleInfo
  {
    public List<OverlayMenu.OverlayToggleInfo> toggleInfoGroup;
    public string requiredTechItem;
    [SerializeField]
    private int activeToggleInfo;

    public OverlayToggleGroup(
      string text,
      string icon_name,
      List<OverlayMenu.OverlayToggleInfo> toggle_group,
      string required_tech_item = "",
      Action hot_key = Action.NumActions,
      string tooltip = "",
      string tooltip_header = "")
      : base(text, icon_name, hotkey: hot_key, tooltip: tooltip, tooltip_header: tooltip_header)
    {
      this.toggleInfoGroup = toggle_group;
    }

    public bool IsUnlocked() => DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);

    public OverlayMenu.OverlayToggleInfo GetActiveToggleInfo() => this.toggleInfoGroup[this.activeToggleInfo];
  }

  private class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
  {
    public HashedString simView;
    public string requiredTechItem;

    public OverlayToggleInfo(
      string text,
      string icon_name,
      HashedString sim_view,
      string required_tech_item = "",
      Action hotKey = Action.NumActions,
      string tooltip = "",
      string tooltip_header = "")
      : base(text, icon_name, hotkey: hotKey, tooltip: tooltip, tooltip_header: tooltip_header)
    {
      this.simView = sim_view;
      this.requiredTechItem = required_tech_item;
    }

    public bool IsUnlocked() => DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
  }
}
