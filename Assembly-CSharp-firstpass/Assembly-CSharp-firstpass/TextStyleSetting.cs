// Decompiled with JetBrains decompiler
// Type: TextStyleSetting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using TMPro;
using UnityEngine;

public class TextStyleSetting : ScriptableObject
{
  public TMP_FontAsset sdfFont;
  public int fontSize;
  public Color textColor;
  public FontStyles style;
  public bool enableWordWrapping = true;

  public void Init(TMP_FontAsset _sdfFont, int _fontSize, Color _color, bool _enableWordWrapping)
  {
    this.sdfFont = _sdfFont;
    this.fontSize = _fontSize;
    this.textColor = _color;
    this.enableWordWrapping = _enableWordWrapping;
  }
}
