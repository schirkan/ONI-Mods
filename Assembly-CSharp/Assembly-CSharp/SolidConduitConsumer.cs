// Decompiled with JetBrains decompiler
// Type: SolidConduitConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour
{
  [SerializeField]
  public Tag capacityTag = GameTags.Any;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  [SerializeField]
  public bool alwaysConsume;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private int utilityCell = -1;
  private bool consuming;

  public bool IsConsuming => this.consuming;

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, 20];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }

  private SolidConduitFlow GetConduitFlow() => Game.Instance.solidConduitFlow;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.utilityCell = this.building.GetUtilityInputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new System.Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitFlow().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitFlow().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitConnectionChanged(object data)
  {
    this.consuming = this.consuming && this.IsConnected;
    this.Trigger(-2094018600, (object) this.IsConnected);
  }

  private void ConduitUpdate(float dt)
  {
    bool flag = false;
    SolidConduitFlow conduitFlow = this.GetConduitFlow();
    if (this.IsConnected)
    {
      SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
      if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
      {
        float num1 = this.capacityTag != GameTags.Any ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
        float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
        float num3 = Mathf.Max(0.0f, num2 - num1);
        if ((double) num3 > 0.0)
        {
          Pickupable pickupable1 = conduitFlow.GetPickupable(contents.pickupableHandle);
          if ((double) pickupable1.PrimaryElement.Mass <= (double) num3 || (double) pickupable1.PrimaryElement.Mass > (double) num2)
          {
            Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
            if ((bool) (UnityEngine.Object) pickupable2)
            {
              this.storage.Store(pickupable2.gameObject, true);
              flag = true;
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.storage.storageNetworkID = this.GetConnectedNetworkID();
    this.consuming = flag;
  }

  private int GetConnectedNetworkID()
  {
    GameObject gameObject = Grid.Objects[this.utilityCell, 20];
    SolidConduit solidConduit = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<SolidConduit>() : (SolidConduit) null;
    UtilityNetwork utilityNetwork = (UnityEngine.Object) solidConduit != (UnityEngine.Object) null ? solidConduit.GetNetwork() : (UtilityNetwork) null;
    return utilityNetwork == null ? -1 : utilityNetwork.id;
  }
}
