// Decompiled with JetBrains decompiler
// Type: ItemPedestalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ItemPedestalConfig : IBuildingConfig
{
  public const string ID = "ItemPedestal";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ItemPedestal", 1, 2, "pedestal_kanim", 10, 30f, tieR2, rawMinerals, 800f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.DefaultAnimState = "pedestal";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "small";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>((IEnumerable<Storage.StoredItemModifier>) new Storage.StoredItemModifier[2]
    {
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Preserve
    }));
    Prioritizable.AddRef(go);
    SingleEntityReceptacle entityReceptacle = go.AddOrGet<SingleEntityReceptacle>();
    entityReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
    entityReceptacle.occupyingObjectRelativePosition = new Vector3(0.0f, 1.2f, -1f);
    go.AddOrGet<DecorProvider>();
    go.AddOrGet<ItemPedestal>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
