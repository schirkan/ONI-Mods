// Decompiled with JetBrains decompiler
// Type: DateTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DateTime : KScreen
{
  public static DateTime Instance;
  public LocText day;
  private int displayedDayCount = -1;
  [SerializeField]
  private LocText text;
  [SerializeField]
  private ToolTip tooltip;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Days;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Playtime;
  [SerializeField]
  public KToggle scheduleToggle;

  public static void DestroyInstance() => DateTime.Instance = (DateTime) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DateTime.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnComplexToolTip = new Func<List<Tuple<string, ScriptableObject>>>(SaveGame.Instance.GetColonyToolTip);
  }

  private void Update()
  {
    if (!((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || this.displayedDayCount == GameUtil.GetCurrentCycle())
      return;
    this.text.text = this.Days();
    this.displayedDayCount = GameUtil.GetCurrentCycle();
  }

  private string Days() => GameUtil.GetCurrentCycle().ToString();
}
