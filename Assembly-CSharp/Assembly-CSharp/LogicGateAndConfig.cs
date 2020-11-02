﻿// Decompiled with JetBrains decompiler
// Type: LogicGateAndConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class LogicGateAndConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateAND";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.And;

  protected override CellOffset[] InputPortOffsets => new CellOffset[2]
  {
    CellOffset.none,
    new CellOffset(0, 1)
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
      name = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
      active = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
      inactive = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
    }
  };

  public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef("LogicGateAND", "logic_and_kanim");
}