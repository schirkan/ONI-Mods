// Decompiled with JetBrains decompiler
// Type: ProgressBarsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarsConfig : ScriptableObject
{
  public GameObject progressBarPrefab;
  public GameObject progressBarUIPrefab;
  public GameObject healthBarPrefab;
  public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();
  public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();
  private static ProgressBarsConfig instance;

  public static void DestroyInstance() => ProgressBarsConfig.instance = (ProgressBarsConfig) null;

  public static ProgressBarsConfig Instance
  {
    get
    {
      if ((UnityEngine.Object) ProgressBarsConfig.instance == (UnityEngine.Object) null)
      {
        ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>(nameof (ProgressBarsConfig));
        ProgressBarsConfig.instance.Initialize();
      }
      return ProgressBarsConfig.instance;
    }
  }

  public void Initialize()
  {
    foreach (ProgressBarsConfig.BarData barColorData in this.barColorDataList)
      this.barColorMap.Add(barColorData.barName, barColorData);
  }

  public string GetBarDescription(string barName)
  {
    string str = "";
    if (this.IsBarNameValid(barName))
      str = (string) Strings.Get(this.barColorMap[barName].barDescriptionKey);
    return str;
  }

  public Color GetBarColor(string barName)
  {
    Color color = Color.clear;
    if (this.IsBarNameValid(barName))
      color = this.barColorMap[barName].barColor;
    return color;
  }

  public bool IsBarNameValid(string barName)
  {
    if (string.IsNullOrEmpty(barName))
    {
      Debug.LogError((object) "The barName provided was null or empty. Don't do that.");
      return false;
    }
    if (this.barColorMap.ContainsKey(barName))
      return true;
    Debug.LogError((object) string.Format("No BarData found for the entry [ {0} ]", (object) barName));
    return false;
  }

  [Serializable]
  public struct BarData
  {
    public string barName;
    public Color barColor;
    public string barDescriptionKey;
  }
}
