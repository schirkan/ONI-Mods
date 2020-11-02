// Decompiled with JetBrains decompiler
// Type: SeedPlantingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class SeedPlantingStates : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>
{
  private const int MAX_NAVIGATE_DISTANCE = 100;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State pickupSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlantLocation;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlot;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToDirt;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.findSeed;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.PLANTINGSEED.NAME, (string) CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP, category: Db.Get().StatusItemCategories.Main).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.UnreserveSeed)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.DropAll)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride));
    this.findSeed.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      SeedPlantingStates.FindSeed(smi);
      if ((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
      {
        SeedPlantingStates.ReserveSeed(smi);
        smi.GoTo((StateMachine.BaseState) this.moveToSeed);
      }
    }));
    this.moveToSeed.MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetSeedCell), this.findPlantLocation, this.behaviourcomplete);
    this.findPlantLocation.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if ((bool) (UnityEngine.Object) smi.targetSeed)
      {
        SeedPlantingStates.FindDirtPlot(smi);
        if ((UnityEngine.Object) smi.targetPlot != (UnityEngine.Object) null || smi.targetDirtPlotCell != Grid.InvalidCell)
          smi.GoTo((StateMachine.BaseState) this.pickupSeed);
        else
          smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.pickupSeed.PlayAnim("gather").Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PickupComplete)).OnAnimQueueComplete(this.moveToPlantLocation);
    this.moveToPlantLocation.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      else if ((UnityEngine.Object) smi.targetPlot != (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.moveToPlot);
      else if (smi.targetDirtPlotCell != Grid.InvalidCell)
        smi.GoTo((StateMachine.BaseState) this.moveToDirt);
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.moveToDirt.MoveTo((Func<SeedPlantingStates.Instance, int>) (smi => smi.targetDirtPlotCell), this.planting, this.behaviourcomplete);
    this.moveToPlot.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if (!((UnityEngine.Object) smi.targetPlot == (UnityEngine.Object) null) && !((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null))
        return;
      smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    })).MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetPlantableCell), this.planting, this.behaviourcomplete);
    this.planting.Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride)).PlayAnim("plant").Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PlantComplete)).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToPlantSeed);
  }

  private static void AddMouthOverride(SeedPlantingStates.Instance smi)
  {
    SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
    KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "sq_mouth_cheeks");
    if (symbol == null)
      return;
    component.AddSymbolOverride((HashedString) "sq_mouth", symbol);
  }

  private static void RemoveMouthOverride(SeedPlantingStates.Instance smi) => smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) "sq_mouth");

  private static void PickupComplete(SeedPlantingStates.Instance smi)
  {
    if (!(bool) (UnityEngine.Object) smi.targetSeed)
    {
      Debug.LogWarningFormat("PickupComplete seed {0} is null", (object) smi.targetSeed);
    }
    else
    {
      SeedPlantingStates.UnreserveSeed(smi);
      int cell = Grid.PosToCell((KMonoBehaviour) smi.targetSeed);
      if (smi.seed_cell != cell)
      {
        Debug.LogWarningFormat("PickupComplete seed {0} moved {1} != {2}", (object) smi.targetSeed, (object) cell, (object) smi.seed_cell);
        smi.targetSeed = (Pickupable) null;
      }
      else if (smi.targetSeed.HasTag(GameTags.Stored))
      {
        Debug.LogWarningFormat("PickupComplete seed {0} was stored by {1}", (object) smi.targetSeed, (object) smi.targetSeed.storage);
        smi.targetSeed = (Pickupable) null;
      }
      else
      {
        smi.targetSeed = EntitySplitter.Split(smi.targetSeed, 1f);
        smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject);
        SeedPlantingStates.AddMouthOverride(smi);
      }
    }
  }

  private static void PlantComplete(SeedPlantingStates.Instance smi)
  {
    PlantableSeed seed = (bool) (UnityEngine.Object) smi.targetSeed ? smi.targetSeed.GetComponent<PlantableSeed>() : (PlantableSeed) null;
    PlantablePlot plot;
    if ((bool) (UnityEngine.Object) seed && SeedPlantingStates.CheckValidPlotCell(smi, seed, smi.targetDirtPlotCell, out plot))
    {
      if ((bool) (UnityEngine.Object) plot)
      {
        if ((UnityEngine.Object) plot.Occupant == (UnityEngine.Object) null)
          plot.ForceDepositPickupable(smi.targetSeed);
      }
      else
        seed.TryPlant(true);
    }
    smi.targetSeed = (Pickupable) null;
    smi.seed_cell = Grid.InvalidCell;
    smi.targetPlot = (PlantablePlot) null;
  }

  private static void DropAll(SeedPlantingStates.Instance smi) => smi.GetComponent<Storage>().DropAll();

  private static int GetPlantableCell(SeedPlantingStates.Instance smi)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) smi.targetPlot);
    return Grid.IsValidCell(cell) ? Grid.CellAbove(cell) : cell;
  }

  private static void FindDirtPlot(SeedPlantingStates.Instance smi)
  {
    smi.targetDirtPlotCell = Grid.InvalidCell;
    PlantableSeed component = smi.targetSeed.GetComponent<PlantableSeed>();
    PlantableCellQuery plantableCellQuery = PathFinderQueries.plantableCellQuery.Reset(component, 20);
    smi.GetComponent<Navigator>().RunQuery((PathFinderQuery) plantableCellQuery);
    if (plantableCellQuery.result_cells.Count <= 0)
      return;
    smi.targetDirtPlotCell = plantableCellQuery.result_cells[UnityEngine.Random.Range(0, plantableCellQuery.result_cells.Count)];
  }

  private static bool CheckValidPlotCell(
    SeedPlantingStates.Instance smi,
    PlantableSeed seed,
    int cell,
    out PlantablePlot plot)
  {
    plot = (PlantablePlot) null;
    if (!Grid.IsValidCell(cell))
      return false;
    int num = seed.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(cell) : Grid.CellAbove(cell);
    if (!Grid.IsValidCell(num) || !Grid.Solid[num])
      return false;
    GameObject gameObject = Grid.Objects[num, 1];
    if (!(bool) (UnityEngine.Object) gameObject)
      return seed.TestSuitableGround(cell);
    plot = gameObject.GetComponent<PlantablePlot>();
    return (UnityEngine.Object) plot != (UnityEngine.Object) null;
  }

  private static int GetSeedCell(SeedPlantingStates.Instance smi)
  {
    Debug.Assert((bool) (UnityEngine.Object) smi.targetSeed);
    Debug.Assert(smi.seed_cell != Grid.InvalidCell);
    return smi.seed_cell;
  }

  private static void FindSeed(SeedPlantingStates.Instance smi)
  {
    Navigator component = smi.GetComponent<Navigator>();
    Pickupable pickupable = (Pickupable) null;
    int num = 100;
    foreach (PlantableSeed plantableSeed in Components.PlantableSeeds)
    {
      if ((plantableSeed.HasTag(GameTags.Seed) || plantableSeed.HasTag(GameTags.CropSeed)) && (!plantableSeed.HasTag(GameTags.Creatures.ReservedByCreature) && (double) Vector2.Distance((Vector2) smi.transform.position, (Vector2) plantableSeed.transform.position) <= 25.0))
      {
        int navigationCost = component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) plantableSeed));
        if (navigationCost != -1 && navigationCost < num)
        {
          pickupable = plantableSeed.GetComponent<Pickupable>();
          num = navigationCost;
        }
      }
    }
    smi.targetSeed = pickupable;
    smi.seed_cell = (bool) (UnityEngine.Object) smi.targetSeed ? Grid.PosToCell((KMonoBehaviour) smi.targetSeed) : Grid.InvalidCell;
  }

  private static void ReserveSeed(SeedPlantingStates.Instance smi)
  {
    GameObject go = (bool) (UnityEngine.Object) smi.targetSeed ? smi.targetSeed.gameObject : (GameObject) null;
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void UnreserveSeed(SeedPlantingStates.Instance smi)
  {
    GameObject go = (bool) (UnityEngine.Object) smi.targetSeed ? smi.targetSeed.gameObject : (GameObject) null;
    if (!((UnityEngine.Object) smi.targetSeed != (UnityEngine.Object) null))
      return;
    go.RemoveTag(GameTags.Creatures.ReservedByCreature);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.GameInstance
  {
    public PlantablePlot targetPlot;
    public int targetDirtPlotCell = Grid.InvalidCell;
    public Element plantElement = ElementLoader.FindElementByHash(SimHashes.Dirt);
    public Pickupable targetSeed;
    public int seed_cell = Grid.InvalidCell;

    public Instance(Chore<SeedPlantingStates.Instance> chore, SeedPlantingStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToPlantSeed);
  }
}
