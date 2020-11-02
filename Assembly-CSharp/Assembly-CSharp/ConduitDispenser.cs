// Decompiled with JetBrains decompiler
// Type: ConduitDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable
{
  [SerializeField]
  public ConduitType conduitType;
  [SerializeField]
  public SimHashes[] elementFilter;
  [SerializeField]
  public bool invertElementFilter;
  [SerializeField]
  public bool alwaysDispense;
  [SerializeField]
  public bool isOn = true;
  [SerializeField]
  public bool blocked;
  private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private int utilityCell = -1;
  private int elementOutputOffset;

  public ConduitType TypeOfConduit => this.conduitType;

  public ConduitFlow.ConduitContents ConduitContents => this.GetConduitManager().GetContents(this.utilityCell);

  public void SetConduitData(ConduitType type) => this.conduitType = type;

  public ConduitFlow GetConduitManager()
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

  private void OnConduitConnectionChanged(object data) => this.Trigger(-2094018600, (object) this.IsConnected);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing)), (object) null, (SchedulerGroup) null);
    this.utilityCell = this.GetComponent<Building>().GetUtilityOutputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[this.conduitType == ConduitType.Gas ? 12 : 16], new System.Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitManager().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitManager().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public void SetOnState(bool onState) => this.isOn = onState;

  private void ConduitUpdate(float dt)
  {
    this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
    this.blocked = false;
    if (!this.isOn)
      return;
    this.Dispense(dt);
  }

  private void Dispense(float dt)
  {
    if (!this.operational.IsOperational && !this.alwaysDispense)
      return;
    PrimaryElement suitableElement = this.FindSuitableElement();
    if (!((UnityEngine.Object) suitableElement != (UnityEngine.Object) null))
      return;
    suitableElement.KeepZeroMassObject = true;
    float num1 = this.GetConduitManager().AddElement(this.utilityCell, suitableElement.ElementID, suitableElement.Mass, suitableElement.Temperature, suitableElement.DiseaseIdx, suitableElement.DiseaseCount);
    if ((double) num1 > 0.0)
    {
      int num2 = (int) ((double) num1 / (double) suitableElement.Mass * (double) suitableElement.DiseaseCount);
      suitableElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
      suitableElement.Mass -= num1;
      this.Trigger(-1697596308, (object) suitableElement.gameObject);
    }
    else
      this.blocked = true;
  }

  private PrimaryElement FindSuitableElement()
  {
    List<GameObject> items = this.storage.items;
    int count = items.Count;
    for (int index1 = 0; index1 < count; ++index1)
    {
      int index2 = (index1 + this.elementOutputOffset) % count;
      PrimaryElement component = items[index2].GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.Mass > 0.0 && ((this.conduitType == ConduitType.Liquid ? (component.Element.IsLiquid ? 1 : 0) : (component.Element.IsGas ? 1 : 0)) != 0 && (this.elementFilter == null || this.elementFilter.Length == 0 || !this.invertElementFilter && this.IsFilteredElement(component.ElementID) || this.invertElementFilter && !this.IsFilteredElement(component.ElementID))))
      {
        this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
        return component;
      }
    }
    return (PrimaryElement) null;
  }

  private bool IsFilteredElement(SimHashes element)
  {
    for (int index = 0; index != this.elementFilter.Length; ++index)
    {
      if (this.elementFilter[index] == element)
        return true;
    }
    return false;
  }

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, this.conduitType == ConduitType.Gas ? 12 : 16];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }
}
