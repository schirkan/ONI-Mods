// Decompiled with JetBrains decompiler
// Type: WireHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireHighWattageConfig : BaseWireConfig
{
  public const string ID = "HighWattageWire";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR5 = BUILDINGS.DECOR.PENALTY.TIER5;
    EffectorValues noise = none;
    BuildingDef buildingDef = this.CreateBuildingDef("HighWattageWire", "utilities_electric_insulated_kanim", 3f, tieR2, 0.05f, tieR5, noise);
    buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go) => this.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);

  public override void DoPostConfigureUnderConstruction(GameObject go) => base.DoPostConfigureUnderConstruction(go);
}
