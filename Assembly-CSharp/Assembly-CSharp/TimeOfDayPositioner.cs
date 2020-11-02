﻿// Decompiled with JetBrains decompiler
// Type: TimeOfDayPositioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDayPositioner")]
public class TimeOfDayPositioner : KMonoBehaviour
{
  [SerializeField]
  private RectTransform targetRect;

  private void Update() => (this.transform as RectTransform).anchoredPosition = this.targetRect.anchoredPosition + new Vector2(Mathf.Round(GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width), 0.0f);
}
