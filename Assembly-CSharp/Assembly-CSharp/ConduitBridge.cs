// Decompiled with JetBrains decompiler
// Type: ConduitBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : KMonoBehaviour, IBridgedNetworkItem
{
  [SerializeField]
  public ConduitType type;
  private int inputCell;
  private int outputCell;
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.accumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    Conduit.GetFlowManager(this.type).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    Game.Instance.accumulators.Remove(this.accumulator);
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
    if (!flowManager.HasConduit(this.inputCell))
      return;
    ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
    if ((double) contents.mass <= 0.0)
      return;
    float delta = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
    if ((double) delta <= 0.0)
      return;
    flowManager.RemoveElement(this.inputCell, delta);
    Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
  }

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
    UtilityNetwork networkForCell1 = networkManager.GetNetworkForCell(this.inputCell);
    if (networkForCell1 != null)
      networks.Add(networkForCell1);
    UtilityNetwork networkForCell2 = networkManager.GetNetworkForCell(this.outputCell);
    if (networkForCell2 == null)
      return;
    networks.Add(networkForCell2);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
    return (false ? 1 : (networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) ? 1 : 0)) != 0 || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
  }

  public int GetNetworkCell() => this.inputCell;
}
