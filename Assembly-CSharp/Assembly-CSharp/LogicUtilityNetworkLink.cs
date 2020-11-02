// Decompiled with JetBrains decompiler
// Type: LogicUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
  public LogicWire.BitDepth bitDepth;
  public int cell_one;
  public int cell_two;

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnConnect(int cell1, int cell2)
  {
    this.cell_one = cell1;
    this.cell_two = cell2;
    Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
    Game.Instance.logicCircuitManager.Connect(this);
  }

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
    Game.Instance.logicCircuitManager.Disconnect(this);
  }

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem;

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    int linked_cell1;
    this.GetCells(out linked_cell1, out int _);
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(linked_cell1);
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    int linked_cell1;
    this.GetCells(out linked_cell1, out int _);
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(linked_cell1);
    return networks.Contains(networkForCell);
  }
}
