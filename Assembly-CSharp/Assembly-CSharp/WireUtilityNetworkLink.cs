// Decompiled with JetBrains decompiler
// Type: WireUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IUtilityNetworkItem, IBridgedNetworkItem
{
  [SerializeField]
  public Wire.WattageRating maxWattageRating;

  public Wire.WattageRating GetMaxWattageRating() => this.maxWattageRating;

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
    Game.Instance.circuitManager.Disconnect(this);
  }

  protected override void OnConnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
    Game.Instance.circuitManager.Connect(this);
  }

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem;

  public ushort NetworkID
  {
    get
    {
      int linked_cell1;
      this.GetCells(out linked_cell1, out int _);
      return !(Game.Instance.electricalConduitSystem.GetNetworkForCell(linked_cell1) is ElectricalUtilityNetwork networkForCell) ? ushort.MaxValue : (ushort) networkForCell.id;
    }
  }

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
