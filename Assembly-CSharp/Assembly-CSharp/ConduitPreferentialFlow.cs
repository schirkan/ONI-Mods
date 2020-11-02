// Decompiled with JetBrains decompiler
// Type: ConduitPreferentialFlow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConduitPreferentialFlow")]
public class ConduitPreferentialFlow : KMonoBehaviour, ISecondaryInput
{
  [SerializeField]
  public ConduitPortInfo portInfo;
  private int inputCell;
  private int outputCell;
  private FlowUtilityNetwork.NetworkItem secondaryInput;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    int cell1 = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), component.GetRotatedOffset(this.portInfo.offset));
    Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.secondaryInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell1, this.gameObject);
    int cell2 = this.secondaryInput.Cell;
    FlowUtilityNetwork.NetworkItem secondaryInput = this.secondaryInput;
    networkManager.AddToNetworks(cell2, (object) secondaryInput, true);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInput.Cell, (object) this.secondaryInput, true);
    Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
    if (!flowManager.HasConduit(this.outputCell))
      return;
    int cell = this.inputCell;
    ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
    if ((double) contents.mass <= 0.0)
    {
      cell = this.secondaryInput.Cell;
      contents = flowManager.GetContents(cell);
    }
    if ((double) contents.mass <= 0.0)
      return;
    float delta = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
    if ((double) delta <= 0.0)
      return;
    flowManager.RemoveElement(cell, delta);
  }

  public ConduitType GetSecondaryConduitType() => this.portInfo.conduitType;

  public CellOffset GetSecondaryConduitOffset() => this.portInfo.offset;
}
