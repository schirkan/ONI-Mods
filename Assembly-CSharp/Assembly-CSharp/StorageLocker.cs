// Decompiled with JetBrains decompiler
// Type: StorageLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
  private LoggerFS log;
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  [Serialize]
  public string lockerName = "";
  protected FilteredStorage filteredStorage;
  [MyCmpGet]
  private UserNameable nameable;
  public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;
  private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>((System.Action<StorageLocker, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit() => this.Initialize(false);

  protected void Initialize(bool use_logic_meter)
  {
    base.OnPrefabInit();
    this.log = new LoggerFS(nameof (StorageLocker));
    ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (Tag[]) null, (IUserControlledCapacity) this, use_logic_meter, fetch_chore_type);
    this.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    this.filteredStorage.FilterChanged();
    if (!((UnityEngine.Object) this.nameable != (UnityEngine.Object) null) || this.lockerName.IsNullOrWhiteSpace())
      return;
    this.nameable.SetName(this.lockerName);
  }

  protected override void OnCleanUp() => this.filteredStorage.CleanUp();

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    StorageLocker component = gameObject.GetComponent<StorageLocker>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public virtual float UserMaxCapacity
  {
    get => Mathf.Min(this.userMaxCapacity, this.GetComponent<Storage>().capacityKg);
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
    }
  }

  public float AmountStored => this.GetComponent<Storage>().MassStored();

  public float MinCapacity => 0.0f;

  public float MaxCapacity => this.GetComponent<Storage>().capacityKg;

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();
}
