// Decompiled with JetBrains decompiler
// Type: IncubationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IncubationMonitor : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>
{
  public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter incubatorIsActive;
  public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter inIncubator;
  public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter isSuppressed;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State incubating;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State entombed;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State suppressed;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pre;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pst;
  public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State not_viable;
  private Effect suppressedEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.incubating;
    this.root.Enter((StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback) (smi => smi.OnOperationalChanged())).Enter((StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback) (smi => Components.IncubationMonitors.Add(smi))).Exit((StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback) (smi => Components.IncubationMonitors.Remove(smi)));
    this.incubating.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(this.hatching_pre, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.IsReadyToHatch), UpdateRate.SIM_1000ms).TagTransition(GameTags.Entombed, this.entombed).ParamTransition<bool>((StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Parameter<bool>) this.isSuppressed, this.suppressed, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsTrue).ToggleEffect((Func<IncubationMonitor.Instance, Effect>) (smi => smi.incubatingEffect));
    this.entombed.TagTransition(GameTags.Entombed, this.incubating, true);
    this.suppressed.ToggleEffect((Func<IncubationMonitor.Instance, Effect>) (smi => this.suppressedEffect)).ParamTransition<bool>((StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Parameter<bool>) this.isSuppressed, this.incubating, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsFalse).TagTransition(GameTags.Entombed, this.entombed).Transition(this.not_viable, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.NoLongerViable), UpdateRate.SIM_1000ms);
    this.hatching_pre.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DropSelfFromStorage)).PlayAnim("hatching_pre").OnAnimQueueComplete(this.hatching_pst);
    this.hatching_pst.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnBaby)).PlayAnim("hatching_pst").OnAnimQueueComplete((GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State) null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
    this.not_viable.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnGenericEgg)).GoTo((GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State) null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
    this.suppressedEffect = new Effect("IncubationSuppressed", (string) CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, (string) CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.TOOLTIP, 0.0f, true, false, true);
    this.suppressedEffect.Add(new AttributeModifier(Db.Get().Amounts.Viability.deltaAttribute.Id, -0.01666667f, (string) CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME));
  }

  private static bool IsReadyToHatch(IncubationMonitor.Instance smi) => !smi.gameObject.HasTag(GameTags.Entombed) && (double) smi.incubation.value >= (double) smi.incubation.GetMax();

  private static void SpawnBaby(IncubationMonitor.Instance smi)
  {
    Vector3 position = smi.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
    gameObject.SetActive(true);
    gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst");
    KSelectable component = smi.gameObject.GetComponent<KSelectable>();
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component)
      SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
    Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
    if ((UnityEngine.Object) smi.incubator != (UnityEngine.Object) null)
      smi.incubator.StoreBaby(gameObject);
    IncubationMonitor.SpawnShell(smi);
    SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
  }

  private static bool NoLongerViable(IncubationMonitor.Instance smi) => !smi.gameObject.HasTag(GameTags.Entombed) && (double) smi.viability.value <= (double) smi.viability.GetMin();

  private static GameObject SpawnShell(IncubationMonitor.Instance smi)
  {
    Vector3 position = smi.transform.GetPosition();
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "EggShell"), position);
    gameObject.GetComponent<PrimaryElement>().Mass = smi.GetComponent<PrimaryElement>().Mass * 0.5f;
    gameObject.SetActive(true);
    return gameObject;
  }

  private static GameObject SpawnEggInnards(IncubationMonitor.Instance smi)
  {
    Vector3 position = smi.transform.GetPosition();
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "RawEgg"), position);
    gameObject.GetComponent<PrimaryElement>().Mass = smi.GetComponent<PrimaryElement>().Mass * 0.5f;
    gameObject.SetActive(true);
    return gameObject;
  }

  private static void SpawnGenericEgg(IncubationMonitor.Instance smi)
  {
    IncubationMonitor.SpawnShell(smi);
    GameObject gameObject = IncubationMonitor.SpawnEggInnards(smi);
    KSelectable component = smi.gameObject.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component))
      return;
    SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
  }

  private static void DeleteSelf(IncubationMonitor.Instance smi) => smi.gameObject.DeleteObject();

  private static void DropSelfFromStorage(IncubationMonitor.Instance smi)
  {
    if (smi.sm.inIncubator.Get(smi))
      return;
    Storage storage = smi.GetStorage();
    if ((bool) (UnityEngine.Object) storage)
      storage.Drop(smi.gameObject);
    smi.gameObject.AddTag(GameTags.StoredPrivate);
  }

  public class Def : StateMachine.BaseDef
  {
    public float baseIncubationRate;
    public Tag spawnedCreature;

    public override void Configure(GameObject prefab)
    {
      List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
      initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
      initialAmounts.Add(Db.Get().Amounts.Incubation.Id);
      initialAmounts.Add(Db.Get().Amounts.Viability.Id);
    }
  }

  public new class Instance : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.GameInstance
  {
    public AmountInstance incubation;
    public AmountInstance wildness;
    public AmountInstance viability;
    public EggIncubator incubator;
    public Effect incubatingEffect;

    public Instance(IStateMachineTarget master, IncubationMonitor.Def def)
      : base(master, def)
    {
      this.incubation = Db.Get().Amounts.Incubation.Lookup(this.gameObject);
      System.Action<object> handler1 = new System.Action<object>(this.OnStore);
      master.Subscribe(856640610, handler1);
      master.Subscribe(1309017699, handler1);
      System.Action<object> handler2 = new System.Action<object>(this.OnOperationalChanged);
      master.Subscribe(1628751838, handler2);
      master.Subscribe(960378201, handler2);
      this.wildness = Db.Get().Amounts.Wildness.Lookup(this.gameObject);
      this.wildness.value = this.wildness.GetMax();
      this.viability = Db.Get().Amounts.Viability.Lookup(this.gameObject);
      this.viability.value = this.viability.GetMax();
      float num = def.baseIncubationRate;
      if (GenericGameSettings.instance.acceleratedLifecycle)
        num = 33.33333f;
      AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, num, (string) CREATURES.MODIFIERS.BASE_INCUBATION_RATE.NAME);
      this.incubatingEffect = new Effect("Incubating", (string) CREATURES.MODIFIERS.INCUBATING.NAME, (string) CREATURES.MODIFIERS.INCUBATING.TOOLTIP, 0.0f, true, false, false);
      this.incubatingEffect.Add(modifier);
    }

    public Storage GetStorage() => !((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null) ? (Storage) null : this.transform.parent.GetComponent<Storage>();

    public void OnStore(object data)
    {
      Storage storage = data as Storage;
      this.UpdateIncubationState((bool) (UnityEngine.Object) storage || data != null && (bool) data, (bool) (UnityEngine.Object) storage ? storage.GetComponent<EggIncubator>() : (EggIncubator) null);
    }

    public void OnOperationalChanged(object data = null)
    {
      bool stored = this.gameObject.HasTag(GameTags.Stored);
      Storage storage = this.GetStorage();
      EggIncubator incubator = (bool) (UnityEngine.Object) storage ? storage.GetComponent<EggIncubator>() : (EggIncubator) null;
      this.UpdateIncubationState(stored, incubator);
    }

    private void UpdateIncubationState(bool stored, EggIncubator incubator)
    {
      this.incubator = incubator;
      this.smi.sm.inIncubator.Set((UnityEngine.Object) incubator != (UnityEngine.Object) null, this.smi);
      this.smi.sm.isSuppressed.Set(stored && !(bool) (UnityEngine.Object) incubator, this.smi);
      Operational operational = (bool) (UnityEngine.Object) incubator ? incubator.GetComponent<Operational>() : (Operational) null;
      this.smi.sm.incubatorIsActive.Set((bool) (UnityEngine.Object) incubator && ((UnityEngine.Object) operational == (UnityEngine.Object) null || operational.IsOperational), this.smi);
    }

    public void ApplySongBuff() => this.GetComponent<Effects>().Add("EggSong", true);

    public bool HasSongBuff() => this.GetComponent<Effects>().HasEffect("EggSong");
  }
}
