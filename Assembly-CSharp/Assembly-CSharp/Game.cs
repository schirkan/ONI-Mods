// Decompiled with JetBrains decompiler
// Type: Game
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
  private static readonly string NextUniqueIDKey = "NextUniqueID";
  private PlayerController playerController;
  private CameraController cameraController;
  public System.Action<Game.GameSaveData> OnSave;
  public System.Action<Game.GameSaveData> OnLoad;
  [NonSerialized]
  public bool baseAlreadyCreated;
  [NonSerialized]
  public bool autoPrioritizeRoles;
  [NonSerialized]
  public bool advancedPersonalPriorities;
  public Game.SavedInfo savedInfo;
  public static bool quitting = false;
  public AssignmentManager assignmentManager;
  public GameObject playerPrefab;
  public GameObject screenManagerPrefab;
  public GameObject cameraControllerPrefab;
  public GameObject tempIntroScreenPrefab;
  public static int BlockSelectionLayerMask;
  public static int PickupableLayer;
  public Element VisualTunerElement;
  public float currentSunlightIntensity;
  public RoomProber roomProber;
  public FetchManager fetchManager;
  public EdiblesManager ediblesManager;
  public SpacecraftManager spacecraftManager;
  public UserMenu userMenu;
  public Unlocks unlocks;
  public Timelapser timelapser;
  private bool sandboxModeActive;
  public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);
  public List<int> callbackManagerManuallyReleasedHandles = new List<int>();
  public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);
  public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);
  public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);
  public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);
  [NonSerialized]
  public Player LocalPlayer;
  [SerializeField]
  public TextAsset maleNamesFile;
  [SerializeField]
  public TextAsset femaleNamesFile;
  [NonSerialized]
  public World world;
  [NonSerialized]
  public CircuitManager circuitManager;
  [NonSerialized]
  public EnergySim energySim;
  [NonSerialized]
  public LogicCircuitManager logicCircuitManager;
  private GameScreenManager screenMgr;
  public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;
  public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;
  public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;
  public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;
  public UtilityNetworkTubesManager travelTubeSystem;
  public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;
  public ConduitFlow gasConduitFlow;
  public ConduitFlow liquidConduitFlow;
  public SolidConduitFlow solidConduitFlow;
  public Accumulators accumulators;
  public PlantElementAbsorbers plantElementAbsorbers;
  public Game.TemperatureOverlayModes temperatureOverlayMode;
  public bool showExpandedTemperatures;
  public List<Tag> tileOverlayFilters = new List<Tag>();
  public bool showGasConduitDisease;
  public bool showLiquidConduitDisease;
  public ConduitFlowVisualizer gasFlowVisualizer;
  public ConduitFlowVisualizer liquidFlowVisualizer;
  public SolidConduitFlowVisualizer solidFlowVisualizer;
  public ConduitTemperatureManager conduitTemperatureManager;
  public ConduitDiseaseManager conduitDiseaseManager;
  public MingleCellTracker mingleCellTracker;
  private int simSubTick;
  private bool hasFirstSimTickRun;
  private float simDt;
  [SerializeField]
  public Game.ConduitVisInfo liquidConduitVisInfo;
  [SerializeField]
  public Game.ConduitVisInfo gasConduitVisInfo;
  [SerializeField]
  public Game.ConduitVisInfo solidConduitVisInfo;
  [SerializeField]
  private Material liquidFlowMaterial;
  [SerializeField]
  private Material gasFlowMaterial;
  [SerializeField]
  private Color flowColour;
  private Vector3 gasFlowPos;
  private Vector3 liquidFlowPos;
  private Vector3 solidFlowPos;
  public bool drawStatusItems = true;
  private List<Klei.SolidInfo> solidInfo = new List<Klei.SolidInfo>();
  private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();
  private List<Klei.SolidInfo> gameSolidInfo = new List<Klei.SolidInfo>();
  private bool IsPaused;
  private HashSet<int> solidChangedFilter = new HashSet<int>();
  private HashedString lastDrawnOverlayMode;
  private BuildingCellVisualizer previewVisualizer;
  public SafetyConditions safetyConditions = new SafetyConditions();
  public SimData simData = new SimData();
  [MyCmpGet]
  private GameScenePartitioner gameScenePartitioner;
  private bool gameStarted;
  private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>((System.Action<Game, object>) ((component, data) => component.MarkStatusItemRendererDirty(data)));
  private ushort[] activeFX;
  private Vector2I simActiveRegionMin;
  private Vector2I simActiveRegionMax;
  public bool debugWasUsed;
  [SerializeField]
  private bool forceActiveArea;
  [SerializeField]
  private Vector2 minForcedActiveArea = new Vector2(0.0f, 0.0f);
  [SerializeField]
  private Vector2 maxForcedActiveArea = new Vector2(128f, 128f);
  private bool isLoading;
  private HashedString previousOverlayMode = OverlayModes.None.ID;
  private float previousGasConduitFlowDiscreteLerpPercent = -1f;
  private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;
  private float previousSolidConduitFlowDiscreteLerpPercent = -1f;
  [SerializeField]
  private Game.SpawnPoolData[] fxSpawnData;
  private Dictionary<int, System.Action<Vector3, float>> fxSpawner = new Dictionary<int, System.Action<Vector3, float>>();
  private Dictionary<int, ObjectPool> fxPools = new Dictionary<int, ObjectPool>();
  private Game.SavingPreCB activatePreCB;
  private Game.SavingActiveCB activateActiveCB;
  private Game.SavingPostCB activatePostCB;
  [SerializeField]
  public Game.UIColours uiColours = new Game.UIColours();
  private float lastTimeWorkStarted = float.NegativeInfinity;

  public static bool IsQuitting() => Game.quitting;

  public KInputHandler inputHandler { get; set; }

  public static Game Instance { get; private set; }

  public bool FastWorkersModeActive
  {
    get => CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
    set
    {
      string str = value ? "Enabled" : "Disabled";
      CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, str);
    }
  }

  public bool SandboxModeActive
  {
    get => this.sandboxModeActive;
    set
    {
      this.sandboxModeActive = value;
      this.Trigger(-1948169901, (object) null);
      if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
        PlanScreen.Instance.Refresh();
      if (!((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null))
        return;
      BuildMenu.Instance.Refresh();
    }
  }

  public bool DebugOnlyBuildingsAllowed
  {
    get
    {
      if (!DebugHandler.enabled)
        return false;
      return this.SandboxModeActive || DebugHandler.InstantBuildMode;
    }
  }

  public StatusItemRenderer statusItemRenderer { get; private set; }

  public PrioritizableRenderer prioritizableRenderer { get; private set; }

  protected override void OnPrefabInit()
  {
    DebugUtil.LogArgs((object) Time.realtimeSinceStartup, (object) "Level Loaded....", (object) SceneManager.GetActiveScene().name);
    Components.BuildingCellVisualizers.OnAdd += new System.Action<BuildingCellVisualizer>(this.OnAddBuildingCellVisualizer);
    Components.BuildingCellVisualizers.OnRemove += new System.Action<BuildingCellVisualizer>(this.OnRemoveBuildingCellVisualizer);
    Singleton<KBatchedAnimUpdater>.CreateInstance();
    Singleton<CellChangeMonitor>.CreateInstance();
    this.userMenu = new UserMenu();
    SimTemperatureTransfer.ClearInstanceMap();
    StructureTemperatureComponents.ClearInstanceMap();
    ElementConsumer.ClearInstanceMap();
    App.OnPreLoadScene += new System.Action(this.StopBE);
    Game.Instance = this;
    this.statusItemRenderer = new StatusItemRenderer();
    this.prioritizableRenderer = new PrioritizableRenderer();
    this.LoadEventHashes();
    this.savedInfo.InitializeEmptyVariables();
    this.gasFlowPos = new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
    this.liquidFlowPos = new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
    this.solidFlowPos = new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
    Shader.WarmupAllShaders();
    Db.Get();
    Game.quitting = false;
    Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
    Game.BlockSelectionLayerMask = LayerMask.GetMask("BlockSelection");
    this.world = World.Instance;
    KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
    this.circuitManager = new CircuitManager();
    this.energySim = new EnergySim();
    this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
    this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
    this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
    this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
    this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
    this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
    this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
    this.conduitTemperatureManager = new ConduitTemperatureManager();
    this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
    this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, (IUtilityNetworkMgr) this.gasConduitSystem, 1f, 0.25f);
    this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, (IUtilityNetworkMgr) this.liquidConduitSystem, 10f, 0.75f);
    this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, (IUtilityNetworkMgr) this.solidConduitSystem, 0.75f);
    this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
    this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
    this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
    this.accumulators = new Accumulators();
    this.plantElementAbsorbers = new PlantElementAbsorbers();
    this.activeFX = new ushort[Grid.CellCount];
    this.simActiveRegionMax = new Vector2I(0, 0);
    this.simActiveRegionMin = new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1);
    this.UnsafePrefabInit();
    Shader.SetGlobalVector("_MetalParameters", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
    Shader.SetGlobalVector("_WaterParameters", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
    this.InitializeFXSpawners();
    PathFinder.Initialize();
    GameNavGrids gameNavGrids = new GameNavGrids(Pathfinding.Instance);
    this.screenMgr = Util.KInstantiate(this.screenManagerPrefab).GetComponent<GameScreenManager>();
    this.roomProber = new RoomProber();
    this.fetchManager = this.gameObject.AddComponent<FetchManager>();
    this.ediblesManager = this.gameObject.AddComponent<EdiblesManager>();
    Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
    this.unlocks = this.GetComponent<Unlocks>();
  }

  public void SetGameStarted() => this.gameStarted = true;

  public bool GameStarted() => this.gameStarted;

  private unsafe void UnsafePrefabInit() => this.StepTheSim(0.0f);

  protected override void OnLoadLevel()
  {
    this.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
    base.OnLoadLevel();
  }

  private void MarkStatusItemRendererDirty(object data) => this.statusItemRenderer.MarkAllDirty();

  protected override void OnForcedCleanUp()
  {
    if (this.prioritizableRenderer != null)
    {
      this.prioritizableRenderer.Cleanup();
      this.prioritizableRenderer = (PrioritizableRenderer) null;
    }
    if (this.statusItemRenderer != null)
    {
      this.statusItemRenderer.Destroy();
      this.statusItemRenderer = (StatusItemRenderer) null;
    }
    if (this.conduitTemperatureManager != null)
      this.conduitTemperatureManager.Shutdown();
    this.gasFlowVisualizer.FreeResources();
    this.liquidFlowVisualizer.FreeResources();
    this.solidFlowVisualizer.FreeResources();
    LightGridManager.Shutdown();
    RadiationGridManager.Shutdown();
    App.OnPreLoadScene -= new System.Action(this.StopBE);
    base.OnForcedCleanUp();
  }

  protected override void OnSpawn()
  {
    Debug.Log((object) "-- GAME --");
    PropertyTextures.FogOfWarScale = 0.0f;
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      CameraController.Instance.EnableFreeCamera(false);
    this.LocalPlayer = this.SpawnPlayer();
    WaterCubes.Instance.Init();
    SpeedControlScreen.Instance.Pause(false);
    LightGridManager.Initialise();
    RadiationGridManager.Initialise();
    this.UnsafeOnSpawn();
    Time.timeScale = 0.0f;
    if ((UnityEngine.Object) this.tempIntroScreenPrefab != (UnityEngine.Object) null)
      Util.KInstantiate(this.tempIntroScreenPrefab);
    if (SaveLoader.Instance.cachedGSD != null)
    {
      this.Reset(SaveLoader.Instance.cachedGSD);
      NewBaseScreen.SetInitialCamera();
    }
    TagManager.FillMissingProperNames();
    CameraController.Instance.SetOrthographicsSize(20f);
    if (SaveLoader.Instance.loadedFromSave)
    {
      this.baseAlreadyCreated = true;
      this.Trigger(-1992507039, (object) null);
      this.Trigger(-838649377, (object) null);
    }
    this.LocalPlayer.ScreenManager.StartScreen(ScreenPrefabs.Instance.ResourceCategoryScreen.gameObject).transform.SetSiblingIndex(1);
    foreach (Renderer renderer in Resources.FindObjectsOfTypeAll(typeof (MeshRenderer)))
      renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    this.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
    this.solidConduitFlow.Initialize();
    SimAndRenderScheduler.instance.Add((object) this.roomProber);
    SimAndRenderScheduler.instance.Add((object) KComponentSpawn.instance);
    SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
    SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
    if (!SaveLoader.Instance.loadedFromSave)
    {
      SettingConfig qualitySetting = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
      SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
      SaveGame.Instance.sandboxEnabled = !qualitySetting.IsDefaultLevel(currentQualitySetting.id);
    }
    this.mingleCellTracker = this.gameObject.AddComponent<MingleCellTracker>();
    if (!((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null))
      return;
    Global.Instance.GetComponent<PerformanceMonitor>().Reset();
    Global.Instance.modManager.NotifyDialog((string) UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    SimAndRenderScheduler.instance.Remove((object) KComponentSpawn.instance);
    SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>((UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate) null);
    SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>((UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate) null);
    this.DestroyInstances();
  }

  private new void OnDestroy()
  {
    base.OnDestroy();
    this.DestroyInstances();
  }

  private unsafe void UnsafeOnSpawn() => this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, (Sim.SolidSubstanceChangeInfo*) null, 0, (Sim.LiquidChangeInfo*) null);

  public void SetMusicEnabled(bool enabled)
  {
    if (enabled)
      MusicManager.instance.PlaySong("Music_FrontEnd");
    else
      MusicManager.instance.StopSong("Music_FrontEnd");
  }

  private Player SpawnPlayer()
  {
    Player component = Util.KInstantiate(this.playerPrefab, this.gameObject).GetComponent<Player>();
    component.ScreenManager = this.screenMgr;
    component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject);
    component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, target: GameScreenManager.UIRenderTarget.HoverTextScreen);
    component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, target: GameScreenManager.UIRenderTarget.HoverTextScreen);
    this.cameraController = Util.KInstantiate(this.cameraControllerPrefab).GetComponent<CameraController>();
    component.CameraController = this.cameraController;
    KInputHandler.Add((IInputHandler) Global.Instance.GetInputManager().GetDefaultController(), (IInputHandler) this.cameraController, 1);
    this.playerController = component.GetComponent<PlayerController>();
    KInputHandler.Add((IInputHandler) Global.Instance.GetInputManager().GetDefaultController(), (IInputHandler) this.playerController, 20);
    return component;
  }

  public void SetDupePassableSolid(int cell, bool passable, bool solid)
  {
    Grid.DupePassable[cell] = passable;
    this.gameSolidInfo.Add(new Klei.SolidInfo(cell, solid));
  }

  private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
  {
    using (new KProfiler.Region(nameof (StepTheSim)))
    {
      IntPtr num1 = IntPtr.Zero;
      using (new KProfiler.Region("WaitingForSim"))
      {
        if (Grid.Visible == null || Grid.Visible.Length == 0)
        {
          Debug.LogError((object) "Invalid Grid.Visible, what have you done?!");
          return (Sim.GameDataUpdate*) null;
        }
        num1 = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
      }
      if (num1 == IntPtr.Zero)
        return (Sim.GameDataUpdate*) null;
      Sim.GameDataUpdate* gameDataUpdatePtr = (Sim.GameDataUpdate*) (void*) num1;
      Grid.elementIdx = gameDataUpdatePtr->elementIdx;
      Grid.temperature = gameDataUpdatePtr->temperature;
      Grid.mass = gameDataUpdatePtr->mass;
      Grid.properties = gameDataUpdatePtr->properties;
      Grid.strengthInfo = gameDataUpdatePtr->strengthInfo;
      Grid.insulation = gameDataUpdatePtr->insulation;
      Grid.diseaseIdx = gameDataUpdatePtr->diseaseIdx;
      Grid.diseaseCount = gameDataUpdatePtr->diseaseCount;
      Grid.AccumulatedFlowValues = gameDataUpdatePtr->accumulatedFlow;
      Grid.exposedToSunlight = (byte*) (void*) gameDataUpdatePtr->propertyTextureExposedToSunlight;
      PropertyTextures.externalFlowTex = gameDataUpdatePtr->propertyTextureFlow;
      PropertyTextures.externalLiquidTex = gameDataUpdatePtr->propertyTextureLiquid;
      PropertyTextures.externalExposedToSunlight = gameDataUpdatePtr->propertyTextureExposedToSunlight;
      List<Element> elements = ElementLoader.elements;
      this.simData.emittedMassEntries = gameDataUpdatePtr->emittedMassEntries;
      this.simData.elementChunks = gameDataUpdatePtr->elementChunkInfos;
      this.simData.buildingTemperatures = gameDataUpdatePtr->buildingTemperatures;
      this.simData.diseaseEmittedInfos = gameDataUpdatePtr->diseaseEmittedInfos;
      this.simData.diseaseConsumedInfos = gameDataUpdatePtr->diseaseConsumedInfos;
      for (int index = 0; index < gameDataUpdatePtr->numSubstanceChangeInfo; ++index)
      {
        Sim.SubstanceChangeInfo substanceChangeInfo = gameDataUpdatePtr->substanceChangeInfo[index];
        Element element = elements[(int) substanceChangeInfo.newElemIdx];
        Grid.Element[substanceChangeInfo.cellIdx] = element;
      }
      for (int index = 0; index < gameDataUpdatePtr->numSolidInfo; ++index)
      {
        Sim.SolidInfo solidInfo = gameDataUpdatePtr->solidInfo[index];
        if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
        {
          this.solidInfo.Add(new Klei.SolidInfo(solidInfo.cellIdx, (uint) solidInfo.isSolid > 0U));
          bool solid = (uint) solidInfo.isSolid > 0U;
          Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
        }
      }
      for (int index = 0; index < gameDataUpdatePtr->numCallbackInfo; ++index)
      {
        Sim.CallbackInfo callbackInfo = gameDataUpdatePtr->callbackInfo[index];
        HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle()
        {
          index = callbackInfo.callbackIdx
        };
        if (!this.IsManuallyReleasedHandle(handle))
          this.callbackInfo.Add(new Klei.CallbackInfo(handle));
      }
      int fallingLiquidInfo1 = gameDataUpdatePtr->numSpawnFallingLiquidInfo;
      for (int index = 0; index < fallingLiquidInfo1; ++index)
      {
        Sim.SpawnFallingLiquidInfo fallingLiquidInfo2 = gameDataUpdatePtr->spawnFallingLiquidInfo[index];
        FallingWater.instance.AddParticle(fallingLiquidInfo2.cellIdx, fallingLiquidInfo2.elemIdx, fallingLiquidInfo2.mass, fallingLiquidInfo2.temperature, fallingLiquidInfo2.diseaseIdx, fallingLiquidInfo2.diseaseCount);
      }
      int numDigInfo = gameDataUpdatePtr->numDigInfo;
      WorldDamage component1 = this.world.GetComponent<WorldDamage>();
      for (int index = 0; index < numDigInfo; ++index)
      {
        Sim.SpawnOreInfo spawnOreInfo = gameDataUpdatePtr->digInfo[index];
        if ((double) spawnOreInfo.temperature <= 0.0 && (double) spawnOreInfo.mass > 0.0)
          Debug.LogError((object) "Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
        component1.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
      }
      int numSpawnOreInfo = gameDataUpdatePtr->numSpawnOreInfo;
      for (int index = 0; index < numSpawnOreInfo; ++index)
      {
        Sim.SpawnOreInfo spawnOreInfo = gameDataUpdatePtr->spawnOreInfo[index];
        Vector3 posCcc = Grid.CellToPosCCC(spawnOreInfo.cellIdx, Grid.SceneLayer.Ore);
        Element element = ElementLoader.elements[(int) spawnOreInfo.elemIdx];
        if ((double) spawnOreInfo.temperature <= 0.0 && (double) spawnOreInfo.mass > 0.0)
          Debug.LogError((object) "Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
        element.substance.SpawnResource(posCcc, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
      }
      int numSpawnFxInfo = gameDataUpdatePtr->numSpawnFXInfo;
      for (int index = 0; index < numSpawnFxInfo; ++index)
      {
        Sim.SpawnFXInfo spawnFxInfo = gameDataUpdatePtr->spawnFXInfo[index];
        this.SpawnFX((SpawnFXHashes) spawnFxInfo.fxHash, spawnFxInfo.cellIdx, spawnFxInfo.rotation);
      }
      UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
      int unstableCellInfo1 = gameDataUpdatePtr->numUnstableCellInfo;
      for (int index = 0; index < unstableCellInfo1; ++index)
      {
        Sim.UnstableCellInfo unstableCellInfo2 = gameDataUpdatePtr->unstableCellInfo[index];
        if (unstableCellInfo2.fallingInfo == (byte) 0)
          component2.Spawn(unstableCellInfo2.cellIdx, ElementLoader.elements[(int) unstableCellInfo2.elemIdx], unstableCellInfo2.mass, unstableCellInfo2.temperature, unstableCellInfo2.diseaseIdx, unstableCellInfo2.diseaseCount);
      }
      int numWorldDamageInfo = gameDataUpdatePtr->numWorldDamageInfo;
      for (int index = 0; index < numWorldDamageInfo; ++index)
      {
        double num2 = (double) WorldDamage.Instance.ApplyDamage(gameDataUpdatePtr->worldDamageInfo[index]);
      }
      for (int index = 0; index < gameDataUpdatePtr->numRemovedMassEntries; ++index)
        ElementConsumer.AddMass(gameDataUpdatePtr->removedMassEntries[index]);
      int consumedCallbacks = gameDataUpdatePtr->numMassConsumedCallbacks;
      HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle1 = new HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle();
      for (int index = 0; index < consumedCallbacks; ++index)
      {
        Sim.MassConsumedCallback consumedCallback = gameDataUpdatePtr->massConsumedCallbacks[index];
        handle1.index = consumedCallback.callbackIdx;
        Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle1, "massConsumedCB");
        if (complexCallbackInfo.cb != null)
          complexCallbackInfo.cb(consumedCallback, complexCallbackInfo.callbackData);
      }
      int emittedCallbacks = gameDataUpdatePtr->numMassEmittedCallbacks;
      HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle2 = new HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle();
      for (int index = 0; index < emittedCallbacks; ++index)
      {
        Sim.MassEmittedCallback massEmittedCallback = gameDataUpdatePtr->massEmittedCallbacks[index];
        handle2.index = massEmittedCallback.callbackIdx;
        if (this.massEmitCallbackManager.IsVersionValid(handle2))
        {
          Game.ComplexCallbackInfo<Sim.MassEmittedCallback> complexCallbackInfo = this.massEmitCallbackManager.GetItem(handle2);
          if (complexCallbackInfo.cb != null)
            complexCallbackInfo.cb(massEmittedCallback, complexCallbackInfo.callbackData);
        }
      }
      int consumptionCallbacks = gameDataUpdatePtr->numDiseaseConsumptionCallbacks;
      HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle3 = new HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle();
      for (int index = 0; index < consumptionCallbacks; ++index)
      {
        Sim.DiseaseConsumptionCallback consumptionCallback = gameDataUpdatePtr->diseaseConsumptionCallbacks[index];
        handle3.index = consumptionCallback.callbackIdx;
        if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle3))
        {
          Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> complexCallbackInfo = this.diseaseConsumptionCallbackManager.GetItem(handle3);
          if (complexCallbackInfo.cb != null)
            complexCallbackInfo.cb(consumptionCallback, complexCallbackInfo.callbackData);
        }
      }
      int stateChangedMessages = gameDataUpdatePtr->numComponentStateChangedMessages;
      HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle4 = new HandleVector<Game.ComplexCallbackInfo<int>>.Handle();
      for (int index = 0; index < stateChangedMessages; ++index)
      {
        Sim.ComponentStateChangedMessage stateChangedMessage = gameDataUpdatePtr->componentStateChangedMessages[index];
        handle4.index = stateChangedMessage.callbackIdx;
        if (this.simComponentCallbackManager.IsVersionValid(handle4))
        {
          Game.ComplexCallbackInfo<int> complexCallbackInfo = this.simComponentCallbackManager.Release(handle4, "component state changed cb");
          if (complexCallbackInfo.cb != null)
            complexCallbackInfo.cb(stateChangedMessage.simHandle, complexCallbackInfo.callbackData);
        }
      }
      int chunkMeltedInfos = gameDataUpdatePtr->numElementChunkMeltedInfos;
      for (int index = 0; index < chunkMeltedInfos; ++index)
        SimTemperatureTransfer.DoStateTransition(gameDataUpdatePtr->elementChunkMeltedInfos[index].handle);
      int buildingOverheatInfos = gameDataUpdatePtr->numBuildingOverheatInfos;
      for (int index = 0; index < buildingOverheatInfos; ++index)
        StructureTemperatureComponents.DoOverheat(gameDataUpdatePtr->buildingOverheatInfos[index].handle);
      int longerOverheatedInfos = gameDataUpdatePtr->numBuildingNoLongerOverheatedInfos;
      for (int index = 0; index < longerOverheatedInfos; ++index)
        StructureTemperatureComponents.DoNoLongerOverheated(gameDataUpdatePtr->buildingNoLongerOverheatedInfos[index].handle);
      int buildingMeltedInfos = gameDataUpdatePtr->numBuildingMeltedInfos;
      for (int index = 0; index < buildingMeltedInfos; ++index)
        StructureTemperatureComponents.DoStateTransition(gameDataUpdatePtr->buildingMeltedInfos[index].handle);
      int numCellMeltedInfos = gameDataUpdatePtr->numCellMeltedInfos;
      for (int index = 0; index < numCellMeltedInfos; ++index)
      {
        int gameCell = gameDataUpdatePtr->cellMeltedInfos[index].gameCell;
        GameObject original = Grid.Objects[gameCell, 9];
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
      if ((double) dt > 0.0)
      {
        this.conduitTemperatureManager.Sim200ms(0.2f);
        this.conduitDiseaseManager.Sim200ms(0.2f);
        this.gasConduitFlow.Sim200ms(0.2f);
        this.liquidConduitFlow.Sim200ms(0.2f);
        this.solidConduitFlow.Sim200ms(0.2f);
        this.accumulators.Sim200ms(0.2f);
        this.plantElementAbsorbers.Sim200ms(0.2f);
      }
      Sim.DebugProperties properties;
      properties.buildingTemperatureScale = 100f;
      properties.contaminatedOxygenEmitProbability = 1f / 1000f;
      properties.contaminatedOxygenConversionPercent = 1f / 1000f;
      properties.biomeTemperatureLerpRate = 1f / 1000f;
      properties.isDebugEditing = !((UnityEngine.Object) DebugPaintElementScreen.Instance != (UnityEngine.Object) null) || !DebugPaintElementScreen.Instance.gameObject.activeSelf ? (byte) 0 : (byte) 1;
      properties.pad0 = properties.pad1 = properties.pad2 = (byte) 0;
      SimMessages.SetDebugProperties(properties);
      if ((double) dt > 0.0)
      {
        if (this.circuitManager != null)
          this.circuitManager.Sim200msFirst(dt);
        if (this.energySim != null)
          this.energySim.EnergySim200ms(dt);
        if (this.circuitManager != null)
          this.circuitManager.Sim200msLast(dt);
      }
      return gameDataUpdatePtr;
    }
  }

  public void AddSolidChangedFilter(int cell) => this.solidChangedFilter.Add(cell);

  public void RemoveSolidChangedFilter(int cell) => this.solidChangedFilter.Remove(cell);

  public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
  {
    this.simActiveRegionMin.x = Mathf.Max(0, Mathf.Min(x0, this.simActiveRegionMin.x));
    this.simActiveRegionMin.y = Mathf.Max(0, Mathf.Min(y0, this.simActiveRegionMin.y));
    this.simActiveRegionMax.x = Mathf.Min(Grid.WidthInCells - 1, Mathf.Max(x1, this.simActiveRegionMax.x));
    this.simActiveRegionMax.y = Mathf.Min(Grid.HeightInCells - 1, Mathf.Max(y1, this.simActiveRegionMax.y));
  }

  public void SetIsLoading() => this.isLoading = true;

  public bool IsLoading() => this.isLoading;

  private void ShowDebugCellInfo()
  {
    int mouseCell = DebugHandler.GetMouseCell();
    int x = 0;
    int y = 0;
    Grid.CellToXY(mouseCell, out x, out y);
    string text = mouseCell.ToString() + " (" + (object) x + ", " + (object) y + ")";
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }

  public void ForceSimStep()
  {
    DebugUtil.LogArgs((object) "Force-stepping the sim");
    this.simDt = 0.2f;
  }

  private void Update()
  {
    if (this.isLoading)
      return;
    float deltaTime = Time.deltaTime;
    if (Debug.developerConsoleVisible)
      Debug.developerConsoleVisible = false;
    if (DebugHandler.DebugCellInfo)
      this.ShowDebugCellInfo();
    this.gasConduitSystem.Update();
    this.liquidConduitSystem.Update();
    this.solidConduitSystem.Update();
    this.circuitManager.RenderEveryTick(deltaTime);
    this.logicCircuitManager.RenderEveryTick(deltaTime);
    this.solidConduitFlow.RenderEveryTick(deltaTime);
    if (this.forceActiveArea)
    {
      this.simActiveRegionMin = new Vector2I((int) Mathf.Max(0.0f, this.minForcedActiveArea.x), (int) Mathf.Max(0.0f, this.minForcedActiveArea.y));
      this.simActiveRegionMax = new Vector2I((int) Mathf.Min((float) (Grid.WidthInCells - 1), this.maxForcedActiveArea.x), (int) Mathf.Min((float) (Grid.HeightInCells - 1), this.maxForcedActiveArea.y));
    }
    this.simActiveRegionMin = new Vector2I(0, 0);
    this.simActiveRegionMax = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
    Pathfinding.Instance.RenderEveryTick();
    Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
    this.SimEveryTick(deltaTime);
  }

  private void SimEveryTick(float dt)
  {
    dt = Mathf.Min(dt, 0.2f);
    this.simDt += dt;
    if ((double) this.simDt >= 0.0166666675359011)
    {
      do
      {
        ++this.simSubTick;
        this.simSubTick %= 12;
        if (this.simSubTick == 0)
        {
          this.hasFirstSimTickRun = true;
          this.UnsafeSim200ms(0.2f);
        }
        if (this.hasFirstSimTickRun)
          Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
        this.simDt -= 0.01666667f;
      }
      while ((double) this.simDt >= 0.0166666675359011);
    }
    else
      this.UnsafeSim200ms(0.0f);
  }

  private unsafe void UnsafeSim200ms(float dt)
  {
    SimMessages.NewGameFrame(dt, this.simActiveRegionMin, this.simActiveRegionMax);
    Sim.GameDataUpdate* gameDataUpdatePtr = this.StepTheSim(dt);
    if ((IntPtr) gameDataUpdatePtr == IntPtr.Zero)
    {
      Debug.LogError((object) "UNEXPECTED!");
    }
    else
    {
      if (gameDataUpdatePtr->numFramesProcessed <= 0)
        return;
      this.gameSolidInfo.AddRange((IEnumerable<Klei.SolidInfo>) this.solidInfo);
      this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, gameDataUpdatePtr->numSolidSubstanceChangeInfo, gameDataUpdatePtr->solidSubstanceChangeInfo, gameDataUpdatePtr->numLiquidChangeInfo, gameDataUpdatePtr->liquidChangeInfo);
      this.gameSolidInfo.Clear();
      this.solidInfo.Clear();
      this.callbackInfo.Clear();
      this.callbackManagerManuallyReleasedHandles.Clear();
      Pathfinding.Instance.UpdateNavGrids();
    }
  }

  private void LateUpdateComponents() => this.UpdateOverlayScreen();

  private void OnAddBuildingCellVisualizer(BuildingCellVisualizer building_cell_visualizer)
  {
    this.lastDrawnOverlayMode = new HashedString();
    if (!((UnityEngine.Object) PlayerController.Instance != (UnityEngine.Object) null))
      return;
    BuildTool activeTool = PlayerController.Instance.ActiveTool as BuildTool;
    if (!((UnityEngine.Object) activeTool != (UnityEngine.Object) null) || !((UnityEngine.Object) activeTool.visualizer == (UnityEngine.Object) building_cell_visualizer.gameObject))
      return;
    this.previewVisualizer = building_cell_visualizer;
  }

  private void OnRemoveBuildingCellVisualizer(BuildingCellVisualizer building_cell_visualizer)
  {
    if (!((UnityEngine.Object) this.previewVisualizer == (UnityEngine.Object) building_cell_visualizer))
      return;
    this.previewVisualizer = (BuildingCellVisualizer) null;
  }

  private void UpdateOverlayScreen()
  {
    if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null)
      return;
    HashedString mode = OverlayScreen.Instance.GetMode();
    if ((UnityEngine.Object) this.previewVisualizer != (UnityEngine.Object) null)
    {
      this.previewVisualizer.DisableIcons();
      this.previewVisualizer.DrawIcons(mode);
    }
    if (mode == this.lastDrawnOverlayMode)
      return;
    foreach (BuildingCellVisualizer buildingCellVisualizer in Components.BuildingCellVisualizers.Items)
    {
      buildingCellVisualizer.DisableIcons();
      buildingCellVisualizer.DrawIcons(mode);
    }
    this.lastDrawnOverlayMode = mode;
  }

  public void ForceOverlayUpdate() => this.previousOverlayMode = OverlayModes.None.ID;

  private void LateUpdate()
  {
    if ((double) Time.timeScale == 0.0 && !this.IsPaused)
    {
      this.IsPaused = true;
      this.Trigger(-1788536802, (object) this.IsPaused);
    }
    else if ((double) Time.timeScale != 0.0 && this.IsPaused)
    {
      this.IsPaused = false;
      this.Trigger(-1788536802, (object) this.IsPaused);
    }
    if (Input.GetMouseButton(0))
    {
      this.VisualTunerElement = (Element) null;
      int mouseCell = DebugHandler.GetMouseCell();
      if (Grid.IsValidCell(mouseCell))
        this.VisualTunerElement = Grid.Element[mouseCell];
    }
    this.gasConduitSystem.Update();
    this.liquidConduitSystem.Update();
    this.solidConduitSystem.Update();
    HashedString mode = SimDebugView.Instance.GetMode();
    if (mode != this.previousOverlayMode)
    {
      this.previousOverlayMode = mode;
      if (mode == OverlayModes.LiquidConduits.ID)
      {
        this.liquidFlowVisualizer.ColourizePipeContents(true, true);
        this.gasFlowVisualizer.ColourizePipeContents(false, true);
        this.solidFlowVisualizer.ColourizePipeContents(false, true);
      }
      else if (mode == OverlayModes.GasConduits.ID)
      {
        this.liquidFlowVisualizer.ColourizePipeContents(false, true);
        this.gasFlowVisualizer.ColourizePipeContents(true, true);
        this.solidFlowVisualizer.ColourizePipeContents(false, true);
      }
      else if (mode == OverlayModes.SolidConveyor.ID)
      {
        this.liquidFlowVisualizer.ColourizePipeContents(false, true);
        this.gasFlowVisualizer.ColourizePipeContents(false, true);
        this.solidFlowVisualizer.ColourizePipeContents(true, true);
      }
      else
      {
        this.liquidFlowVisualizer.ColourizePipeContents(false, false);
        this.gasFlowVisualizer.ColourizePipeContents(false, false);
        this.solidFlowVisualizer.ColourizePipeContents(false, false);
      }
    }
    this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && (double) this.gasConduitFlow.DiscreteLerpPercent != (double) this.previousGasConduitFlowDiscreteLerpPercent);
    this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && (double) this.liquidConduitFlow.DiscreteLerpPercent != (double) this.previousLiquidConduitFlowDiscreteLerpPercent);
    this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && (double) this.solidConduitFlow.DiscreteLerpPercent != (double) this.previousSolidConduitFlowDiscreteLerpPercent);
    this.previousGasConduitFlowDiscreteLerpPercent = mode == OverlayModes.GasConduits.ID ? this.gasConduitFlow.DiscreteLerpPercent : -1f;
    this.previousLiquidConduitFlowDiscreteLerpPercent = mode == OverlayModes.LiquidConduits.ID ? this.liquidConduitFlow.DiscreteLerpPercent : -1f;
    this.previousSolidConduitFlowDiscreteLerpPercent = mode == OverlayModes.SolidConveyor.ID ? this.solidConduitFlow.DiscreteLerpPercent : -1f;
    Vector3 worldPoint1 = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
    Vector3 worldPoint2 = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
    Shader.SetGlobalVector("_WsToCs", new Vector4(worldPoint1.x / (float) Grid.WidthInCells, worldPoint1.y / (float) Grid.HeightInCells, (worldPoint2.x - worldPoint1.x) / (float) Grid.WidthInCells, (worldPoint2.y - worldPoint1.y) / (float) Grid.HeightInCells));
    if (this.drawStatusItems)
    {
      this.statusItemRenderer.RenderEveryTick();
      this.prioritizableRenderer.RenderEveryTick();
    }
    this.LateUpdateComponents();
    Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
    Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
    {
      Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.DrawPath();
    }
    KFMOD.RenderEveryTick(Time.deltaTime);
    if ((double) GenericGameSettings.instance.performanceCapture.waitTime == 0.0)
      return;
    this.UpdatePerformanceCapture();
  }

  private void UpdatePerformanceCapture()
  {
    if (this.IsPaused && (UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Unpause();
    if ((double) Time.timeSinceLevelLoad < (double) GenericGameSettings.instance.performanceCapture.waitTime)
      return;
    uint num1 = 420700;
    string shortDateString = System.DateTime.Now.ToShortDateString();
    string shortTimeString = System.DateTime.Now.ToShortTimeString();
    string fileName = System.IO.Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
    string str1 = "Version,Date,Time,SaveGame";
    string str2 = string.Format("{0},{1},{2},{3}", (object) num1, (object) shortDateString, (object) shortTimeString, (object) fileName);
    float num2 = 0.1f;
    if (GenericGameSettings.instance.performanceCapture.gcStats)
    {
      Debug.Log((object) "Begin GC profiling...");
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      GC.Collect();
      num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
      Debug.Log((object) ("\tGC.Collect() took " + num2.ToString() + " seconds"));
      MemorySnapshot memorySnapshot = new MemorySnapshot();
      string format = "{0},{1},{2},{3}";
      string path = "./memory/GCTypeMetrics.csv";
      if (!File.Exists(path))
      {
        using (StreamWriter streamWriter = new StreamWriter(path))
          streamWriter.WriteLine(string.Format(format, (object) str1, (object) "Type", (object) "Instances", (object) "References"));
      }
      using (StreamWriter streamWriter = new StreamWriter(path, true))
      {
        foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
          streamWriter.WriteLine(string.Format(format, (object) str2, (object) ("\"" + typeData.type.ToString() + "\""), (object) typeData.instanceCount, (object) typeData.refCount));
      }
      Debug.Log((object) "...end GC profiling");
    }
    float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
    Directory.CreateDirectory("./memory");
    string format1 = "{0},{1},{2}";
    string path1 = "./memory/GeneralMetrics.csv";
    if (!File.Exists(path1))
    {
      using (StreamWriter streamWriter = new StreamWriter(path1))
        streamWriter.WriteLine(string.Format(format1, (object) str1, (object) "GCDuration", (object) "FPS"));
    }
    using (StreamWriter streamWriter = new StreamWriter(path1, true))
      streamWriter.WriteLine(string.Format(format1, (object) str2, (object) num2, (object) fps));
    GenericGameSettings.instance.performanceCapture.waitTime = 0.0f;
    App.Quit();
  }

  public void Reset(GameSpawnData gsd)
  {
    using (new KProfiler.Region("World.Reset"))
    {
      if (gsd == null)
        return;
      foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
      {
        if (keyValuePair.Value)
          Grid.PreventFogOfWarReveal[Grid.PosToCell((Vector2) keyValuePair.Key)] = keyValuePair.Value;
      }
    }
  }

  private void OnApplicationQuit()
  {
    Game.quitting = true;
    Sim.Shutdown();
    AudioMixer.Destroy();
    if ((UnityEngine.Object) this.screenMgr != (UnityEngine.Object) null && (UnityEngine.Object) this.screenMgr.gameObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.screenMgr.gameObject);
    Console.WriteLine("Game.OnApplicationQuit()");
  }

  private void InitializeFXSpawners()
  {
    for (int index1 = 0; index1 < this.fxSpawnData.Length; ++index1)
    {
      int fx_idx = index1;
      this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
      ushort fx_mask = (ushort) (1 << fx_idx);
      System.Action<SpawnFXHashes, GameObject> destroyer = (System.Action<SpawnFXHashes, GameObject>) ((fxid, go) =>
      {
        if (Game.IsQuitting())
          return;
        this.activeFX[Grid.PosToCell(go)] &= ~fx_mask;
        go.GetComponent<KAnimControllerBase>().enabled = false;
        this.fxPools[(int) fxid].ReleaseInstance(go);
      });
      ObjectPool pool = new ObjectPool((Func<GameObject>) (() =>
      {
        GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front);
        KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
        component.enabled = false;
        gameObject.SetActive(true);
        component.onDestroySelf = (System.Action<GameObject>) (go => destroyer(this.fxSpawnData[fx_idx].id, go));
        return gameObject;
      }), this.fxSpawnData[fx_idx].initialCount);
      this.fxPools[(int) this.fxSpawnData[fx_idx].id] = pool;
      this.fxSpawner[(int) this.fxSpawnData[fx_idx].id] = (System.Action<Vector3, float>) ((pos, rotation) => GameScheduler.Instance.Schedule("SpawnFX", 0.0f, (System.Action<object>) (obj =>
      {
        int cell = Grid.PosToCell(pos);
        if (((int) this.activeFX[cell] & (int) fx_mask) != 0)
          return;
        this.activeFX[cell] |= fx_mask;
        GameObject instance = pool.GetInstance();
        Game.SpawnPoolData spawnPoolData = this.fxSpawnData[fx_idx];
        Quaternion quaternion = Quaternion.identity;
        bool flag = false;
        string str = spawnPoolData.initialAnim;
        switch (spawnPoolData.rotationConfig)
        {
          case Game.SpawnRotationConfig.Normal:
            quaternion = Quaternion.Euler(0.0f, 0.0f, rotation);
            break;
          case Game.SpawnRotationConfig.StringName:
            int index = (int) ((double) rotation / 90.0);
            if (index < 0)
              index += spawnPoolData.rotationData.Length;
            str = spawnPoolData.rotationData[index].animName;
            flag = spawnPoolData.rotationData[index].flip;
            break;
        }
        pos += spawnPoolData.spawnOffset;
        Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
        insideUnitCircle.x *= spawnPoolData.spawnRandomOffset.x;
        insideUnitCircle.y *= spawnPoolData.spawnRandomOffset.y;
        Vector2 vector2 = (Vector2) (quaternion * (Vector3) insideUnitCircle);
        pos.x += vector2.x;
        pos.y += vector2.y;
        instance.transform.SetPosition(pos);
        instance.transform.rotation = quaternion;
        KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
        component.FlipX = flag;
        component.TintColour = spawnPoolData.colour;
        component.Play((HashedString) str);
        component.enabled = true;
      }), (object) null, (SchedulerGroup) null));
    }
  }

  public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
  {
    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
    if (!CameraController.Instance.IsVisiblePos(posCbc))
      return;
    this.fxSpawner[(int) fx_id](posCbc, rotation);
  }

  public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation) => this.fxSpawner[(int) fx_id](pos, rotation);

  public static void SaveSettings(BinaryWriter writer) => Serializer.Serialize((object) new Game.Settings(Game.Instance), writer);

  public static void LoadSettings(Deserializer deserializer)
  {
    Game.Settings settings = new Game.Settings();
    deserializer.Deserialize((object) settings);
    KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
    KleiMetrics.SetGameID(settings.gameID);
  }

  public void Save(BinaryWriter writer)
  {
    Game.GameSaveData gameSaveData = new Game.GameSaveData();
    gameSaveData.gasConduitFlow = this.gasConduitFlow;
    gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
    gameSaveData.simActiveRegionMin = this.simActiveRegionMin;
    gameSaveData.simActiveRegionMax = this.simActiveRegionMax;
    gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
    gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
    gameSaveData.worldDetail = SaveLoader.Instance.worldDetailSave;
    gameSaveData.debugWasUsed = this.debugWasUsed;
    gameSaveData.customGameSettings = CustomGameSettings.Instance;
    gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
    gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
    gameSaveData.savedInfo = this.savedInfo;
    Debug.Assert(gameSaveData.worldDetail != null, (object) "World detail null");
    if (this.OnSave != null)
      this.OnSave(gameSaveData);
    Serializer.Serialize((object) gameSaveData, writer);
  }

  public void Load(Deserializer deserializer)
  {
    Game.GameSaveData gameSaveData = new Game.GameSaveData();
    gameSaveData.gasConduitFlow = this.gasConduitFlow;
    gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
    gameSaveData.simActiveRegionMin = new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1);
    gameSaveData.simActiveRegionMax = new Vector2I(0, 0);
    gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
    gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
    gameSaveData.worldDetail = new WorldDetailSave();
    gameSaveData.customGameSettings = CustomGameSettings.Instance;
    deserializer.Deserialize((object) gameSaveData);
    this.gasConduitFlow = gameSaveData.gasConduitFlow;
    this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
    this.simActiveRegionMin = gameSaveData.simActiveRegionMin;
    this.simActiveRegionMax = gameSaveData.simActiveRegionMax;
    this.debugWasUsed = gameSaveData.debugWasUsed;
    this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
    this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
    this.savedInfo = gameSaveData.savedInfo;
    this.savedInfo.InitializeEmptyVariables();
    CustomGameSettings.Instance.Print();
    KCrashReporter.debugWasUsed = this.debugWasUsed;
    SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
    if (this.OnLoad == null)
      return;
    this.OnLoad(gameSaveData);
  }

  public void SetAutoSaveCallbacks(
    Game.SavingPreCB activatePreCB,
    Game.SavingActiveCB activateActiveCB,
    Game.SavingPostCB activatePostCB)
  {
    this.activatePreCB = activatePreCB;
    this.activateActiveCB = activateActiveCB;
    this.activatePostCB = activatePostCB;
  }

  public void StartDelayedInitialSave() => this.StartCoroutine(this.DelayedInitialSave());

  private IEnumerator DelayedInitialSave()
  {
    for (int i = 0; i < 1; ++i)
      yield return (object) null;
    SaveLoader.Instance.InitialSave();
  }

  public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
  {
    if (this.activatePreCB != null)
      this.activatePreCB((Game.CansaveCB) (() => this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer))));
    else
      this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
  }

  private IEnumerator DelayedSave(
    string filename,
    bool isAutoSave,
    bool updateSavePointer)
  {
    while (PlayerController.Instance.IsDragging())
      yield return (object) null;
    PlayerController.Instance.CancelDragging();
    PlayerController.Instance.AllowDragging(false);
    int i;
    for (i = 0; i < 1; ++i)
      yield return (object) null;
    if (this.activateActiveCB != null)
    {
      this.activateActiveCB();
      for (i = 0; i < 1; ++i)
        yield return (object) null;
    }
    SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
    if (this.activatePostCB != null)
      this.activatePostCB();
    for (i = 0; i < 5; ++i)
      yield return (object) null;
    PlayerController.Instance.AllowDragging(true);
  }

  public void StartDelayed(int tick_delay, System.Action action) => this.StartCoroutine(this.DelayedExecutor(tick_delay, action));

  private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
  {
    for (int i = 0; i < tick_delay; ++i)
      yield return (object) null;
    action();
  }

  private void LoadEventHashes()
  {
    foreach (GameHashes gameHashes in Enum.GetValues(typeof (GameHashes)))
      HashCache.Get().Add((int) gameHashes, gameHashes.ToString());
    foreach (UtilHashes utilHashes in Enum.GetValues(typeof (UtilHashes)))
      HashCache.Get().Add((int) utilHashes, utilHashes.ToString());
    foreach (UIHashes uiHashes in Enum.GetValues(typeof (UIHashes)))
      HashCache.Get().Add((int) uiHashes, uiHashes.ToString());
  }

  public void StopFE()
  {
    if ((bool) (UnityEngine.Object) SteamUGCService.Instance)
      SteamUGCService.Instance.enabled = false;
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FrontEndSnapshot);
    if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
      MusicManager.instance.StopSong("Music_FrontEnd");
    if (!MusicManager.instance.SongIsPlaying("Music_TitleTheme"))
      return;
    MusicManager.instance.StopSong("Music_TitleTheme");
  }

  public void StartBE()
  {
    Resources.UnloadUnusedAssets();
    if ((UnityEngine.Object) TimeOfDay.Instance != (UnityEngine.Object) null && !MusicManager.instance.SongIsPlaying("Stinger_Loop_Night") && TimeOfDay.Instance.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
    {
      MusicManager.instance.PlaySong("Stinger_Loop_Night");
      MusicManager.instance.SetSongParameter("Stinger_Loop_Night", "Music_PlayStinger", 0.0f);
    }
    AudioMixer.instance.Reset();
    AudioMixer.instance.StartPersistentSnapshots();
    if (!MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
      return;
    MusicManager.instance.PlayDynamicMusic();
  }

  public void StopBE()
  {
    if ((bool) (UnityEngine.Object) SteamUGCService.Instance)
      SteamUGCService.Instance.enabled = true;
    LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
    if ((UnityEngine.Object) loopingSoundManager != (UnityEngine.Object) null)
      loopingSoundManager.StopAllSounds();
    MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
    AudioMixer.instance.StopPersistentSnapshots();
    foreach (List<SaveLoadRoot> saveLoadRootList in SaveLoader.Instance.saveManager.GetLists().Values)
    {
      foreach (SaveLoadRoot saveLoadRoot in saveLoadRootList)
      {
        if ((UnityEngine.Object) saveLoadRoot.gameObject != (UnityEngine.Object) null)
          Util.KDestroyGameObject(saveLoadRoot.gameObject);
      }
    }
    this.GetComponent<EntombedItemVisualizer>().Clear();
    SimTemperatureTransfer.ClearInstanceMap();
    StructureTemperatureComponents.ClearInstanceMap();
    ElementConsumer.ClearInstanceMap();
    KComponentSpawn.instance.comps.Clear();
    KInputHandler.Remove((IInputHandler) Global.Instance.GetInputManager().GetDefaultController(), (IInputHandler) this.cameraController);
    KInputHandler.Remove((IInputHandler) Global.Instance.GetInputManager().GetDefaultController(), (IInputHandler) this.playerController);
    Sim.Shutdown();
    SimAndRenderScheduler.instance.Reset();
    Resources.UnloadUnusedAssets();
  }

  public void SetStatusItemOffset(Transform transform, Vector3 offset) => this.statusItemRenderer.SetOffset(transform, offset);

  public void AddStatusItem(Transform transform, StatusItem status_item) => this.statusItemRenderer.Add(transform, status_item);

  public void RemoveStatusItem(Transform transform, StatusItem status_item) => this.statusItemRenderer.Remove(transform, status_item);

  public float LastTimeWorkStarted => this.lastTimeWorkStarted;

  public void StartedWork() => this.lastTimeWorkStarted = Time.time;

  private void SpawnOxygenBubbles(Vector3 position, float angle)
  {
  }

  public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
  {
    if (!handle.IsValid())
      return;
    this.callbackManagerManuallyReleasedHandles.Add(handle.index);
    this.callbackManager.Release(handle);
  }

  private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle) => !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);

  [ContextMenu("Print")]
  private void Print()
  {
    Console.WriteLine("This is a console writeline test");
    Debug.Log((object) "This is a debug log test");
  }

  private void DestroyInstances()
  {
    KMonoBehaviour.lastGameObject = (GameObject) null;
    KMonoBehaviour.lastObj = (KObject) null;
    GridSettings.ClearGrid();
    StateMachineManager.ResetParameters();
    ChoreTable.Instance.ResetParameters();
    BubbleManager.DestroyInstance();
    AmbientSoundManager.Destroy();
    AutoDisinfectableManager.DestroyInstance();
    BuildMenu.DestroyInstance();
    CancelTool.DestroyInstance();
    ClearTool.DestroyInstance();
    ChoreGroupManager.DestroyInstance();
    CO2Manager.DestroyInstance();
    ConsumerManager.DestroyInstance();
    CopySettingsTool.DestroyInstance();
    DateTime.DestroyInstance();
    DebugBaseTemplateButton.DestroyInstance();
    DebugPaintElementScreen.DestroyInstance();
    DetailsScreen.DestroyInstance();
    DietManager.DestroyInstance();
    DebugText.DestroyInstance();
    FactionManager.DestroyInstance();
    EmptyPipeTool.DestroyInstance();
    FetchListStatusItemUpdater.DestroyInstance();
    FishOvercrowingManager.DestroyInstance();
    FallingWater.DestroyInstance();
    GridCompositor.DestroyInstance();
    Infrared.DestroyInstance();
    KPrefabIDTracker.DestroyInstance();
    ManagementMenu.DestroyInstance();
    MaterialNeeds.DestroyInstance();
    Messenger.DestroyInstance();
    LoopingSoundManager.DestroyInstance();
    MeterScreen.DestroyInstance();
    MinionGroupProber.DestroyInstance();
    NavPathDrawer.DestroyInstance();
    MinionIdentity.DestroyStatics();
    PathFinder.PathGrid.OnCleanUp();
    PathFinder.PathGrid = (PathGrid) null;
    Pathfinding.DestroyInstance();
    PrebuildTool.DestroyInstance();
    PrioritizeTool.DestroyInstance();
    SelectTool.DestroyInstance();
    PopFXManager.DestroyInstance();
    ProgressBarsConfig.DestroyInstance();
    PropertyTextures.DestroyInstance();
    RationTracker.DestroyInstance();
    ReportManager.DestroyInstance();
    VignetteManager.Instance.DestroyInstance();
    Research.DestroyInstance();
    RootMenu.DestroyInstance();
    SaveLoader.DestroyInstance();
    Scenario.DestroyInstance();
    SimDebugView.DestroyInstance();
    SpriteSheetAnimManager.DestroyInstance();
    ScheduleManager.DestroyInstance();
    Sounds.DestroyInstance();
    ToolMenu.DestroyInstance();
    WorldDamage.DestroyInstance();
    WaterCubes.DestroyInstance();
    WireBuildTool.DestroyInstance();
    VisibilityTester.DestroyInstance();
    Traces.DestroyInstance();
    TopLeftControlScreen.DestroyInstance();
    UtilityBuildTool.DestroyInstance();
    ReportScreen.DestroyInstance();
    ChorePreconditions.DestroyInstance();
    SandboxBrushTool.DestroyInstance();
    SandboxHeatTool.DestroyInstance();
    SandboxClearFloorTool.DestroyInstance();
    GameScreenManager.DestroyInstance();
    GameScheduler.DestroyInstance();
    NavigationReservations.DestroyInstance();
    Tutorial.DestroyInstance();
    CameraController.DestroyInstance();
    CellEventLogger.DestroyInstance();
    GameFlowManager.DestroyInstance();
    Immigration.DestroyInstance();
    BuildTool.DestroyInstance();
    DebugTool.DestroyInstance();
    DeconstructTool.DestroyInstance();
    DigTool.DestroyInstance();
    DisinfectTool.DestroyInstance();
    HarvestTool.DestroyInstance();
    MopTool.DestroyInstance();
    MoveToLocationTool.DestroyInstance();
    PlaceTool.DestroyInstance();
    SpacecraftManager.DestroyInstance();
    SandboxDestroyerTool.DestroyInstance();
    SandboxFOWTool.DestroyInstance();
    SandboxFloodTool.DestroyInstance();
    SandboxSprinkleTool.DestroyInstance();
    StampTool.DestroyInstance();
    OnDemandUpdater.DestroyInstance();
    HoverTextScreen.DestroyInstance();
    ImmigrantScreen.DestroyInstance();
    OverlayMenu.DestroyInstance();
    NameDisplayScreen.DestroyInstance();
    PlanScreen.DestroyInstance();
    ResourceCategoryScreen.DestroyInstance();
    ResourceRemainingDisplayScreen.DestroyInstance();
    SandboxToolParameterMenu.DestroyInstance();
    SpeedControlScreen.DestroyInstance();
    Vignette.DestroyInstance();
    PlayerController.DestroyInstance();
    NotificationScreen.DestroyInstance();
    BuildingCellVisualizerResources.DestroyInstance();
    PauseScreen.DestroyInstance();
    SaveLoadRoot.DestroyStatics();
    KTime.DestroyInstance();
    DemoTimer.DestroyInstance();
    UIScheduler.DestroyInstance();
    SaveGame.DestroyInstance();
    GameClock.DestroyInstance();
    TimeOfDay.DestroyInstance();
    DeserializeWarnings.DestroyInstance();
    UISounds.DestroyInstance();
    RenderTextureDestroyer.DestroyInstance();
    WorldInspector.DestroyStatics();
    LoadScreen.DestroyInstance();
    LoadingOverlay.DestroyInstance();
    SimAndRenderScheduler.DestroyInstance();
    Singleton<CellChangeMonitor>.DestroyInstance();
    Singleton<StateMachineManager>.Instance.Clear();
    Singleton<StateMachineUpdater>.Instance.Clear();
    UpdateObjectCountParameter.Clear();
    MaterialSelectionPanel.ClearStatics();
    StarmapScreen.DestroyInstance();
    SpacecraftManager.DestroyInstance();
    Game.Instance = (Game) null;
    Grid.OnReveal = (System.Action<int>) null;
    this.VisualTunerElement = (Element) null;
    Assets.ClearOnAddPrefab();
    KMonoBehaviour.lastGameObject = (GameObject) null;
    KMonoBehaviour.lastObj = (KObject) null;
    (KComponentSpawn.instance.comps as GameComps).Clear();
  }

  [Serializable]
  public struct SavedInfo
  {
    public bool discoveredSurface;
    public bool discoveredOilField;
    public bool curedDisease;
    public bool blockedCometWithBunkerDoor;
    public Dictionary<Tag, float> creaturePoopAmount;
    public Dictionary<Tag, float> powerCreatedbyGeneratorType;

    [OnDeserialized]
    private void OnDeserialized() => this.InitializeEmptyVariables();

    public void InitializeEmptyVariables()
    {
      if (this.creaturePoopAmount == null)
        this.creaturePoopAmount = new Dictionary<Tag, float>();
      if (this.powerCreatedbyGeneratorType != null)
        return;
      this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
    }
  }

  public struct CallbackInfo
  {
    public System.Action cb;
    public bool manuallyRelease;

    public CallbackInfo(System.Action cb, bool manually_release = false)
    {
      this.cb = cb;
      this.manuallyRelease = manually_release;
    }
  }

  public struct ComplexCallbackInfo<DataType>
  {
    public System.Action<DataType, object> cb;
    public object callbackData;
    public string debugInfo;

    public ComplexCallbackInfo(
      System.Action<DataType, object> cb,
      object callback_data,
      string debug_info)
    {
      this.cb = cb;
      this.debugInfo = debug_info;
      this.callbackData = callback_data;
    }
  }

  public class ComplexCallbackHandleVector<DataType>
  {
    private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;
    private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();

    public ComplexCallbackHandleVector(int initial_size) => this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);

    public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(
      System.Action<DataType, object> cb,
      object callback_data,
      string debug_info)
    {
      return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
    }

    public Game.ComplexCallbackInfo<DataType> GetItem(
      HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
    {
      try
      {
        return this.baseMgr.GetItem(handle);
      }
      catch (Exception ex)
      {
        int index;
        this.baseMgr.UnpackHandleUnchecked(handle, out byte _, out index);
        string str = (string) null;
        if (this.releaseInfo.TryGetValue(index, out str))
          KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str);
        else
          KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically");
        throw ex;
      }
    }

    public Game.ComplexCallbackInfo<DataType> Release(
      HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle,
      string release_info)
    {
      byte version;
      int index;
      try
      {
        this.baseMgr.UnpackHandle(handle, out version, out index);
        this.releaseInfo[index] = release_info;
        return this.baseMgr.Release(handle);
      }
      catch (Exception ex)
      {
        this.baseMgr.UnpackHandleUnchecked(handle, out version, out index);
        string str = (string) null;
        if (this.releaseInfo.TryGetValue(index, out str))
          KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str);
        else
          KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing");
        throw ex;
      }
    }

    public void Clear() => this.baseMgr.Clear();

    public bool IsVersionValid(
      HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
    {
      return this.baseMgr.IsVersionValid(handle);
    }
  }

  public enum TemperatureOverlayModes
  {
    AbsoluteTemperature,
    AdaptiveTemperature,
    HeatFlow,
    StateChange,
  }

  [Serializable]
  public class ConduitVisInfo
  {
    public GameObject prefab;
    [Header("Main View")]
    public Color32 tint;
    public Color32 insulatedTint;
    public Color32 radiantTint;
    [Header("Overlay")]
    public string overlayTintName;
    public string overlayInsulatedTintName;
    public string overlayRadiantTintName;
    public Vector2 overlayMassScaleRange = (Vector2) new Vector2f(1f, 1000f);
    public Vector2 overlayMassScaleValues = (Vector2) new Vector2f(0.1f, 1f);
  }

  private enum SpawnRotationConfig
  {
    Normal,
    StringName,
  }

  [Serializable]
  private struct SpawnRotationData
  {
    public string animName;
    public bool flip;
  }

  [Serializable]
  private struct SpawnPoolData
  {
    [HashedEnum]
    public SpawnFXHashes id;
    public int initialCount;
    public Color32 colour;
    public GameObject fxPrefab;
    public string initialAnim;
    public Vector3 spawnOffset;
    public Vector2 spawnRandomOffset;
    public Game.SpawnRotationConfig rotationConfig;
    public Game.SpawnRotationData[] rotationData;
  }

  [Serializable]
  private class Settings
  {
    public int nextUniqueID;
    public int gameID;

    public Settings(Game game)
    {
      this.nextUniqueID = KPrefabID.NextUniqueID;
      this.gameID = KleiMetrics.GameID();
    }

    public Settings()
    {
    }
  }

  public class GameSaveData
  {
    public ConduitFlow gasConduitFlow;
    public ConduitFlow liquidConduitFlow;
    public Vector2I simActiveRegionMin;
    public Vector2I simActiveRegionMax;
    public FallingWater fallingWater;
    public UnstableGroundManager unstableGround;
    public WorldDetailSave worldDetail;
    public CustomGameSettings customGameSettings;
    public bool debugWasUsed;
    public bool autoPrioritizeRoles;
    public bool advancedPersonalPriorities;
    public Game.SavedInfo savedInfo;
  }

  public delegate void CansaveCB();

  public delegate void SavingPreCB(Game.CansaveCB cb);

  public delegate void SavingActiveCB();

  public delegate void SavingPostCB();

  [Serializable]
  public struct LocationColours
  {
    public Color unreachable;
    public Color invalidLocation;
    public Color validLocation;
    public Color requiresRole;
    public Color unreachable_requiresRole;
  }

  [Serializable]
  public class UIColours
  {
    [SerializeField]
    private Game.LocationColours digColours;
    [SerializeField]
    private Game.LocationColours buildColours;

    public Game.LocationColours Dig => this.digColours;

    public Game.LocationColours Build => this.buildColours;
  }
}
