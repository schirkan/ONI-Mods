// Decompiled with JetBrains decompiler
// Type: ElementFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElementFilter")]
public class ElementFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
{
  [SerializeField]
  public ConduitPortInfo portInfo;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  private Filterable filterable;
  private Guid needsConduitStatusItemGuid;
  private Guid conduitBlockedStatusItemGuid;
  private int inputCell = -1;
  private int outputCell = -1;
  private int filteredCell = -1;
  private FlowUtilityNetwork.NetworkItem itemFilter;
  private HandleVector<int>.Handle partitionerEntry;
  private static StatusItem filterStatusItem;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.InitializeStatusItems();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.inputCell = this.building.GetUtilityInputCell();
    this.outputCell = this.building.GetUtilityOutputCell();
    this.filteredCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.building.GetRotatedOffset(this.portInfo.offset));
    IUtilityNetworkMgr utilityNetworkMgr = this.portInfo.conduitType == ConduitType.Solid ? SolidConduit.GetFlowManager().networkMgr : Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, this.gameObject);
    int filteredCell = this.filteredCell;
    FlowUtilityNetwork.NetworkItem itemFilter = this.itemFilter;
    utilityNetworkMgr.AddToNetworks(filteredCell, (object) itemFilter, true);
    if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
      this.GetComponent<ConduitConsumer>().isConsuming = false;
    this.OnFilterChanged(this.filterable.SelectedTag);
    this.filterable.onFilterChanged += new System.Action<Tag>(this.OnFilterChanged);
    if (this.portInfo.conduitType == ConduitType.Solid)
      SolidConduit.GetFlowManager().AddConduitUpdater(new System.Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
    else
      Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new System.Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, (object) this);
    this.UpdateConduitExistsStatus();
    this.UpdateConduitBlockedStatus();
    ScenePartitionerLayer layer = (ScenePartitionerLayer) null;
    switch (this.portInfo.conduitType)
    {
      case ConduitType.Gas:
        layer = GameScenePartitioner.Instance.gasConduitsLayer;
        break;
      case ConduitType.Liquid:
        layer = GameScenePartitioner.Instance.liquidConduitsLayer;
        break;
      case ConduitType.Solid:
        layer = GameScenePartitioner.Instance.solidConduitsLayer;
        break;
    }
    if (layer == null)
      return;
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ElementFilterConduitExists", (object) this.gameObject, this.filteredCell, layer, (System.Action<object>) (data => this.UpdateConduitExistsStatus()));
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, (object) this.itemFilter, true);
    if (this.portInfo.conduitType == ConduitType.Solid)
      SolidConduit.GetFlowManager().RemoveConduitUpdater(new System.Action<float>(this.OnConduitTick));
    else
      Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new System.Action<float>(this.OnConduitTick));
    if (this.partitionerEntry.IsValid() && (UnityEngine.Object) GameScenePartitioner.Instance != (UnityEngine.Object) null)
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitTick(float dt)
  {
    bool flag = false;
    this.UpdateConduitBlockedStatus();
    if (this.operational.IsOperational)
    {
      if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
      {
        ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
        ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
        int num = contents1.element.CreateTag() == this.filterable.SelectedTag ? this.filteredCell : this.outputCell;
        ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
        if ((double) contents1.mass > 0.0 && (double) contents2.mass <= 0.0)
        {
          flag = true;
          float delta = flowManager.AddElement(num, contents1.element, contents1.mass, contents1.temperature, contents1.diseaseIdx, contents1.diseaseCount);
          if ((double) delta > 0.0)
            flowManager.RemoveElement(this.inputCell, delta);
        }
      }
      else
      {
        SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
        SolidConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
        Pickupable pickupable1 = flowManager.GetPickupable(contents1.pickupableHandle);
        if ((UnityEngine.Object) pickupable1 != (UnityEngine.Object) null)
        {
          int num = pickupable1.GetComponent<KPrefabID>().PrefabTag == this.filterable.SelectedTag ? this.filteredCell : this.outputCell;
          SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
          Pickupable pickupable2 = flowManager.GetPickupable(contents2.pickupableHandle);
          PrimaryElement primaryElement = (PrimaryElement) null;
          if ((UnityEngine.Object) pickupable2 != (UnityEngine.Object) null)
            primaryElement = pickupable2.PrimaryElement;
          if ((double) pickupable1.PrimaryElement.Mass > 0.0 && ((UnityEngine.Object) pickupable2 == (UnityEngine.Object) null || (double) primaryElement.Mass <= 0.0))
          {
            flag = true;
            Pickupable pickupable3 = flowManager.RemovePickupable(this.inputCell);
            if ((UnityEngine.Object) pickupable3 != (UnityEngine.Object) null)
              flowManager.AddPickupable(num, pickupable3);
          }
        }
        else
          flowManager.RemovePickupable(this.inputCell);
      }
    }
    this.operational.SetActive(flag);
  }

  private void UpdateConduitExistsStatus()
  {
    bool flag1 = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
    StatusItem status_item;
    switch (this.portInfo.conduitType)
    {
      case ConduitType.Gas:
        status_item = Db.Get().BuildingStatusItems.NeedGasOut;
        break;
      case ConduitType.Liquid:
        status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
        break;
      case ConduitType.Solid:
        status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
    if (flag1 != flag2)
      return;
    this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag1);
  }

  private void UpdateConduitBlockedStatus()
  {
    bool flag1 = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
    StatusItem blockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
    bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
    if (flag1 != flag2)
      return;
    this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(blockedMultiples, this.conduitBlockedStatusItemGuid, !flag1);
  }

  private void OnFilterChanged(Tag tag)
  {
    bool on = !tag.IsValid || tag == GameTags.Void;
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on);
  }

  private void InitializeStatusItems()
  {
    if (ElementFilter.filterStatusItem != null)
      return;
    ElementFilter.filterStatusItem = new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID);
    ElementFilter.filterStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      ElementFilter elementFilter = (ElementFilter) data;
      str = !elementFilter.filterable.SelectedTag.IsValid || elementFilter.filterable.SelectedTag == GameTags.Void ? string.Format((string) BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, (object) BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED) : string.Format((string) BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, (object) elementFilter.filterable.SelectedTag.ProperName());
      return str;
    });
    ElementFilter.filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(this.ShowInUtilityOverlay);
  }

  private bool ShowInUtilityOverlay(HashedString mode, object data)
  {
    bool flag = false;
    switch (((ElementFilter) data).portInfo.conduitType)
    {
      case ConduitType.Gas:
        flag = mode == OverlayModes.GasConduits.ID;
        break;
      case ConduitType.Liquid:
        flag = mode == OverlayModes.LiquidConduits.ID;
        break;
      case ConduitType.Solid:
        flag = mode == OverlayModes.SolidConveyor.ID;
        break;
    }
    return flag;
  }

  public ConduitType GetSecondaryConduitType() => this.portInfo.conduitType;

  public CellOffset GetSecondaryConduitOffset() => this.portInfo.offset;

  public int GetFilteredCell() => this.filteredCell;
}
