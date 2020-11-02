// Decompiled with JetBrains decompiler
// Type: SliderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
  public bool isPercentValue = true;
  public KSlider slider;
  public LocText nameLabel;
  public LocText valueLabel;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
  }

  public void UpdateSliderLabel(float newValue)
  {
    if (this.isPercentValue)
      this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
    else
      this.valueLabel.text = newValue.ToString();
  }
}
