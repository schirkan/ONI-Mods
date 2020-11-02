// Decompiled with JetBrains decompiler
// Type: SetTextStyleSetting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("KMonoBehaviour/Plugins/SetTextStyleSetting")]
public class SetTextStyleSetting : KMonoBehaviour
{
  [MyCmpGet]
  private Text text;
  [MyCmpGet]
  private TextMeshProUGUI sdfText;
  [SerializeField]
  private TextStyleSetting style;
  private TextStyleSetting currentStyle;

  public static void ApplyStyle(TextMeshProUGUI sdfText, TextStyleSetting style)
  {
    if (!(bool) (Object) sdfText || !(bool) (Object) style)
      return;
    sdfText.enableWordWrapping = style.enableWordWrapping;
    sdfText.enableKerning = true;
    sdfText.extraPadding = true;
    sdfText.fontSize = (float) style.fontSize;
    sdfText.color = style.textColor;
    sdfText.font = style.sdfFont;
    sdfText.fontStyle = style.style;
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public void SetStyle(TextStyleSetting newstyle)
  {
    if ((Object) this.sdfText == (Object) null)
      this.sdfText = this.GetComponent<TextMeshProUGUI>();
    if (!((Object) this.currentStyle != (Object) newstyle))
      return;
    this.currentStyle = newstyle;
    this.style = this.currentStyle;
    SetTextStyleSetting.ApplyStyle(this.sdfText, this.style);
  }

  public enum TextStyle
  {
    Standard,
    Header,
  }
}
