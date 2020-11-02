﻿// Decompiled with JetBrains decompiler
// Type: FishFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
  public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;
  public FishFeeder.OperationalState operational;
  public static HashedString[] ballSymbols;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.notoperational;
    this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange));
    this.notoperational.TagTransition(GameTags.Operational, (GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State) this.operational);
    this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
    this.operational.on.DoNothing();
    int length = 19;
    FishFeeder.ballSymbols = new HashedString[length];
    for (int index = 0; index < length; ++index)
      FishFeeder.ballSymbols[index] = (HashedString) ("ball" + index.ToString());
  }

  private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
  {
    Storage storage = smi.Get<Storage>();
    smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
    smi.fishFeederTop.RefreshStorage();
    smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
    smi.fishFeederBot.RefreshStorage();
  }

  private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
  {
    smi.fishFeederTop.Cleanup();
    smi.fishFeederBot.Cleanup();
  }

  private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
  {
    foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
    {
      if (!((Object) gameObject == (Object) null))
        FishFeeder.OnStorageChange(smi, (object) gameObject);
    }
  }

  private static void OnStorageChange(FishFeeder.Instance smi, object data)
  {
    if ((Object) data == (Object) null)
      return;
    smi.fishFeederTop.RefreshStorage();
    smi.fishFeederBot.RefreshStorage();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
  {
    public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
  }

  public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
  {
    public FishFeeder.FishFeederTop fishFeederTop;
    public FishFeeder.FishFeederBot fishFeederBot;

    public Instance(IStateMachineTarget master, FishFeeder.Def def)
      : base(master, def)
    {
    }
  }

  public class FishFeederTop : IRenderEveryTick
  {
    private FishFeeder.Instance smi;
    private float mass;
    private float targetMass;
    private HashedString[] ballSymbols;
    private float massPerBall;
    private float timeSinceLastBallAppeared;

    public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
    {
      this.smi = smi;
      this.ballSymbols = ball_symbols;
      this.massPerBall = capacity / (float) ball_symbols.Length;
      this.FillFeeder(this.mass);
      SimAndRenderScheduler.instance.Add((object) this);
    }

    private void FillFeeder(float mass)
    {
      KBatchedAnimController component1 = this.smi.GetComponent<KBatchedAnimController>();
      SymbolOverrideController component2 = this.smi.GetComponent<SymbolOverrideController>();
      KAnim.Build.Symbol source_symbol = (KAnim.Build.Symbol) null;
      Storage component3 = this.smi.GetComponent<Storage>();
      if (component3.items.Count > 0 && (Object) component3.items[0] != (Object) null)
        source_symbol = this.smi.GetComponent<Storage>().items[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "algae");
      for (int index = 0; index < this.ballSymbols.Length; ++index)
      {
        bool is_visible = (double) mass > (double) (index + 1) * (double) this.massPerBall;
        component1.SetSymbolVisiblity((KAnimHashedString) this.ballSymbols[index], is_visible);
        if (source_symbol != null)
          component2.AddSymbolOverride(this.ballSymbols[index], source_symbol);
      }
    }

    public void RefreshStorage()
    {
      float num = 0.0f;
      foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
      {
        if (!((Object) gameObject == (Object) null))
          num += gameObject.GetComponent<PrimaryElement>().Mass;
      }
      this.targetMass = num;
    }

    public void RenderEveryTick(float dt)
    {
      this.timeSinceLastBallAppeared += dt;
      if ((double) this.targetMass <= (double) this.mass || (double) this.timeSinceLastBallAppeared <= 0.025000000372529)
        return;
      this.mass += Mathf.Min(this.massPerBall, this.targetMass - this.mass);
      this.FillFeeder(this.mass);
      this.timeSinceLastBallAppeared = 0.0f;
    }

    public void Cleanup() => SimAndRenderScheduler.instance.Remove((object) this);
  }

  public class FishFeederBot
  {
    private KBatchedAnimController anim;
    private Storage topStorage;
    private Storage botStorage;
    private bool refreshingStorage;
    private FishFeeder.Instance smi;
    private float massPerBall;
    private static readonly HashedString HASH_FEEDBALL = (HashedString) "feedball";

    public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
    {
      this.smi = smi;
      this.massPerBall = mass_per_ball;
      this.anim = GameUtil.KInstantiate(Assets.GetPrefab((Tag) nameof (FishFeederBot)), smi.transform.GetPosition(), Grid.SceneLayer.Front).GetComponent<KBatchedAnimController>();
      this.anim.transform.SetParent(smi.transform);
      this.anim.gameObject.SetActive(true);
      this.anim.SetSceneLayer(Grid.SceneLayer.Building);
      this.anim.Play((HashedString) "ball");
      this.anim.Stop();
      foreach (HashedString ballSymbol in ball_symbols)
        this.anim.SetSymbolVisiblity((KAnimHashedString) ballSymbol, false);
      Storage[] components = smi.gameObject.GetComponents<Storage>();
      this.topStorage = components[0];
      this.botStorage = components[1];
    }

    public void RefreshStorage()
    {
      if (this.refreshingStorage)
        return;
      this.refreshingStorage = true;
      float num1 = 0.0f;
      foreach (GameObject gameObject in this.botStorage.items)
      {
        if (!((Object) gameObject == (Object) null))
        {
          num1 += gameObject.GetComponent<PrimaryElement>().Mass;
          int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
          gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingBack));
        }
      }
      if ((double) num1 == 0.0)
      {
        float num2 = 0.0f;
        foreach (GameObject gameObject in this.topStorage.items)
        {
          if (!((Object) gameObject == (Object) null))
            num2 += gameObject.GetComponent<PrimaryElement>().Mass;
        }
        if ((double) num2 > 0.0)
        {
          this.anim.SetSymbolVisiblity((KAnimHashedString) FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
          this.anim.Play((HashedString) "ball");
          Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
          KAnim.Build.Symbol symbol = pickupable.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "algae");
          if (symbol != null)
            this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol);
          this.botStorage.Store(pickupable.gameObject);
          int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
          pickupable.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
        }
        else
          this.anim.SetSymbolVisiblity((KAnimHashedString) FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
      }
      this.refreshingStorage = false;
    }

    public void Cleanup()
    {
    }
  }
}
