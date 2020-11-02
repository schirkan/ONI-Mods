﻿// Decompiled with JetBrains decompiler
// Type: ScaleGrowthMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGrowthMonitor : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
  public ScaleGrowthMonitor.GrowingState growing;
  public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;
  private AttributeModifier scaleGrowthModifier;
  private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[5]
  {
    (HashedString) "scale_0",
    (HashedString) "scale_1",
    (HashedString) "scale_2",
    (HashedString) "scale_3",
    (HashedString) "scale_4"
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    this.root.Enter((StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback) (smi => ScaleGrowthMonitor.UpdateScales(smi, 0.0f))).Update(new System.Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales), UpdateRate.SIM_1000ms);
    this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
    this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere)), UpdateRate.SIM_1000ms).Enter(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier));
    this.growing.stunted.Transition(this.growing.growing, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere), UpdateRate.SIM_1000ms).ToggleStatusItem((string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME, (string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.fullyGrown.ToggleTag(GameTags.Creatures.ScalesGrown).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasTag(GameTags.Creatures.CanMolt))).Transition((GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State) this.growing, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
  }

  private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
  {
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    return Grid.Element[cell].id == smi.def.targetAtmosphere;
  }

  private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi) => (double) smi.scaleGrowth.value >= (double) smi.scaleGrowth.GetMax();

  private static void ApplyModifier(ScaleGrowthMonitor.Instance smi) => smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);

  private static void RemoveModifier(ScaleGrowthMonitor.Instance smi) => smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);

  private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
  {
    int num = (int) ((double) smi.def.levelCount * (double) smi.scaleGrowth.value / 100.0);
    if (smi.currentScaleLevel == num)
      return;
    KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
    for (int index = 0; index < ScaleGrowthMonitor.SCALE_SYMBOL_NAMES.Length; ++index)
    {
      bool is_visible = index <= num - 1;
      component.SetSymbolVisiblity((KAnimHashedString) ScaleGrowthMonitor.SCALE_SYMBOL_NAMES[index], is_visible);
    }
    smi.currentScaleLevel = num;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public int levelCount;
    public float defaultGrowthRate;
    public SimHashes targetAtmosphere;
    public Tag itemDroppedOnShear;
    public float dropMass;

    public override void Configure(GameObject prefab) => prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      if (this.targetAtmosphere == (SimHashes) 0)
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate))));
      else
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName())));
      return descriptorList;
    }
  }

  public class GrowingState : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
  {
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
  }

  public new class Instance : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance
  {
    public AmountInstance scaleGrowth;
    public AttributeModifier scaleGrowthModifier;
    public int currentScaleLevel = -1;

    public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def)
      : base(master, def)
    {
      this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(this.gameObject);
      this.scaleGrowth.value = this.scaleGrowth.GetMax();
      this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, (string) CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME);
    }

    public bool IsFullyGrown() => this.currentScaleLevel == this.def.levelCount;

    public void Shear()
    {
      PrimaryElement component1 = this.smi.GetComponent<PrimaryElement>();
      GameObject go = Util.KInstantiate(Assets.GetPrefab(this.def.itemDroppedOnShear));
      go.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell((StateMachine.Instance) this)), Grid.SceneLayer.Ore));
      PrimaryElement component2 = go.GetComponent<PrimaryElement>();
      component2.Temperature = component1.Temperature;
      component2.Mass = this.def.dropMass;
      component2.AddDisease(component1.DiseaseIdx, component1.DiseaseCount, "Shearing");
      go.SetActive(true);
      Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, (float) ((double) UnityEngine.Random.value * 2.0 + 2.0));
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, initial_velocity);
      this.scaleGrowth.value = 0.0f;
      ScaleGrowthMonitor.UpdateScales(this, 0.0f);
    }
  }
}