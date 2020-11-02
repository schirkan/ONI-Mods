﻿// Decompiled with JetBrains decompiler
// Type: PlantAirConditioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
  private Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>((System.Action<PlantAirConditioner, object>) ((component, data) => component.OnFertilized(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>((System.Action<PlantAirConditioner, object>) ((component, data) => component.OnUnfertilized(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
    this.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
  }

  private void OnFertilized(object data) => this.operational.SetFlag(this.fertilizedFlag, true);

  private void OnUnfertilized(object data) => this.operational.SetFlag(this.fertilizedFlag, false);
}
