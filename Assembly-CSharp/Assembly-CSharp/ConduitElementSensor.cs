// Decompiled with JetBrains decompiler
// Type: ConduitElementSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
  [MyCmpGet]
  private Filterable filterable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.filterable.onFilterChanged += new System.Action<Tag>(this.OnFilterChanged);
    this.OnFilterChanged(this.filterable.SelectedTag);
  }

  private void OnFilterChanged(Tag tag)
  {
    if (!tag.IsValid)
      return;
    bool on = tag == GameTags.Void;
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on);
  }

  protected override void ConduitUpdate(float dt)
  {
    Tag element;
    bool hasMass;
    this.GetContentsElement(out element, out hasMass);
    if (!this.IsSwitchedOn)
    {
      if (!(element == this.filterable.SelectedTag & hasMass))
        return;
      this.Toggle();
    }
    else
    {
      if (!(element != this.filterable.SelectedTag) && hasMass)
        return;
      this.Toggle();
    }
  }

  private void GetContentsElement(out Tag element, out bool hasMass)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
    {
      ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
      element = contents.element.CreateTag();
      hasMass = (double) contents.mass > 0.0;
    }
    else
    {
      SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
      Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(cell).pickupableHandle);
      KPrefabID kprefabId = (UnityEngine.Object) pickupable != (UnityEngine.Object) null ? pickupable.GetComponent<KPrefabID>() : (KPrefabID) null;
      if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null && (double) pickupable.PrimaryElement.Mass > 0.0)
      {
        element = kprefabId.PrefabTag;
        hasMass = true;
      }
      else
      {
        element = GameTags.Void;
        hasMass = false;
      }
    }
  }
}
