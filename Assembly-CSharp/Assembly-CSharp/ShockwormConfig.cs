// Decompiled with JetBrains decompiler
// Type: ShockwormConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShockwormConfig : IEntityConfig
{
  public const string ID = "ShockWorm";

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.SHOCKWORM.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.SHOCKWORM.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "shockworm_kanim");
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ShockWorm", name, desc, 50f, anim, "idle", Grid.SceneLayer.Creatures, 1, 2, decor, noise);
    float freezing2 = TUNING.CREATURES.TEMPERATURE.FREEZING_2;
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Hostile, NavGridName: "FlyerNavGrid1x2", navType: NavType.Hover, onDeathDropCount: 3, warningLowTemperature: TUNING.CREATURES.TEMPERATURE.FREEZING_1, warningHighTemperature: TUNING.CREATURES.TEMPERATURE.HOT_1, lethalLowTemperature: freezing2, lethalHighTemperature: TUNING.CREATURES.TEMPERATURE.HOT_2);
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddWeapon(3f, 6f, targetType: AttackProperties.TargetType.AreaOfEffect, maxHits: 10, aoeRadius: 4f).AddEffect();
    SoundEventVolumeCache.instance.AddVolume("shockworm_kanim", "Shockworm_attack_arc", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
