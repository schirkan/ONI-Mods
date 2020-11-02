// Decompiled with JetBrains decompiler
// Type: GeneratedBuildings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedBuildings
{
  public static void LoadGeneratedBuildings(List<System.Type> types)
  {
    System.Type type1 = typeof (IBuildingConfig);
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type type2 in types)
    {
      if (type1.IsAssignableFrom(type2) && !type2.IsAbstract && !type2.IsInterface)
        typeList.Add(type2);
    }
    foreach (System.Type type2 in typeList)
    {
      object instance = Activator.CreateInstance(type2);
      BuildingConfigManager.Instance.RegisterBuilding(instance as IBuildingConfig);
    }
  }

  public static void MakeBuildingAlwaysOperational(GameObject go)
  {
    BuildingDef def = go.GetComponent<BuildingComplete>().Def;
    if (def.LogicInputPorts != null || def.LogicOutputPorts != null)
      Debug.LogWarning((object) "Do not call MakeBuildingAlwaysOperational directly if LogicInputPorts or LogicOutputPorts are defined. Instead set BuildingDef.AlwaysOperational = true");
    GeneratedBuildings.MakeBuildingAlwaysOperationalImpl(go);
  }

  public static void RemoveLoopingSounds(GameObject go) => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LoopingSounds>());

  public static void RemoveDefaultLogicPorts(GameObject go) => UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LogicPorts>());

  public static void RegisterWithOverlay(HashSet<Tag> overlay_tags, string id)
  {
    overlay_tags.Add(new Tag(id));
    overlay_tags.Add(new Tag(id + "UnderConstruction"));
  }

  public static void RegisterSingleLogicInputPort(GameObject go)
  {
    LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0)).ToArray();
    logicPorts.outputPortInfo = (LogicPorts.Port[]) null;
  }

  private static void MakeBuildingAlwaysOperationalImpl(GameObject go)
  {
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<BuildingEnabledButton>());
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<Operational>());
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LogicPorts>());
  }

  public static void InitializeLogicPorts(GameObject go, BuildingDef def)
  {
    if (def.AlwaysOperational)
      GeneratedBuildings.MakeBuildingAlwaysOperationalImpl(go);
    if (def.LogicInputPorts == null && def.LogicOutputPorts == null)
      return;
    LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = def.LogicInputPorts != null ? def.LogicInputPorts.ToArray() : (LogicPorts.Port[]) null;
    logicPorts.outputPortInfo = def.LogicOutputPorts != null ? def.LogicOutputPorts.ToArray() : (LogicPorts.Port[]) null;
  }
}
