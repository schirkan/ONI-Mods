// Decompiled with JetBrains decompiler
// Type: DiseaseVisualization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseVisualization : ScriptableObject
{
  public Sprite overlaySprite;
  public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

  public DiseaseVisualization.Info GetInfo(HashedString id)
  {
    foreach (DiseaseVisualization.Info info in this.info)
    {
      if (id == (HashedString) info.name)
        return info;
    }
    return new DiseaseVisualization.Info();
  }

  [Serializable]
  public struct Info
  {
    public string name;
    public string overlayColourName;

    public Info(string name)
    {
      this.name = name;
      this.overlayColourName = "germFoodPoisoning";
    }
  }
}
