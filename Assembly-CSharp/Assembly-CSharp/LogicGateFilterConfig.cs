﻿// Decompiled with JetBrains decompiler
// Type: LogicGateFilterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LogicGateFilterConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateFILTER";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.CustomSingle;

  protected override CellOffset[] InputPortOffsets => new CellOffset[1]
  {
    CellOffset.none
  };

  protected override CellOffset[] OutputPortOffsets => new CellOffset[1]
  {
    new CellOffset(1, 0)
  };

  protected override CellOffset[] ControlPortOffsets => (CellOffset[]) null;

  protected override LogicGate.LogicGateDescriptions GetDescriptions() => new LogicGate.LogicGateDescriptions()
  {
    outputOne = new LogicGate.LogicGateDescriptions.Description()
    {
      name = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
      active = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
      inactive = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
    }
  };

  public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", height: 1);

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
    logicGateFilter.op = this.GetLogicOp();
    logicGateFilter.inputPortOffsets = this.InputPortOffsets;
    logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
    logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions()));
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
