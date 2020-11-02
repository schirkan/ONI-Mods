// Decompiled with JetBrains decompiler
// Type: LogicRibbonWriter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
  public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");
  public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>((System.Action<LogicRibbonWriter, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>((System.Action<LogicRibbonWriter, object>) ((component, data) => component.OnCopySettings(data)));
  private LogicPorts ports;
  public int bitDepth = 4;
  [Serialize]
  public int selectedBit;
  [Serialize]
  private int currentValue;
  private KBatchedAnimController kbac;
  private Color colorOn = new Color(0.3411765f, 0.7254902f, 0.3686275f);
  private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);
  private static KAnimHashedString BIT_ONE_SYMBOL = (KAnimHashedString) "bit1_bloom";
  private static KAnimHashedString BIT_TWO_SYMBOL = (KAnimHashedString) "bit2_bloom";
  private static KAnimHashedString BIT_THREE_SYMBOL = (KAnimHashedString) "bit3_bloom";
  private static KAnimHashedString BIT_FOUR_SYMBOL = (KAnimHashedString) "bit4_bloom";
  private static KAnimHashedString INPUT_SYMBOL = (KAnimHashedString) "input_light_bloom";

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
    this.ports = this.GetComponent<LogicPorts>();
    this.kbac = this.GetComponent<KBatchedAnimController>();
    this.kbac.Play((HashedString) "idle");
  }

  public void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
      return;
    this.currentValue = logicValueChanged.newValue;
    this.UpdateLogicCircuit();
    this.UpdateVisuals();
  }

  private void OnCopySettings(object data)
  {
    LogicRibbonWriter component = ((GameObject) data).GetComponent<LogicRibbonWriter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.SetBitSelection(component.selectedBit);
  }

  private void UpdateLogicCircuit()
  {
    int new_value = this.currentValue << this.selectedBit;
    this.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
  }

  public void Render200ms(float dt) => this.UpdateVisuals();

  private LogicCircuitNetwork GetInputNetwork()
  {
    LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork) null;
    if ((UnityEngine.Object) this.ports != (UnityEngine.Object) null)
      logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID));
    return logicCircuitNetwork;
  }

  private LogicCircuitNetwork GetOutputNetwork()
  {
    LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork) null;
    if ((UnityEngine.Object) this.ports != (UnityEngine.Object) null)
      logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID));
    return logicCircuitNetwork;
  }

  public void SetBitSelection(int bit)
  {
    this.selectedBit = bit;
    this.UpdateLogicCircuit();
  }

  public int GetBitSelection() => this.selectedBit;

  public int GetBitDepth() => this.bitDepth;

  public string SideScreenTitle => "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";

  public string SideScreenDescription => (string) UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;

  public bool SideScreenDisplayWriterDescription() => true;

  public bool SideScreenDisplayReaderDescription() => false;

  public bool IsBitActive(int bit)
  {
    LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork) null;
    if ((UnityEngine.Object) this.ports != (UnityEngine.Object) null)
      logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID));
    return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
  }

  public int GetInputValue()
  {
    LogicPorts component = this.GetComponent<LogicPorts>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
  }

  public int GetOutputValue()
  {
    LogicPorts component = this.GetComponent<LogicPorts>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
  }

  public void UpdateVisuals()
  {
    LogicCircuitNetwork inputNetwork = this.GetInputNetwork();
    LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
    int num = 0;
    if (inputNetwork != null)
    {
      ++num;
      this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
    }
    if (outputNetwork != null)
    {
      num += 4;
      this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
      this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
      this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
      this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
    }
    this.kbac.Play((HashedString) (num.ToString() + "_" + (this.GetBitSelection() + 1).ToString()));
  }
}
