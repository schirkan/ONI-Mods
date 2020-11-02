﻿// Decompiled with JetBrains decompiler
// Type: IActivationRangeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IActivationRangeTarget
{
  float ActivateValue { get; set; }

  float DeactivateValue { get; set; }

  float MinValue { get; }

  float MaxValue { get; }

  bool UseWholeNumbers { get; }

  string ActivationRangeTitleText { get; }

  string ActivateSliderLabelText { get; }

  string DeactivateSliderLabelText { get; }

  string ActivateTooltip { get; }

  string DeactivateTooltip { get; }
}
