// Decompiled with JetBrains decompiler
// Type: Telescope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms
{
  public int clearScanCellRadius = 15;
  private OxygenBreather.IGasProvider workerGasProvider;
  private Operational operational;
  private float percentClear;
  private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);
  private static StatusItem reducedVisibilityStatusItem;
  private static StatusItem noVisibilityStatusItem;
  private Storage storage;
  public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition()
  {
    id = nameof (ContainsOxygen),
    sortOrder = 1,
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen) != (UnityEngine.Object) null)
  };
  private Chore chore;
  private Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SpacecraftManager.instance.Subscribe(532901469, new System.Action<object>(this.UpdateWorkingState));
    Components.Telescopes.Add(this);
    if (Telescope.reducedVisibilityStatusItem == null)
    {
      Telescope.reducedVisibilityStatusItem = new StatusItem("SPACE_VISIBILITY_REDUCED", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      Telescope.reducedVisibilityStatusItem.resolveStringCallback = new Func<string, object, string>(Telescope.GetStatusItemString);
      Telescope.noVisibilityStatusItem = new StatusItem("SPACE_VISIBILITY_NONE", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
      Telescope.noVisibilityStatusItem.resolveStringCallback = new Func<string, object, string>(Telescope.GetStatusItemString);
    }
    this.OnWorkableEventCB = this.OnWorkableEventCB + new System.Action<Workable.WorkableEvent>(this.OnWorkableEvent);
    this.operational = this.GetComponent<Operational>();
    this.storage = this.GetComponent<Storage>();
    this.UpdateWorkingState((object) null);
  }

  protected override void OnCleanUp()
  {
    Components.Telescopes.Remove(this);
    SpacecraftManager.instance.Unsubscribe(532901469, new System.Action<object>(this.UpdateWorkingState));
    base.OnCleanUp();
  }

  public void Sim200ms(float dt)
  {
    Extents extents = this.GetComponent<Building>().GetExtents();
    int x1 = Mathf.Max(0, extents.x - this.clearScanCellRadius);
    int x2 = Mathf.Min(new int[1]
    {
      extents.x + this.clearScanCellRadius
    });
    int y = extents.y + extents.height - 3;
    int num1 = x2 - x1 + 1;
    int cell1 = Grid.XYToCell(x1, y);
    int cell2 = Grid.XYToCell(x2, y);
    int num2 = 0;
    for (int i = cell1; i <= cell2; ++i)
    {
      if (Grid.ExposedToSunlight[i] >= (byte) 253)
        ++num2;
    }
    Operational component1 = this.GetComponent<Operational>();
    component1.SetFlag(Telescope.visibleSkyFlag, num2 > 0);
    bool on = num2 < num1;
    KSelectable component2 = this.GetComponent<KSelectable>();
    if (num2 > 0)
    {
      component2.ToggleStatusItem(Telescope.noVisibilityStatusItem, false);
      component2.ToggleStatusItem(Telescope.reducedVisibilityStatusItem, on, (object) this);
    }
    else
    {
      component2.ToggleStatusItem(Telescope.noVisibilityStatusItem, true, (object) this);
      component2.ToggleStatusItem(Telescope.reducedVisibilityStatusItem, false);
    }
    this.percentClear = (float) num2 / (float) num1;
    if (component1.IsActive || !component1.IsOperational || this.chore != null)
      return;
    this.chore = this.CreateChore();
    this.SetWorkTime(float.PositiveInfinity);
  }

  private static string GetStatusItemString(string src_str, object data)
  {
    Telescope telescope = (Telescope) data;
    return src_str.Replace("{VISIBILITY}", GameUtil.GetFormattedPercent(telescope.percentClear * 100f)).Replace("{RADIUS}", telescope.clearScanCellRadius.ToString());
  }

  private void OnWorkableEvent(Workable.WorkableEvent ev)
  {
    Worker worker = this.worker;
    if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    OxygenBreather component1 = worker.GetComponent<OxygenBreather>();
    KPrefabID component2 = worker.GetComponent<KPrefabID>();
    switch (ev)
    {
      case Workable.WorkableEvent.WorkStarted:
        this.ShowProgressBar(true);
        this.progressBar.SetUpdateFunc((Func<float>) (() => SpacecraftManager.instance.HasAnalysisTarget() ? SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE : 0.0f));
        this.workerGasProvider = component1.GetGasProvider();
        component1.SetGasProvider((OxygenBreather.IGasProvider) this);
        component1.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
        component2.AddTag(GameTags.Shaded);
        break;
      case Workable.WorkableEvent.WorkStopped:
        component1.SetGasProvider(this.workerGasProvider);
        component1.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
        this.ShowProgressBar(false);
        component2.RemoveTag(GameTags.Shaded);
        break;
    }
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (SpacecraftManager.instance.HasAnalysisTarget())
    {
      int analysisDestinationId = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
      float num1 = 1f / (float) SpacecraftManager.instance.GetDestination(analysisDestinationId).OneBasedDistance;
      float num2 = (float) ((double) TUNING.ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED / (double) TUNING.ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY / 600.0);
      float points = dt * num1 * num2;
      SpacecraftManager.instance.EarnDestinationAnalysisPoints(analysisDestinationId, points);
    }
    return base.OnWorkTick(worker, dt);
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Oxygen);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(elementByHash.tag.ProperName(), string.Format((string) STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, (object) elementByHash.tag.ProperName()), Descriptor.DescriptorType.Requirement);
    descriptors.Add(descriptor);
    return descriptors;
  }

  protected Chore CreateChore()
  {
    WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this);
    workChore.AddPrecondition(Telescope.ContainsOxygen);
    return (Chore) workChore;
  }

  protected void UpdateWorkingState(object data)
  {
    bool flag1 = false;
    if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
      flag1 = true;
    KSelectable component = this.GetComponent<KSelectable>();
    bool flag2 = !flag1 && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
    StatusItem analysisSelected = Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected;
    int num = flag2 ? 1 : 0;
    component.ToggleStatusItem(analysisSelected, num != 0);
    this.operational.SetFlag(this.flag, flag1);
    if (flag1 || !(bool) (UnityEngine.Object) this.worker)
      return;
    this.StopWork(this.worker, true);
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ShouldEmitCO2() => false;

  public bool ShouldStoreCO2() => false;

  public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
  {
    if (this.storage.items.Count <= 0)
      return false;
    GameObject gameObject = this.storage.items[0];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    int num = (double) component.Mass >= (double) amount ? 1 : 0;
    component.Mass = Mathf.Max(0.0f, component.Mass - amount);
    return num != 0;
  }
}
