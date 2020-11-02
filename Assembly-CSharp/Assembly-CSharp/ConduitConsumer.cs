// Decompiled with JetBrains decompiler
// Type: ConduitConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour
{
  [SerializeField]
  public ConduitType conduitType;
  [SerializeField]
  public bool ignoreMinMassCheck;
  [SerializeField]
  public Tag capacityTag = GameTags.Any;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  [SerializeField]
  public bool forceAlwaysSatisfied;
  [SerializeField]
  public bool alwaysConsume;
  [SerializeField]
  public bool keepZeroMassObject = true;
  [SerializeField]
  public bool useSecondaryInput;
  [SerializeField]
  public bool isOn = true;
  [NonSerialized]
  public bool isConsuming = true;
  [MyCmpReq]
  public Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  public Storage storage;
  private int utilityCell = -1;
  public float consumptionRate = float.PositiveInfinity;
  public SimHashes lastConsumedElement = SimHashes.Vacuum;
  private HandleVector<int>.Handle partitionerEntry;
  private bool satisfied;
  public ConduitConsumer.WrongElementResult wrongElementResult;

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, this.conduitType == ConduitType.Gas ? 12 : 16];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }

  public bool CanConsume
  {
    get
    {
      bool flag = false;
      if (this.IsConnected)
        flag = (double) this.GetConduitManager().GetContents(this.utilityCell).mass > 0.0;
      return flag;
    }
  }

  public float stored_mass
  {
    get
    {
      if ((UnityEngine.Object) this.storage == (UnityEngine.Object) null)
        return 0.0f;
      return !(this.capacityTag != GameTags.Any) ? this.storage.MassStored() : this.storage.GetMassAvailable(this.capacityTag);
    }
  }

  public float space_remaining_kg
  {
    get
    {
      float b = this.capacityKG - this.stored_mass;
      return !((UnityEngine.Object) this.storage == (UnityEngine.Object) null) ? Mathf.Min(this.storage.RemainingCapacity(), b) : b;
    }
  }

  public void SetConduitData(ConduitType type) => this.conduitType = type;

  public ConduitType TypeOfConduit => this.conduitType;

  public bool IsAlmostEmpty => !this.ignoreMinMassCheck && (double) this.MassAvailable < (double) this.ConsumptionRate * 30.0;

  public bool IsEmpty
  {
    get
    {
      if (this.ignoreMinMassCheck)
        return false;
      return (double) this.MassAvailable == 0.0 || (double) this.MassAvailable < (double) this.ConsumptionRate;
    }
  }

  public float ConsumptionRate => this.consumptionRate;

  public bool IsSatisfied
  {
    get => this.satisfied || !this.isConsuming;
    set => this.satisfied = value || this.forceAlwaysSatisfied;
  }

  private ConduitFlow GetConduitManager()
  {
    switch (this.conduitType)
    {
      case ConduitType.Gas:
        return Game.Instance.gasConduitFlow;
      case ConduitType.Liquid:
        return Game.Instance.liquidConduitFlow;
      default:
        return (ConduitFlow) null;
    }
  }

  public float MassAvailable
  {
    get
    {
      int inputCell = this.GetInputCell();
      return this.GetConduitManager().GetContents(inputCell).mass;
    }
  }

  private int GetInputCell()
  {
    if (!this.useSecondaryInput)
      return this.building.GetUtilityInputCell();
    ISecondaryInput component = this.GetComponent<ISecondaryInput>();
    return Grid.OffsetCell(this.building.NaturalBuildingCell(), component.GetSecondaryConduitOffset());
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing)), (object) null, (SchedulerGroup) null);
    this.utilityCell = this.GetInputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[this.conduitType == ConduitType.Gas ? 12 : 16], new System.Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitManager().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitManager().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitConnectionChanged(object data) => this.Trigger(-2094018600, (object) this.IsConnected);

  public void SetOnState(bool onState) => this.isOn = onState;

  private void ConduitUpdate(float dt)
  {
    if (!this.isConsuming || !this.isOn)
      return;
    ConduitFlow conduitManager = this.GetConduitManager();
    this.Consume(dt, conduitManager);
  }

  private void Consume(float dt, ConduitFlow conduit_mgr)
  {
    this.IsSatisfied = false;
    if (this.building.Def.CanMove)
      this.utilityCell = this.GetInputCell();
    if (!this.IsConnected)
      return;
    ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
    if ((double) contents.mass <= 0.0)
      return;
    this.IsSatisfied = true;
    if (!this.alwaysConsume && !this.operational.IsOperational)
      return;
    float delta = Mathf.Min(this.ConsumptionRate * dt, this.space_remaining_kg);
    float mass = 0.0f;
    if ((double) delta > 0.0)
    {
      ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, delta);
      mass = conduitContents.mass;
      this.lastConsumedElement = conduitContents.element;
    }
    bool flag = ElementLoader.FindElementByHash(contents.element).HasTag(this.capacityTag);
    if ((double) mass > 0.0 && this.capacityTag != GameTags.Any && !flag)
      this.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
      {
        damage = 1,
        source = (string) BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
        popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
      });
    if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || (contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any))
    {
      if ((double) mass <= 0.0)
        return;
      int disease_count = (int) ((double) contents.diseaseCount * ((double) mass / (double) contents.mass));
      Element elementByHash = ElementLoader.FindElementByHash(contents.element);
      switch (this.conduitType)
      {
        case ConduitType.Gas:
          if (elementByHash.IsGas)
          {
            this.storage.AddGasChunk(contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
            break;
          }
          Debug.LogWarning((object) ("Gas conduit consumer consuming non gas: " + elementByHash.id.ToString()));
          break;
        case ConduitType.Liquid:
          if (elementByHash.IsLiquid)
          {
            this.storage.AddLiquid(contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
            break;
          }
          Debug.LogWarning((object) ("Liquid conduit consumer consuming non liquid: " + elementByHash.id.ToString()));
          break;
      }
    }
    else
    {
      if ((double) mass <= 0.0 || this.wrongElementResult != ConduitConsumer.WrongElementResult.Dump)
        return;
      int disease_count = (int) ((double) contents.diseaseCount * ((double) mass / (double) contents.mass));
      SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, mass, contents.temperature, contents.diseaseIdx, disease_count);
    }
  }

  public enum WrongElementResult
  {
    Destroy,
    Dump,
    Store,
  }
}
