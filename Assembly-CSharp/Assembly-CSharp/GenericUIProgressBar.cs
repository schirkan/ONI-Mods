// Decompiled with JetBrains decompiler
// Type: GenericUIProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
  public Image fill;
  public LocText label;
  private float maxValue;

  public void SetMaxValue(float max) => this.maxValue = max;

  public void SetFillPercentage(float value)
  {
    this.fill.fillAmount = value;
    this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
  }
}
