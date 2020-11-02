// Decompiled with JetBrains decompiler
// Type: CreatureLure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureLure : StateMachineComponent<CreatureLure.StatesInstance>
{
  public static float CONSUMPTION_RATE = 1f;
  [Serialize]
  public Tag activeBaitSetting;
  public List<Tag> baitTypes;
  public Storage baitStorage;
  protected FetchChore fetchChore;
  private Operational operational;
  private Operational.Flag baited = new Operational.Flag("Baited", Operational.Flag.Type.Requirement);
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureLure>((System.Action<CreatureLure, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CreatureLure>((System.Action<CreatureLure, object>) ((component, data) => component.OnStorageChange(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.operational = this.GetComponent<Operational>();
    this.Subscribe<CreatureLure>(-905833192, CreatureLure.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    CreatureLure component = ((GameObject) data).GetComponent<CreatureLure>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.ChangeBaitSetting(component.activeBaitSetting);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (this.activeBaitSetting == Tag.Invalid)
    {
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected);
    }
    else
    {
      this.ChangeBaitSetting(this.activeBaitSetting);
      this.OnStorageChange();
    }
    this.Subscribe<CreatureLure>(-1697596308, CreatureLure.OnStorageChangeDelegate);
  }

  private void OnStorageChange(object data = null) => this.operational.SetFlag(this.baited, (double) this.baitStorage.GetAmountAvailable(this.activeBaitSetting) > 0.0);

  public void ChangeBaitSetting(Tag baitSetting)
  {
    if (this.fetchChore != null)
      this.fetchChore.Cancel("SwitchedResource");
    if (baitSetting != this.activeBaitSetting)
    {
      this.activeBaitSetting = baitSetting;
      this.baitStorage.DropAll();
    }
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.idle);
    this.baitStorage.storageFilters = new List<Tag>()
    {
      this.activeBaitSetting
    };
    if (baitSetting != Tag.Invalid)
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected);
      if (!this.smi.master.baitStorage.IsEmpty())
        return;
      this.CreateFetchChore();
    }
    else
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected);
  }

  protected void CreateFetchChore()
  {
    if (this.fetchChore != null)
      this.fetchChore.Cancel("Overwrite");
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery);
    if (this.activeBaitSetting == Tag.Invalid)
      return;
    this.fetchChore = new FetchChore(Db.Get().ChoreTypes.RanchingFetch, this.baitStorage, 100f, new Tag[1]
    {
      this.activeBaitSetting
    }, operational_requirement: FetchOrder2.OperationalRequirement.None);
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery);
  }

  public class StatesInstance : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.GameInstance
  {
    public StatesInstance(CreatureLure master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure>
  {
    public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State idle;
    public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State working;
    public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State empty;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.PlayAnim("off", KAnim.PlayMode.Loop).Enter((StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback) (smi =>
      {
        if (!(smi.master.activeBaitSetting != Tag.Invalid))
          return;
        if (smi.master.baitStorage.IsEmpty())
        {
          smi.master.CreateFetchChore();
        }
        else
        {
          if (!smi.master.operational.IsOperational)
            return;
          smi.GoTo((StateMachine.BaseState) this.working);
        }
      })).EventTransition(GameHashes.OnStorageChange, this.working, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.working, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational));
      this.working.Enter((StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery);
        HashedString batchTag = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.batchTag;
        KAnim.Build build = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.GetData().build;
        KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
        HashedString target_symbol = (HashedString) "slime_mold";
        SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
        component.TryRemoveSymbolOverride(target_symbol);
        component.AddSymbolOverride(target_symbol, symbol);
        smi.GetSMI<Lure.Instance>().SetActiveLures(new Tag[1]
        {
          smi.master.activeBaitSetting
        });
      })).Exit(new StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback(CreatureLure.States.ClearBait)).QueueAnim("working_pre").QueueAnim("working_loop", true).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid)).EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational && !smi.master.baitStorage.IsEmpty()));
      this.empty.QueueAnim("working_pst").QueueAnim("off").Enter((StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback) (smi => smi.master.CreateFetchChore())).EventTransition(GameHashes.OnStorageChange, this.working, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.working, (StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.Transition.ConditionCallback) (smi => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational));
    }

    private static void ClearBait(StateMachine.Instance smi)
    {
      if (smi.GetSMI<Lure.Instance>() == null)
        return;
      smi.GetSMI<Lure.Instance>().SetActiveLures((Tag[]) null);
    }
  }
}
