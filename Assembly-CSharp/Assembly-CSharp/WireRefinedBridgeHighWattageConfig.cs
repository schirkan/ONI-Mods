﻿// Decompiled with JetBrains decompiler
// Type: WireRefinedBridgeHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
  public new const string ID = "WireRefinedBridgeHighWattage";

  protected override string GetID() => "WireRefinedBridgeHighWattage";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = base.CreateBuildingDef();
    buildingDef.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "heavywatttile_conductive_kanim")
    };
    buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
    return buildingDef;
  }

  protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
    return utilityNetworkLink;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }
}
