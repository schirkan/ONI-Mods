// Decompiled with JetBrains decompiler
// Type: BuddingTrunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour, ISim4000ms
{
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private WiltCondition wilting;
  [MyCmpReq]
  private UprootedMonitor uprooted;
  public string budPrefabID;
  public int maxBuds = 5;
  [Serialize]
  private Ref<HarvestDesignatable>[] buds = new Ref<HarvestDesignatable>[7];
  private StatusItem growingBranchesStatusItem;
  [Serialize]
  private bool hasExtraSeedAvailable;
  private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>((System.Action<BuddingTrunk, object>) ((component, data) => component.OnNewGameSpawn(data)));
  private Coroutine newGameSpawnRoutine;
  private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>((System.Action<BuddingTrunk, object>) ((component, data) => component.OnUprooted(data)));
  private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnDrownedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>((System.Action<BuddingTrunk, object>) ((component, data) => component.OnUprooted(data)));
  private static readonly EventSystem.IntraObjectHandler<BuddingTrunk> OnHarvestDesignationChangedDelegate = new EventSystem.IntraObjectHandler<BuddingTrunk>((System.Action<BuddingTrunk, object>) ((component, data) => component.UpdateAllBudsHarvestStatus(data)));
  private static List<int> spawn_choices = new List<int>();

  public bool ExtraSeedAvailable => this.hasExtraSeedAvailable;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.simRenderLoadBalance = true;
    this.growingBranchesStatusItem = new StatusItem("GROWINGBRANCHES", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    this.Subscribe<BuddingTrunk>(1119167081, BuddingTrunk.OnNewGameSpawnDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<BuddingTrunk>(-216549700, BuddingTrunk.OnUprootedDelegate);
    this.Subscribe<BuddingTrunk>(-750750377, BuddingTrunk.OnDrownedDelegate);
    this.Subscribe<BuddingTrunk>(-266953818, BuddingTrunk.OnHarvestDesignationChangedDelegate);
  }

  protected override void OnCleanUp()
  {
    if (this.newGameSpawnRoutine != null)
      this.StopCoroutine(this.newGameSpawnRoutine);
    base.OnCleanUp();
  }

  private void OnNewGameSpawn(object data)
  {
    float percent = 1f;
    if ((double) UnityEngine.Random.value < 0.1)
      percent = UnityEngine.Random.Range(0.75f, 0.99f);
    else
      this.newGameSpawnRoutine = this.StartCoroutine(this.NewGameSproutBudRoutine());
    this.growing.OverrideMaturityLevel(percent);
  }

  private IEnumerator NewGameSproutBudRoutine()
  {
    for (int i = 0; i < this.buds.Length; ++i)
    {
      yield return (object) new WaitForEndOfFrame();
      this.TrySpawnRandomBud(growth_percentage: UnityEngine.Random.Range(0.0f, 1f));
    }
    this.newGameSpawnRoutine = (Coroutine) null;
    yield return (object) 0;
  }

  public void Sim4000ms(float dt)
  {
    if (this.growing.IsGrown() && !this.wilting.IsWilting())
    {
      this.TrySpawnRandomBud();
      this.GetComponent<KSelectable>().AddStatusItem(this.growingBranchesStatusItem);
    }
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(this.growingBranchesStatusItem);
  }

  private void OnUprooted(object data = null) => this.YieldWood();

  private void YieldWood()
  {
    foreach (Ref<HarvestDesignatable> bud in this.buds)
    {
      HarvestDesignatable harvestDesignatable = bud?.Get();
      if ((UnityEngine.Object) harvestDesignatable != (UnityEngine.Object) null)
        harvestDesignatable.Trigger(-216549700, (object) null);
    }
  }

  public float GetMaxBranchMaturity()
  {
    float max_maturity = 0.0f;
    this.GetMostMatureBranch(out max_maturity);
    return max_maturity;
  }

  public void ConsumeMass(float mass_to_consume)
  {
    HarvestDesignatable mostMatureBranch = this.GetMostMatureBranch(out float _);
    if (!(bool) (UnityEngine.Object) mostMatureBranch)
      return;
    Growing component = mostMatureBranch.GetComponent<Growing>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.ConsumeMass(mass_to_consume);
  }

  private HarvestDesignatable GetMostMatureBranch(out float max_maturity)
  {
    max_maturity = 0.0f;
    HarvestDesignatable harvestDesignatable1 = (HarvestDesignatable) null;
    foreach (Ref<HarvestDesignatable> bud in this.buds)
    {
      HarvestDesignatable harvestDesignatable2 = bud?.Get();
      if ((UnityEngine.Object) harvestDesignatable2 != (UnityEngine.Object) null)
      {
        AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup((Component) harvestDesignatable2);
        if (amountInstance != null)
        {
          float num = amountInstance.value / amountInstance.GetMax();
          if ((double) num > (double) max_maturity)
          {
            max_maturity = num;
            harvestDesignatable1 = harvestDesignatable2;
          }
        }
      }
    }
    return harvestDesignatable1;
  }

  public void TrySpawnRandomBud(object data = null, float growth_percentage = 0.0f)
  {
    if (this.uprooted.IsUprooted)
      return;
    BuddingTrunk.spawn_choices.Clear();
    int num = 0;
    for (int idx = 0; idx < this.buds.Length; ++idx)
    {
      int cell = Grid.PosToCell(this.GetBudPosition(idx));
      if ((this.buds[idx] == null || (UnityEngine.Object) this.buds[idx].Get() == (UnityEngine.Object) null) && this.CanGrowInto(cell))
        BuddingTrunk.spawn_choices.Add(idx);
      else if (this.buds[idx] != null && (UnityEngine.Object) this.buds[idx].Get() != (UnityEngine.Object) null)
        ++num;
    }
    if (num >= this.maxBuds)
      return;
    BuddingTrunk.spawn_choices.Shuffle<int>();
    if (BuddingTrunk.spawn_choices.Count <= 0)
      return;
    int spawnChoice = BuddingTrunk.spawn_choices[0];
    Vector3 budPosition = this.GetBudPosition(spawnChoice);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) this.budPrefabID), budPosition);
    gameObject.SetActive(true);
    gameObject.GetComponent<Growing>().OverrideMaturityLevel(growth_percentage);
    gameObject.GetComponent<TreeBud>().SetTrunkPosition(this, spawnChoice);
    HarvestDesignatable component = gameObject.GetComponent<HarvestDesignatable>();
    this.buds[spawnChoice] = new Ref<HarvestDesignatable>(component);
    this.UpdateBudHarvestState(component);
    this.TryRollNewSeed();
  }

  public void TryRollNewSeed()
  {
    if (this.hasExtraSeedAvailable || UnityEngine.Random.Range(0, 100) >= 5)
      return;
    this.hasExtraSeedAvailable = true;
  }

  public TreeBud GetBranchAtPosition(int idx)
  {
    if (this.buds[idx] == null)
      return (TreeBud) null;
    HarvestDesignatable harvestDesignatable = this.buds[idx].Get();
    return !((UnityEngine.Object) harvestDesignatable != (UnityEngine.Object) null) ? (TreeBud) null : harvestDesignatable.GetComponent<TreeBud>();
  }

  public void ExtractExtraSeed()
  {
    if (!this.hasExtraSeedAvailable)
      return;
    this.hasExtraSeedAvailable = false;
    Vector3 position = this.transform.position;
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
    Util.KInstantiate(Assets.GetPrefab((Tag) "ForestTreeSeed"), position).SetActive(true);
  }

  private void UpdateBudHarvestState(HarvestDesignatable bud)
  {
    HarvestDesignatable component = this.GetComponent<HarvestDesignatable>();
    bud.SetHarvestWhenReady(component.HarvestWhenReady);
  }

  public void OnBranchRemoved(int idx, TreeBud treeBud)
  {
    if (idx < 0 || idx >= this.buds.Length)
      Debug.Assert(false, (object) ("invalid branch index " + (object) idx));
    HarvestDesignatable component = treeBud.GetComponent<HarvestDesignatable>();
    HarvestDesignatable harvestDesignatable = this.buds[idx] != null ? this.buds[idx].Get() : (HarvestDesignatable) null;
    if ((UnityEngine.Object) component != (UnityEngine.Object) harvestDesignatable)
      Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "OnBranchRemoved branch {0} does not match known branch {1}", (object) component, (object) harvestDesignatable);
    this.buds[idx] = (Ref<HarvestDesignatable>) null;
  }

  private void UpdateAllBudsHarvestStatus(object data = null)
  {
    foreach (Ref<HarvestDesignatable> bud1 in this.buds)
    {
      if (bud1 != null)
      {
        HarvestDesignatable bud2 = bud1.Get();
        if ((UnityEngine.Object) bud2 == (UnityEngine.Object) null)
          Debug.LogWarning((object) "harvest_designatable was null");
        else
          this.UpdateBudHarvestState(bud2);
      }
    }
  }

  public bool CanGrowInto(int cell)
  {
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
      return false;
    int cell1 = Grid.CellAbove(cell);
    return Grid.IsValidCell(cell1) && !Grid.IsSubstantialLiquid(cell1) && (!((UnityEngine.Object) Grid.Objects[cell, 1] != (UnityEngine.Object) null) && !((UnityEngine.Object) Grid.Objects[cell, 5] != (UnityEngine.Object) null)) && !Grid.Foundation[cell];
  }

  private Vector3 GetBudPosition(int idx)
  {
    Vector3 vector3 = this.transform.position;
    switch (idx)
    {
      case 0:
        vector3 = this.transform.position + Vector3.left;
        break;
      case 1:
        vector3 = this.transform.position + Vector3.left + Vector3.up;
        break;
      case 2:
        vector3 = this.transform.position + Vector3.left + Vector3.up + Vector3.up;
        break;
      case 3:
        vector3 = this.transform.position + Vector3.up + Vector3.up;
        break;
      case 4:
        vector3 = this.transform.position + Vector3.right + Vector3.up + Vector3.up;
        break;
      case 5:
        vector3 = this.transform.position + Vector3.right + Vector3.up;
        break;
      case 6:
        vector3 = this.transform.position + Vector3.right;
        break;
    }
    return vector3;
  }
}
