// Decompiled with JetBrains decompiler
// Type: Database.MiscStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

namespace Database
{
  public class MiscStatusItems : StatusItems
  {
    public StatusItem MarkedForDisinfection;
    public StatusItem MarkedForCompost;
    public StatusItem MarkedForCompostInStorage;
    public StatusItem PendingClear;
    public StatusItem PendingClearNoStorage;
    public StatusItem Edible;
    public StatusItem WaitingForDig;
    public StatusItem WaitingForMop;
    public StatusItem OreMass;
    public StatusItem OreTemp;
    public StatusItem ElementalCategory;
    public StatusItem ElementalState;
    public StatusItem ElementalTemperature;
    public StatusItem ElementalMass;
    public StatusItem ElementalDisease;
    public StatusItem TreeFilterableTags;
    public StatusItem OxyRockInactive;
    public StatusItem OxyRockEmitting;
    public StatusItem OxyRockBlocked;
    public StatusItem BuriedItem;
    public StatusItem SpoutOverPressure;
    public StatusItem SpoutEmitting;
    public StatusItem SpoutPressureBuilding;
    public StatusItem SpoutIdle;
    public StatusItem SpoutDormant;
    public StatusItem OrderAttack;
    public StatusItem OrderCapture;
    public StatusItem PendingHarvest;
    public StatusItem NotMarkedForHarvest;
    public StatusItem PendingUproot;
    public StatusItem PickupableUnreachable;
    public StatusItem Prioritized;
    public StatusItem Using;
    public StatusItem Operating;
    public StatusItem Cleaning;
    public StatusItem RegionIsBlocked;
    public StatusItem NoClearLocationsAvailable;
    public StatusItem AwaitingStudy;
    public StatusItem Studied;
    public StatusItem StudiedGeyserTimeRemaining;
    public StatusItem Space;

    public MiscStatusItems(ResourceSet parent)
      : base(nameof (MiscStatusItems), parent)
      => this.CreateStatusItems();

    private StatusItem CreateStatusItem(
      string id,
      string prefix,
      string icon,
      StatusItem.IconType icon_type,
      NotificationType notification_type,
      bool allow_multiples,
      HashedString render_overlay,
      bool showWorldIcon = true,
      int status_overlays = 129022)
    {
      return this.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays));
    }

    private StatusItem CreateStatusItem(
      string id,
      string name,
      string tooltip,
      string icon,
      StatusItem.IconType icon_type,
      NotificationType notification_type,
      bool allow_multiples,
      HashedString render_overlay,
      int status_overlays = 129022)
    {
      return this.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays));
    }

    private void CreateStatusItems()
    {
      this.Edible = this.CreateStatusItem("Edible", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Edible.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Edible edible = (global::Edible) data;
        str = string.Format(str, (object) GameUtil.GetFormattedCalories(edible.Calories));
        return str;
      });
      this.PendingClear = this.CreateStatusItem("PendingClear", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingClearNoStorage = this.CreateStatusItem("PendingClearNoStorage", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.MarkedForCompost = this.CreateStatusItem("MarkedForCompost", "MISC", "status_item_pending_compost", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.MarkedForCompostInStorage = this.CreateStatusItem("MarkedForCompostInStorage", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.MarkedForDisinfection = this.CreateStatusItem("MarkedForDisinfection", "MISC", "status_item_disinfect", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Disease.ID);
      this.NoClearLocationsAvailable = this.CreateStatusItem("NoClearLocationsAvailable", "MISC", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.WaitingForDig = this.CreateStatusItem("WaitingForDig", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.WaitingForMop = this.CreateStatusItem("WaitingForMop", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OreMass = this.CreateStatusItem("OreMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OreMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        GameObject gameObject = (GameObject) data;
        str = str.Replace("{Mass}", GameUtil.GetFormattedMass(gameObject.GetComponent<PrimaryElement>().Mass));
        return str;
      });
      this.OreTemp = this.CreateStatusItem("OreTemp", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OreTemp.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        GameObject gameObject = (GameObject) data;
        str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(gameObject.GetComponent<PrimaryElement>().Temperature));
        return str;
      });
      this.ElementalState = this.CreateStatusItem("ElementalState", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ElementalState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element element = ((Func<Element>) data)();
        str = str.Replace("{State}", element.GetStateString());
        return str;
      });
      this.ElementalCategory = this.CreateStatusItem("ElementalCategory", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ElementalCategory.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element element = ((Func<Element>) data)();
        str = str.Replace("{Category}", element.GetMaterialCategoryTag().ProperName());
        return str;
      });
      this.ElementalTemperature = this.CreateStatusItem("ElementalTemperature", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ElementalTemperature.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(cellSelectionObject.temperature));
        return str;
      });
      this.ElementalMass = this.CreateStatusItem("ElementalMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ElementalMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Mass}", GameUtil.GetFormattedMass(cellSelectionObject.Mass));
        return str;
      });
      this.ElementalDisease = this.CreateStatusItem("ElementalDisease", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ElementalDisease.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount));
        return str;
      });
      this.ElementalDisease.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, true));
        return str;
      });
      this.TreeFilterableTags = this.CreateStatusItem("TreeFilterableTags", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.TreeFilterableTags.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TreeFilterable treeFilterable = (TreeFilterable) data;
        str = str.Replace("{Tags}", treeFilterable.GetTagsAsStatus());
        return str;
      });
      this.OxyRockEmitting = this.CreateStatusItem("OxyRockEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OxyRockEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(cellSelectionObject.FlowRate, GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.OxyRockBlocked = this.CreateStatusItem("OxyRockBlocked", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OxyRockBlocked.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        bool all_not_gaseous;
        bool all_over_pressure;
        GameUtil.IsEmissionBlocked(((CellSelectionObject) data).SelectedCell, out all_not_gaseous, out all_over_pressure);
        string newValue = (string) null;
        if (all_not_gaseous)
          newValue = (string) MISC.STATUSITEMS.OXYROCK.NEIGHBORSBLOCKED.NAME;
        else if (all_over_pressure)
          newValue = (string) MISC.STATUSITEMS.OXYROCK.OVERPRESSURE.NAME;
        str = str.Replace("{BlockedString}", newValue);
        return str;
      });
      this.OxyRockInactive = this.CreateStatusItem("OxyRockInactive", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Space = this.CreateStatusItem("Space", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
      this.BuriedItem = this.CreateStatusItem("BuriedItem", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SpoutOverPressure = this.CreateStatusItem("SpoutOverPressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SpoutOverPressure.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTOVERPRESSURE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime())));
        return str;
      });
      this.SpoutEmitting = this.CreateStatusItem("SpoutEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SpoutEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTEMITTING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime())));
        return str;
      });
      this.SpoutPressureBuilding = this.CreateStatusItem("SpoutPressureBuilding", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SpoutPressureBuilding.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTPRESSUREBUILDING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime())));
        return str;
      });
      this.SpoutIdle = this.CreateStatusItem("SpoutIdle", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SpoutIdle.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTIDLE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime())));
        return str;
      });
      this.SpoutDormant = this.CreateStatusItem("SpoutDormant", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OrderAttack = this.CreateStatusItem("OrderAttack", "MISC", "status_item_attack", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.OrderCapture = this.CreateStatusItem("OrderCapture", "MISC", "status_item_capture", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingHarvest = this.CreateStatusItem("PendingHarvest", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NotMarkedForHarvest = this.CreateStatusItem("NotMarkedForHarvest", "MISC", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NotMarkedForHarvest.conditionalOverlayCallback = (Func<HashedString, object, bool>) ((viewMode, o) => !(viewMode != OverlayModes.None.ID));
      this.PendingUproot = this.CreateStatusItem("PendingUproot", "MISC", "status_item_pending_uproot", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PickupableUnreachable = this.CreateStatusItem("PickupableUnreachable", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Prioritized = this.CreateStatusItem("Prioritized", "MISC", "status_item_prioritized", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Using = this.CreateStatusItem("Using", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Using.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Workable workable = (Workable) data;
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
        {
          KSelectable component = workable.GetComponent<KSelectable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            str = str.Replace("{Target}", component.GetName());
        }
        return str;
      });
      this.Operating = this.CreateStatusItem("Operating", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Cleaning = this.CreateStatusItem("Cleaning", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.RegionIsBlocked = this.CreateStatusItem("RegionIsBlocked", "MISC", "status_item_solids_blocking", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.AwaitingStudy = this.CreateStatusItem("AwaitingStudy", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Studied = this.CreateStatusItem("Studied", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    }
  }
}
