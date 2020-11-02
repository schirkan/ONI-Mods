// Decompiled with JetBrains decompiler
// Type: BaseLightBugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class BaseLightBugConfig
{
  public static GameObject BaseLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    Color lightColor,
    EffectorValues decor,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues effectorValues = decor;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor1 = effectorValues;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 5f, anim, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor1, noise);
    GameObject template = placedEntity;
    string initialTraitID = traitId;
    float freezing2 = CREATURES.TEMPERATURE.FREEZING_2;
    double freezing1 = (double) CREATURES.TEMPERATURE.FREEZING_1;
    double hot1 = (double) CREATURES.TEMPERATURE.HOT_1;
    double num = (double) freezing2;
    double hot2 = (double) CREATURES.TEMPERATURE.HOT_2;
    EntityTemplates.ExtendEntityToBasicCreature(template, initialTraitID: initialTraitID, NavGridName: "FlyerNavGrid1x1", navType: NavType.Hover, onDeathDropCount: 0, warningLowTemperature: ((float) freezing1), warningHighTemperature: ((float) hot1), lethalLowTemperature: ((float) num), lethalHighTemperature: ((float) hot2));
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    KPrefabID component1 = placedEntity.GetComponent<KPrefabID>();
    component1.AddTag(GameTags.Creatures.Flyer);
    component1.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Phosphorite
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false);
    if (is_baby)
    {
      KBatchedAnimController component2 = placedEntity.GetComponent<KBatchedAnimController>();
      component2.animWidth = 0.5f;
      component2.animHeight = 0.5f;
    }
    if (lightColor != Color.black)
    {
      Light2D light2D = placedEntity.AddOrGet<Light2D>();
      light2D.Color = lightColor;
      light2D.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
      light2D.Range = 5f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
      light2D.Offset = LIGHT2D.LIGHTBUG_OFFSET;
      light2D.shape = LightShape.Circle;
      light2D.drawOverlay = true;
      light2D.Lux = 1800;
      placedEntity.AddOrGet<LightSymbolTracker>().targetSymbol = (HashedString) "snapTo_light_locator";
      placedEntity.AddOrGetDef<CreatureLightToggleController.Def>();
    }
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def()).Add((StateMachine.BaseDef) new LayEggStates.Def()).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CallAdultStates.Def()).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.LightBugSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    HashSet<Tag> consumed_tags,
    Tag producedTag,
    float caloriesPerKg)
  {
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(consumed_tags, producedTag, caloriesPerKg)
    });
    prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  public static void SetupLoopingSounds(GameObject inst) => inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("ShineBug_wings_LP"));
}
