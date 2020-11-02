// Decompiled with JetBrains decompiler
// Type: BaseMooConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public static class BaseMooConfig
{
  public static GameObject BaseMoo(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    bool is_baby,
    string symbol_override_prefix)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 50f, anim, "idle_loop", Grid.SceneLayer.Creatures, 2, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, initialTraitID: traitId, NavGridName: "FlyerNavGrid2x2", navType: NavType.Hover, onDeathDropCount: 10, warningLowTemperature: 123.15f, warningHighTemperature: 423.15f, lethalLowTemperature: 73.14999f, lethalHighTemperature: 473.15f);
    if (!string.IsNullOrEmpty(symbol_override_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_override_prefix);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      SimHashes.BleachStone.CreateTag()
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false);
    placedEntity.AddOrGetDef<RanchableMonitor.Def>();
    placedEntity.AddOrGetDef<FixedCapturableMonitor.Def>();
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def()).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP)).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMooConfig.CustomIdleAnim)
    });
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.MooSpecies, symbol_override_prefix);
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
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>() { consumed_tag }, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    });
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = minPoopSizeInKg * caloriesPerKg;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
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
