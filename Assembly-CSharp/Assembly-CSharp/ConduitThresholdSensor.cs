// Decompiled with JetBrains decompiler
// Type: ConduitThresholdSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
  [SerializeField]
  [Serialize]
  protected float threshold;
  [SerializeField]
  [Serialize]
  protected bool activateAboveThreshold = true;
  [Serialize]
  private bool dirty = true;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>((System.Action<ConduitThresholdSensor, object>) ((component, data) => component.OnCopySettings(data)));

  public abstract float CurrentValue { get; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    ConduitThresholdSensor component = ((GameObject) data).GetComponent<ConduitThresholdSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void ConduitUpdate(float dt)
  {
    if ((double) this.GetContainedMass() <= 0.0 && !this.dirty)
      return;
    float currentValue = this.CurrentValue;
    this.dirty = false;
    if (this.activateAboveThreshold)
    {
      if (((double) currentValue <= (double) this.threshold || this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || !this.IsSwitchedOn))
        return;
      this.Toggle();
    }
    else
    {
      if (((double) currentValue <= (double) this.threshold || !this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || this.IsSwitchedOn))
        return;
      this.Toggle();
    }
  }

  private float GetContainedMass()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
      return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
    SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
    Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(cell).pickupableHandle);
    return (UnityEngine.Object) pickupable != (UnityEngine.Object) null ? pickupable.PrimaryElement.Mass : 0.0f;
  }

  public float Threshold
  {
    get => this.threshold;
    set
    {
      this.threshold = value;
      this.dirty = true;
    }
  }

  public bool ActivateAboveThreshold
  {
    get => this.activateAboveThreshold;
    set
    {
      this.activateAboveThreshold = value;
      this.dirty = true;
    }
  }
}
