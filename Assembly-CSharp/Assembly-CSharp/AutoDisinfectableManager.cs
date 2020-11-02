﻿// Decompiled with JetBrains decompiler
// Type: AutoDisinfectableManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
  private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();
  public static AutoDisinfectableManager Instance;

  public static void DestroyInstance() => AutoDisinfectableManager.Instance = (AutoDisinfectableManager) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    AutoDisinfectableManager.Instance = this;
  }

  public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable) => this.autoDisinfectables.Add(auto_disinfectable);

  public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
  {
    auto_disinfectable.CancelChore();
    this.autoDisinfectables.Remove(auto_disinfectable);
  }

  public void Sim1000ms(float dt)
  {
    for (int index = 0; index < this.autoDisinfectables.Count; ++index)
      this.autoDisinfectables[index].RefreshChore();
  }
}
