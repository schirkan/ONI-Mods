﻿// Decompiled with JetBrains decompiler
// Type: SicknessExposureInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct SicknessExposureInfo
{
  public string sicknessID;
  public string sourceInfo;

  public SicknessExposureInfo(string id, string infection_source_info)
  {
    this.sicknessID = id;
    this.sourceInfo = infection_source_info;
  }
}
