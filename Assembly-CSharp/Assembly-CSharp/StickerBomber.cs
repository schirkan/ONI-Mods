// Decompiled with JetBrains decompiler
// Type: StickerBomber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
  public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;
  public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;
  public StickerBomber.OverjoyedStates overjoyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State) this.overjoyed).Exit((StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB));
    this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle);
    this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) GameClock.Instance.GetTime() >= (double) smi.nextStickerBomb));
    this.overjoyed.place_stickers.Exit((StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB)).ToggleReactable((Func<StickerBomber.Instance, Reactable>) (smi => smi.CreateReactable())).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
  }

  public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
  }

  public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
  {
    [Serialize]
    public float nextStickerBomb;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public Reactable CreateReactable() => (Reactable) new StickerBomber.Instance.StickerBombReactable(this.master.gameObject, this.smi);

    private class StickerBombReactable : Reactable
    {
      private int stickersToPlace;
      private int stickersPlaced;
      private int placementCell;
      private float tile_random_range = 1f;
      private float tile_random_rotation = 90f;
      private float TIME_PER_STICKER_PLACED = 0.66f;
      private float STICKER_PLACE_TIMER;
      private KBatchedAnimController kbac;
      private KAnimFile animset = Assets.GetAnim((HashedString) "anim_stickers_kanim");
      private HashedString pre_anim = (HashedString) "working_pre";
      private HashedString loop_anim = (HashedString) "working_loop";
      private HashedString pst_anim = (HashedString) "working_pst";
      private StickerBomber.Instance stickerBomber;
      private Func<int, bool> canPlaceStickerCb = (Func<int, bool>) (cell => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && ((!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)])) && ((!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell)));

      public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber)
        : base(gameObject, (HashedString) nameof (StickerBombReactable), Db.Get().ChoreTypes.Build, 2, 1)
      {
        this.preventChoreInterruption = true;
        this.stickerBomber = stickerBomber;
      }

      public override bool InternalCanBegin(
        GameObject new_reactor,
        Navigator.ActiveTransition transition)
      {
        if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null || (UnityEngine.Object) new_reactor == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject != (UnityEngine.Object) new_reactor)
          return false;
        Navigator component = new_reactor.GetComponent<Navigator>();
        return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CurrentNavType != NavType.Tube && (component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole);
      }

      protected override void InternalBegin()
      {
        this.stickersToPlace = UnityEngine.Random.Range(4, 6);
        this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
        this.placementCell = this.FindPlacementCell();
        if (this.placementCell == 0)
        {
          this.End();
        }
        else
        {
          this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
          this.kbac.AddAnimOverrides(this.animset);
          this.kbac.Play(this.pre_anim);
          this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop);
        }
      }

      public override void Update(float dt)
      {
        this.STICKER_PLACE_TIMER -= dt;
        if ((double) this.STICKER_PLACE_TIMER <= 0.0)
        {
          this.PlaceSticker();
          this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
        }
        if (this.stickersPlaced < this.stickersToPlace)
          return;
        this.kbac.Play(this.pst_anim);
        this.End();
      }

      protected override void InternalEnd()
      {
        if ((UnityEngine.Object) this.kbac != (UnityEngine.Object) null)
        {
          this.kbac.RemoveAnimOverrides(this.animset);
          this.kbac = (KBatchedAnimController) null;
        }
        this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
        this.stickersPlaced = 0;
      }

      private int FindPlacementCell()
      {
        int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
        ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
        ListPool<int, PathFinder>.PooledList tList = ListPool<int, PathFinder>.Allocate();
        QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
        pooledQueue.Enqueue(new GameUtil.FloodFillInfo()
        {
          cell = cell,
          depth = 0
        });
        GameUtil.FloodFillConditional((Queue<GameUtil.FloodFillInfo>) pooledQueue, this.canPlaceStickerCb, (ICollection<int>) pooledList, (ICollection<int>) tList, 2);
        if (tList.Count <= 0)
          return 0;
        int random = tList.GetRandom<int>();
        pooledList.Recycle();
        tList.Recycle();
        pooledQueue.Recycle();
        return random;
      }

      private void PlaceSticker()
      {
        ++this.stickersPlaced;
        Vector3 pos = Grid.CellToPos(this.placementCell);
        int num = 10;
        while (num > 0)
        {
          --num;
          Vector3 position = pos + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
          if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
          {
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)));
            gameObject.GetComponent<StickerBomb>().SetStickerType(this.reactor.GetComponent<MinionIdentity>().stickerType);
            gameObject.SetActive(true);
            num = 0;
          }
        }
      }

      protected override void InternalCleanup()
      {
      }
    }
  }
}
