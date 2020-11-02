// Decompiled with JetBrains decompiler
// Type: PixelPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PixelPack")]
public class PixelPack : KMonoBehaviour, ISaveLoadable
{
  protected KBatchedAnimController animController;
  private static readonly EventSystem.IntraObjectHandler<PixelPack> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>((System.Action<PixelPack, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<PixelPack> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>((System.Action<PixelPack, object>) ((component, data) => component.OnOperationalChanged(data)));
  public static readonly HashedString PORT_ID = new HashedString("PixelPackInput");
  public static readonly HashedString SYMBOL_ONE_NAME = (HashedString) "screen1";
  public static readonly HashedString SYMBOL_TWO_NAME = (HashedString) "screen2";
  public static readonly HashedString SYMBOL_THREE_NAME = (HashedString) "screen3";
  public static readonly HashedString SYMBOL_FOUR_NAME = (HashedString) "screen4";
  [MyCmpGet]
  private Operational operational;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<PixelPack> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PixelPack>((System.Action<PixelPack, object>) ((component, data) => component.OnCopySettings(data)));
  public int logicValue;
  [Serialize]
  public List<PixelPack.ColorPair> colorSettings;
  private Color defaultActive = new Color(0.345098f, 0.8470588f, 0.3294118f);
  private Color defaultStandby = new Color(0.972549f, 0.4705882f, 0.345098f);
  protected static readonly HashedString[] ON_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pre",
    (HashedString) "on"
  };
  protected static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "off_pre",
    (HashedString) "off"
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PixelPack>(-905833192, PixelPack.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    PixelPack component = ((GameObject) data).GetComponent<PixelPack>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      for (int index = 0; index < component.colorSettings.Count; ++index)
        this.colorSettings[index] = component.colorSettings[index];
    }
    this.UpdateColors();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.Subscribe<PixelPack>(-801688580, PixelPack.OnLogicValueChangedDelegate);
    this.Subscribe<PixelPack>(-592767678, PixelPack.OnOperationalChangedDelegate);
    if (this.colorSettings != null)
      return;
    PixelPack.ColorPair colorPair1 = new PixelPack.ColorPair()
    {
      activeColor = this.defaultActive,
      standbyColor = this.defaultStandby
    };
    PixelPack.ColorPair colorPair2 = new PixelPack.ColorPair();
    colorPair2.activeColor = this.defaultActive;
    colorPair2.standbyColor = this.defaultStandby;
    PixelPack.ColorPair colorPair3 = colorPair2;
    colorPair2 = new PixelPack.ColorPair();
    colorPair2.activeColor = this.defaultActive;
    colorPair2.standbyColor = this.defaultStandby;
    PixelPack.ColorPair colorPair4 = colorPair2;
    colorPair2 = new PixelPack.ColorPair();
    colorPair2.activeColor = this.defaultActive;
    colorPair2.standbyColor = this.defaultStandby;
    PixelPack.ColorPair colorPair5 = colorPair2;
    this.colorSettings = new List<PixelPack.ColorPair>();
    this.colorSettings.Add(colorPair1);
    this.colorSettings.Add(colorPair3);
    this.colorSettings.Add(colorPair4);
    this.colorSettings.Add(colorPair5);
  }

  private void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == PixelPack.PORT_ID))
      return;
    this.logicValue = logicValueChanged.newValue;
    this.UpdateColors();
  }

  private void OnOperationalChanged(object data)
  {
    if (this.operational.IsOperational)
    {
      this.UpdateColors();
      this.animController.Play(PixelPack.ON_ANIMS);
    }
    else
      this.animController.Play(PixelPack.OFF_ANIMS);
    this.operational.SetActive(this.operational.IsOperational);
  }

  public void UpdateColors()
  {
    if (!this.operational.IsOperational)
      return;
    LogicPorts component = this.GetComponent<LogicPorts>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    switch (component.GetConnectedWireBitDepth(PixelPack.PORT_ID))
    {
      case LogicWire.BitDepth.OneBit:
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
        break;
      case LogicWire.BitDepth.FourBit:
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(1, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(2, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
        this.animController.SetSymbolTint((KAnimHashedString) PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(3, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
        break;
    }
  }

  public struct ColorPair
  {
    public Color activeColor;
    public Color standbyColor;
  }
}
