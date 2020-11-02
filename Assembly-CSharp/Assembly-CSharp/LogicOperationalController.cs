// Decompiled with JetBrains decompiler
// Type: LogicOperationalController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LogicOperationalController")]
public class LogicOperationalController : KMonoBehaviour
{
  public static readonly HashedString PORT_ID = (HashedString) "LogicOperational";
  public int unNetworkedValue = 1;
  public static readonly Operational.Flag LogicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);
  private static StatusItem infoStatusItem;
  [MyCmpGet]
  public Operational operational;
  private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>((System.Action<LogicOperationalController, object>) ((component, data) => component.OnLogicValueChanged(data)));

  public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset) => new List<LogicPorts.Port>()
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE)
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
    if (LogicOperationalController.infoStatusItem == null)
    {
      LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      LogicOperationalController.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
    }
    this.CheckWireState();
  }

  private LogicCircuitNetwork GetNetwork() => Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID));

  private LogicCircuitNetwork CheckWireState()
  {
    LogicCircuitNetwork network = this.GetNetwork();
    int num = network != null ? network.OutputValue : this.unNetworkedValue;
    this.operational.SetFlag(LogicOperationalController.LogicOperationalFlag, LogicCircuitNetwork.IsBitActive(0, num));
    return network;
  }

  private static string ResolveInfoStatusItemString(string format_str, object data) => (string) (((LogicOperationalController) data).operational.GetFlag(LogicOperationalController.LogicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED);

  private void OnLogicValueChanged(object data)
  {
    if (!(((LogicValueChanged) data).portID == LogicOperationalController.PORT_ID))
      return;
    LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
    this.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, (object) this);
  }
}
