// Decompiled with JetBrains decompiler
// Type: WireRefinedBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedBridgeConfig : WireBridgeConfig
{
  public new const string ID = "WireRefinedBridge";

  protected override string GetID() => "WireRefinedBridge";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = base.CreateBuildingDef();
    buildingDef.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "utilityelectricbridgeconductive_kanim")
    };
    buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
    return buildingDef;
  }

  protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
    return utilityNetworkLink;
  }
}
