// Decompiled with JetBrains decompiler
// Type: SeedProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public SeedProducer.SeedInfo seedInfo;
  private bool droppedSeedAlready;
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((System.Action<SeedProducer, object>) ((component, data) => component.DropSeed(data)));
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((System.Action<SeedProducer, object>) ((component, data) => component.CropPicked(data)));

  public void Configure(
    string SeedID,
    SeedProducer.ProductionType productionType,
    int newSeedsProduced = 1)
  {
    this.seedInfo.seedId = SeedID;
    this.seedInfo.productionType = productionType;
    this.seedInfo.newSeedsProduced = newSeedsProduced;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
  }

  public GameObject ProduceSeed(string seedId, int units = 1)
  {
    if (seedId == null || units <= 0)
      return (GameObject) null;
    Vector3 position = this.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f);
    GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(seedId)), position, Grid.SceneLayer.Ore);
    PrimaryElement component1 = this.gameObject.GetComponent<PrimaryElement>();
    PrimaryElement component2 = go.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Units = (float) units;
    this.Trigger(472291861, (object) go.GetComponent<PlantableSeed>());
    go.SetActive(true);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, go.GetProperName(), go.transform);
    return go;
  }

  public void DropSeed(object data = null)
  {
    if (this.droppedSeedAlready)
      return;
    this.Trigger(-1736624145, (object) this.ProduceSeed(this.seedInfo.seedId).GetComponent<PlantableSeed>());
    this.droppedSeedAlready = true;
  }

  public void CropDepleted(object data) => this.DropSeed();

  public void CropPicked(object data)
  {
    if (this.seedInfo.productionType != SeedProducer.ProductionType.Harvest)
      return;
    Worker completedBy = this.GetComponent<Harvestable>().completed_by;
    float num = 0.1f;
    if ((UnityEngine.Object) completedBy != (UnityEngine.Object) null)
      num += completedBy.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
    this.ProduceSeed(this.seedInfo.seedId, (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num ? 1 : 0);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    int num = (UnityEngine.Object) Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != (UnityEngine.Object) null ? 1 : 0;
    switch (this.seedInfo.productionType)
    {
      case SeedProducer.ProductionType.DigOnly:
        return (List<Descriptor>) null;
      case SeedProducer.ProductionType.Harvest:
        descriptorList.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
        descriptorList.Add(new Descriptor(string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, (object) GameUtil.GetFormattedPercent(10f)), string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, (object) GameUtil.GetFormattedPercent(10f))));
        break;
      case SeedProducer.ProductionType.Fruit:
        descriptorList.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
        break;
      default:
        return (List<Descriptor>) null;
    }
    return descriptorList;
  }

  [Serializable]
  public struct SeedInfo
  {
    public string seedId;
    public SeedProducer.ProductionType productionType;
    public int newSeedsProduced;
  }

  public enum ProductionType
  {
    Hidden,
    DigOnly,
    Harvest,
    Fruit,
  }
}
