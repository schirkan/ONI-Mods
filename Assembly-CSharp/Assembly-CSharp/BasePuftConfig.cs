// Decompiled with JetBrains decompiler
// Type: BasePuftConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class BasePuftConfig
{
  public static GameObject BasePuft(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    bool is_baby,
    string symbol_override_prefix,
    float warningLowTemperature,
    float warningHighTemperature)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 50f, anim, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, initialTraitID: traitId, NavGridName: "FlyerNavGrid1x1", navType: NavType.Hover, warningLowTemperature: warningLowTemperature, warningHighTemperature: warningHighTemperature, lethalLowTemperature: (warningLowTemperature - 45f), lethalHighTemperature: (warningHighTemperature + 50f));
    if (!string.IsNullOrEmpty(symbol_override_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_override_prefix);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.SlimeMold
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_intake", NOISE_POLLUTION.CREATURES.TIER4);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_toot", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_inflated", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false);
    string str = "Puft_air_intake";
    if (is_baby)
      str = "PuftBaby_air_intake";
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def()).Add((StateMachine.BaseDef) new UpTopPoopStates.Def()).Add((StateMachine.BaseDef) new LayEggStates.Def()).Add((StateMachine.BaseDef) new InhaleStates.Def()
    {
      inhaleSound = str
    }).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CallAdultStates.Def()).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BasePuftConfig.CustomIdleAnim)
    });
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.PuftSpecies, symbol_override_prefix);
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Tag consumed_tag,
    Tag producedTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced,
    float minPoopSizeInKg)
  {
    Diet.Info[] diet_infos = new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>() { consumed_tag }, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
    return BasePuftConfig.SetupDiet(prefab, diet_infos, caloriesPerKg, minPoopSizeInKg);
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Diet.Info[] diet_infos,
    float caloriesPerKg,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos);
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = minPoopSizeInKg * caloriesPerKg;
    prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  private static HashedString CustomIdleAnim(
    IdleStates.Instance smi,
    ref HashedString pre_anim)
  {
    CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
    return (HashedString) (smi1 == null || !smi1.stomach.IsReadyToPoop() ? "idle_loop" : "idle_loop_full");
  }

  public static void OnSpawn(GameObject inst)
  {
    Navigator component = inst.GetComponent<Navigator>();
    component.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new FullPuftTransitionLayer(component));
  }
}
