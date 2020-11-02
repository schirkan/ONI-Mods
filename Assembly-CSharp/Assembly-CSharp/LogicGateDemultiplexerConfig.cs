﻿// Decompiled with JetBrains decompiler
// Type: LogicGateDemultiplexerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateDemultiplexer";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.Demultiplexer;

  protected override CellOffset[] InputPortOffsets => new CellOffset[1]
  {
    new CellOffset(-1, 3)
  };

  protected override CellOffset[] OutputPortOffsets => new CellOffset[4]
  {
    new CellOffset(1, 3),
    new CellOffset(1, 2),
    new CellOffset(1, 1),
    new CellOffset(1, 0)
  };

  protected override CellOffset[] ControlPortOffsets => new CellOffset[2]
  {
    new CellOffset(-1, 0),
    new CellOffset(0, 0)
  };

  protected override LogicGate.LogicGateDescriptions GetDescriptions() => new LogicGate.LogicGateDescriptions()
  {
    outputOne = new LogicGate.LogicGateDescriptions.Description()
    {
      name = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
      active = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
      inactive = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
    }
  };

  public override BuildingDef CreateBuildingDef() => this.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
}
