// Decompiled with JetBrains decompiler
// Type: Deconstructable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Deconstructable")]
public class Deconstructable : Workable
{
  private Chore chore;
  public bool allowDeconstruction = true;
  [Serialize]
  private bool isMarkedForDeconstruction;
  [Serialize]
  public Tag[] constructionElements;
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnDeconstruct(data)));
  private static readonly Vector2 INITIAL_VELOCITY_RANGE = new Vector2(0.5f, 4f);
  private bool destroyed;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
    this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.minimumAttributeMultiplier = 0.75f;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    Building component = this.GetComponent<Building>();
    CellOffset[][] table = OffsetGroups.InvertedStandardTable;
    if (component.Def.IsTilePiece)
      table = OffsetGroups.InvertedStandardTableWithCorners;
    this.SetOffsetTable(OffsetGroups.BuildReachabilityTable(component.Def.PlacementOffsets, table, component.Def.ConstructionOffsetFilter));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Deconstructable>(493375141, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(-111137758, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(2127324410, Deconstructable.OnCancelDelegate);
    this.Subscribe<Deconstructable>(-790448070, Deconstructable.OnDeconstructDelegate);
    if (this.constructionElements == null || this.constructionElements.Length == 0)
    {
      this.constructionElements = new Tag[1];
      this.constructionElements[0] = this.GetComponent<PrimaryElement>().Element.tag;
    }
    if (!this.isMarkedForDeconstruction)
      return;
    this.QueueDeconstruction();
  }

  public override float GetWorkTime() => this.GetComponent<Building>().Def.ConstructionTime * 0.5f;

  protected override void OnStartWork(Worker worker)
  {
    this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Building component1 = this.GetComponent<Building>();
    SimCellOccupier component2 = this.GetComponent<SimCellOccupier>();
    if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null && DetailsScreen.Instance.CompareTargetWith(this.gameObject))
      DetailsScreen.Instance.Show(false);
    PrimaryElement component3 = this.GetComponent<PrimaryElement>();
    float temperature = component3.Temperature;
    byte disease_idx = component3.DiseaseIdx;
    int disease_count = component3.DiseaseCount;
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      if (component1.Def.TileLayer != ObjectLayer.NumLayers)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        if ((UnityEngine.Object) Grid.Objects[cell, (int) component1.Def.TileLayer] == (UnityEngine.Object) this.gameObject)
        {
          Grid.Objects[cell, (int) component1.Def.ObjectLayer] = (GameObject) null;
          Grid.Objects[cell, (int) component1.Def.TileLayer] = (GameObject) null;
          Grid.Foundation[cell] = false;
          TileVisualizer.RefreshCell(cell, component1.Def.TileLayer, component1.Def.ReplacementLayer);
        }
      }
      component2.DestroySelf((System.Action) (() => this.TriggerDestroy(temperature, disease_idx, disease_count)));
    }
    else
      this.TriggerDestroy(temperature, disease_idx, disease_count);
    string sound = GlobalAssets.GetSound("Finish_Deconstruction_" + component1.Def.AudioSize);
    if (sound != null)
      KMonoBehaviour.PlaySound3DAtLocation(sound, this.gameObject.transform.GetPosition());
    this.Trigger(-702296337, (object) this);
  }

  private void TriggerDestroy(float temperature, byte disease_idx, int disease_count)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.destroyed)
      return;
    this.SpawnItemsFromConstruction(temperature, disease_idx, disease_count);
    this.destroyed = true;
    this.gameObject.DeleteObject();
  }

  private void QueueDeconstruction()
  {
    if (this.chore != null)
      return;
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.ReplacementLayer != ObjectLayer.NumLayers)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) component);
      if ((UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] != (UnityEngine.Object) null)
        return;
    }
    if (DebugHandler.InstantBuildMode)
    {
      this.OnCompleteWork((Worker) null);
    }
    else
    {
      Prioritizable.AddRef(this.gameObject);
      this.chore = (Chore) new WorkChore<Deconstructable>(Db.Get().ChoreTypes.Deconstruct, (IStateMachineTarget) this, only_when_operational: false, is_preemptable: true, ignore_building_assignment: true);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, (object) this);
      this.isMarkedForDeconstruction = true;
      this.Trigger(2108245096, (object) "Deconstruct");
    }
  }

  private void OnDeconstruct()
  {
    if (this.chore == null)
      this.QueueDeconstruction();
    else
      this.CancelDeconstruction();
  }

  public bool IsMarkedForDeconstruction() => this.chore != null;

  public void SetAllowDeconstruction(bool allow)
  {
    this.allowDeconstruction = allow;
    if (this.allowDeconstruction)
      return;
    this.CancelDeconstruction();
  }

  public void SpawnItemsFromConstruction()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    this.SpawnItemsFromConstruction(component.Temperature, component.DiseaseIdx, component.DiseaseCount);
  }

  private void SpawnItemsFromConstruction(float temperature, byte disease_idx, int disease_count)
  {
    Building component = this.GetComponent<Building>();
    for (int index = 0; index < this.constructionElements.Length && component.Def.Mass.Length > index; ++index)
    {
      GameObject go = Deconstructable.SpawnItem(this.transform.GetPosition(), component.Def, this.constructionElements[index], component.Def.Mass[index], temperature, disease_idx, disease_count);
      go.transform.SetPosition(go.transform.GetPosition() + Vector3.up * 0.5f);
      int cell = Grid.PosToCell(go.transform.GetPosition());
      int num = Grid.CellAbove(cell);
      Vector2 initial_velocity = Grid.IsValidCell(cell) && Grid.Solid[cell] || Grid.IsValidCell(num) && Grid.Solid[num] ? Vector2.zero : new Vector2(UnityEngine.Random.Range(-1f, 1f) * Deconstructable.INITIAL_VELOCITY_RANGE.x, Deconstructable.INITIAL_VELOCITY_RANGE.y);
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, initial_velocity);
    }
  }

  private static GameObject SpawnItem(
    Vector3 position,
    BuildingDef def,
    Tag src_element,
    float src_mass,
    float src_temperature,
    byte disease_idx,
    int disease_count)
  {
    GameObject gameObject = (GameObject) null;
    int cell1 = Grid.PosToCell(position);
    CellOffset[] placementOffsets = def.PlacementOffsets;
    Element element = ElementLoader.GetElement(src_element);
    if (element != null)
    {
      float num = src_mass;
      for (int index1 = 0; (double) index1 < (double) src_mass / 400.0; ++index1)
      {
        int index2 = index1 % def.PlacementOffsets.Length;
        int cell2 = Grid.OffsetCell(cell1, placementOffsets[index2]);
        float mass = num;
        if ((double) num > 400.0)
        {
          mass = 400f;
          num -= 400f;
        }
        gameObject = element.substance.SpawnResource(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), mass, src_temperature, disease_idx, disease_count);
      }
    }
    else
    {
      for (int index1 = 0; (double) index1 < (double) src_mass; ++index1)
      {
        int index2 = index1 % def.PlacementOffsets.Length;
        int cell2 = Grid.OffsetCell(cell1, placementOffsets[index2]);
        gameObject = GameUtil.KInstantiate(Assets.GetPrefab(src_element), Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
        gameObject.SetActive(true);
      }
    }
    return gameObject;
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.allowDeconstruction)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore == null ? new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DEMOLISH.NAME, new System.Action(this.OnDeconstruct), tooltipText: ((string) UI.USERMENUACTIONS.DEMOLISH.TOOLTIP)) : new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DEMOLISH.NAME_OFF, new System.Action(this.OnDeconstruct), tooltipText: ((string) UI.USERMENUACTIONS.DEMOLISH.TOOLTIP_OFF)), 0.0f);
  }

  public void CancelDeconstruction()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Cancelled deconstruction");
    this.chore = (Chore) null;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction);
    this.ShowProgressBar(false);
    this.isMarkedForDeconstruction = false;
    Prioritizable.RemoveRef(this.gameObject);
  }

  private void OnCancel(object data) => this.CancelDeconstruction();

  private void OnDeconstruct(object data)
  {
    if (!this.allowDeconstruction)
      return;
    this.QueueDeconstruction();
  }
}
