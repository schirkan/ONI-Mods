// Decompiled with JetBrains decompiler
// Type: PlantableSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
  public Tag PlantID;
  public Tag PreviewID;
  [Serialize]
  public float timeUntilSelfPlant;
  public Tag replantGroundTag;
  public string domesticatedDescription;
  public SingleEntityReceptacle.ReceptacleDirection direction;
  private static readonly EventSystem.IntraObjectHandler<PlantableSeed> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PlantableSeed>((System.Action<PlantableSeed, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantableSeed> OnSplitDelegate = new EventSystem.IntraObjectHandler<PlantableSeed>((System.Action<PlantableSeed, object>) ((component, data) => component.OnSplit(data)));

  public SingleEntityReceptacle.ReceptacleDirection Direction => this.direction;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PlantableSeed>(-2064133523, PlantableSeed.OnAbsorbDelegate);
    this.Subscribe<PlantableSeed>(1335436905, PlantableSeed.OnSplitDelegate);
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.PlantableSeeds.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.PlantableSeeds.Remove(this);
    base.OnCleanUp();
  }

  private void OnAbsorb(object data)
  {
  }

  private void OnSplit(object data)
  {
  }

  public void TryPlant(bool allow_plant_from_storage = false)
  {
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
    if (!allow_plant_from_storage && this.gameObject.HasTag(GameTags.Stored))
      return;
    int cell = Grid.PosToCell(this.gameObject);
    if (!this.TestSuitableGround(cell))
      return;
    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), posCbc, Grid.SceneLayer.BuildingFront);
    gameObject.SetActive(true);
    Pickupable pickupable = this.GetComponent<Pickupable>().Take(1f);
    if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
    {
      int num = (UnityEngine.Object) gameObject.GetComponent<Crop>() != (UnityEngine.Object) null ? 1 : 0;
      Util.KDestroyGameObject(pickupable.gameObject);
    }
    else
      KCrashReporter.Assert(false, "Seed has fractional total amount < 1f");
  }

  public bool TestSuitableGround(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int index = this.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(cell) : Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index) || Grid.Foundation[index] || Grid.Element[index].hardness >= (byte) 150 || this.replantGroundTag.IsValid && !Grid.Element[index].HasTag(this.replantGroundTag))
      return false;
    GameObject prefab = Assets.GetPrefab(this.PlantID);
    EntombVulnerable component1 = prefab.GetComponent<EntombVulnerable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !component1.IsCellSafe(cell))
      return false;
    DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.IsCellSafe(cell))
      return false;
    TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && !component3.IsCellSafe(cell))
      return false;
    UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && !component4.IsCellSafe(cell))
      return false;
    OccupyArea component5 = prefab.GetComponent<OccupyArea>();
    return !((UnityEngine.Object) component5 != (UnityEngine.Object) null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
    }
    else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }
}
