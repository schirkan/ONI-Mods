// Decompiled with JetBrains decompiler
// Type: DupeGreetingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DupeGreetingManager")]
public class DupeGreetingManager : KMonoBehaviour, ISim200ms
{
  private const float COOLDOWN_TIME = 720f;
  private Dictionary<int, MinionIdentity> candidateCells;
  private List<DupeGreetingManager.GreetingSetup> activeSetups;
  private Dictionary<MinionIdentity, float> cooldowns;
  private static readonly List<string> waveAnims = new List<string>()
  {
    "anim_react_wave_kanim",
    "anim_react_wave_shy_kanim",
    "anim_react_fingerguns_kanim"
  };

  protected override void OnPrefabInit()
  {
    this.candidateCells = new Dictionary<int, MinionIdentity>();
    this.activeSetups = new List<DupeGreetingManager.GreetingSetup>();
    this.cooldowns = new Dictionary<MinionIdentity, float>();
  }

  public void Sim200ms(float dt)
  {
    if ((double) GameClock.Instance.GetTime() / 600.0 < (double) TuningData<DupeGreetingManager.Tuning>.Get().cyclesBeforeFirstGreeting)
      return;
    for (int index = this.activeSetups.Count - 1; index >= 0; --index)
    {
      DupeGreetingManager.GreetingSetup activeSetup = this.activeSetups[index];
      if (!this.ValidNavigatingMinion(activeSetup.A.minion) || !this.ValidOppositionalMinion(activeSetup.A.minion, activeSetup.B.minion))
      {
        activeSetup.A.reactable.Cleanup();
        activeSetup.B.reactable.Cleanup();
        this.activeSetups.RemoveAt(index);
      }
    }
    this.candidateCells.Clear();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      if ((!this.cooldowns.ContainsKey(minionIdentity) || (double) GameClock.Instance.GetTime() - (double) this.cooldowns[minionIdentity] >= 720.0 * (double) TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier) && this.ValidNavigatingMinion(minionIdentity))
      {
        for (int offset = 0; offset <= 2; ++offset)
        {
          int offsetCell = this.GetOffsetCell(minionIdentity, offset);
          if (this.candidateCells.ContainsKey(offsetCell) && this.ValidOppositionalMinion(minionIdentity, this.candidateCells[offsetCell]))
          {
            this.BeginNewGreeting(minionIdentity, this.candidateCells[offsetCell], offsetCell);
            break;
          }
          this.candidateCells[offsetCell] = minionIdentity;
        }
      }
    }
  }

  private int GetOffsetCell(MinionIdentity minion, int offset) => !minion.GetComponent<Facing>().GetFacing() ? Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) minion), offset, 0) : Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) minion), -offset, 0);

  private bool ValidNavigatingMinion(MinionIdentity minion)
  {
    if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      return false;
    Navigator component = minion.GetComponent<Navigator>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsMoving() && component.CurrentNavType == NavType.Floor;
  }

  private bool ValidOppositionalMinion(MinionIdentity reference_minion, MinionIdentity minion)
  {
    if ((UnityEngine.Object) reference_minion == (UnityEngine.Object) null || (UnityEngine.Object) minion == (UnityEngine.Object) null)
      return false;
    Facing component1 = minion.GetComponent<Facing>();
    Facing component2 = reference_minion.GetComponent<Facing>();
    return this.ValidNavigatingMinion(minion) && (UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) null && component1.GetFacing() != component2.GetFacing();
  }

  private void BeginNewGreeting(MinionIdentity minion_a, MinionIdentity minion_b, int cell) => this.activeSetups.Add(new DupeGreetingManager.GreetingSetup()
  {
    cell = cell,
    A = new DupeGreetingManager.GreetingUnit(minion_a, this.GetReactable(minion_a)),
    B = new DupeGreetingManager.GreetingUnit(minion_b, this.GetReactable(minion_b))
  });

  private Reactable GetReactable(MinionIdentity minion) => (Reactable) new SelfEmoteReactable(minion.gameObject, (HashedString) "NavigatorPassingGreeting", Db.Get().ChoreTypes.Emote, (HashedString) DupeGreetingManager.waveAnims[UnityEngine.Random.Range(0, DupeGreetingManager.waveAnims.Count)], 1000f).AddStep(new EmoteReactable.EmoteStep()
  {
    anim = (HashedString) "react",
    startcb = new System.Action<GameObject>(this.BeginReacting)
  }).AddThought(Db.Get().Thoughts.Chatty);

  private void BeginReacting(GameObject minionGO)
  {
    if ((UnityEngine.Object) minionGO == (UnityEngine.Object) null)
      return;
    MinionIdentity component = minionGO.GetComponent<MinionIdentity>();
    Vector3 vector3 = Vector3.zero;
    foreach (DupeGreetingManager.GreetingSetup activeSetup in this.activeSetups)
    {
      if ((UnityEngine.Object) activeSetup.A.minion == (UnityEngine.Object) component)
      {
        if ((UnityEngine.Object) activeSetup.B.minion != (UnityEngine.Object) null)
        {
          vector3 = activeSetup.B.minion.transform.GetPosition();
          break;
        }
        break;
      }
      if ((UnityEngine.Object) activeSetup.B.minion == (UnityEngine.Object) component)
      {
        if ((UnityEngine.Object) activeSetup.A.minion != (UnityEngine.Object) null)
        {
          vector3 = activeSetup.A.minion.transform.GetPosition();
          break;
        }
        break;
      }
    }
    minionGO.GetComponent<Facing>().SetFacing((double) vector3.x < (double) minionGO.transform.GetPosition().x);
    minionGO.GetComponent<Effects>().Add("Greeting", true);
    this.cooldowns[component] = GameClock.Instance.GetTime();
  }

  public class Tuning : TuningData<DupeGreetingManager.Tuning>
  {
    public float cyclesBeforeFirstGreeting;
    public float greetingDelayMultiplier;
  }

  private class GreetingUnit
  {
    public MinionIdentity minion;
    public Reactable reactable;

    public GreetingUnit(MinionIdentity minion, Reactable reactable)
    {
      this.minion = minion;
      this.reactable = reactable;
    }
  }

  private class GreetingSetup
  {
    public int cell;
    public DupeGreetingManager.GreetingUnit A;
    public DupeGreetingManager.GreetingUnit B;
  }
}
