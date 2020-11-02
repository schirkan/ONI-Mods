// Decompiled with JetBrains decompiler
// Type: ResearchCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class ResearchCenter : Workable, IGameObjectEffectDescriptor, ISim200ms
{
  private Chore chore;
  [MyCmpAdd]
  protected Notifier notifier;
  [MyCmpAdd]
  protected Operational operational;
  [MyCmpAdd]
  protected Storage storage;
  [MyCmpGet]
  private ElementConverter elementConverter;
  [SerializeField]
  public string research_point_type_id;
  [SerializeField]
  public Tag inputMaterial;
  [SerializeField]
  public float mass_per_point;
  [SerializeField]
  private float remainder_mass_points;
  public static readonly Operational.Flag ResearchSelectedFlag = new Operational.Flag("researchSelected", Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<ResearchCenter> UpdateWorkingStateDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>((System.Action<ResearchCenter, object>) ((component, data) => component.UpdateWorkingState(data)));
  private static readonly EventSystem.IntraObjectHandler<ResearchCenter> CheckHasMaterialDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>((System.Action<ResearchCenter, object>) ((component, data) => component.CheckHasMaterial(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
    this.elementConverter.onConvertMass += new System.Action<float>(this.ConvertMassToResearchPoints);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Research.Instance.Subscribe(-1914338957, new System.Action<object>(this.UpdateWorkingState));
    Research.Instance.Subscribe(-125623018, new System.Action<object>(this.UpdateWorkingState));
    this.Subscribe<ResearchCenter>(187661686, ResearchCenter.UpdateWorkingStateDelegate);
    this.Subscribe<ResearchCenter>(-1697596308, ResearchCenter.CheckHasMaterialDelegate);
    Components.ResearchCenters.Add(this);
    this.UpdateWorkingState((object) null);
  }

  private void ConvertMassToResearchPoints(float mass_consumed)
  {
    this.remainder_mass_points += mass_consumed / this.mass_per_point - (float) Mathf.FloorToInt(mass_consumed / this.mass_per_point);
    int num = Mathf.FloorToInt(mass_consumed / this.mass_per_point) + Mathf.FloorToInt(this.remainder_mass_points);
    this.remainder_mass_points -= (float) Mathf.FloorToInt(this.remainder_mass_points);
    ResearchType researchType = Research.Instance.GetResearchType(this.research_point_type_id);
    if (num <= 0)
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, this.transform);
    for (int index = 0; index < num; ++index)
      Research.Instance.AddResearchPoints(this.research_point_type_id, 1f);
  }

  public void Sim200ms(float dt)
  {
    if (this.operational.IsActive || !this.operational.IsOperational || (this.chore != null || !this.HasMaterial()))
      return;
    this.chore = this.CreateChore();
    this.SetWorkTime(float.PositiveInfinity);
  }

  protected virtual Chore CreateChore() => (Chore) new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this, is_preemptable: true)
  {
    preemption_cb = new Func<Chore.Precondition.Context, bool>(ResearchCenter.CanPreemptCB)
  };

  private static bool CanPreemptCB(Chore.Precondition.Context context)
  {
    Worker component = context.chore.driver.GetComponent<Worker>();
    float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup((Component) component).Evaluate();
    Worker worker = context.consumerState.worker;
    return (double) Db.Get().AttributeConverters.ResearchSpeed.Lookup((Component) worker).Evaluate() > (double) num;
  }

  public override float GetPercentComplete()
  {
    if (Research.Instance.GetActiveResearch() == null)
      return 0.0f;
    float num1 = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID[this.research_point_type_id];
    float num2 = 0.0f;
    return !Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2) ? 1f : num1 / num2;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.operational.SetActive(true);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    float num = this.currentlyLit ? 1f + DUPLICANTSTATS.LIGHT.LIGHT_WORK_EFFICIENCY_BONUS : 1f;
    float speed = 1f + Db.Get().AttributeConverters.ResearchSpeed.Lookup((Component) worker).Evaluate() + num;
    if (Game.Instance.FastWorkersModeActive)
      speed *= 2f;
    this.elementConverter.SetWorkSpeedMultiplier(speed);
    return base.OnWorkTick(worker, dt);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.ShowProgressBar(false);
    this.operational.SetActive(false);
  }

  protected bool ResearchComponentCompleted()
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch != null)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      activeResearch.progressInventory.PointsByTypeID.TryGetValue(this.research_point_type_id, out num1);
      activeResearch.tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2);
      if ((double) num1 >= (double) num2)
        return true;
    }
    return false;
  }

  protected bool IsAllResearchComplete()
  {
    foreach (Tech resource in Db.Get().Techs.resources)
    {
      if (!resource.IsComplete())
        return false;
    }
    return true;
  }

  protected virtual void UpdateWorkingState(object data)
  {
    bool flag1 = false;
    bool flag2 = false;
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch != null)
    {
      flag1 = true;
      if (activeResearch.tech.costsByResearchTypeID.ContainsKey(this.research_point_type_id) && (double) Research.Instance.Get(activeResearch.tech).progressInventory.PointsByTypeID[this.research_point_type_id] < (double) activeResearch.tech.costsByResearchTypeID[this.research_point_type_id])
        flag2 = true;
    }
    if (this.operational.GetFlag(EnergyConsumer.PoweredFlag) && !this.IsAllResearchComplete())
    {
      if (flag1)
      {
        this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected);
        if (!flag2 && !this.ResearchComponentCompleted())
        {
          this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected);
          this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected);
        }
        else
          this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected);
      }
      else
      {
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected);
        this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected);
      }
    }
    else
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected);
    }
    this.operational.SetFlag(ResearchCenter.ResearchSelectedFlag, flag1 & flag2);
    if (flag1 && flag2 || !(bool) (UnityEngine.Object) this.worker)
      return;
    this.StopWork(this.worker, true);
  }

  private void ClearResearchScreen() => Game.Instance.Trigger(-1974454597, (object) null);

  private void CheckHasMaterial(object o = null)
  {
    if (this.HasMaterial() || this.chore == null)
      return;
    this.chore.Cancel("No material remaining");
    this.chore = (Chore) null;
  }

  private bool HasMaterial() => (double) this.storage.MassStored() > 0.0;

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Research.Instance.Unsubscribe(-1914338957, new System.Action<object>(this.UpdateWorkingState));
    Research.Instance.Unsubscribe(-125623018, new System.Action<object>(this.UpdateWorkingState));
    this.Unsubscribe(-1852328367, new System.Action<object>(this.UpdateWorkingState));
    Components.ResearchCenters.Remove(this);
    this.ClearResearchScreen();
  }

  public string GetStatusString()
  {
    string str = (string) RESEARCH.MESSAGING.NORESEARCHSELECTED;
    if (Research.Instance.GetActiveResearch() != null)
    {
      str = "<b>" + Research.Instance.GetActiveResearch().tech.Name + "</b>";
      int num = 0;
      foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
      {
        if ((double) Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0.0)
          ++num;
      }
      foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
      {
        if ((double) Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0.0 && keyValuePair.Key == this.research_point_type_id)
        {
          str = str + "\n   - " + Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name;
          str = str + ": " + (object) keyValuePair.Value + "/" + (object) Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key];
        }
      }
      foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
      {
        if ((double) Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0.0 && !(keyValuePair.Key == this.research_point_type_id))
          str = num <= 1 ? str + "\n   - " + string.Format((string) RESEARCH.MESSAGING.RESEARCHTYPEREQUIRED, (object) Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name) : str + "\n   - " + string.Format((string) RESEARCH.MESSAGING.RESEARCHTYPEALSOREQUIRED, (object) Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name);
      }
    }
    return str;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, (object) this.inputMaterial.ProperName(), (object) GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, (object) this.inputMaterial.ProperName(), (object) GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point)), Descriptor.DescriptorType.Requirement));
    descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, (object) Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, (object) Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name)));
    return descriptors;
  }
}
