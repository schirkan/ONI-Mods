// Decompiled with JetBrains decompiler
// Type: SimpleVent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
  [MyCmpGet]
  private Operational operational;
  private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>((System.Action<SimpleVent, object>) ((component, data) => component.OnChanged(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
    this.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
  }

  protected override void OnSpawn() => this.OnChanged((object) null);

  private void OnChanged(object data)
  {
    if (this.operational.IsFunctional)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, (object) this);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
  }
}
