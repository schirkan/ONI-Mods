// Decompiled with JetBrains decompiler
// Type: IntSliderSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class IntSliderSideScreen : SideScreenContent
{
  private IIntSliderControl target;
  public List<SliderSet> sliderSets;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.sliderSets.Count; ++index)
    {
      this.sliderSets[index].SetupSlider(index);
      this.sliderSets[index].valueSlider.wholeNumbers = true;
    }
  }

  public override bool IsValidForTarget(GameObject target) => target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;

  public override void SetTarget(GameObject new_target)
  {
    if ((Object) new_target == (Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IIntSliderControl>();
      if (this.target == null)
        this.target = new_target.GetSMI<IIntSliderControl>();
      if (this.target == null)
      {
        Debug.LogError((object) "The gameObject received does not contain a Manual Generator component");
      }
      else
      {
        this.titleKey = this.target.SliderTitleKey;
        for (int index = 0; index < this.sliderSets.Count; ++index)
          this.sliderSets[index].SetTarget((ISliderControl) this.target);
      }
    }
  }
}
