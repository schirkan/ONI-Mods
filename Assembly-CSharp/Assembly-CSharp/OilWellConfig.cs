// Decompiled with JetBrains decompiler
// Type: OilWellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OilWellConfig : IEntityConfig
{
  public const string ID = "OilWell";

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.OIL_WELL.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.OIL_WELL.DESC;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues tieR5 = TUNING.NOISE_POLLUTION.NOISY.TIER5;
    KAnimFile anim = Assets.GetAnim((HashedString) "geyser_side_oil_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = tieR5;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("OilWell", name, desc, 2000f, anim, "off", Grid.SceneLayer.BuildingBack, 4, 2, decor, noise);
    placedEntity.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.SedimentaryRock);
    component.Temperature = 372.15f;
    placedEntity.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.OilWell, (AttachableBuilding) null)
    };
    SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", TUNING.NOISE_POLLUTION.NOISY.TIER5);
    SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", TUNING.NOISE_POLLUTION.NOISY.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
