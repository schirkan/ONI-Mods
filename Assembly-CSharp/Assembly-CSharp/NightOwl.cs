// Decompiled with JetBrains decompiler
// Type: NightOwl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization]
public class NightOwl : StateMachineComponent<NightOwl.StatesInstance>
{
  [MyCmpReq]
  private KPrefabID kPrefabID;
  private AttributeModifier[] attributeModifiers;

  protected override void OnSpawn()
  {
    this.attributeModifiers = new AttributeModifier[11]
    {
      new AttributeModifier("Construction", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Digging", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Machinery", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Athletics", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Learning", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Cooking", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Art", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Strength", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Caring", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Botanist", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME),
      new AttributeModifier("Ranching", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME)
    };
    this.smi.StartSM();
  }

  public void ApplyModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Add(attributeModifier);
    }
  }

  public void RemoveModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Remove(attributeModifier);
    }
  }

  public class StatesInstance : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.GameInstance
  {
    public StatesInstance(NightOwl master)
      : base(master)
    {
    }

    public bool IsNight() => !((Object) GameClock.Instance == (Object) null) && !(this.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && GameClock.Instance.IsNighttime();
  }

  public class States : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl>
  {
    public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State idle;
    public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State early;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State) null);
      this.idle.Transition(this.early, (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.Transition.ConditionCallback) (smi => smi.IsNight()));
      this.early.Enter("Night", (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State.Callback) (smi => smi.master.ApplyModifiers())).Exit("NotNight", (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State.Callback) (smi => smi.master.RemoveModifiers())).ToggleStatusItem(Db.Get().DuplicantStatusItems.NightTime).ToggleExpression(Db.Get().Expressions.Happy).Transition(this.idle, (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.Transition.ConditionCallback) (smi => !smi.IsNight()));
    }
  }
}
