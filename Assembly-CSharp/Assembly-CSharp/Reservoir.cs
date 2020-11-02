// Decompiled with JetBrains decompiler
// Type: Reservoir
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
  private MeterController meter;
  [MyCmpGet]
  private Storage storage;
  private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>((System.Action<Reservoir, object>) ((component, data) => component.OnStorageChange(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[2]
    {
      "meter_fill",
      "meter_OL"
    });
    this.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
    this.OnStorageChange((object) null);
  }

  private void OnStorageChange(object data) => this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
}
