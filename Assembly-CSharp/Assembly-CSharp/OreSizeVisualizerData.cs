﻿// Decompiled with JetBrains decompiler
// Type: OreSizeVisualizerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct OreSizeVisualizerData
{
  public PrimaryElement primaryElement;
  public System.Action<object> onMassChangedCB;

  public OreSizeVisualizerData(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.onMassChangedCB = (System.Action<object>) null;
  }
}