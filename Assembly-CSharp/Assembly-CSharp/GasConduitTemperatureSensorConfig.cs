// Decompiled with JetBrains decompiler
// Type: GasConduitTemperatureSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GasConduitTemperatureSensorConfig : ConduitSensorConfig
{
  public static string ID = "GasConduitTemperatureSensor";

  protected override ConduitType ConduitType => ConduitType.Gas;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = this.CreateBuildingDef(GasConduitTemperatureSensorConfig.ID, "gas_temperature_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT_INACTIVE, true)
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, GasConduitTemperatureSensorConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    base.DoPostConfigureComplete(go);
    ConduitTemperatureSensor temperatureSensor = go.AddComponent<ConduitTemperatureSensor>();
    temperatureSensor.conduitType = this.ConduitType;
    temperatureSensor.Threshold = 280f;
    temperatureSensor.ActivateAboveThreshold = true;
    temperatureSensor.manuallyControlled = false;
    temperatureSensor.rangeMin = 0.0f;
    temperatureSensor.rangeMax = 9999f;
    temperatureSensor.defaultState = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
