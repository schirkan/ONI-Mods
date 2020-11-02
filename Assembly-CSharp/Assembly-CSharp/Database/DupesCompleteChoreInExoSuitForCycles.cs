// Decompiled with JetBrains decompiler
// Type: Database.DupesCompleteChoreInExoSuitForCycles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Database
{
  public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement
  {
    public int currentCycleStreak;
    public int numCycles;

    public DupesCompleteChoreInExoSuitForCycles(int numCycles) => this.numCycles = numCycles;

    public override bool Success()
    {
      Dictionary<int, List<int>> completeChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
      Dictionary<int, float> dictionary = new Dictionary<int, float>();
      foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
        dictionary.Add(minionIdentity.GetComponent<KPrefabID>().InstanceID, minionIdentity.arrivalTime);
      int val2 = 0;
      int num = Math.Min(completeChoresInSuits.Count, this.numCycles);
      for (int key1 = GameClock.Instance.GetCycle() - num; key1 <= GameClock.Instance.GetCycle(); ++key1)
      {
        if (completeChoresInSuits.ContainsKey(key1))
        {
          List<int> list = dictionary.Keys.Except<int>((IEnumerable<int>) completeChoresInSuits[key1]).ToList<int>();
          bool flag = true;
          foreach (int key2 in list)
          {
            if ((double) dictionary[key2] < (double) key1)
            {
              flag = false;
              break;
            }
          }
          val2 = flag ? val2 + 1 : 0;
          this.currentCycleStreak = val2;
          if (val2 >= this.numCycles)
          {
            this.currentCycleStreak = this.numCycles;
            return true;
          }
        }
        else
        {
          this.currentCycleStreak = Math.Max(this.currentCycleStreak, val2);
          val2 = 0;
        }
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer) => writer.Write(this.numCycles);

    public override void Deserialize(IReader reader) => this.numCycles = reader.ReadInt32();

    public int GetNumberOfDupesForCycle(int cycle)
    {
      int num = 0;
      Dictionary<int, List<int>> completeChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
      if (completeChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
        num = completeChoresInSuits[GameClock.Instance.GetCycle()].Count;
      return num;
    }
  }
}
