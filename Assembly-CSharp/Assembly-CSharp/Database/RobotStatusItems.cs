// Decompiled with JetBrains decompiler
// Type: Database.RobotStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace Database
{
  public class RobotStatusItems : StatusItems
  {
    public StatusItem LowBattery;
    public StatusItem CantReachStation;
    public StatusItem DustBinFull;
    public StatusItem Working;
    public StatusItem UnloadingStorage;
    public StatusItem ReactPositive;
    public StatusItem ReactNegative;
    public StatusItem MovingToChargeStation;

    public RobotStatusItems(ResourceSet parent)
      : base(nameof (RobotStatusItems), parent)
      => this.CreateStatusItems();

    private void CreateStatusItems()
    {
      this.CantReachStation = new StatusItem("CantReachStation", "ROBOTS", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
      this.CantReachStation.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.LowBattery = new StatusItem("LowBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
      this.LowBattery.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.DustBinFull = new StatusItem("DustBinFull", "ROBOTS", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.DustBinFull.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.Working = new StatusItem("Working", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.Working.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.MovingToChargeStation = new StatusItem("MovingToChargeStation", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.MovingToChargeStation.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.UnloadingStorage = new StatusItem("UnloadingStorage", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.UnloadingStorage.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.ReactPositive = new StatusItem("ReactPositive", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.ReactPositive.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.ReactNegative = new StatusItem("ReactNegative", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
      this.ReactNegative.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    }
  }
}
