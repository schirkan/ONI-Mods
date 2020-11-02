// Decompiled with JetBrains decompiler
// Type: Edible
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor
{
  public string FoodID;
  private EdiblesManager.FoodInfo foodInfo;
  public float unitsConsumed = float.NaN;
  public float caloriesConsumed = float.NaN;
  private float totalFeedingTime = float.NaN;
  private float totalUnits = float.NaN;
  private float totalConsumableCalories = float.NaN;
  private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, (string) DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, uiOnly: true);
  private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", (float) ((1.0 + (double) DUPLICANTSTATS.LIGHT.LIGHT_WORK_EFFICIENCY_BONUS) / 1.99999994947575E-05), (string) DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, uiOnly: true);
  private AttributeModifier currentModifier;
  private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>((System.Action<Edible, object>) ((component, data) => component.OnCraft(data)));
  private static readonly HashedString[] normalWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[2]
  {
    (HashedString) "hat_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] saltWorkAnims = new HashedString[2]
  {
    (HashedString) "salt_pre",
    (HashedString) "salt_loop"
  };
  private static readonly HashedString[] saltHatWorkAnims = new HashedString[2]
  {
    (HashedString) "salt_hat_pre",
    (HashedString) "salt_hat_loop"
  };
  private static readonly HashedString[] normalWorkPstAnim = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  private static readonly HashedString[] hatWorkPstAnim = new HashedString[1]
  {
    (HashedString) "hat_pst"
  };
  private static readonly HashedString[] saltWorkPstAnim = new HashedString[1]
  {
    (HashedString) "salt_pst"
  };
  private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[1]
  {
    (HashedString) "salt_hat_pst"
  };
  private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>()
  {
    {
      -1,
      "EdibleMinus3"
    },
    {
      0,
      "EdibleMinus2"
    },
    {
      1,
      "EdibleMinus1"
    },
    {
      2,
      "Edible0"
    },
    {
      3,
      "Edible1"
    },
    {
      4,
      "Edible2"
    },
    {
      5,
      "Edible3"
    }
  };

  public float Units
  {
    get => this.GetComponent<PrimaryElement>().Units;
    set => this.GetComponent<PrimaryElement>().Units = value;
  }

  public float Calories
  {
    get => this.Units * this.foodInfo.CaloriesPerUnit;
    set => this.Units = value / this.foodInfo.CaloriesPerUnit;
  }

  public EdiblesManager.FoodInfo FoodInfo
  {
    get => this.foodInfo;
    set
    {
      this.foodInfo = value;
      this.FoodID = this.foodInfo.Id;
    }
  }

  public bool isBeingConsumed { get; private set; }

  private Edible()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.shouldTransferDiseaseWithWorker = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.foodInfo == null)
    {
      if (this.FoodID == null)
        Debug.LogError((object) "No food FoodID");
      this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Edible);
    this.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
    this.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
    this.synchronizeAnims = false;
    Components.Edibles.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, (object) this);
  }

  public override HashedString[] GetWorkAnims(Worker worker)
  {
    EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
    bool flag = smi != null && smi.UseSalt();
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? (!flag ? Edible.hatWorkAnims : Edible.saltHatWorkAnims) : (!flag ? Edible.normalWorkAnims : Edible.saltWorkAnims);
  }

  public override HashedString[] GetWorkPstAnims(
    Worker worker,
    bool successfully_completed)
  {
    EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
    bool flag = smi != null && smi.UseSalt();
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? (!flag ? Edible.hatWorkPstAnim : Edible.saltHatWorkPstAnim) : (!flag ? Edible.normalWorkPstAnim : Edible.saltWorkPstAnim);
  }

  private void OnCraft(object data) => RationTracker.Get().RegisterCaloriesProduced(this.Calories);

  public float GetFeedingTime(Worker worker)
  {
    float num = this.Calories * 2E-05f;
    if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
    {
      BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
      if (smi != null && smi.IsBingeEating())
        num /= 2f;
    }
    return num;
  }

  protected override void OnStartWork(Worker worker)
  {
    this.totalFeedingTime = this.GetFeedingTime(worker);
    this.SetWorkTime(this.totalFeedingTime);
    this.caloriesConsumed = 0.0f;
    this.unitsConsumed = 0.0f;
    this.totalUnits = this.Units;
    worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse);
    this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
    this.StartConsuming();
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (this.currentlyLit)
    {
      if (this.currentModifier != this.caloriesLitSpaceModifier)
      {
        worker.GetAttributes().Remove(this.currentModifier);
        worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
        this.currentModifier = this.caloriesLitSpaceModifier;
      }
    }
    else if (this.currentModifier != this.caloriesModifier)
    {
      worker.GetAttributes().Remove(this.currentModifier);
      worker.GetAttributes().Add(this.caloriesModifier);
      this.currentModifier = this.caloriesModifier;
    }
    return this.OnTickConsume(worker, dt);
  }

  protected override void OnStopWork(Worker worker)
  {
    if (this.currentModifier != null)
    {
      worker.GetAttributes().Remove(this.currentModifier);
      this.currentModifier = (AttributeModifier) null;
    }
    worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
    this.StopConsuming(worker);
  }

  private bool OnTickConsume(Worker worker, float dt)
  {
    if (!this.isBeingConsumed)
    {
      DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
      return true;
    }
    bool flag = false;
    float num1 = dt / this.totalFeedingTime;
    float num2 = num1 * this.totalConsumableCalories;
    if ((double) this.caloriesConsumed + (double) num2 > (double) this.totalConsumableCalories)
      num2 = this.totalConsumableCalories - this.caloriesConsumed;
    this.caloriesConsumed += num2;
    worker.GetAmounts().Get("Calories").value += num2;
    float num3 = this.totalUnits * num1;
    if ((double) this.Units - (double) num3 < 0.0)
      num3 = this.Units;
    this.Units -= num3;
    this.unitsConsumed += num3;
    if ((double) this.Units <= 0.0)
      flag = true;
    return flag;
  }

  private void StartConsuming()
  {
    DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started");
    this.isBeingConsumed = true;
    this.worker.Trigger(1406130139, (object) this);
  }

  private void StopConsuming(Worker worker)
  {
    DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()");
    this.isBeingConsumed = false;
    PrimaryElement component = this.gameObject.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.DiseaseCount > 0)
    {
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_react_contaminated_food_kanim", new HashedString[1]
      {
        (HashedString) "react"
      }, (Func<StatusItem>) null);
    }
    for (int index = 0; index < this.foodInfo.Effects.Count; ++index)
      worker.GetComponent<Effects>().Add(this.foodInfo.Effects[index], true);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
    this.AddQualityEffects(worker);
    worker.Trigger(1121894420, (object) this);
    this.Trigger(-10536414, (object) worker.gameObject);
    this.unitsConsumed = float.NaN;
    this.caloriesConsumed = float.NaN;
    this.totalUnits = float.NaN;
    if ((double) this.Units > 0.0)
      return;
    this.gameObject.DeleteObject();
  }

  public static string GetEffectForFoodQuality(int qualityLevel)
  {
    qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
    return Edible.qualityEffects[qualityLevel];
  }

  private void AddQualityEffects(Worker worker)
  {
    int qualityLevel = this.FoodInfo.Quality + Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
    worker.GetComponent<Effects>().Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Edibles.Remove(this);
  }

  public int GetQuality() => this.foodInfo.Quality;

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.CALORIES, (object) GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, (object) GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit)), Descriptor.DescriptorType.Information));
    descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality))));
    foreach (string effect in this.foodInfo.Effects)
      descriptorList.Add(new Descriptor((string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".NAME"), (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".DESCRIPTION")));
    return descriptorList;
  }

  public class EdibleStartWorkInfo : Worker.StartWorkInfo
  {
    public float amount { get; private set; }

    public EdibleStartWorkInfo(Workable workable, float amount)
      : base(workable)
      => this.amount = amount;
  }
}
