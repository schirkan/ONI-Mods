// Decompiled with JetBrains decompiler
// Type: SweepBotStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SweepBotStation")]
public class SweepBotStation : KMonoBehaviour
{
  [Serialize]
  public Ref<KSelectable> sweepBot;
  [Serialize]
  public string storedName;
  private Operational.Flag dockedRobot = new Operational.Flag(nameof (dockedRobot), Operational.Flag.Type.Functional);
  private MeterController meter;
  private Storage sweepStorage;
  private Storage botMaterialStorage;
  private SchedulerHandle newSweepyHandle;
  private static readonly EventSystem.IntraObjectHandler<SweepBotStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SweepBotStation>((System.Action<SweepBotStation, object>) ((component, data) => component.OnOperationalChanged(data)));
  private int refreshSweepbotHandle = -1;
  private int sweepBotNameChangeHandle = -1;

  protected override void OnPrefabInit()
  {
    this.Initialize(false);
    this.Subscribe<SweepBotStation>(-592767678, SweepBotStation.OnOperationalChangedDelegate);
  }

  protected void Initialize(bool use_logic_meter)
  {
    base.OnPrefabInit();
    this.GetComponent<Operational>().SetFlag(this.dockedRobot, false);
  }

  protected override void OnSpawn()
  {
    this.Subscribe(-1697596308, new System.Action<object>(this.OnStorageChanged));
    this.meter = new MeterController((KAnimControllerBase) this.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[2]
    {
      "meter_frame",
      "meter_level"
    });
    this.botMaterialStorage = this.GetComponents<Storage>()[0];
    this.sweepStorage = this.GetComponents<Storage>()[1];
    if (this.sweepBot == null || (UnityEngine.Object) this.sweepBot.Get() == (UnityEngine.Object) null)
    {
      this.RequestNewSweepBot();
    }
    else
    {
      StorageUnloadMonitor.Instance smi = this.sweepBot.Get().GetSMI<StorageUnloadMonitor.Instance>();
      smi.sm.sweepLocker.Set(this.sweepStorage, smi);
      this.RefreshSweepBotSubscription();
    }
    this.UpdateMeter();
    this.UpdateNameDisplay();
  }

  private void RequestNewSweepBot(object data = null)
  {
    if ((UnityEngine.Object) this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS) == (UnityEngine.Object) null)
    {
      FetchList2 fetchList2 = new FetchList2(this.botMaterialStorage, Db.Get().ChoreTypes.Fetch);
      fetchList2.Add(GameTags.RefinedMetal, amount: SweepBotConfig.MASS);
      fetchList2.Submit((System.Action) null, true);
    }
    else
      this.MakeNewSweepBot();
  }

  private void MakeNewSweepBot(object data = null)
  {
    if (this.newSweepyHandle.IsValid || (double) this.botMaterialStorage.GetAmountAvailable(GameTags.RefinedMetal) < (double) SweepBotConfig.MASS)
      return;
    PrimaryElement firstWithMass = this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS);
    if ((UnityEngine.Object) firstWithMass == (UnityEngine.Object) null)
      return;
    SimHashes sweepBotMaterial = firstWithMass.ElementID;
    firstWithMass.Mass -= SweepBotConfig.MASS;
    this.UpdateMeter();
    this.newSweepyHandle = GameScheduler.Instance.Schedule("MakeSweepy", 2f, (System.Action<object>) (obj =>
    {
      GameObject go = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "SweepBot"), Grid.CellToPos(Grid.CellRight(Grid.PosToCell(this.gameObject))), Grid.SceneLayer.Creatures);
      go.SetActive(true);
      this.sweepBot = new Ref<KSelectable>(go.GetComponent<KSelectable>());
      if (!string.IsNullOrEmpty(this.storedName))
        this.sweepBot.Get().GetComponent<UserNameable>().SetName(this.storedName);
      this.UpdateNameDisplay();
      StorageUnloadMonitor.Instance smi = go.GetSMI<StorageUnloadMonitor.Instance>();
      smi.sm.sweepLocker.Set(this.sweepStorage, smi);
      this.sweepBot.Get().GetComponent<PrimaryElement>().ElementID = sweepBotMaterial;
      this.RefreshSweepBotSubscription();
      this.newSweepyHandle.ClearScheduler();
    }), (object) null, (SchedulerGroup) null);
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "newsweepy");
  }

  private void RefreshSweepBotSubscription()
  {
    if (this.refreshSweepbotHandle != -1)
    {
      this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
      this.sweepBot.Get().Unsubscribe(this.sweepBotNameChangeHandle);
    }
    this.refreshSweepbotHandle = this.sweepBot.Get().Subscribe(1969584890, new System.Action<object>(this.RequestNewSweepBot));
    this.sweepBotNameChangeHandle = this.sweepBot.Get().Subscribe(1102426921, new System.Action<object>(this.UpdateStoredName));
  }

  private void UpdateStoredName(object data)
  {
    this.storedName = (string) data;
    this.UpdateNameDisplay();
  }

  private void UpdateNameDisplay()
  {
    if (string.IsNullOrEmpty(this.storedName))
      this.GetComponent<KSelectable>().SetName(string.Format((string) BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, (object) ROBOTS.MODELS.SWEEPBOT.NAME));
    else
      this.GetComponent<KSelectable>().SetName(string.Format((string) BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, (object) this.storedName));
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
  }

  public void DockRobot(bool docked) => this.GetComponent<Operational>().SetFlag(this.dockedRobot, docked);

  public void StartCharging()
  {
    this.GetComponent<KBatchedAnimController>().Queue((HashedString) "sleep_pre");
    this.GetComponent<KBatchedAnimController>().Queue((HashedString) "sleep_idle", KAnim.PlayMode.Loop);
  }

  public void StopCharging()
  {
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "sleep_pst");
    this.UpdateNameDisplay();
  }

  protected override void OnCleanUp()
  {
    if (this.newSweepyHandle.IsValid)
      this.newSweepyHandle.ClearScheduler();
    if (this.refreshSweepbotHandle == -1 || !((UnityEngine.Object) this.sweepBot.Get() != (UnityEngine.Object) null))
      return;
    this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
  }

  private void UpdateMeter()
  {
    float minusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
    float percent_full = Mathf.Clamp01(this.GetAmountStored() / minusStorageMargin);
    if (this.meter == null)
      return;
    this.meter.SetPositionPercent(percent_full);
  }

  private void OnStorageChanged(object data)
  {
    this.UpdateMeter();
    if (this.sweepBot == null || (UnityEngine.Object) this.sweepBot.Get() == (UnityEngine.Object) null)
      this.RequestNewSweepBot();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (component.currentFrame >= component.GetCurrentNumFrames())
      this.GetComponent<KBatchedAnimController>().Play((HashedString) "remove");
    for (int idx = 0; idx < this.sweepStorage.Count; ++idx)
      this.sweepStorage[idx].GetComponent<Clearable>().MarkForClear(allowWhenStored: true);
  }

  private void OnOperationalChanged(object data)
  {
    Operational component = this.GetComponent<Operational>();
    if (component.Flags.ContainsValue(false))
      component.SetActive(false);
    else
      component.SetActive(true);
    if (this.sweepBot != null && !((UnityEngine.Object) this.sweepBot.Get() == (UnityEngine.Object) null))
      return;
    this.RequestNewSweepBot();
  }

  private float GetMaxCapacityMinusStorageMargin() => this.sweepStorage.Capacity() - this.sweepStorage.storageFullMargin;

  private float GetAmountStored() => this.sweepStorage.MassStored();
}
