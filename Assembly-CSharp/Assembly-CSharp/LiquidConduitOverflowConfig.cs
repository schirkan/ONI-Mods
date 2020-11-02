﻿// Decompiled with JetBrains decompiler
// Type: LiquidConduitOverflowConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidConduitOverflowConfig : IBuildingConfig
{
  public const string ID = "LiquidConduitOverflow";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
  private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidConduitOverflow", 2, 2, "valveliquid_kanim", 10, 3f, tieR1, rawMinerals, 1600f, BuildLocationRule.Conduit, none2, noise);
    buildingDef.Deprecated = true;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, buildingDef.PrefabID);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AttachPort(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AttachPort(go);
  }

  private void AttachPort(GameObject go) => go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<ConduitOverflow>().portInfo = this.secondaryPort;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<RequireInputs>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
