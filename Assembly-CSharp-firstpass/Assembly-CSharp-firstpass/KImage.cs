// Decompiled with JetBrains decompiler
// Type: KImage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

public class KImage : Image
{
  public KImage.ColorSelector defaultState = KImage.ColorSelector.Inactive;
  private KImage.ColorSelector colorSelector = KImage.ColorSelector.Inactive;
  public ColorStyleSetting colorStyleSetting;
  public bool clearMaskOnDisable = true;

  public KImage.ColorSelector ColorState
  {
    set
    {
      this.colorSelector = value;
      this.ApplyColorStyleSetting();
    }
  }

  protected override void Awake()
  {
    base.Awake();
    this.ColorState = this.defaultState;
  }

  protected override void OnEnable() => base.OnEnable();

  protected override void OnDisable() => base.OnDisable();

  protected override void OnDestroy() => base.OnDestroy();

  [ContextMenu("Apply Color Style Settings")]
  public void ApplyColorStyleSetting()
  {
    if (!((Object) this.colorStyleSetting != (Object) null))
      return;
    switch (this.colorSelector)
    {
      case KImage.ColorSelector.Active:
        this.color = this.colorStyleSetting.activeColor;
        break;
      case KImage.ColorSelector.Inactive:
        this.color = this.colorStyleSetting.inactiveColor;
        break;
      case KImage.ColorSelector.Disabled:
        this.color = this.colorStyleSetting.disabledColor;
        break;
      case KImage.ColorSelector.Hover:
        this.color = this.colorStyleSetting.hoverColor;
        break;
    }
  }

  public enum ColorSelector
  {
    Active,
    Inactive,
    Disabled,
    Hover,
  }
}
