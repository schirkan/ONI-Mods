// Decompiled with JetBrains decompiler
// Type: ElementSplitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public struct ElementSplitter
{
  public PrimaryElement primaryElement;
  public Func<float, Pickupable> onTakeCB;
  public Func<Pickupable, bool> canAbsorbCB;

  public ElementSplitter(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.onTakeCB = (Func<float, Pickupable>) null;
    this.canAbsorbCB = (Func<Pickupable, bool>) null;
  }
}
