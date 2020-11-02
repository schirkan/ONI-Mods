// Decompiled with JetBrains decompiler
// Type: Crop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
  [MyCmpReq]
  private KSelectable selectable;
  public Crop.CropVal cropVal;
  public string domesticatedDesc = "";
  private Storage planterStorage;
  private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>((System.Action<Crop, object>) ((component, data) => component.OnHarvest(data)));
  private static readonly EventSystem.IntraObjectHandler<Crop> OnSeedDroppedDelegate = new EventSystem.IntraObjectHandler<Crop>((System.Action<Crop, object>) ((component, data) => component.OnSeedDropped(data)));

  public string cropId => this.cropVal.cropId;

  public Storage PlanterStorage
  {
    get => this.planterStorage;
    set => this.planterStorage = value;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Crops.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
    this.Subscribe<Crop>(-1736624145, Crop.OnSeedDroppedDelegate);
  }

  public void Configure(Crop.CropVal cropval) => this.cropVal = cropval;

  public bool CanGrow() => this.cropVal.renewable;

  public void SpawnFruit(object callbackParam)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    Crop.CropVal cropVal = this.cropVal;
    if (string.IsNullOrEmpty(cropVal.cropId))
      return;
    GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject), 0, 0, cropVal.cropId);
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      float y = 0.75f;
      gameObject.transform.SetPosition(gameObject.transform.GetPosition() + new Vector3(0.0f, y, 0.0f));
      gameObject.SetActive(true);
      PrimaryElement component1 = gameObject.GetComponent<PrimaryElement>();
      component1.Units = (float) cropVal.numProduced;
      component1.Temperature = this.gameObject.GetComponent<PrimaryElement>().Temperature;
      Edible component2 = gameObject.GetComponent<Edible>();
      if ((bool) (UnityEngine.Object) component2)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", component2.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
    }
    else
      DebugUtil.LogErrorArgs((UnityEngine.Object) this.gameObject, (object) "tried to spawn an invalid crop prefab:", (object) cropVal.cropId);
    this.Trigger(-1072826864, (object) null);
  }

  protected override void OnCleanUp()
  {
    Components.Crops.Remove(this);
    base.OnCleanUp();
  }

  private void OnHarvest(object obj)
  {
  }

  public void OnSeedDropped(object data)
  {
  }

  public List<Descriptor> RequirementDescriptors(GameObject go) => new List<Descriptor>();

  public List<Descriptor> InformationDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Tag tag = new Tag(this.cropVal.cropId);
    GameObject prefab = Assets.GetPrefab(tag);
    Edible component1 = prefab.GetComponent<Edible>();
    float calories1 = 0.0f;
    string str1 = "";
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      calories1 = component1.FoodInfo.CaloriesPerUnit;
    float calories2 = calories1 * (float) this.cropVal.numProduced;
    InfoDescription component2 = prefab.GetComponent<InfoDescription>();
    if ((bool) (UnityEngine.Object) component2)
      str1 = component2.description;
    string str2 = !GameTags.DisplayAsCalories.Contains(tag) ? (!GameTags.DisplayAsUnits.Contains(tag) ? GameUtil.GetFormattedMass((float) this.cropVal.numProduced) : GameUtil.GetFormattedUnits((float) this.cropVal.numProduced, displaySuffix: false)) : GameUtil.GetFormattedCalories(calories2);
    descriptorList.Add(new Descriptor(string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, (object) prefab.GetProperName(), (object) str2), string.Format((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, (object) str1, (object) GameUtil.GetFormattedCalories(calories1), (object) GameUtil.GetFormattedCalories(calories2))));
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (Descriptor requirementDescriptor in this.RequirementDescriptors(go))
      descriptorList.Add(requirementDescriptor);
    foreach (Descriptor informationDescriptor in this.InformationDescriptors(go))
      descriptorList.Add(informationDescriptor);
    return descriptorList;
  }

  [Serializable]
  public struct CropVal
  {
    public string cropId;
    public float cropDuration;
    public int numProduced;
    public bool renewable;

    public CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
    {
      this.cropId = crop_id;
      this.cropDuration = crop_duration;
      this.numProduced = num_produced;
      this.renewable = renewable;
    }
  }
}
