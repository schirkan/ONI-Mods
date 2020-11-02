// Decompiled with JetBrains decompiler
// Type: ColorStyleSetting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class ColorStyleSetting : ScriptableObject
{
  public Color activeColor;
  public Color inactiveColor;
  public Color disabledColor;
  public Color disabledActiveColor;
  public Color hoverColor;
  public Color disabledhoverColor = Color.grey;

  public void Init(Color _color)
  {
    this.activeColor = _color;
    this.inactiveColor = _color;
    this.disabledColor = _color;
    this.disabledActiveColor = _color;
    this.hoverColor = _color;
    this.disabledhoverColor = _color;
  }
}
