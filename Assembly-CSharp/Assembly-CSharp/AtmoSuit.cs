﻿// Decompiled with JetBrains decompiler
// Type: AtmoSuit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>((System.Action<AtmoSuit, object>) ((component, data) => component.RefreshStatusEffects(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
  }

  private void RefreshStatusEffects(object data)
  {
    Equippable component1 = this.GetComponent<Equippable>();
    bool flag = this.GetComponent<Storage>().Has(GameTags.AnyWater);
    if (!(component1.assignee != null & flag))
      return;
    Ownables soleOwner = component1.assignee.GetSoleOwner();
    if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
      return;
    GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    if (!(bool) (UnityEngine.Object) targetGameObject)
      return;
    Effects component2 = targetGameObject.GetComponent<Effects>();
    if (component2.HasEffect("SoiledSuit"))
      return;
    component2.Add("SoiledSuit", true);
  }
}
