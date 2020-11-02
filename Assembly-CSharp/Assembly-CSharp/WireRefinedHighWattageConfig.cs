// Decompiled with JetBrains decompiler
// Type: WireRefinedHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedHighWattageConfig : BaseWireConfig
{
  public const string ID = "WireRefinedHighWattage";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = BUILDINGS.DECOR.PENALTY.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = this.CreateBuildingDef("WireRefinedHighWattage", "utilities_electric_conduct_hiwatt_kanim", 3f, tieR2, 0.05f, tieR3, noise);
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go) => this.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }
}
