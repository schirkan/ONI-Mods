// Decompiled with JetBrains decompiler
// Type: ISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface ISliderControl
{
  string SliderTitleKey { get; }

  string SliderUnits { get; }

  int SliderDecimalPlaces(int index);

  float GetSliderMin(int index);

  float GetSliderMax(int index);

  float GetSliderValue(int index);

  void SetSliderValue(float percent, int index);

  string GetSliderTooltipKey(int index);

  string GetSliderTooltip();
}
