// Decompiled with JetBrains decompiler
// Type: CommandModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CommandModuleConfig : IBuildingConfig
{
  public const string ID = "CommandModule";
  private const string TRIGGER_LAUNCH_PORT_ID = "TriggerLaunch";
  private const string LAUNCH_READY_PORT_ID = "LaunchReady";

  public override BuildingDef CreateBuildingDef()
  {
    float[] commandModuleMass = TUNING.BUILDINGS.ROCKETRY_MASS_KG.COMMAND_MODULE_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = TUNING.NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CommandModule", 5, 5, "rocket_command_module_kanim", 1000, 60f, commandModuleMass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort((HashedString) "TriggerLaunch", new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_INACTIVE)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "LaunchReady", new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_INACTIVE)
    };
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_command_module_bg_kanim"));
    LaunchConditionManager conditionManager = go.AddOrGet<LaunchConditionManager>();
    conditionManager.triggerPort = (HashedString) "TriggerLaunch";
    conditionManager.statusPort = (HashedString) "LaunchReady";
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    go.AddOrGet<CommandModule>();
    go.AddOrGet<CommandModuleWorkable>();
    go.AddOrGet<MinionStorage>();
    go.AddOrGet<ArtifactFinder>();
    go.AddOrGet<LaunchableRocket>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.RocketCommandModule.Id;
    ownable.canBePublic = false;
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
