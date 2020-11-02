// Decompiled with JetBrains decompiler
// Type: WildnessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
  public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;
  public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;
  private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[2]
  {
    (KAnimHashedString) "tag",
    (KAnimHashedString) "snapto_tag"
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.tame;
    this.serializable = true;
    this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback) (smi => !WildnessMonitor.IsWild(smi)), UpdateRate.SIM_1000ms).ToggleEffect((Func<WildnessMonitor.Instance, Effect>) (smi => smi.def.wildEffect)).ToggleTag(GameTags.Creatures.Wild);
    this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((Func<WildnessMonitor.Instance, Effect>) (smi => smi.def.tameEffect));
  }

  private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
  {
    foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
      smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
  }

  private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
  {
    foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
      smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
  }

  private static bool IsWild(WildnessMonitor.Instance smi) => (double) smi.wildness.value > 0.0;

  private static void RefreshAmounts(WildnessMonitor.Instance smi)
  {
    bool flag = WildnessMonitor.IsWild(smi);
    smi.wildness.hide = !flag;
    Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject).hide = flag;
    Db.Get().Amounts.Calories.Lookup(smi.gameObject).hide = flag;
    Db.Get().Amounts.Temperature.Lookup(smi.gameObject).hide = flag;
    AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
    if (amountInstance == null)
      return;
    amountInstance.hide = flag;
  }

  public class Def : StateMachine.BaseDef
  {
    public Effect wildEffect;
    public Effect tameEffect;

    public override void Configure(GameObject prefab) => prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
  }

  public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
  {
    public AmountInstance wildness;

    public Instance(IStateMachineTarget master, WildnessMonitor.Def def)
      : base(master, def)
    {
      this.wildness = Db.Get().Amounts.Wildness.Lookup(this.gameObject);
      this.wildness.value = this.wildness.GetMax();
    }
  }
}
