﻿// Decompiled with JetBrains decompiler
// Type: OvercrowdingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;

public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
  public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;
  public static Effect futureOvercrowdedEffect;
  public static Effect overcrowdedEffect;
  public static Effect stuckEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update(new System.Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
    OvercrowdingMonitor.futureOvercrowdedEffect = new Effect("FutureOvercrowded", (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0.0f, true, false, true);
    OvercrowdingMonitor.futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, true));
    OvercrowdingMonitor.overcrowdedEffect = new Effect("Overcrowded", (string) CREATURES.MODIFIERS.OVERCROWDED.NAME, (string) CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0.0f, true, false, true);
    OvercrowdingMonitor.overcrowdedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, (string) CREATURES.MODIFIERS.OVERCROWDED.NAME));
    OvercrowdingMonitor.stuckEffect = new Effect("Confined", (string) CREATURES.MODIFIERS.CONFINED.NAME, (string) CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0.0f, true, false, true);
    OvercrowdingMonitor.stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, (string) CREATURES.MODIFIERS.CONFINED.NAME));
  }

  private static bool IsConfined(OvercrowdingMonitor.Instance smi) => !smi.HasTag(GameTags.Creatures.Burrowed) && !smi.HasTag(GameTags.Creatures.Digger) && (smi.cavity == null || smi.cavity.numCells < smi.def.spaceRequiredPerCreature);

  private static bool IsFutureOvercrowded(OvercrowdingMonitor.Instance smi)
  {
    if (smi.def.spaceRequiredPerCreature == 0 || smi.cavity == null)
      return false;
    int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
    return num != 0 && smi.cavity.eggs.Count != 0 && smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
  }

  private static bool IsOvercrowded(OvercrowdingMonitor.Instance smi)
  {
    if (smi.def.spaceRequiredPerCreature == 0)
      return false;
    FishOvercrowdingMonitor.Instance smi1 = smi.GetSMI<FishOvercrowdingMonitor.Instance>();
    if (smi1 != null)
    {
      int fishCount = smi1.fishCount;
      return fishCount > 0 && smi1.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
    }
    return smi.cavity != null && smi.cavity.creatures.Count > 1 && smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
  }

  private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
  {
    OvercrowdingMonitor.UpdateCavity(smi, dt);
    bool set1 = OvercrowdingMonitor.IsConfined(smi);
    bool set2 = OvercrowdingMonitor.IsOvercrowded(smi);
    bool set3 = !smi.isBaby && OvercrowdingMonitor.IsFutureOvercrowded(smi);
    KPrefabID component = smi.gameObject.GetComponent<KPrefabID>();
    component.SetTag(GameTags.Creatures.Confined, set1);
    component.SetTag(GameTags.Creatures.Overcrowded, set2);
    component.SetTag(GameTags.Creatures.Expecting, set3);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.stuckEffect, set1);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.overcrowdedEffect, !set1 & set2);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.futureOvercrowdedEffect, !set1 & set3);
  }

  private static void SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set)
  {
    Effects component = smi.GetComponent<Effects>();
    if (set)
      component.Add(effect, false);
    else
      component.Remove(effect);
  }

  private static List<KPrefabID> GetCreatureCollection(
    OvercrowdingMonitor.Instance smi,
    CavityInfo cavity_info)
  {
    return smi.HasTag(GameTags.Egg) ? cavity_info.eggs : cavity_info.creatures;
  }

  private static void UpdateCavity(OvercrowdingMonitor.Instance smi, float dt)
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi));
    if (cavityForCell == smi.cavity)
      return;
    KPrefabID component = smi.GetComponent<KPrefabID>();
    if (smi.cavity != null)
    {
      OvercrowdingMonitor.GetCreatureCollection(smi, smi.cavity).Remove(component);
      Game.Instance.roomProber.UpdateRoom(cavityForCell);
    }
    smi.cavity = cavityForCell;
    if (smi.cavity == null)
      return;
    OvercrowdingMonitor.GetCreatureCollection(smi, smi.cavity).Add(component);
    Game.Instance.roomProber.UpdateRoom(smi.cavity);
  }

  public class Def : StateMachine.BaseDef
  {
    public int spaceRequiredPerCreature;
  }

  public new class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
  {
    public CavityInfo cavity;
    public bool isBaby;

    public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def)
      : base(master, def)
      => this.isBaby = master.gameObject.GetDef<BabyMonitor.Def>() != null;

    protected override void OnCleanUp()
    {
      KPrefabID component = this.master.GetComponent<KPrefabID>();
      if (this.cavity == null)
        return;
      OvercrowdingMonitor.GetCreatureCollection(this, this.cavity).Remove(component);
    }
  }
}
