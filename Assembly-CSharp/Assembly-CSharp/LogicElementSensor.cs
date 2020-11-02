// Decompiled with JetBrains decompiler
// Type: LogicElementSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
  private bool wasOn;
  public Element.State desiredState = Element.State.Gas;
  private const int WINDOW_SIZE = 8;
  private bool[] samples = new bool[8];
  private int sampleIdx;
  private byte desiredElementIdx = byte.MaxValue;
  private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>((System.Action<LogicElementSensor, object>) ((component, data) => component.OnOperationalChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<Filterable>().onFilterChanged += new System.Action<Tag>(this.OnElementSelected);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
    this.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.sampleIdx < 8)
    {
      this.samples[this.sampleIdx] = (int) Grid.ElementIdx[cell] == (int) this.desiredElementIdx;
      ++this.sampleIdx;
    }
    else
    {
      this.sampleIdx = 0;
      bool flag = true;
      foreach (bool sample in this.samples)
        flag = sample & flag;
      if (this.IsSwitchedOn == flag)
        return;
      this.Toggle();
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  private void UpdateLogicCircuit()
  {
    bool flag = this.switchedOn && this.GetComponent<Operational>().IsOperational;
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, flag ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  private void OnElementSelected(Tag element_tag)
  {
    if (!element_tag.IsValid)
      return;
    Element element = ElementLoader.GetElement(element_tag);
    bool on = true;
    if (element != null)
    {
      this.desiredElementIdx = (byte) ElementLoader.GetElementIndex(element.id);
      on = element.id == SimHashes.Void || element.id == SimHashes.Vacuum;
    }
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on);
  }

  private void OnOperationalChanged(object data)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
