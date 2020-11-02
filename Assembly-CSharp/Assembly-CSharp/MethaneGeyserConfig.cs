// Decompiled with JetBrains decompiler
// Type: MethaneGeyserConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MethaneGeyserConfig : IEntityConfig
{
  public const string ID = "MethaneGeyser";

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.METHANEGEYSER.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.METHANEGEYSER.DESC;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues tieR5 = TUNING.NOISE_POLLUTION.NOISY.TIER5;
    KAnimFile anim = Assets.GetAnim((HashedString) "geyser_side_methane_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = tieR5;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MethaneGeyser", name, desc, 2000f, anim, "inactive", Grid.SceneLayer.BuildingBack, 4, 2, decor, noise);
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.IgneousRock);
    component.Temperature = 372.15f;
    placedEntity.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
    GeyserConfigurator geyserConfigurator = placedEntity.AddOrGet<GeyserConfigurator>();
    geyserConfigurator.presetType = (HashedString) "methane";
    geyserConfigurator.presetMin = 0.35f;
    geyserConfigurator.presetMax = 0.65f;
    Studyable studyable = placedEntity.AddOrGet<Studyable>();
    studyable.meterTrackerSymbol = "geotracker_target";
    studyable.meterAnim = "tracker";
    placedEntity.AddOrGet<LoopingSounds>();
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
