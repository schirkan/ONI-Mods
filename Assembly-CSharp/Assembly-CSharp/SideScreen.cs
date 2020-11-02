﻿// Decompiled with JetBrains decompiler
// Type: SideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SideScreen : KScreen
{
  [SerializeField]
  private GameObject contentBody;

  public void SetContent(SideScreenContent sideScreenContent, GameObject target)
  {
    if ((Object) sideScreenContent.transform.parent != (Object) this.contentBody.transform)
      sideScreenContent.transform.SetParent(this.contentBody.transform);
    sideScreenContent.SetTarget(target);
  }
}
