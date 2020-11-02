// Decompiled with JetBrains decompiler
// Type: Spacecraft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
  [Serialize]
  public int id = -1;
  [Serialize]
  public string rocketName = (string) UI.STARMAP.DEFAULT_NAME;
  [Serialize]
  public int moduleCount;
  [Serialize]
  public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();
  [Serialize]
  public Spacecraft.MissionState state;
  [Serialize]
  private float missionElapsed;
  [Serialize]
  private float missionDuration;

  public Spacecraft(LaunchConditionManager launchConditions) => this.launchConditions = launchConditions;

  public LaunchConditionManager launchConditions
  {
    get => this.refLaunchConditions.Get();
    set => this.refLaunchConditions.Set(value);
  }

  public void SetRocketName(string newName)
  {
    this.rocketName = newName;
    this.UpdateNameOnRocketModules();
  }

  public string GetRocketName() => this.rocketName;

  public void UpdateNameOnRocketModules()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
    {
      RocketModule component = gameObject.GetComponent<RocketModule>();
      if ((Object) component != (Object) null)
        component.SetParentRocketName(this.rocketName);
    }
  }

  public bool HasInvalidID() => this.id == -1;

  public void SetID(int id) => this.id = id;

  public void SetState(Spacecraft.MissionState state) => this.state = state;

  public void BeginMission(SpaceDestination destination)
  {
    this.missionElapsed = 0.0f;
    this.missionDuration = (float) destination.OneBasedDistance * TUNING.ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
    this.SetState(Spacecraft.MissionState.Launching);
  }

  private float GetPilotNavigationEfficiency()
  {
    List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
    if (storedMinionInfo.Count < 1)
      return 1f;
    StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
    string id = Db.Get().Attributes.SpaceNavigation.Id;
    float num = 1f;
    foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
    {
      foreach (SkillPerk perk in Db.Get().Skills.Get(keyValuePair.Key).perks)
      {
        if (perk is SkillAttributePerk skillAttributePerk && skillAttributePerk.modifier.AttributeId == id)
          num += skillAttributePerk.modifier.Value;
      }
    }
    return num;
  }

  public void ForceComplete() => this.missionElapsed = this.missionDuration;

  public void ProgressMission(float deltaTime)
  {
    if (this.state != Spacecraft.MissionState.Underway)
      return;
    this.missionElapsed += deltaTime;
    if ((double) this.missionElapsed <= (double) this.missionDuration)
      return;
    this.CompleteMission();
  }

  public float GetTimeLeft() => this.missionDuration - this.missionElapsed;

  public float GetDuration() => this.missionDuration;

  public void CompleteMission()
  {
    SpacecraftManager.instance.PushReadyToLandNotification(this);
    this.SetState(Spacecraft.MissionState.WaitingToLand);
    this.Land();
  }

  private void Land()
  {
    this.launchConditions.Trigger(1366341636, (object) SpacecraftManager.instance.GetSpacecraftDestination(this.id));
    foreach (GameObject go in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
    {
      if ((Object) go != (Object) this.launchConditions.gameObject)
        go.Trigger(1366341636, (object) SpacecraftManager.instance.GetSpacecraftDestination(this.id));
    }
  }

  public void TemporallyTear()
  {
    SpacecraftManager.instance.hasVisitedWormHole = true;
    LaunchConditionManager launchConditions = this.launchConditions;
    for (int index1 = launchConditions.rocketModules.Count - 1; index1 >= 0; --index1)
    {
      Storage component1 = launchConditions.rocketModules[index1].GetComponent<Storage>();
      if ((Object) component1 != (Object) null)
        component1.ConsumeAllIgnoringDisease();
      MinionStorage component2 = launchConditions.rocketModules[index1].GetComponent<MinionStorage>();
      if ((Object) component2 != (Object) null)
      {
        List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
        for (int index2 = storedMinionInfo.Count - 1; index2 >= 0; --index2)
          component2.DeleteStoredMinion(storedMinionInfo[index2].id);
      }
      Util.KDestroyGameObject(launchConditions.rocketModules[index1].gameObject);
    }
  }

  public void GenerateName() => this.SetRocketName(GameUtil.GenerateRandomRocketName());

  public enum MissionState
  {
    Grounded,
    Launching,
    Underway,
    WaitingToLand,
    Landing,
    Destroyed,
  }
}
