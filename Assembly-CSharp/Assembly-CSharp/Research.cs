// Decompiled with JetBrains decompiler
// Type: Research
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Research")]
public class Research : KMonoBehaviour, ISaveLoadable
{
  public static Research Instance;
  [MyCmpAdd]
  private Notifier notifier;
  private List<TechInstance> techs = new List<TechInstance>();
  private List<TechInstance> queuedTech = new List<TechInstance>();
  private TechInstance activeResearch;
  private Notification NoResearcherRole = new Notification((string) RESEARCH.MESSAGING.NO_RESEARCHER_SKILL, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((list, data) => (string) RESEARCH.MESSAGING.NO_RESEARCHER_SKILL_TOOLTIP), expires: false, delay: 12f);
  private Notification MissingResearchStation = new Notification((string) RESEARCH.MESSAGING.MISSING_RESEARCH_STATION, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((list, data) => RESEARCH.MESSAGING.MISSING_RESEARCH_STATION_TOOLTIP.ToString().Replace("{0}", Research.Instance.GetMissingResearchBuildingName())), expires: false, delay: 11f);
  private List<ResearchCenter> researchCenterPrefabs = new List<ResearchCenter>();
  public ResearchTypes researchTypes;
  public bool UseGlobalPointInventory;
  [Serialize]
  public ResearchPointInventory globalPointInventory;
  [Serialize]
  private Research.SaveData saveData;
  private static readonly EventSystem.IntraObjectHandler<Research> OnRolesUpdatedDelegate = new EventSystem.IntraObjectHandler<Research>((System.Action<Research, object>) ((component, data) => component.OnRolesUpdated(data)));

  public static void DestroyInstance() => Research.Instance = (Research) null;

  public bool IsBeingResearched(Tech tech) => this.activeResearch != null && tech != null && this.activeResearch.tech == tech;

  protected override void OnPrefabInit()
  {
    Research.Instance = this;
    this.researchTypes = new ResearchTypes();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.globalPointInventory == null)
      this.globalPointInventory = new ResearchPointInventory();
    this.Subscribe<Research>(-1523247426, Research.OnRolesUpdatedDelegate);
    Components.ResearchCenters.OnAdd += new System.Action<ResearchCenter>(this.CheckResearchBuildings);
    Components.ResearchCenters.OnRemove += new System.Action<ResearchCenter>(this.CheckResearchBuildings);
    foreach (Component prefab in Assets.Prefabs)
    {
      ResearchCenter component = prefab.GetComponent<ResearchCenter>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.researchCenterPrefabs.Add(component);
    }
  }

  public ResearchType GetResearchType(string id) => this.researchTypes.GetResearchType(id);

  public TechInstance GetActiveResearch() => this.activeResearch;

  public TechInstance GetTargetResearch() => this.queuedTech != null && this.queuedTech.Count > 0 ? this.queuedTech[this.queuedTech.Count - 1] : (TechInstance) null;

  public TechInstance Get(Tech tech)
  {
    foreach (TechInstance tech1 in this.techs)
    {
      if (tech1.tech == tech)
        return tech1;
    }
    return (TechInstance) null;
  }

  public TechInstance GetOrAdd(Tech tech)
  {
    TechInstance techInstance1 = this.techs.Find((Predicate<TechInstance>) (tc => tc.tech == tech));
    if (techInstance1 != null)
      return techInstance1;
    TechInstance techInstance2 = new TechInstance(tech);
    this.techs.Add(techInstance2);
    return techInstance2;
  }

  public void GetNextTech()
  {
    if (this.queuedTech.Count > 0)
      this.queuedTech.RemoveAt(0);
    if (this.queuedTech.Count > 0)
      this.SetActiveResearch(this.queuedTech[this.queuedTech.Count - 1].tech);
    else
      this.SetActiveResearch((Tech) null);
  }

  private void AddTechToQueue(Tech tech)
  {
    TechInstance orAdd = this.GetOrAdd(tech);
    if (!orAdd.IsComplete())
      this.queuedTech.Add(orAdd);
    orAdd.tech.requiredTech.ForEach((System.Action<Tech>) (_tech => this.AddTechToQueue(_tech)));
  }

  public void CancelResearch(Tech tech, bool clickedEntry = true)
  {
    TechInstance ti = this.queuedTech.Find((Predicate<TechInstance>) (qt => qt.tech == tech));
    if (ti == null)
      return;
    if (ti == this.queuedTech[this.queuedTech.Count - 1] & clickedEntry)
      this.SetActiveResearch((Tech) null);
    for (int i = ti.tech.unlockedTech.Count - 1; i >= 0; i--)
    {
      if (this.queuedTech.Find((Predicate<TechInstance>) (qt => qt.tech == ti.tech.unlockedTech[i])) != null)
        this.CancelResearch(ti.tech.unlockedTech[i], false);
    }
    this.queuedTech.Remove(ti);
    if (!clickedEntry)
      return;
    this.Trigger(-1914338957, (object) this.queuedTech);
  }

  public void SetActiveResearch(Tech tech, bool clearQueue = false)
  {
    if (clearQueue)
      this.queuedTech.Clear();
    this.activeResearch = (TechInstance) null;
    if (tech != null)
    {
      if (this.queuedTech.Count == 0)
        this.AddTechToQueue(tech);
      if (this.queuedTech.Count > 0)
      {
        this.queuedTech.Sort((Comparison<TechInstance>) ((x, y) => x.tech.tier.CompareTo(y.tech.tier)));
        this.activeResearch = this.queuedTech[0];
      }
    }
    else
      this.queuedTech.Clear();
    this.Trigger(-1914338957, (object) this.queuedTech);
    this.CheckBuyResearch();
    this.CheckResearchBuildings((object) null);
    if (this.activeResearch != null)
    {
      if (this.activeResearch.tech.costsByResearchTypeID.Count > 1)
      {
        if (!MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id))
        {
          this.notifier.Remove(this.NoResearcherRole);
          this.notifier.Add(this.NoResearcherRole);
        }
      }
      else
        this.notifier.Remove(this.NoResearcherRole);
      if (this.activeResearch.tech.costsByResearchTypeID.Count > 2)
      {
        if (MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id))
          return;
        this.notifier.Remove(this.NoResearcherRole);
        this.notifier.Add(this.NoResearcherRole);
      }
      else
        this.notifier.Remove(this.NoResearcherRole);
    }
    else
      this.notifier.Remove(this.NoResearcherRole);
  }

  public void AddResearchPoints(string researchTypeID, float points)
  {
    if (!this.UseGlobalPointInventory && this.activeResearch == null)
    {
      Debug.LogWarning((object) "No active research to add research points to. Global research inventory is disabled.");
    }
    else
    {
      (this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory).AddResearchPoints(researchTypeID, points);
      this.CheckBuyResearch();
      this.Trigger(-125623018, (object) null);
    }
  }

  private void CheckBuyResearch()
  {
    if (this.activeResearch == null)
      return;
    ResearchPointInventory pointInventory = this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory;
    if (!this.activeResearch.tech.CanAfford(pointInventory))
      return;
    foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
      pointInventory.RemoveResearchPoints(keyValuePair.Key, keyValuePair.Value);
    this.activeResearch.Purchased();
    Game.Instance.Trigger(-107300940, (object) this.activeResearch.tech);
    this.GetNextTech();
  }

  protected override void OnCleanUp()
  {
    Components.ResearchCenters.OnAdd -= new System.Action<ResearchCenter>(this.CheckResearchBuildings);
    Components.ResearchCenters.OnRemove -= new System.Action<ResearchCenter>(this.CheckResearchBuildings);
    base.OnCleanUp();
  }

  public void CompleteQueue()
  {
    while (this.queuedTech.Count > 0)
    {
      foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
        this.AddResearchPoints(keyValuePair.Key, keyValuePair.Value);
    }
  }

  public List<TechInstance> GetResearchQueue() => new List<TechInstance>((IEnumerable<TechInstance>) this.queuedTech);

  [System.Runtime.Serialization.OnSerializing]
  internal void OnSerializing()
  {
    this.saveData = new Research.SaveData();
    this.saveData.activeResearchId = this.activeResearch == null ? "" : this.activeResearch.tech.Id;
    this.saveData.targetResearchId = this.queuedTech == null || this.queuedTech.Count <= 0 ? "" : this.queuedTech[this.queuedTech.Count - 1].tech.Id;
    this.saveData.techs = new TechInstance.SaveData[this.techs.Count];
    for (int index = 0; index < this.techs.Count; ++index)
      this.saveData.techs[index] = this.techs[index].Save();
  }

  [System.Runtime.Serialization.OnDeserialized]
  internal void OnDeserialized()
  {
    if (this.saveData.techs != null)
    {
      foreach (TechInstance.SaveData tech1 in this.saveData.techs)
      {
        Tech tech2 = Db.Get().Techs.TryGet(tech1.techId);
        if (tech2 != null)
          this.GetOrAdd(tech2).Load(tech1);
      }
    }
    foreach (TechInstance tech in this.techs)
    {
      if (this.saveData.targetResearchId == tech.tech.Id)
      {
        this.SetActiveResearch(tech.tech);
        break;
      }
    }
  }

  private void OnRolesUpdated(object data)
  {
    if (this.activeResearch != null && this.activeResearch.tech.costsByResearchTypeID.Count > 1)
    {
      if (!MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id))
        this.notifier.Add(this.NoResearcherRole);
      else
        this.notifier.Remove(this.NoResearcherRole);
    }
    else
      this.notifier.Remove(this.NoResearcherRole);
  }

  public string GetMissingResearchBuildingName()
  {
    foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
    {
      bool flag = true;
      if ((double) keyValuePair.Value > 0.0)
      {
        flag = false;
        foreach (ResearchCenter researchCenter in Components.ResearchCenters.Items)
        {
          if (researchCenter.research_point_type_id == keyValuePair.Key)
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        foreach (ResearchCenter researchCenterPrefab in this.researchCenterPrefabs)
        {
          if (researchCenterPrefab.research_point_type_id == keyValuePair.Key)
            return researchCenterPrefab.GetProperName();
        }
        return (string) null;
      }
    }
    return (string) null;
  }

  private void CheckResearchBuildings(object data)
  {
    if (this.activeResearch == null)
      this.notifier.Remove(this.MissingResearchStation);
    else if (string.IsNullOrEmpty(this.GetMissingResearchBuildingName()))
      this.notifier.Remove(this.MissingResearchStation);
    else
      this.notifier.Add(this.MissingResearchStation);
  }

  private struct SaveData
  {
    public string activeResearchId;
    public string targetResearchId;
    public TechInstance.SaveData[] techs;
  }
}
