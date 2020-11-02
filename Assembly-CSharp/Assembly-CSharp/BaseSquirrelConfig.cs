// Decompiled with JetBrains decompiler
// Type: BaseSquirrelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public static class BaseSquirrelConfig
{
  public static GameObject BaseSquirrel(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 100f, anim, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    string NavGridName = "SquirrelNavGrid";
    if (is_baby)
      NavGridName = "DreckoBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Pest, traitId, NavGridName, entombVulnerable: false);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddComponent<Storage>();
    if (!is_baby)
    {
      placedEntity.AddOrGetDef<SeedPlantingMonitor.Def>();
      placedEntity.AddOrGetDef<ClimbableTreeMonitor.Def>();
    }
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", TUNING.NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", TUNING.NOISE_POLLUTION.CREATURES.TIER1);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", TUNING.NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", TUNING.NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def()).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def()).Add((StateMachine.BaseDef) new LayEggStates.Def()).Add((StateMachine.BaseDef) new TreeClimbStates.Def()).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP)).Add((StateMachine.BaseDef) new CallAdultStates.Def()).Add((StateMachine.BaseDef) new SeedPlantingStates.Def()).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.SquirrelSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static Diet.Info[] BasicWoodDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "ForestTree"
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, eats_plants_directly: true)
    };
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Diet.Info[] diet_infos,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos);
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = minPoopSizeInKg;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  private static int AdjustSpawnLocationCB(int cell)
  {
    int num;
    for (; !Grid.Solid[cell]; cell = num)
    {
      num = Grid.CellBelow(cell);
      if (!Grid.IsValidCell(cell))
        break;
    }
    return cell;
  }
}
