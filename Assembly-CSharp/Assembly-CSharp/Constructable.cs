// Decompiled with JetBrains decompiler
// Type: Constructable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Constructable")]
public class Constructable : Workable, ISaveLoadable
{
  [MyCmpAdd]
  private Storage storage;
  [MyCmpAdd]
  private Notifier notifier;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  private Rotatable rotatable;
  private Notification invalidLocation;
  private float initialTemperature = -1f;
  [Serialize]
  private bool isPrioritized;
  private FetchList2 fetchList;
  private Chore buildChore;
  private bool materialNeedsCleared;
  private bool hasUnreachableDigs;
  private bool finished;
  private bool unmarked;
  public bool isDiggingRequired = true;
  private bool waitForFetchesBeforeDigging;
  private bool hasLadderNearby;
  private Extents ladderDetectionExtents;
  [Serialize]
  public bool IsReplacementTile;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle digPartitionerEntry;
  private HandleVector<int>.Handle ladderParititonerEntry;
  private LoggerFSS log = new LoggerFSS(nameof (Constructable));
  [Serialize]
  private Tag[] selectedElementsTags;
  private Element[] selectedElements;
  [Serialize]
  private int[] ids;
  private static readonly EventSystem.IntraObjectHandler<Constructable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Constructable>((System.Action<Constructable, object>) ((component, data) => component.OnReachableChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Constructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Constructable>((System.Action<Constructable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Constructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Constructable>((System.Action<Constructable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public Recipe Recipe => this.building.Def.CraftRecipe;

  public IList<Tag> SelectedElementsTags
  {
    get => (IList<Tag>) this.selectedElementsTags;
    set
    {
      if (this.selectedElementsTags == null || this.selectedElementsTags.Length != value.Count)
        this.selectedElementsTags = new Tag[value.Count];
      value.CopyTo(this.selectedElementsTags, 0);
    }
  }

  public override string GetConversationTopic() => this.building.Def.PrefabID;

  protected override void OnCompleteWork(Worker worker)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    bool flag1 = true;
    foreach (GameObject gameObject in this.storage.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        {
          num1 += component.Mass;
          num2 += component.Temperature * component.Mass;
          flag1 = flag1 && component.HasTag(GameTags.Liquifiable);
        }
      }
    }
    if ((double) num1 <= 0.0)
    {
      DebugUtil.LogWarningArgs((UnityEngine.Object) this.gameObject, (object) "uhhh this constructable is about to generate a nan", (object) "Item Count: ", (object) this.storage.items.Count);
    }
    else
    {
      this.initialTemperature = !flag1 ? Mathf.Clamp(num2 / num1, 288.15f, 318.15f) : Mathf.Min(num2 / num1, 318.15f);
      KAnimGraphTileVisualizer component1 = this.GetComponent<KAnimGraphTileVisualizer>();
      UtilityConnections connections = (UnityEngine.Object) component1 == (UnityEngine.Object) null ? (UtilityConnections) 0 : component1.Connections;
      bool flag2 = true;
      if (this.IsReplacementTile)
      {
        int cell = Grid.PosToCell(this.transform.GetLocalPosition());
        GameObject replacementCandidate = this.building.Def.GetReplacementCandidate(cell);
        if ((UnityEngine.Object) replacementCandidate != (UnityEngine.Object) null)
        {
          flag2 = false;
          SimCellOccupier component2 = replacementCandidate.GetComponent<SimCellOccupier>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            component2.DestroySelf((System.Action) (() =>
            {
              if (!((UnityEngine.Object) this != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
                return;
              this.FinishConstruction(connections);
            }));
          }
          else
          {
            Conduit component3 = replacementCandidate.GetComponent<Conduit>();
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
              component3.GetFlowManager().MarkForReplacement(cell);
            BuildingComplete component4 = replacementCandidate.GetComponent<BuildingComplete>();
            if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
            {
              component4.Subscribe(-21016276, (System.Action<object>) (data => this.FinishConstruction(connections)));
            }
            else
            {
              Debug.LogWarning((object) ("Why am I trying to replace a: " + replacementCandidate.name));
              this.FinishConstruction(connections);
            }
          }
          KAnimGraphTileVisualizer component5 = replacementCandidate.GetComponent<KAnimGraphTileVisualizer>();
          if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
            component5.skipCleanup = true;
          Deconstructable component6 = replacementCandidate.GetComponent<Deconstructable>();
          if ((UnityEngine.Object) component6 != (UnityEngine.Object) null)
            component6.SpawnItemsFromConstruction();
          replacementCandidate.Trigger(1606648047, (object) this.building.Def.TileLayer);
          replacementCandidate.DeleteObject();
        }
      }
      if (flag2)
        this.FinishConstruction(connections);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, this.GetComponent<KSelectable>().GetName(), this.transform);
    }
  }

  private void FinishConstruction(UtilityConnections connections)
  {
    Rotatable component1 = this.GetComponent<Rotatable>();
    Orientation orientation = (UnityEngine.Object) component1 != (UnityEngine.Object) null ? component1.GetOrientation() : Orientation.Neutral;
    int cell = Grid.PosToCell(this.transform.GetLocalPosition());
    this.UnmarkArea();
    GameObject gameObject = this.building.Def.Build(cell, orientation, this.storage, (IList<Tag>) this.selectedElementsTags, this.initialTemperature, timeBuilt: GameClock.Instance.GetTime());
    gameObject.transform.rotation = this.transform.rotation;
    Rotatable component2 = gameObject.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.SetOrientation(orientation);
    KAnimGraphTileVisualizer component3 = this.GetComponent<KAnimGraphTileVisualizer>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      gameObject.GetComponent<KAnimGraphTileVisualizer>().Connections = connections;
      component3.skipCleanup = true;
    }
    KSelectable component4 = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && component4.IsSelected && (UnityEngine.Object) gameObject.GetComponent<KSelectable>() != (UnityEngine.Object) null)
    {
      component4.Unselect();
      if (PlayerController.Instance.ActiveTool.name == "SelectTool")
        ((SelectTool) PlayerController.Instance.ActiveTool).SelectNextFrame(gameObject.GetComponent<KSelectable>());
    }
    this.storage.ConsumeAllIgnoringDisease();
    this.finished = true;
    this.DeleteObject();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.invalidLocation = new Notification((string) MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.TOOLTIP + notificationList.ReduceMessages(false)));
    CellOffset[][] table = OffsetGroups.InvertedStandardTable;
    if (this.building.Def.IsTilePiece)
      table = OffsetGroups.InvertedStandardTableWithCorners;
    CellOffset[][] offset_table = OffsetGroups.BuildReachabilityTable(this.building.Def.PlacementOffsets, table, this.building.Def.ConstructionOffsetFilter);
    this.SetOffsetTable(offset_table);
    this.storage.SetOffsetTable(offset_table);
    this.faceTargetWhenWorking = true;
    this.Subscribe<Constructable>(-1432940121, Constructable.OnReachableChangedDelegate);
    if ((UnityEngine.Object) this.rotatable == (UnityEngine.Object) null)
      this.MarkArea();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Building;
    this.workingStatusItem = (StatusItem) null;
    this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.minimumAttributeMultiplier = 0.75f;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    Prioritizable.AddRef(this.gameObject);
    this.synchronizeAnims = false;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Constructable>(2127324410, Constructable.OnCancelDelegate);
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      this.MarkArea();
    this.fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.BuildFetch);
    PrimaryElement component1 = this.GetComponent<PrimaryElement>();
    Element element = ElementLoader.GetElement(this.SelectedElementsTags[0]);
    Debug.Assert(element != null, (object) "Missing primary element for Constructable");
    component1.ElementID = element.id;
    double num1;
    float num2 = (float) (num1 = 293.149993896484);
    component1.Temperature = (float) num1;
    component1.Temperature = num2;
    foreach (Recipe.Ingredient allIngredient in this.Recipe.GetAllIngredients((IList<Tag>) this.selectedElementsTags))
    {
      this.fetchList.Add(allIngredient.tag, amount: allIngredient.amount);
      MaterialNeeds.Instance.UpdateNeed(allIngredient.tag, allIngredient.amount);
    }
    if (!this.building.Def.IsTilePiece)
      this.gameObject.layer = LayerMask.NameToLayer("Construction");
    this.building.RunOnArea((System.Action<int>) (offset_cell =>
    {
      if (!((UnityEngine.Object) this.gameObject.GetComponent<ConduitBridge>() == (UnityEngine.Object) null))
        return;
      GameObject go = Grid.Objects[offset_cell, 7];
      if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
        return;
      go.DeleteObject();
    }));
    if (this.IsReplacementTile)
    {
      if (this.building.Def.ReplacementLayer != ObjectLayer.NumLayers)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        GameObject gameObject1 = Grid.Objects[cell, (int) this.building.Def.ReplacementLayer];
        if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject1 == (UnityEngine.Object) this.gameObject)
        {
          Grid.Objects[cell, (int) this.building.Def.ReplacementLayer] = this.gameObject;
          if ((UnityEngine.Object) this.gameObject.GetComponent<SimCellOccupier>() != (UnityEngine.Object) null)
            World.Instance.blockTileRenderer.AddBlock(LayerMask.NameToLayer("Overlay"), this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
          TileVisualizer.RefreshCell(cell, this.building.Def.TileLayer, this.building.Def.ReplacementLayer);
        }
        else
        {
          Debug.LogError((object) "multiple replacement tiles on the same cell!");
          Util.KDestroyGameObject(this.gameObject);
        }
        GameObject gameObject2 = Grid.Objects[cell, (int) this.building.Def.ObjectLayer];
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          Deconstructable component2 = gameObject2.GetComponent<Deconstructable>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            component2.CancelDeconstruction();
        }
      }
    }
    bool component3 = (bool) (UnityEngine.Object) this.building.Def.BuildingComplete.GetComponent<Ladder>();
    this.waitForFetchesBeforeDigging = component3 || (bool) (UnityEngine.Object) this.building.Def.BuildingComplete.GetComponent<SimCellOccupier>() || (bool) (UnityEngine.Object) this.building.Def.BuildingComplete.GetComponent<Door>() || (bool) (UnityEngine.Object) this.building.Def.BuildingComplete.GetComponent<LiquidPumpingStation>();
    if (component3)
    {
      int x = 0;
      int y1 = 0;
      Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out x, out y1);
      int y2 = y1 - 3;
      this.ladderDetectionExtents = new Extents(x, y2, 1, 5);
      this.ladderParititonerEntry = GameScenePartitioner.Instance.Add("Constructable.OnNearbyBuildingLayerChanged", (object) this.gameObject, this.ladderDetectionExtents, GameScenePartitioner.Instance.objectLayers[1], new System.Action<object>(this.OnNearbyBuildingLayerChanged));
      this.OnNearbyBuildingLayerChanged((object) null);
    }
    this.fetchList.Submit(new System.Action(this.OnFetchListComplete), true);
    this.PlaceDiggables();
    new ReachabilityMonitor.Instance((Workable) this).StartSM();
    this.Subscribe<Constructable>(493375141, Constructable.OnRefreshUserMenuDelegate);
    Prioritizable component4 = this.GetComponent<Prioritizable>();
    component4.onPriorityChanged += new System.Action<PrioritySetting>(this.OnPriorityChanged);
    this.OnPriorityChanged(component4.GetMasterPriority());
  }

  private void OnPriorityChanged(PrioritySetting priority) => this.building.RunOnArea((System.Action<int>) (cell =>
  {
    Diggable diggable = Diggable.GetDiggable(cell);
    if (!((UnityEngine.Object) diggable != (UnityEngine.Object) null))
      return;
    diggable.GetComponent<Prioritizable>().SetMasterPriority(priority);
  }));

  private void MarkArea()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingDef def = this.building.Def;
    Orientation orientation = this.building.Orientation;
    ObjectLayer layer = this.IsReplacementTile ? def.ReplacementLayer : def.ObjectLayer;
    def.MarkArea(cell, orientation, layer, this.gameObject);
    if (!def.IsTilePiece)
      return;
    if ((UnityEngine.Object) Grid.Objects[cell, (int) def.TileLayer] == (UnityEngine.Object) null)
    {
      def.MarkArea(cell, orientation, def.TileLayer, this.gameObject);
      def.RunOnArea(cell, orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, def.TileLayer, def.ReplacementLayer)));
    }
    Grid.IsTileUnderConstruction[cell] = true;
  }

  private void UnmarkArea()
  {
    if (this.unmarked)
      return;
    this.unmarked = true;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingDef def = this.building.Def;
    ObjectLayer layer = this.IsReplacementTile ? this.building.Def.ReplacementLayer : this.building.Def.ObjectLayer;
    def.UnmarkArea(cell, this.building.Orientation, layer, this.gameObject);
    if (!def.IsTilePiece)
      return;
    Grid.IsTileUnderConstruction[cell] = false;
  }

  private void OnNearbyBuildingLayerChanged(object data)
  {
    this.hasLadderNearby = false;
    for (int y = this.ladderDetectionExtents.y; y < this.ladderDetectionExtents.y + this.ladderDetectionExtents.height; ++y)
    {
      int num = Grid.OffsetCell(0, this.ladderDetectionExtents.x, y);
      if (Grid.IsValidCell(num))
      {
        GameObject gameObject = (GameObject) null;
        Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Ladder>() != (UnityEngine.Object) null)
        {
          this.hasLadderNearby = true;
          break;
        }
      }
    }
  }

  private bool IsWire() => this.building.Def.name.Contains("Wire");

  public bool IconConnectionAnimation(
    float delay,
    int connectionCount,
    string defName,
    string soundName)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (this.building.Def.Name.Contains(defName))
    {
      Building building = (Building) null;
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        building = gameObject.GetComponent<Building>();
      if ((UnityEngine.Object) building != (UnityEngine.Object) null)
      {
        bool flag = this.IsWire();
        int num1 = flag ? building.GetPowerInputCell() : building.GetUtilityInputCell();
        int num2 = flag ? num1 : building.GetUtilityOutputCell();
        if (cell == num1 || cell == num2)
        {
          BuildingCellVisualizer component = building.gameObject.GetComponent<BuildingCellVisualizer>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && (flag ? (component.RequiresPower ? 1 : 0) : (component.RequiresUtilityConnection ? 1 : 0)) != 0)
          {
            component.ConnectedEventWithDelay(delay, connectionCount, cell, soundName);
            return true;
          }
        }
      }
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    if (this.IsReplacementTile && this.building.Def.isKAnimTile)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      GameObject gameObject = Grid.Objects[cell, (int) this.building.Def.ReplacementLayer];
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) this.gameObject && (UnityEngine.Object) gameObject.GetComponent<SimCellOccupier>() != (UnityEngine.Object) null)
        World.Instance.blockTileRenderer.RemoveBlock(this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
    }
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.digPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.ladderParititonerEntry);
    SaveLoadRoot component = this.GetComponent<SaveLoadRoot>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      SaveLoader.Instance.saveManager.Unregister(component);
    if (this.fetchList != null)
      this.fetchList.Cancel("Constructable destroyed");
    this.UnmarkArea();
    foreach (int placementCell in this.building.PlacementCells)
    {
      Diggable diggable = Diggable.GetDiggable(placementCell);
      if ((UnityEngine.Object) diggable != (UnityEngine.Object) null)
        diggable.gameObject.DeleteObject();
    }
    base.OnCleanUp();
  }

  private void OnDiggableReachabilityChanged(object data)
  {
    if (this.IsReplacementTile)
      return;
    int diggable_count = 0;
    int unreachable_count = 0;
    this.building.RunOnArea((System.Action<int>) (offset_cell =>
    {
      Diggable diggable = Diggable.GetDiggable(offset_cell);
      if (!((UnityEngine.Object) diggable != (UnityEngine.Object) null))
        return;
      ++diggable_count;
      if (diggable.GetComponent<KPrefabID>().HasTag(GameTags.Reachable))
        return;
      ++unreachable_count;
    }));
    bool flag = unreachable_count > 0 && unreachable_count == diggable_count;
    if (flag == this.hasUnreachableDigs)
      return;
    if (flag)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable);
    this.hasUnreachableDigs = flag;
  }

  private void PlaceDiggables()
  {
    if (this.waitForFetchesBeforeDigging && this.fetchList != null && !this.hasLadderNearby)
    {
      this.OnDiggableReachabilityChanged((object) null);
    }
    else
    {
      bool digs_complete = true;
      if (!this.solidPartitionerEntry.IsValid())
      {
        Extents placementExtents = this.building.GetValidPlacementExtents();
        this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete", (object) this.gameObject, placementExtents, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChangedOrDigDestroyed));
        this.digPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete", (object) this.gameObject, placementExtents, GameScenePartitioner.Instance.digDestroyedLayer, new System.Action<object>(this.OnSolidChangedOrDigDestroyed));
      }
      if (!this.IsReplacementTile)
      {
        this.building.RunOnArea((System.Action<int>) (offset_cell =>
        {
          PrioritySetting masterPriority = this.GetComponent<Prioritizable>().GetMasterPriority();
          if (!Diggable.IsDiggable(offset_cell))
            return;
          digs_complete = false;
          Diggable diggable = Diggable.GetDiggable(offset_cell);
          if ((UnityEngine.Object) diggable == (UnityEngine.Object) null)
          {
            diggable = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), Grid.SceneLayer.Move).GetComponent<Diggable>();
            diggable.gameObject.SetActive(true);
            diggable.transform.SetPosition(Grid.CellToPosCBC(offset_cell, Grid.SceneLayer.Move));
            diggable.Subscribe(-1432940121, new System.Action<object>(this.OnDiggableReachabilityChanged));
            Grid.Objects[offset_cell, 7] = diggable.gameObject;
          }
          else
          {
            diggable.Unsubscribe(-1432940121, new System.Action<object>(this.OnDiggableReachabilityChanged));
            diggable.Subscribe(-1432940121, new System.Action<object>(this.OnDiggableReachabilityChanged));
          }
          diggable.choreTypeIdHash = Db.Get().ChoreTypes.BuildDig.IdHash;
          diggable.GetComponent<Prioritizable>().SetMasterPriority(masterPriority);
          RenderUtil.EnableRenderer(diggable.transform, false);
          SaveLoadRoot component = diggable.GetComponent<SaveLoadRoot>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          UnityEngine.Object.Destroy((UnityEngine.Object) component);
        }));
        this.OnDiggableReachabilityChanged((object) null);
      }
      bool flag1 = this.building.Def.IsValidBuildLocation(this.gameObject, this.transform.GetPosition(), this.building.Orientation);
      if (flag1)
        this.notifier.Remove(this.invalidLocation);
      else
        this.notifier.Add(this.invalidLocation);
      this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidBuildingLocation, !flag1, (object) this);
      bool flag2 = digs_complete & flag1 && this.fetchList == null;
      if (flag2 && this.buildChore == null)
      {
        this.buildChore = (Chore) new WorkChore<Constructable>(Db.Get().ChoreTypes.Build, (IStateMachineTarget) this, on_complete: new System.Action<Chore>(this.UpdateBuildState), on_begin: new System.Action<Chore>(this.UpdateBuildState), on_end: new System.Action<Chore>(this.UpdateBuildState), is_preemptable: true);
        this.UpdateBuildState(this.buildChore);
      }
      else
      {
        if (flag2 || this.buildChore == null)
          return;
        this.buildChore.Cancel("Need to dig");
        this.buildChore = (Chore) null;
      }
    }
  }

  private void OnFetchListComplete()
  {
    this.fetchList = (FetchList2) null;
    this.PlaceDiggables();
    this.ClearMaterialNeeds();
  }

  private void ClearMaterialNeeds()
  {
    if (this.materialNeedsCleared)
      return;
    foreach (Recipe.Ingredient allIngredient in this.Recipe.GetAllIngredients(this.SelectedElementsTags))
      MaterialNeeds.Instance.UpdateNeed(allIngredient.tag, -allIngredient.amount);
    this.materialNeedsCleared = true;
  }

  private void OnSolidChangedOrDigDestroyed(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.finished)
      return;
    this.PlaceDiggables();
  }

  private void UpdateBuildState(Chore chore)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (chore.InProgress())
      component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.UnderConstruction);
    else
      component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.UnderConstructionNoWorker);
  }

  [System.Runtime.Serialization.OnDeserialized]
  internal void OnDeserialized()
  {
    if (this.ids == null)
      return;
    this.selectedElements = new Element[this.ids.Length];
    for (int index = 0; index < this.ids.Length; ++index)
      this.selectedElements[index] = ElementLoader.FindElementByHash((SimHashes) this.ids[index]);
    if (this.selectedElementsTags == null)
    {
      this.selectedElementsTags = new Tag[this.ids.Length];
      for (int index = 0; index < this.ids.Length; ++index)
        this.selectedElementsTags[index] = ElementLoader.FindElementByHash((SimHashes) this.ids[index]).tag;
    }
    Debug.Assert(this.selectedElements.Length == this.selectedElementsTags.Length);
    for (int index = 0; index < this.selectedElements.Length; ++index)
      Debug.Assert(this.selectedElements[index].tag == this.SelectedElementsTags[index]);
  }

  private void OnReachableChanged(object data)
  {
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    if ((bool) data)
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable);
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.TintColour = (Color32) Game.Instance.uiColours.Build.validLocation;
    }
    else
    {
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, (object) this);
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.TintColour = (Color32) Game.Instance.uiColours.Build.unreachable;
    }
  }

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_cancel", (string) UI.USERMENUACTIONS.CANCELCONSTRUCTION.NAME, new System.Action(this.OnPressCancel), tooltipText: ((string) UI.USERMENUACTIONS.CANCELCONSTRUCTION.TOOLTIP)));

  private void OnPressCancel() => this.gameObject.Trigger(2127324410);

  private void OnCancel(object data = null)
  {
    DetailsScreen.Instance.Show(false);
    this.ClearMaterialNeeds();
  }
}
