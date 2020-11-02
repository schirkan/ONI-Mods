﻿// Decompiled with JetBrains decompiler
// Type: GasConduitDiseaseSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GasConduitDiseaseSensorConfig : ConduitSensorConfig
{
  public static string ID = "GasConduitDiseaseSensor";

  protected override ConduitType ConduitType => ConduitType.Gas;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = this.CreateBuildingDef(GasConduitDiseaseSensorConfig.ID, "gas_germs_sensor_kanim", new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, new string[2]{ "RefinedMetal", "Plastic" }, new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT_INACTIVE, true)
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, GasConduitDiseaseSensorConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    base.DoPostConfigureComplete(go);
    ConduitDiseaseSensor conduitDiseaseSensor = go.AddComponent<ConduitDiseaseSensor>();
    conduitDiseaseSensor.conduitType = this.ConduitType;
    conduitDiseaseSensor.Threshold = 0.0f;
    conduitDiseaseSensor.ActivateAboveThreshold = true;
    conduitDiseaseSensor.manuallyControlled = false;
    conduitDiseaseSensor.defaultState = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}