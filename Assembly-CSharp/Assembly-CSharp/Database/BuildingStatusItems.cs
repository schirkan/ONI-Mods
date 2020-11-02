// Decompiled with JetBrains decompiler
// Type: Database.BuildingStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
  public class BuildingStatusItems : StatusItems
  {
    public StatusItem MissingRequirements;
    public StatusItem GettingReady;
    public StatusItem Working;
    public MaterialsStatusItem MaterialsUnavailable;
    public MaterialsStatusItem MaterialsUnavailableForRefill;
    public StatusItem AngerDamage;
    public StatusItem ClinicOutsideHospital;
    public StatusItem DigUnreachable;
    public StatusItem MopUnreachable;
    public StatusItem ConstructableDigUnreachable;
    public StatusItem ConstructionUnreachable;
    public StatusItem NewDuplicantsAvailable;
    public StatusItem NeedPlant;
    public StatusItem NeedPower;
    public StatusItem NotEnoughPower;
    public StatusItem PowerLoopDetected;
    public StatusItem NeedLiquidIn;
    public StatusItem NeedGasIn;
    public StatusItem NeedResourceMass;
    public StatusItem NeedSolidIn;
    public StatusItem NeedLiquidOut;
    public StatusItem NeedGasOut;
    public StatusItem NeedSolidOut;
    public StatusItem InvalidBuildingLocation;
    public StatusItem PendingDeconstruction;
    public StatusItem PendingSwitchToggle;
    public StatusItem GasVentObstructed;
    public StatusItem LiquidVentObstructed;
    public StatusItem LiquidPipeEmpty;
    public StatusItem LiquidPipeObstructed;
    public StatusItem GasPipeEmpty;
    public StatusItem GasPipeObstructed;
    public StatusItem SolidPipeObstructed;
    public StatusItem PartiallyDamaged;
    public StatusItem Broken;
    public StatusItem PendingRepair;
    public StatusItem PendingUpgrade;
    public StatusItem RequiresSkillPerk;
    public StatusItem DigRequiresSkillPerk;
    public StatusItem ColonyLacksRequiredSkillPerk;
    public StatusItem PendingWork;
    public StatusItem Flooded;
    public StatusItem PowerButtonOff;
    public StatusItem SwitchStatusActive;
    public StatusItem SwitchStatusInactive;
    public StatusItem LogicSwitchStatusActive;
    public StatusItem LogicSwitchStatusInactive;
    public StatusItem LogicSensorStatusActive;
    public StatusItem LogicSensorStatusInactive;
    public StatusItem ChangeDoorControlState;
    public StatusItem CurrentDoorControlState;
    public StatusItem Entombed;
    public MaterialsStatusItem WaitingForMaterials;
    public StatusItem WaitingForRepairMaterials;
    public StatusItem MissingFoundation;
    public StatusItem NeutroniumUnminable;
    public StatusItem NoStorageFilterSet;
    public StatusItem PendingFish;
    public StatusItem NoFishableWaterBelow;
    public StatusItem GasVentOverPressure;
    public StatusItem LiquidVentOverPressure;
    public StatusItem NoWireConnected;
    public StatusItem NoLogicWireConnected;
    public StatusItem NoTubeConnected;
    public StatusItem NoTubeExits;
    public StatusItem StoredCharge;
    public StatusItem NoPowerConsumers;
    public StatusItem PressureOk;
    public StatusItem UnderPressure;
    public StatusItem AssignedTo;
    public StatusItem Unassigned;
    public StatusItem AssignedPublic;
    public StatusItem AssignedToRoom;
    public StatusItem RationBoxContents;
    public StatusItem ConduitBlocked;
    public StatusItem OutputPipeFull;
    public StatusItem ConduitBlockedMultiples;
    public StatusItem MeltingDown;
    public StatusItem UnderConstruction;
    public StatusItem UnderConstructionNoWorker;
    public StatusItem Normal;
    public StatusItem ManualGeneratorChargingUp;
    public StatusItem ManualGeneratorReleasingEnergy;
    public StatusItem GeneratorOffline;
    public StatusItem Pipe;
    public StatusItem Conveyor;
    public StatusItem FabricatorIdle;
    public StatusItem FabricatorEmpty;
    public StatusItem FlushToilet;
    public StatusItem FlushToiletInUse;
    public StatusItem Toilet;
    public StatusItem ToiletNeedsEmptying;
    public StatusItem DesalinatorNeedsEmptying;
    public StatusItem Unusable;
    public StatusItem NoResearchSelected;
    public StatusItem NoApplicableResearchSelected;
    public StatusItem NoApplicableAnalysisSelected;
    public StatusItem NoResearchOrDestinationSelected;
    public StatusItem Researching;
    public StatusItem ValveRequest;
    public StatusItem EmittingLight;
    public StatusItem EmittingElement;
    public StatusItem EmittingOxygenAvg;
    public StatusItem EmittingGasAvg;
    public StatusItem PumpingLiquidOrGas;
    public StatusItem NoLiquidElementToPump;
    public StatusItem NoGasElementToPump;
    public StatusItem PipeFull;
    public StatusItem PipeMayMelt;
    public StatusItem ElementConsumer;
    public StatusItem ElementEmitterOutput;
    public StatusItem AwaitingWaste;
    public StatusItem AwaitingCompostFlip;
    public StatusItem JoulesAvailable;
    public StatusItem Wattage;
    public StatusItem SolarPanelWattage;
    public StatusItem SteamTurbineWattage;
    public StatusItem Wattson;
    public StatusItem WireConnected;
    public StatusItem WireNominal;
    public StatusItem WireDisconnected;
    public StatusItem Cooling;
    public StatusItem CoolingStalledHotEnv;
    public StatusItem CoolingStalledColdGas;
    public StatusItem CoolingStalledHotLiquid;
    public StatusItem CoolingStalledColdLiquid;
    public StatusItem CannotCoolFurther;
    public StatusItem NeedsValidRegion;
    public StatusItem NeedSeed;
    public StatusItem AwaitingSeedDelivery;
    public StatusItem AwaitingBaitDelivery;
    public StatusItem NoAvailableSeed;
    public StatusItem NeedEgg;
    public StatusItem AwaitingEggDelivery;
    public StatusItem NoAvailableEgg;
    public StatusItem Grave;
    public StatusItem GraveEmpty;
    public StatusItem NoFilterElementSelected;
    public StatusItem NoLureElementSelected;
    public StatusItem BuildingDisabled;
    public StatusItem Overheated;
    public StatusItem Overloaded;
    public StatusItem LogicOverloaded;
    public StatusItem Expired;
    public StatusItem PumpingStation;
    public StatusItem EmptyPumpingStation;
    public StatusItem GeneShuffleCompleted;
    public StatusItem DirectionControl;
    public StatusItem WellPressurizing;
    public StatusItem WellOverpressure;
    public StatusItem ReleasingPressure;
    public StatusItem NoSuitMarker;
    public StatusItem SuitMarkerWrongSide;
    public StatusItem SuitMarkerTraversalAnytime;
    public StatusItem SuitMarkerTraversalOnlyWhenRoomAvailable;
    public StatusItem TooCold;
    public StatusItem NotInAnyRoom;
    public StatusItem NotInRequiredRoom;
    public StatusItem NotInRecommendedRoom;
    public StatusItem IncubatorProgress;
    public StatusItem HabitatNeedsEmptying;
    public StatusItem DetectorScanning;
    public StatusItem IncomingMeteors;
    public StatusItem HasGantry;
    public StatusItem MissingGantry;
    public StatusItem DisembarkingDuplicant;
    public StatusItem RocketName;
    public StatusItem PathNotClear;
    public StatusItem InvalidPortOverlap;
    public StatusItem EmergencyPriority;
    public StatusItem SkillPointsAvailable;
    public StatusItem Baited;
    public StatusItem TanningLightSufficient;
    public StatusItem TanningLightInsufficient;
    public StatusItem HotTubWaterTooCold;
    public StatusItem HotTubTooHot;
    public StatusItem HotTubFilling;
    public StatusItem WindTunnelIntake;

    public BuildingStatusItems(ResourceSet parent)
      : base(nameof (BuildingStatusItems), parent)
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
      this.AngerDamage = this.CreateStatusItem("AngerDamage", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
      this.AssignedTo = this.CreateStatusItem("AssignedTo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.AssignedTo.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IAssignableIdentity assignee = ((Assignable) data).assignee;
        if (assignee != null)
        {
          string properName = assignee.GetProperName();
          str = str.Replace("{Assignee}", properName);
        }
        return str;
      });
      this.AssignedToRoom = this.CreateStatusItem("AssignedToRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.AssignedToRoom.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IAssignableIdentity assignee = ((Assignable) data).assignee;
        if (assignee != null)
        {
          string properName = assignee.GetProperName();
          str = str.Replace("{Assignee}", properName);
        }
        return str;
      });
      this.Broken = this.CreateStatusItem("Broken", "BUILDING", "status_item_broken", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Broken.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        BuildingHP.DamageSourceInfo damageSourceInfo = ((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GenericInstance) data).master.GetDamageSourceInfo();
        return str.Replace("{DamageInfo}", damageSourceInfo.ToString());
      });
      this.Broken.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
      this.ChangeDoorControlState = this.CreateStatusItem("ChangeDoorControlState", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ChangeDoorControlState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Door door = (Door) data;
        return str.Replace("{ControlState}", door.RequestedState.ToString());
      });
      this.CurrentDoorControlState = this.CreateStatusItem("CurrentDoorControlState", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.CurrentDoorControlState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = (string) Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + ((Door) data).CurrentState.ToString().ToUpper());
        return str.Replace("{ControlState}", newValue);
      });
      this.ClinicOutsideHospital = this.CreateStatusItem("ClinicOutsideHospital", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
      this.ConduitBlocked = this.CreateStatusItem("ConduitBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.OutputPipeFull = this.CreateStatusItem("OutputPipeFull", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.ConstructionUnreachable = this.CreateStatusItem("ConstructionUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.ConduitBlockedMultiples = this.CreateStatusItem("ConduitBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
      this.DigUnreachable = this.CreateStatusItem("DigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.MopUnreachable = this.CreateStatusItem("MopUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.DirectionControl = this.CreateStatusItem("DirectionControl", (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME, (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.DirectionControl.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::DirectionControl directionControl = (global::DirectionControl) data;
        string newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.BOTH;
        switch (directionControl.allowedDirection)
        {
          case WorkableReactable.AllowedDirection.Left:
            newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.LEFT;
            break;
          case WorkableReactable.AllowedDirection.Right:
            newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.RIGHT;
            break;
        }
        str = str.Replace("{Direction}", newValue);
        return str;
      });
      this.ConstructableDigUnreachable = this.CreateStatusItem("ConstructableDigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Entombed = this.CreateStatusItem("Entombed", "BUILDING", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Entombed.AddNotification();
      this.Flooded = this.CreateStatusItem("Flooded", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Flooded.AddNotification();
      this.GasVentObstructed = this.CreateStatusItem("GasVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID);
      this.GasVentOverPressure = this.CreateStatusItem("GasVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID);
      this.GeneShuffleCompleted = this.CreateStatusItem("GeneShuffleCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.InvalidBuildingLocation = this.CreateStatusItem("InvalidBuildingLocation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.LiquidVentObstructed = this.CreateStatusItem("LiquidVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID);
      this.LiquidVentOverPressure = this.CreateStatusItem("LiquidVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID);
      this.MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.None.ID);
      this.MaterialsUnavailable.AddNotification();
      this.MaterialsUnavailable.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = "";
        Dictionary<Tag, float> dictionary = (Dictionary<Tag, float>) null;
        if (data is IFetchList)
          dictionary = ((IFetchList) data).GetRemainingMinimum();
        else if (data is Dictionary<Tag, float>)
          dictionary = data as Dictionary<Tag, float>;
        if (dictionary.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in dictionary)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                newValue += "\n";
              newValue = !Assets.IsTagCountable(keyValuePair.Key) ? newValue + string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value)) : newValue + string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_UNITS, (object) GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value));
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", newValue);
        return str;
      });
      this.MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
      this.MaterialsUnavailableForRefill.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IFetchList fetchList = (IFetchList) data;
        string newValue = "";
        Dictionary<Tag, float> remaining = fetchList.GetRemaining();
        if (remaining.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                newValue += "\n";
              newValue += string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLEFORREFILL.LINE_ITEM, (object) keyValuePair.Key.ProperName());
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", newValue);
        return str;
      });
      Func<string, object, string> func1 = (Func<string, object, string>) ((str, data) =>
      {
        RoomType roomType = Db.Get().RoomTypes.Get((string) data);
        return roomType != null ? string.Format(str, (object) roomType.Name) : str;
      });
      this.NotInAnyRoom = this.CreateStatusItem("NotInAnyRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NotInRequiredRoom = this.CreateStatusItem("NotInRequiredRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NotInRequiredRoom.resolveStringCallback = func1;
      this.NotInRecommendedRoom = this.CreateStatusItem("NotInRecommendedRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NotInRecommendedRoom.resolveStringCallback = func1;
      this.WaitingForRepairMaterials = this.CreateStatusItem("WaitingForRepairMaterials", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID, false);
      this.WaitingForRepairMaterials.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        KeyValuePair<Tag, float> keyValuePair = (KeyValuePair<Tag, float>) data;
        if ((double) keyValuePair.Value != 0.0)
        {
          string newValue = string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value));
          str = str.Replace("{ItemsRemaining}", newValue);
        }
        return str;
      });
      this.WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.WaitingForMaterials.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IFetchList fetchList = (IFetchList) data;
        string newValue = "";
        Dictionary<Tag, float> remaining = fetchList.GetRemaining();
        if (remaining.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                newValue += "\n";
              newValue = !Assets.IsTagCountable(keyValuePair.Key) ? newValue + string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value)) : newValue + string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_UNITS, (object) GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value));
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", newValue);
        return str;
      });
      this.MeltingDown = this.CreateStatusItem("MeltingDown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.MissingFoundation = this.CreateStatusItem("MissingFoundation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NeutroniumUnminable = this.CreateStatusItem("NeutroniumUnminable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NeedGasIn = this.CreateStatusItem("NeedGasIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID);
      this.NeedGasIn.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>) data;
        string newValue = string.Format((string) BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM, (object) tuple.second.ProperName());
        str = str.Replace("{GasRequired}", newValue);
        return str;
      });
      this.NeedGasOut = this.CreateStatusItem("NeedGasOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.GasConduits.ID);
      this.NeedLiquidIn = this.CreateStatusItem("NeedLiquidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID);
      this.NeedLiquidIn.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>) data;
        string newValue = string.Format((string) BUILDING.STATUSITEMS.NEEDLIQUIDIN.LINE_ITEM, (object) tuple.second.ProperName());
        str = str.Replace("{LiquidRequired}", newValue);
        return str;
      });
      this.NeedLiquidOut = this.CreateStatusItem("NeedLiquidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.LiquidConduits.ID);
      this.NeedSolidIn = this.CreateStatusItem("NeedSolidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID);
      this.NeedSolidOut = this.CreateStatusItem("NeedSolidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.SolidConveyor.ID);
      this.NeedResourceMass = this.CreateStatusItem("NeedResourceMass", "BUILDING", "status_item_need_resource", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NeedResourceMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = "";
        EnergyGenerator.Formula formula = (EnergyGenerator.Formula) data;
        if (formula.inputs.Length != 0)
        {
          bool flag = true;
          foreach (EnergyGenerator.InputItem input in formula.inputs)
          {
            if (!flag)
            {
              newValue += "\n";
              flag = false;
            }
            newValue += string.Format((string) BUILDING.STATUSITEMS.NEEDRESOURCEMASS.LINE_ITEM, (object) input.tag.ProperName());
          }
        }
        str = str.Replace("{ResourcesRequired}", newValue);
        return str;
      });
      this.LiquidPipeEmpty = this.CreateStatusItem("LiquidPipeEmpty", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID);
      this.LiquidPipeObstructed = this.CreateStatusItem("LiquidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.LiquidConduits.ID);
      this.GasPipeEmpty = this.CreateStatusItem("GasPipeEmpty", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID);
      this.GasPipeObstructed = this.CreateStatusItem("GasPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.GasConduits.ID);
      this.SolidPipeObstructed = this.CreateStatusItem("SolidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.SolidConveyor.ID);
      this.NeedPlant = this.CreateStatusItem("NeedPlant", "BUILDING", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NeedPower = this.CreateStatusItem("NeedPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.NotEnoughPower = this.CreateStatusItem("NotEnoughPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.PowerLoopDetected = this.CreateStatusItem("PowerLoopDetected", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.NewDuplicantsAvailable = this.CreateStatusItem("NewDuplicantsAvailable", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NewDuplicantsAvailable.AddNotification();
      this.NewDuplicantsAvailable.notificationClickCallback = (Notification.ClickCallback) (data =>
      {
        ImmigrantScreen.InitializeImmigrantScreen((Telepad) data);
        Game.Instance.Trigger(288942073, (object) null);
      });
      this.NoStorageFilterSet = this.CreateStatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoSuitMarker = this.CreateStatusItem("NoSuitMarker", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.SuitMarkerWrongSide = this.CreateStatusItem("suitMarkerWrongSide", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.SuitMarkerTraversalAnytime = this.CreateStatusItem("suitMarkerTraversalAnytime", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SuitMarkerTraversalOnlyWhenRoomAvailable = this.CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NoFishableWaterBelow = this.CreateStatusItem("NoFishableWaterBelow", "BUILDING", "status_item_no_fishable_water_below", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoPowerConsumers = this.CreateStatusItem("NoPowerConsumers", "BUILDING", "status_item_no_power_consumers", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.NoWireConnected = this.CreateStatusItem("NoWireConnected", "BUILDING", "status_item_no_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.Power.ID);
      this.NoLogicWireConnected = this.CreateStatusItem("NoLogicWireConnected", "BUILDING", "status_item_no_logic_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Logic.ID);
      this.NoTubeConnected = this.CreateStatusItem("NoTubeConnected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoTubeExits = this.CreateStatusItem("NoTubeExits", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.StoredCharge = this.CreateStatusItem("StoredCharge", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.StoredCharge.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TravelTubeEntrance.SMInstance smInstance = (TravelTubeEntrance.SMInstance) data;
        if (smInstance != null)
          str = string.Format(str, (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.AvailableJoules), (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.TotalCapacity), (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.UsageJoules));
        return str;
      });
      this.PendingDeconstruction = this.CreateStatusItem("PendingDeconstruction", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingDeconstruction.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
      this.PendingRepair = this.CreateStatusItem("PendingRepair", "BUILDING", "status_item_pending_repair", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.PendingRepair.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        BuildingHP.DamageSourceInfo damageSourceInfo = ((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GenericInstance) data).master.GetComponent<BuildingHP>().GetDamageSourceInfo();
        return str.Replace("{DamageInfo}", damageSourceInfo.ToString());
      });
      this.PendingRepair.conditionalOverlayCallback = (Func<HashedString, object, bool>) ((mode, data) => true);
      this.RequiresSkillPerk = this.CreateStatusItem("RequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.RequiresSkillPerk.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string id = (string) data;
        SkillPerk perk = Db.Get().SkillPerks.Get(id);
        List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
        List<string> stringList = new List<string>();
        foreach (Skill skill in skillsWithPerk)
          stringList.Add(skill.Name);
        str = str.Replace("{Skills}", string.Join(", ", stringList.ToArray()));
        return str;
      });
      this.DigRequiresSkillPerk = this.CreateStatusItem("DigRequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.DigRequiresSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
      this.ColonyLacksRequiredSkillPerk = this.CreateStatusItem("ColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.ColonyLacksRequiredSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
      this.SwitchStatusActive = this.CreateStatusItem("SwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.SwitchStatusInactive = this.CreateStatusItem("SwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.LogicSwitchStatusActive = this.CreateStatusItem("LogicSwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.LogicSwitchStatusInactive = this.CreateStatusItem("LogicSwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.LogicSensorStatusActive = this.CreateStatusItem("LogicSensorStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.LogicSensorStatusInactive = this.CreateStatusItem("LogicSensorStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingFish = this.CreateStatusItem("PendingFish", "BUILDING", "status_item_pending_fish", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingSwitchToggle = this.CreateStatusItem("PendingSwitchToggle", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingUpgrade = this.CreateStatusItem("PendingUpgrade", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PendingWork = this.CreateStatusItem("PendingWork", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.PowerButtonOff = this.CreateStatusItem("PowerButtonOff", "BUILDING", "status_item_power_button_off", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.PressureOk = this.CreateStatusItem("PressureOk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID);
      this.UnderPressure = this.CreateStatusItem("UnderPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID);
      this.Unassigned = this.CreateStatusItem("Unassigned", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Rooms.ID);
      this.AssignedPublic = this.CreateStatusItem("AssignedPublic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Rooms.ID);
      this.UnderConstruction = this.CreateStatusItem("UnderConstruction", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.UnderConstructionNoWorker = this.CreateStatusItem("UnderConstructionNoWorker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Normal = this.CreateStatusItem("Normal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ManualGeneratorChargingUp = this.CreateStatusItem("ManualGeneratorChargingUp", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.ManualGeneratorReleasingEnergy = this.CreateStatusItem("ManualGeneratorReleasingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.GeneratorOffline = this.CreateStatusItem("GeneratorOffline", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.Pipe = this.CreateStatusItem("Pipe", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID);
      this.Pipe.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Conduit conduit = (Conduit) data;
        int cell = Grid.PosToCell((KMonoBehaviour) conduit);
        ConduitFlow.ConduitContents contents = conduit.GetFlowManager().GetContents(cell);
        string newValue = (string) BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
        if ((double) contents.mass > 0.0)
        {
          Element elementByHash = ElementLoader.FindElementByHash(contents.element);
          newValue = string.Format((string) BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS, (object) GameUtil.GetFormattedMass(contents.mass), (object) elementByHash.name, (object) GameUtil.GetFormattedTemperature(contents.temperature));
          if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && contents.diseaseIdx != byte.MaxValue)
            newValue += string.Format((string) BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS_WITH_DISEASE, (object) GameUtil.GetFormattedDisease(contents.diseaseIdx, contents.diseaseCount, true));
        }
        str = str.Replace("{Contents}", newValue);
        return str;
      });
      this.Conveyor = this.CreateStatusItem("Conveyor", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.SolidConveyor.ID);
      this.Conveyor.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        int cell = Grid.PosToCell((KMonoBehaviour) data);
        SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
        SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
        string newValue = (string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
        if (contents.pickupableHandle.IsValid())
        {
          Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
          if ((bool) (UnityEngine.Object) pickupable)
          {
            PrimaryElement component = pickupable.GetComponent<PrimaryElement>();
            float mass = component.Mass;
            if ((double) mass > 0.0)
            {
              newValue = string.Format((string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS, (object) GameUtil.GetFormattedMass(mass), (object) pickupable.GetProperName(), (object) GameUtil.GetFormattedTemperature(component.Temperature));
              if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && component.DiseaseIdx != byte.MaxValue)
                newValue += string.Format((string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE, (object) GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true));
            }
          }
        }
        str = str.Replace("{Contents}", newValue);
        return str;
      });
      this.FabricatorIdle = this.CreateStatusItem("FabricatorIdle", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.FabricatorEmpty = this.CreateStatusItem("FabricatorEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Toilet = this.CreateStatusItem("Toilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Toilet.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Toilet.StatesInstance statesInstance = (global::Toilet.StatesInstance) data;
        if (statesInstance != null)
          str = str.Replace("{FlushesRemaining}", statesInstance.GetFlushesRemaining().ToString());
        return str;
      });
      this.ToiletNeedsEmptying = this.CreateStatusItem("ToiletNeedsEmptying", "BUILDING", "status_item_toilet_needs_emptying", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.DesalinatorNeedsEmptying = this.CreateStatusItem("DesalinatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Unusable = this.CreateStatusItem("Unusable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoResearchSelected = this.CreateStatusItem("NoResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoResearchSelected.AddNotification();
      this.NoResearchSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
        str = str.Replace("{RESEARCH_MENU_KEY}", newValue);
        return str;
      });
      this.NoResearchSelected.notificationClickCallback = (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenResearch());
      this.NoApplicableResearchSelected = this.CreateStatusItem("NoApplicableResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoApplicableResearchSelected.AddNotification();
      this.NoApplicableAnalysisSelected = this.CreateStatusItem("NoApplicableAnalysisSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoApplicableAnalysisSelected.AddNotification();
      this.NoApplicableAnalysisSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
        str = str.Replace("{STARMAP_MENU_KEY}", newValue);
        return str;
      });
      this.NoApplicableAnalysisSelected.notificationClickCallback = (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenStarmap());
      this.NoResearchOrDestinationSelected = this.CreateStatusItem("NoResearchOrDestinationSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoResearchOrDestinationSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue1 = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
        str = str.Replace("{STARMAP_MENU_KEY}", newValue1);
        string newValue2 = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
        str = str.Replace("{RESEARCH_MENU_KEY}", newValue2);
        return str;
      });
      this.NoResearchOrDestinationSelected.AddNotification();
      this.ValveRequest = this.CreateStatusItem("ValveRequest", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.ValveRequest.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Valve valve = (Valve) data;
        str = str.Replace("{QueuedMaxFlow}", GameUtil.GetFormattedMass(valve.QueuedMaxFlow, GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.EmittingLight = this.CreateStatusItem("EmittingLight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.EmittingLight.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.Overlay5).mKeyCode.ToString();
        str = str.Replace("{LightGridOverlay}", newValue);
        return str;
      });
      this.RationBoxContents = this.CreateStatusItem("RationBoxContents", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.RationBoxContents.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        RationBox rationBox = (RationBox) data;
        if ((UnityEngine.Object) rationBox == (UnityEngine.Object) null)
          return str;
        Storage component1 = rationBox.GetComponent<Storage>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
          return str;
        float calories = 0.0f;
        foreach (GameObject gameObject in component1.items)
        {
          Edible component2 = gameObject.GetComponent<Edible>();
          if ((bool) (UnityEngine.Object) component2)
            calories += component2.Calories;
        }
        str = str.Replace("{Stored}", GameUtil.GetFormattedCalories(calories));
        return str;
      });
      this.EmittingElement = this.CreateStatusItem("EmittingElement", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.EmittingElement.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IElementEmitter elementEmitter = (IElementEmitter) data;
        string newValue = ElementLoader.FindElementByHash(elementEmitter.Element).tag.ProperName();
        str = str.Replace("{ElementType}", newValue);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.AverageEmitRate, GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.EmittingOxygenAvg = this.CreateStatusItem("EmittingOxygenAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.EmittingOxygenAvg.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Sublimates sublimates = (Sublimates) data;
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.EmittingGasAvg = this.CreateStatusItem("EmittingGasAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.EmittingGasAvg.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Sublimates sublimates = (Sublimates) data;
        str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.PumpingLiquidOrGas = this.CreateStatusItem("PumpingLiquidOrGas", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID);
      this.PumpingLiquidOrGas.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        float averageRate = Game.Instance.accumulators.GetAverageRate((HandleVector<int>.Handle) data);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(averageRate, GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.PipeMayMelt = this.CreateStatusItem("PipeMayMelt", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoLiquidElementToPump = this.CreateStatusItem("NoLiquidElementToPump", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID);
      this.NoGasElementToPump = this.CreateStatusItem("NoGasElementToPump", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID);
      this.NoFilterElementSelected = this.CreateStatusItem("NoFilterElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NoLureElementSelected = this.CreateStatusItem("NoLureElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.ElementConsumer = this.CreateStatusItem("ElementConsumer", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.ElementConsumer.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::ElementConsumer elementConsumer = (global::ElementConsumer) data;
        string newValue = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag.ProperName();
        str = str.Replace("{ElementTypes}", newValue);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementConsumer.AverageConsumeRate, GameUtil.TimeSlice.PerSecond));
        return str;
      });
      this.ElementEmitterOutput = this.CreateStatusItem("ElementEmitterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.ElementEmitterOutput.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        ElementEmitter elementEmitter = (ElementEmitter) data;
        if ((UnityEngine.Object) elementEmitter != (UnityEngine.Object) null)
        {
          str = str.Replace("{ElementTypes}", elementEmitter.outputElement.Name);
          str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.outputElement.massGenerationRate / elementEmitter.emissionFrequency, GameUtil.TimeSlice.PerSecond));
        }
        return str;
      });
      this.AwaitingWaste = this.CreateStatusItem("AwaitingWaste", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.AwaitingCompostFlip = this.CreateStatusItem("AwaitingCompostFlip", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.JoulesAvailable = this.CreateStatusItem("JoulesAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.JoulesAvailable.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Battery battery = (Battery) data;
        str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(battery.JoulesAvailable));
        str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(battery.Capacity));
        return str;
      });
      this.Wattage = this.CreateStatusItem("Wattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.Wattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Generator generator = (Generator) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(generator.WattageRating));
        return str;
      });
      this.SolarPanelWattage = this.CreateStatusItem("SolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.SolarPanelWattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SolarPanel solarPanel = (SolarPanel) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage));
        return str;
      });
      this.SteamTurbineWattage = this.CreateStatusItem("SteamTurbineWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.SteamTurbineWattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SteamTurbine steamTurbine = (SteamTurbine) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage));
        return str;
      });
      this.Wattson = this.CreateStatusItem("Wattson", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Wattson.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Telepad telepad = (Telepad) data;
        str = !((UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null) || !GameFlowManager.Instance.IsGameOver() ? (!telepad.GetComponent<Operational>().IsOperational ? str.Replace("{TimeRemaining}", (string) BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE) : str.Replace("{TimeRemaining}", GameUtil.GetFormattedCycles(telepad.GetTimeRemaining()))) : (string) BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME;
        return str;
      });
      this.FlushToilet = this.CreateStatusItem("FlushToilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.FlushToiletInUse = this.CreateStatusItem("FlushToiletInUse", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.WireNominal = this.CreateStatusItem("WireNominal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.WireConnected = this.CreateStatusItem("WireConnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID);
      this.WireDisconnected = this.CreateStatusItem("WireDisconnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID);
      this.Overheated = this.CreateStatusItem("Overheated", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
      this.Overloaded = this.CreateStatusItem("Overloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
      this.LogicOverloaded = this.CreateStatusItem("LogicOverloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
      this.Cooling = this.CreateStatusItem("Cooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      Func<string, object, string> func2 = (Func<string, object, string>) ((str, data) =>
      {
        AirConditioner airConditioner = (AirConditioner) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp));
      });
      this.CoolingStalledColdGas = this.CreateStatusItem("CoolingStalledColdGas", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.CoolingStalledColdGas.resolveStringCallback = func2;
      this.CoolingStalledColdLiquid = this.CreateStatusItem("CoolingStalledColdLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.CoolingStalledColdLiquid.resolveStringCallback = func2;
      Func<string, object, string> func3 = (Func<string, object, string>) ((str, data) =>
      {
        AirConditioner airConditioner = (AirConditioner) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(airConditioner.lastEnvTemp), (object) GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp), (object) GameUtil.GetFormattedTemperature(airConditioner.maxEnvironmentDelta, interpretation: GameUtil.TemperatureInterpretation.Relative));
      });
      this.CoolingStalledHotEnv = this.CreateStatusItem("CoolingStalledHotEnv", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.CoolingStalledHotEnv.resolveStringCallback = func3;
      this.CoolingStalledHotLiquid = this.CreateStatusItem("CoolingStalledHotLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.CoolingStalledHotLiquid.resolveStringCallback = func3;
      this.MissingRequirements = this.CreateStatusItem("MissingRequirements", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.GettingReady = this.CreateStatusItem("GettingReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Working = this.CreateStatusItem("Working", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NeedsValidRegion = this.CreateStatusItem("NeedsValidRegion", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NeedSeed = this.CreateStatusItem("NeedSeed", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.AwaitingSeedDelivery = this.CreateStatusItem("AwaitingSeedDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.AwaitingBaitDelivery = this.CreateStatusItem("AwaitingBaitDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NoAvailableSeed = this.CreateStatusItem("NoAvailableSeed", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.NeedEgg = this.CreateStatusItem("NeedEgg", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.AwaitingEggDelivery = this.CreateStatusItem("AwaitingEggDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.NoAvailableEgg = this.CreateStatusItem("NoAvailableEgg", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.Grave = this.CreateStatusItem("Grave", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Grave.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Grave.StatesInstance statesInstance = (global::Grave.StatesInstance) data;
        string str1 = str.Replace("{DeadDupe}", statesInstance.master.graveName);
        string[] strings = LocString.GetStrings(typeof (NAMEGEN.GRAVE.EPITAPHS));
        int index = statesInstance.master.epitaphIdx % strings.Length;
        string newValue = strings[index];
        return str1.Replace("{Epitaph}", newValue);
      });
      this.GraveEmpty = this.CreateStatusItem("GraveEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.CannotCoolFurther = this.CreateStatusItem("CannotCoolFurther", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.BuildingDisabled = this.CreateStatusItem("BuildingDisabled", "BUILDING", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Expired = this.CreateStatusItem("Expired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PumpingStation = this.CreateStatusItem("PumpingStation", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.PumpingStation.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        LiquidPumpingStation liquidPumpingStation = (LiquidPumpingStation) data;
        return (UnityEngine.Object) liquidPumpingStation != (UnityEngine.Object) null ? liquidPumpingStation.ResolveString(str) : str;
      });
      this.EmptyPumpingStation = this.CreateStatusItem("EmptyPumpingStation", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.WellPressurizing = this.CreateStatusItem("WellPressurizing", (string) BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME, (string) BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.WellPressurizing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        OilWellCap.StatesInstance statesInstance = (OilWellCap.StatesInstance) data;
        return statesInstance != null ? string.Format(str, (object) GameUtil.GetFormattedPercent(100f * statesInstance.GetPressurePercent())) : str;
      });
      this.WellOverpressure = this.CreateStatusItem("WellOverpressure", (string) BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME, (string) BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.ReleasingPressure = this.CreateStatusItem("ReleasingPressure", (string) BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME, (string) BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.TooCold = this.CreateStatusItem("TooCold", (string) BUILDING.STATUSITEMS.TOO_COLD.NAME, (string) BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.IncubatorProgress = this.CreateStatusItem("IncubatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.IncubatorProgress.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        EggIncubator eggIncubator = (EggIncubator) data;
        str = str.Replace("{Percent}", GameUtil.GetFormattedPercent(eggIncubator.GetProgress() * 100f));
        return str;
      });
      this.HabitatNeedsEmptying = this.CreateStatusItem("HabitatNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.DetectorScanning = this.CreateStatusItem("DetectorScanning", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.IncomingMeteors = this.CreateStatusItem("IncomingMeteors", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.HasGantry = this.CreateStatusItem("HasGantry", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.MissingGantry = this.CreateStatusItem("MissingGantry", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.DisembarkingDuplicant = this.CreateStatusItem("DisembarkingDuplicant", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.RocketName = this.CreateStatusItem("RocketName", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.RocketName.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        RocketModule rocketModule = (RocketModule) data;
        return (UnityEngine.Object) rocketModule != (UnityEngine.Object) null ? str.Replace("{0}", rocketModule.GetParentRocketName()) : str;
      });
      this.RocketName.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        RocketModule rocketModule = (RocketModule) data;
        return (UnityEngine.Object) rocketModule != (UnityEngine.Object) null ? str.Replace("{0}", rocketModule.GetParentRocketName()) : str;
      });
      this.PathNotClear = new StatusItem("PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.PathNotClear.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        ConditionFlightPathIsClear flightPathIsClear = (ConditionFlightPathIsClear) data;
        if (flightPathIsClear != null)
          str = string.Format(str, (object) flightPathIsClear.GetObstruction());
        return str;
      });
      this.InvalidPortOverlap = this.CreateStatusItem("InvalidPortOverlap", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.InvalidPortOverlap.AddNotification();
      this.EmergencyPriority = this.CreateStatusItem("EmergencyPriority", (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP, "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID);
      this.EmergencyPriority.AddNotification(notification_text: ((string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME), notification_tooltip: ((string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP));
      this.SkillPointsAvailable = this.CreateStatusItem("SkillPointsAvailable", (string) BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME, (string) BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP, "status_item_jobs", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.Baited = this.CreateStatusItem("Baited", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.Baited.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element elementByName = ElementLoader.FindElementByName(((StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GenericInstance) data).master.baitElement.ToString());
        str = str.Replace("{0}", elementByName.name);
        return str;
      });
      this.Baited.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element elementByName = ElementLoader.FindElementByName(((StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GenericInstance) data).master.baitElement.ToString());
        str = str.Replace("{0}", elementByName.name);
        return str;
      });
      this.TanningLightSufficient = this.CreateStatusItem("TanningLightSufficient", (string) BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.NAME, (string) BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.TanningLightInsufficient = this.CreateStatusItem("TanningLightInsufficient", (string) BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.NAME, (string) BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.HotTubWaterTooCold = this.CreateStatusItem("HotTubWaterTooCold", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
      this.HotTubWaterTooCold.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        HotTub hotTub = (HotTub) data;
        str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.minimumWaterTemperature));
        return str;
      });
      this.HotTubTooHot = this.CreateStatusItem("HotTubTooHot", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
      this.HotTubTooHot.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        HotTub hotTub = (HotTub) data;
        str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.maxOperatingTemperature));
        return str;
      });
      this.HotTubFilling = this.CreateStatusItem("HotTubFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.HotTubFilling.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        HotTub hotTub = (HotTub) data;
        str = str.Replace("{fullness}", GameUtil.GetFormattedPercent(hotTub.PercentFull));
        return str;
      });
      this.WindTunnelIntake = this.CreateStatusItem("WindTunnelIntake", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    }

    private static bool ShowInUtilityOverlay(HashedString mode, object data)
    {
      Transform transform = (Transform) data;
      bool flag = false;
      if (mode == OverlayModes.GasConduits.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.GasVentIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.LiquidConduits.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.LiquidVentIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.Power.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.WireIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.Logic.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.SolidConveyor.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.SolidConveyorIDs.Contains(prefabTag);
      }
      return flag;
    }
  }
}
