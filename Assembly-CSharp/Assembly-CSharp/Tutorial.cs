// Decompiled with JetBrains decompiler
// Type: Tutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Tutorial")]
public class Tutorial : KMonoBehaviour, IRender1000ms
{
  [MyCmpAdd]
  private Notifier notifier;
  [Serialize]
  private SerializedList<Tutorial.TutorialMessages> tutorialMessagesRemaining = new SerializedList<Tutorial.TutorialMessages>();
  private const string HIDDEN_TUTORIAL_PREF_KEY_PREFIX = "HideTutorial_";
  public const string HIDDEN_TUTORIAL_PREF_BUTTON_KEY = "HideTutorial_CheckState";
  private Dictionary<Tutorial.TutorialMessages, bool> hiddenTutorialMessages = new Dictionary<Tutorial.TutorialMessages, bool>();
  private int debugMessageCount;
  private bool queuedPrioritiesMessage;
  private const float LOW_RATION_AMOUNT = 1f;
  private List<List<Tutorial.Item>> itemTree = new List<List<Tutorial.Item>>();
  private List<Tutorial.Item> warningItems = new List<Tutorial.Item>();
  private Vector3 notifierPosition;
  public List<GameObject> oxygenGenerators = new List<GameObject>();
  private int focusedOxygenGenerator;

  public static void ResetHiddenTutorialMessages()
  {
    if ((UnityEngine.Object) Tutorial.Instance != (UnityEngine.Object) null)
      Tutorial.Instance.tutorialMessagesRemaining.Clear();
    foreach (Tutorial.TutorialMessages key in Enum.GetValues(typeof (Tutorial.TutorialMessages)))
    {
      KPlayerPrefs.SetInt("HideTutorial_" + key.ToString(), 0);
      if ((UnityEngine.Object) Tutorial.Instance != (UnityEngine.Object) null)
      {
        Tutorial.Instance.tutorialMessagesRemaining.Add(key);
        Tutorial.Instance.hiddenTutorialMessages[key] = false;
      }
    }
    KPlayerPrefs.SetInt("HideTutorial_CheckState", 0);
  }

  private void LoadHiddenTutorialMessages()
  {
    foreach (Tutorial.TutorialMessages key in Enum.GetValues(typeof (Tutorial.TutorialMessages)))
    {
      bool flag = (uint) KPlayerPrefs.GetInt("HideTutorial_" + key.ToString(), 0) > 0U;
      this.hiddenTutorialMessages[key] = flag;
    }
  }

  public void HideTutorialMessage(Tutorial.TutorialMessages message)
  {
    this.hiddenTutorialMessages[message] = true;
    KPlayerPrefs.SetInt("HideTutorial_" + message.ToString(), 1);
  }

  public static Tutorial Instance { get; private set; }

  public static void DestroyInstance() => Tutorial.Instance = (Tutorial) null;

  private void UpdateNotifierPosition()
  {
    if (this.notifierPosition == Vector3.zero)
    {
      GameObject telepad = GameUtil.GetTelepad();
      if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
        this.notifierPosition = telepad.transform.GetPosition();
    }
    this.notifier.transform.SetPosition(this.notifierPosition);
  }

  protected override void OnPrefabInit()
  {
    Tutorial.Instance = this;
    this.LoadHiddenTutorialMessages();
  }

  protected override void OnSpawn()
  {
    if (this.tutorialMessagesRemaining.Count == 0)
    {
      for (int index = 0; index <= 19; ++index)
        this.tutorialMessagesRemaining.Add((Tutorial.TutorialMessages) index);
    }
    this.itemTree.Add(new List<Tutorial.Item>()
    {
      new Tutorial.Item()
      {
        notification = new Notification((string) MISC.NOTIFICATIONS.NEEDTOILET.NAME, NotificationType.Tutorial, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text), delay: 5f, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Plumbing")))),
        requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.ToiletExists)
      }
    });
    this.itemTree.Add(new List<Tutorial.Item>()
    {
      new Tutorial.Item()
      {
        notification = new Notification((string) MISC.NOTIFICATIONS.NEEDFOOD.NAME, NotificationType.Tutorial, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => MISC.NOTIFICATIONS.NEEDFOOD.TOOLTIP.text), delay: 20f, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Food")))),
        requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodSourceExists)
      },
      new Tutorial.Item()
      {
        notification = new Notification((string) MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, NotificationType.Tutorial, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP.text))
      }
    });
    this.itemTree.Add(new List<Tutorial.Item>()
    {
      new Tutorial.Item()
      {
        notification = new Notification((string) MISC.NOTIFICATIONS.HYGENE_NEEDED.NAME, NotificationType.Tutorial, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => (string) MISC.NOTIFICATIONS.HYGENE_NEEDED.TOOLTIP), delay: 20f, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Medicine")))),
        requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.HygeneExists)
      }
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification((string) MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.NAME, NotificationType.Tutorial, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => (string) MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.TOOLTIP), expires: false, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Oxygen")))),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.OxygenGeneratorBuilt),
      minTimeToNotify = 80f,
      lastNotifyTime = 0.0f
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification((string) MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.NAME, NotificationType.Tutorial, HashedString.Invalid, new Func<List<Notification>, object, string>(this.OnOxygenTooltip), expires: false, custom_click_callback: ((Notification.ClickCallback) (d => this.ZoomToNextOxygenGenerator()))),
      hideCondition = new Tutorial.HideConditionDelegate(this.OxygenGeneratorNotBuilt),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.SufficientOxygenLastCycleAndThisCycle),
      minTimeToNotify = 80f,
      lastNotifyTime = 0.0f
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification((string) MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.NAME, NotificationType.Tutorial, HashedString.Invalid, new Func<List<Notification>, object, string>(this.UnrefrigeratedFoodTooltip), expires: false, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Food")))),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodIsRefrigerated),
      minTimeToNotify = 6f,
      lastNotifyTime = 0.0f
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification((string) MISC.NOTIFICATIONS.FOODLOW.NAME, NotificationType.Bad, HashedString.Invalid, new Func<List<Notification>, object, string>(this.OnLowFoodTooltip), expires: false, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Food")))),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.EnoughFood),
      minTimeToNotify = 10f,
      lastNotifyTime = 0.0f
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification((string) MISC.NOTIFICATIONS.NO_MEDICAL_COTS.NAME, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, o) => (string) MISC.NOTIFICATIONS.NO_MEDICAL_COTS.TOOLTIP), expires: false, custom_click_callback: ((Notification.ClickCallback) (d => PlanScreen.Instance.OpenCategoryByName("Medicine")))),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.CanTreatSickDuplicant),
      minTimeToNotify = 10f,
      lastNotifyTime = 0.0f
    });
    this.warningItems.Add(new Tutorial.Item()
    {
      notification = new Notification(string.Format((string) UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_TITLE, (object[]) Array.Empty<object>()), NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => string.Format((string) UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_MESSAGE, (object) GameUtil.GetFormattedPercent(40f))), custom_click_callback: ((Notification.ClickCallback) (d => ManagementMenu.Instance.OpenReports(GameClock.Instance.GetCycle())))),
      requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.LongTravelTimes),
      minTimeToNotify = 1f,
      lastNotifyTime = 0.0f
    });
  }

  public Message TutorialMessage(Tutorial.TutorialMessages tm, bool queueMessage = true)
  {
    bool flag = false;
    Message message = (Message) null;
    switch (tm)
    {
      case Tutorial.TutorialMessages.TM_Basics:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Basics, (string) MISC.NOTIFICATIONS.BASICCONTROLS.NAME, (string) MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY, (string) MISC.NOTIFICATIONS.BASICCONTROLS.TOOLTIP);
        break;
      case Tutorial.TutorialMessages.TM_Welcome:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, (string) MISC.NOTIFICATIONS.WELCOMEMESSAGE.NAME, (string) MISC.NOTIFICATIONS.WELCOMEMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.WELCOMEMESSAGE.TOOLTIP);
        break;
      case Tutorial.TutorialMessages.TM_StressManagement:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_StressManagement, (string) MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.NAME, (string) MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.TOOLTIP, icon: "hud_stress");
        break;
      case Tutorial.TutorialMessages.TM_Scheduling:
        flag = true;
        break;
      case Tutorial.TutorialMessages.TM_Mopping:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Mopping, (string) MISC.NOTIFICATIONS.MOPPINGMESSAGE.NAME, (string) MISC.NOTIFICATIONS.MOPPINGMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.MOPPINGMESSAGE.TOOLTIP, icon: "icon_action_mop");
        break;
      case Tutorial.TutorialMessages.TM_Locomotion:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, (string) MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.NAME, (string) MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP, "tutorials\\Locomotion", "Tute_Locomotion", (string) VIDEOS.LOCOMOTION, "action_navigable_regions");
        break;
      case Tutorial.TutorialMessages.TM_Priorities:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Priorities, (string) MISC.NOTIFICATIONS.PRIORITIESMESSAGE.NAME, (string) MISC.NOTIFICATIONS.PRIORITIESMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.PRIORITIESMESSAGE.TOOLTIP, icon: "icon_action_prioritize");
        break;
      case Tutorial.TutorialMessages.TM_FetchingWater:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, (string) MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.NAME, (string) MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.TOOLTIP, icon: "element_liquid");
        break;
      case Tutorial.TutorialMessages.TM_ThermalComfort:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, (string) MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, (string) MISC.NOTIFICATIONS.THERMALCOMFORT.MESSAGEBODY, (string) MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP, icon: "temperature");
        break;
      case Tutorial.TutorialMessages.TM_OverheatingBuildings:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, (string) MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.NAME, (string) MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.MESSAGEBODY, (string) MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.TOOLTIP, icon: "temperature");
        break;
      case Tutorial.TutorialMessages.TM_LotsOfGerms:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, (string) MISC.NOTIFICATIONS.LOTS_OF_GERMS.NAME, (string) MISC.NOTIFICATIONS.LOTS_OF_GERMS.MESSAGEBODY, (string) MISC.NOTIFICATIONS.LOTS_OF_GERMS.TOOLTIP, icon: "overlay_disease");
        break;
      case Tutorial.TutorialMessages.TM_DiseaseCooking:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, (string) MISC.NOTIFICATIONS.DISEASE_COOKING.NAME, (string) MISC.NOTIFICATIONS.DISEASE_COOKING.MESSAGEBODY, (string) MISC.NOTIFICATIONS.DISEASE_COOKING.TOOLTIP, icon: "icon_category_food");
        break;
      case Tutorial.TutorialMessages.TM_Suits:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Suits, (string) MISC.NOTIFICATIONS.SUITS.NAME, (string) MISC.NOTIFICATIONS.SUITS.MESSAGEBODY, (string) MISC.NOTIFICATIONS.SUITS.TOOLTIP, icon: "overlay_suit");
        break;
      case Tutorial.TutorialMessages.TM_Morale:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Morale, (string) MISC.NOTIFICATIONS.MORALE.NAME, (string) MISC.NOTIFICATIONS.MORALE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.MORALE.TOOLTIP, "tutorials\\Morale", "Tute_Morale", (string) VIDEOS.MORALE, "icon_category_morale");
        break;
      case Tutorial.TutorialMessages.TM_Schedule:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, (string) MISC.NOTIFICATIONS.SCHEDULEMESSAGE.NAME, (string) MISC.NOTIFICATIONS.SCHEDULEMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.SCHEDULEMESSAGE.TOOLTIP, icon: "OverviewUI_schedule2_icon");
        break;
      case Tutorial.TutorialMessages.TM_Digging:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Digging, (string) MISC.NOTIFICATIONS.DIGGING.NAME, (string) MISC.NOTIFICATIONS.DIGGING.MESSAGEBODY, (string) MISC.NOTIFICATIONS.DIGGING.TOOLTIP, "tutorials\\Digging", "Tute_Digging", (string) VIDEOS.DIGGING, "icon_action_dig");
        break;
      case Tutorial.TutorialMessages.TM_Power:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Power, (string) MISC.NOTIFICATIONS.POWER.NAME, (string) MISC.NOTIFICATIONS.POWER.MESSAGEBODY, (string) MISC.NOTIFICATIONS.POWER.TOOLTIP, "tutorials\\Power", "Tute_Power", (string) VIDEOS.POWER, "overlay_power");
        break;
      case Tutorial.TutorialMessages.TM_Insulation:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, (string) MISC.NOTIFICATIONS.INSULATION.NAME, (string) MISC.NOTIFICATIONS.INSULATION.MESSAGEBODY, (string) MISC.NOTIFICATIONS.INSULATION.TOOLTIP, "tutorials\\Insulation", "Tute_Insulation", (string) VIDEOS.INSULATION, "icon_thermal_conductivity");
        break;
      case Tutorial.TutorialMessages.TM_Plumbing:
        message = (Message) new global::TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, (string) MISC.NOTIFICATIONS.PLUMBING.NAME, (string) MISC.NOTIFICATIONS.PLUMBING.MESSAGEBODY, (string) MISC.NOTIFICATIONS.PLUMBING.TOOLTIP, "tutorials\\Piping", "Tute_Plumbing", (string) VIDEOS.PLUMBING, "icon_category_plumbing");
        break;
    }
    Debug.Assert(message != null | flag, (object) string.Format("No Tutorial message: {0}", (object) tm.ToString()));
    if (queueMessage)
    {
      Debug.Assert(!flag, (object) ("Attempted to queue deprecated Tutorial Message " + tm.ToString()));
      if (!this.tutorialMessagesRemaining.Contains(tm))
        return (Message) null;
      if (this.hiddenTutorialMessages.ContainsKey(tm) && this.hiddenTutorialMessages[tm])
        return (Message) null;
      this.tutorialMessagesRemaining.Remove(tm);
      Messenger.Instance.QueueMessage(message);
    }
    return message;
  }

  private string OnOxygenTooltip(List<Notification> notifications, object data)
  {
    ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
    return ((string) MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.TOOLTIP).Replace("{EmittingRate}", GameUtil.GetFormattedMass(entry.Positive)).Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(Mathf.Abs(entry.Negative)));
  }

  private string UnrefrigeratedFoodTooltip(List<Notification> notifications, object data)
  {
    string str = (string) MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.TOOLTIP;
    List<string> foods = new List<string>();
    this.GetUnrefrigeratedFood(foods);
    for (int index = 0; index < foods.Count; ++index)
      str = str + "\n" + foods[index];
    return str;
  }

  private string OnLowFoodTooltip(List<Notification> notifications, object data)
  {
    float calories = RationTracker.Get().CountRations((Dictionary<string, float>) null);
    float f = (float) Components.LiveMinionIdentities.Count * -1000000f;
    return string.Format((string) MISC.NOTIFICATIONS.FOODLOW.TOOLTIP, (object) GameUtil.GetFormattedCalories(calories), (object) GameUtil.GetFormattedCalories(Mathf.Abs(f)));
  }

  public void DebugNotification()
  {
    NotificationType type;
    string str;
    if (this.debugMessageCount % 3 == 0)
    {
      type = NotificationType.Tutorial;
      str = "Warning message e.g. \"not enough oxygen\" uses Warning Color";
    }
    else if (this.debugMessageCount % 3 == 1)
    {
      type = NotificationType.BadMinor;
      str = "Normal message e.g. Idle. Uses Normal Color BG";
    }
    else
    {
      type = NotificationType.Bad;
      str = "Urgent important message. Uses Bad Color BG";
    }
    this.notifier.Add(new Notification(string.Format("{0} ({1})", (object) str, (object) this.debugMessageCount++.ToString()), type, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text)));
  }

  public void DebugNotificationMessage()
  {
    Message message = (Message) new GenericMessage("This is a message notification. " + this.debugMessageCount++.ToString(), (string) MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, (string) MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP);
    Messenger.Instance.QueueMessage(message);
  }

  public void Render1000ms(float dt)
  {
    if (App.isLoading || Components.LiveMinionIdentities.Count == 0)
      return;
    if (this.itemTree.Count > 0)
    {
      List<Tutorial.Item> objList = this.itemTree[0];
      for (int index = objList.Count - 1; index >= 0; --index)
      {
        Tutorial.Item obj = objList[index];
        if (obj != null)
        {
          if (obj.requirementSatisfied == null || obj.requirementSatisfied())
          {
            obj.notification.Clear();
            objList.RemoveAt(index);
          }
          else if (obj.hideCondition != null && obj.hideCondition())
          {
            obj.notification.Clear();
            objList.RemoveAt(index);
          }
          else
          {
            this.UpdateNotifierPosition();
            this.notifier.Add(obj.notification);
          }
        }
      }
      if (objList.Count == 0)
        this.itemTree.RemoveAt(0);
    }
    foreach (Tutorial.Item warningItem in this.warningItems)
    {
      if (warningItem.requirementSatisfied())
      {
        warningItem.notification.Clear();
        warningItem.lastNotifyTime = Time.time;
      }
      else if (warningItem.hideCondition != null && warningItem.hideCondition())
      {
        warningItem.notification.Clear();
        warningItem.lastNotifyTime = Time.time;
      }
      else if ((double) warningItem.lastNotifyTime == 0.0 || (double) Time.time - (double) warningItem.lastNotifyTime > (double) warningItem.minTimeToNotify)
      {
        this.notifier.Add(warningItem.notification);
        warningItem.lastNotifyTime = Time.time;
      }
    }
    if (GameClock.Instance.GetCycle() <= 0 || this.tutorialMessagesRemaining.Contains(Tutorial.TutorialMessages.TM_Priorities) || this.queuedPrioritiesMessage)
      return;
    this.queuedPrioritiesMessage = true;
    GameScheduler.Instance.Schedule("PrioritiesTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Priorities)), (object) null, (SchedulerGroup) null);
  }

  private bool OxygenGeneratorBuilt() => this.oxygenGenerators.Count > 0;

  private bool OxygenGeneratorNotBuilt() => this.oxygenGenerators.Count == 0;

  private bool SufficientOxygenLastCycleAndThisCycle()
  {
    if (ReportManager.Instance.YesterdaysReport == null)
      return true;
    ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
    if ((double) ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.OxygenCreated).Net > 9.99999974737875E-05 || (double) entry.Net > 9.99999974737875E-05)
      return true;
    return GameClock.Instance.GetCycle() < 1 && !GameClock.Instance.IsNighttime();
  }

  private bool FoodIsRefrigerated() => this.GetUnrefrigeratedFood((List<string>) null) <= 0;

  private int GetUnrefrigeratedFood(List<string> foods)
  {
    int num = 0;
    if ((UnityEngine.Object) WorldInventory.Instance != (UnityEngine.Object) null)
    {
      ICollection<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
      if (pickupables == null)
        return 0;
      foreach (Pickupable cmp in (IEnumerable<Pickupable>) pickupables)
      {
        if ((UnityEngine.Object) cmp.storage != (UnityEngine.Object) null && ((UnityEngine.Object) cmp.storage.GetComponent<RationBox>() != (UnityEngine.Object) null || (UnityEngine.Object) cmp.storage.GetComponent<Refrigerator>() != (UnityEngine.Object) null) && (!Rottable.IsRefrigerated(cmp.gameObject) && Rottable.AtmosphereQuality(cmp.gameObject) != Rottable.RotAtmosphereQuality.Sterilizing))
        {
          Rottable.Instance smi = cmp.GetSMI<Rottable.Instance>();
          if (smi != null && (double) smi.RotConstitutionPercentage < 0.800000011920929)
          {
            ++num;
            foods?.Add(cmp.GetProperName());
          }
        }
      }
    }
    return num;
  }

  private bool EnergySourceExists() => Game.Instance.circuitManager.HasGenerators();

  private bool BedExists() => Components.Sleepables.Count > 0;

  private bool EnoughFood() => (double) RationTracker.Get().CountRations((Dictionary<string, float>) null) / (double) ((float) Components.LiveMinionIdentities.Count * 1000000f) > 1.0;

  private bool CanTreatSickDuplicant()
  {
    bool flag1 = Components.Clinics.Count >= 1;
    bool flag2 = false;
    for (int idx = 0; idx < Components.LiveMinionIdentities.Count; ++idx)
    {
      foreach (SicknessInstance sickness in (Modifications<Sickness, SicknessInstance>) Components.LiveMinionIdentities[idx].GetSicknesses())
      {
        if (sickness.Sickness.severity >= Sickness.Severity.Major)
        {
          flag2 = true;
          break;
        }
      }
      if (flag2)
        break;
    }
    return !flag2 || flag1;
  }

  private bool LongTravelTimes()
  {
    if (ReportManager.Instance.reports.Count < 3)
      return true;
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = ReportManager.Instance.reports.Count - 1; index >= ReportManager.Instance.reports.Count - 3; --index)
    {
      ReportManager.ReportEntry entry = ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.TravelTime);
      num1 += entry.Net;
      num2 += 600f * (float) entry.contextEntries.Count;
    }
    return (double) num1 / (double) num2 <= 0.400000005960464;
  }

  private bool FoodSourceExists()
  {
    foreach (object obj in Components.ComplexFabricators.Items)
    {
      if (obj.GetType() == typeof (MicrobeMusher))
        return true;
    }
    return Components.PlantablePlots.Count > 0;
  }

  private bool HygeneExists() => Components.HandSanitizers.Count > 0;

  private bool ToiletExists() => Components.Toilets.Count > 0;

  private void ZoomToNextOxygenGenerator()
  {
    if (this.oxygenGenerators.Count == 0)
      return;
    this.focusedOxygenGenerator %= this.oxygenGenerators.Count;
    GameObject oxygenGenerator = this.oxygenGenerators[this.focusedOxygenGenerator];
    if ((UnityEngine.Object) oxygenGenerator != (UnityEngine.Object) null)
      CameraController.Instance.SetTargetPos(oxygenGenerator.transform.position, 8f, true);
    else
      DebugUtil.DevLogErrorFormat("ZoomToNextOxygenGenerator generator was null: {0}", (object) oxygenGenerator);
    ++this.focusedOxygenGenerator;
  }

  public enum TutorialMessages
  {
    TM_Basics,
    TM_Welcome,
    TM_StressManagement,
    TM_Scheduling,
    TM_Mopping,
    TM_Locomotion,
    TM_Priorities,
    TM_FetchingWater,
    TM_ThermalComfort,
    TM_OverheatingBuildings,
    TM_LotsOfGerms,
    TM_DiseaseCooking,
    TM_Suits,
    TM_Morale,
    TM_Schedule,
    TM_Digging,
    TM_Power,
    TM_Insulation,
    TM_Plumbing,
    TM_COUNT,
  }

  private delegate bool HideConditionDelegate();

  private delegate bool RequirementSatisfiedDelegate();

  private class Item
  {
    public Notification notification;
    public Tutorial.HideConditionDelegate hideCondition;
    public Tutorial.RequirementSatisfiedDelegate requirementSatisfied;
    public float minTimeToNotify;
    public float lastNotifyTime;
  }
}
