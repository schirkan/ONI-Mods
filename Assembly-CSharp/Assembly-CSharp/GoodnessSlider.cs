// Decompiled with JetBrains decompiler
// Type: GoodnessSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
  public Image icon;
  public Text text;
  public Slider slider;
  public Image fill;
  public Gradient gradient;
  public string[] names;

  protected override void OnSpawn()
  {
    this.Spawn();
    this.UpdateValues();
  }

  public void UpdateValues()
  {
    this.text.color = this.fill.color = this.gradient.Evaluate(this.slider.value);
    for (int index = 0; index < this.gradient.colorKeys.Length; ++index)
    {
      if ((double) this.gradient.colorKeys[index].time < (double) this.slider.value)
        this.text.text = this.names[index];
      if (index == this.gradient.colorKeys.Length - 1 && (double) this.gradient.colorKeys[index - 1].time < (double) this.slider.value)
        this.text.text = this.names[index];
    }
  }
}
