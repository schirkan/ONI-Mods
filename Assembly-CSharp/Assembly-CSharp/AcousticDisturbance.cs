// Decompiled with JetBrains decompiler
// Type: AcousticDisturbance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AcousticDisturbance
{
  private static readonly HashedString[] PreAnims = new HashedString[2]
  {
    (HashedString) "grid_pre",
    (HashedString) "grid_loop"
  };
  private static readonly HashedString PostAnim = (HashedString) "grid_pst";
  private static float distanceDelay = 0.25f;
  private static float duration = 3f;
  private static HashSet<int> cellsInRange = new HashSet<int>();

  public static void Emit(object data, int EmissionRadius)
  {
    GameObject gameObject = (GameObject) data;
    Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
    Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
    int cell1 = Grid.PosToCell(position1);
    int num = EmissionRadius * EmissionRadius;
    AcousticDisturbance.DetermineCellsInRadius(cell1, 0, Mathf.CeilToInt((float) EmissionRadius), AcousticDisturbance.cellsInRange);
    AcousticDisturbance.DrawVisualEffect(cell1, AcousticDisturbance.cellsInRange);
    for (int idx = 0; idx < minionIdentities.Count; ++idx)
    {
      MinionIdentity cmp = minionIdentities[idx];
      if ((UnityEngine.Object) cmp.gameObject != (UnityEngine.Object) gameObject.gameObject)
      {
        Vector2 position2 = (Vector2) cmp.transform.GetPosition();
        if ((double) Vector2.SqrMagnitude(position1 - position2) <= (double) num)
        {
          int cell2 = Grid.PosToCell(position2);
          if (AcousticDisturbance.cellsInRange.Contains(cell2) && cmp.GetSMI<StaminaMonitor.Instance>().IsSleeping())
          {
            cmp.Trigger(-527751701, data);
            cmp.Trigger(1621815900, data);
          }
        }
      }
    }
    AcousticDisturbance.cellsInRange.Clear();
  }

  private static void DrawVisualEffect(int center_cell, HashSet<int> cells)
  {
    SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(center_cell));
    foreach (int cell in cells)
    {
      int gridDistance = AcousticDisturbance.GetGridDistance(cell, center_cell);
      GameScheduler.Instance.Schedule("radialgrid_pre", AcousticDisturbance.distanceDelay * (float) gridDistance, new System.Action<object>(AcousticDisturbance.SpawnEffect), (object) cell, (SchedulerGroup) null);
    }
  }

  private static void SpawnEffect(object data)
  {
    Grid.SceneLayer layer = Grid.SceneLayer.InteriorWall;
    KBatchedAnimController effect = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC((int) data, layer), layer: layer);
    effect.destroyOnAnimComplete = false;
    effect.Play(AcousticDisturbance.PreAnims, KAnim.PlayMode.Loop);
    GameScheduler.Instance.Schedule("radialgrid_loop", AcousticDisturbance.duration, new System.Action<object>(AcousticDisturbance.DestroyEffect), (object) effect, (SchedulerGroup) null);
  }

  private static void DestroyEffect(object data)
  {
    KBatchedAnimController kbatchedAnimController = (KBatchedAnimController) data;
    kbatchedAnimController.destroyOnAnimComplete = true;
    kbatchedAnimController.Play(AcousticDisturbance.PostAnim);
  }

  private static int GetGridDistance(int cell, int center_cell)
  {
    Vector2I vector2I = Grid.CellToXY(cell) - Grid.CellToXY(center_cell);
    return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
  }

  private static void DetermineCellsInRadius(
    int cell,
    int depth,
    int max_depth,
    HashSet<int> cells_in_range)
  {
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
      return;
    cells_in_range.Add(cell);
    if (depth >= max_depth)
      return;
    int depth1 = depth + 1;
    int num1 = Grid.CellBelow(cell);
    int num2 = Grid.CellAbove(cell);
    int num3 = cell - 1;
    int num4 = cell + 1;
    int num5 = !Grid.IsValidCell(num1) ? 0 : (!Grid.Solid[num1] ? 1 : 0);
    bool flag1 = Grid.IsValidCell(num2) && !Grid.Solid[num2];
    bool flag2 = Grid.IsValidCell(num3) && !Grid.Solid[num3];
    bool flag3 = Grid.IsValidCell(num4) && !Grid.Solid[num4];
    if ((num5 | (flag2 ? 1 : 0)) != 0)
      AcousticDisturbance.DetermineCellsInRadius(num1 - 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(num1, depth1, max_depth, cells_in_range);
    if ((num5 | (flag3 ? 1 : 0)) != 0)
      AcousticDisturbance.DetermineCellsInRadius(num1 + 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(num3, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(num4, depth1, max_depth, cells_in_range);
    if (flag1 | flag2)
      AcousticDisturbance.DetermineCellsInRadius(num2 - 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(num2, depth1, max_depth, AcousticDisturbance.cellsInRange);
    if (!(flag1 | flag3))
      return;
    AcousticDisturbance.DetermineCellsInRadius(num2 + 1, depth1, max_depth, cells_in_range);
  }
}
