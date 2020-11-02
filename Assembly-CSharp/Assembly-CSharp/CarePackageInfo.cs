﻿// Decompiled with JetBrains decompiler
// Type: CarePackageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class CarePackageInfo : ITelepadDeliverable
{
  public readonly string id;
  public readonly float quantity;
  public readonly Func<bool> requirement;

  public CarePackageInfo(string ID, float amount, Func<bool> requirement)
  {
    this.id = ID;
    this.quantity = amount;
    this.requirement = requirement;
  }

  public GameObject Deliver(Vector3 location)
  {
    location += Vector3.right / 2f;
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) CarePackageConfig.ID), location);
    gameObject.SetActive(true);
    gameObject.GetComponent<CarePackage>().SetInfo(this);
    return gameObject;
  }
}
