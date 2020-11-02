// Decompiled with JetBrains decompiler
// Type: BalloonStandCellSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BalloonStandCellSensor : Sensor
{
  private MinionBrain brain;
  private Navigator navigator;
  private int cell;
  private int standCell;

  public BalloonStandCellSensor(Sensors sensors)
    : base(sensors)
  {
    this.navigator = this.GetComponent<Navigator>();
    this.brain = this.GetComponent<MinionBrain>();
  }

  public override void Update()
  {
    this.cell = Grid.InvalidCell;
    int num1 = int.MaxValue;
    ListPool<int[], BalloonStandCellSensor>.PooledList pooledList = ListPool<int[], BalloonStandCellSensor>.Allocate();
    int num2 = 50;
    foreach (int mingleCell in Game.Instance.mingleCellTracker.mingleCells)
    {
      if (this.brain.IsCellClear(mingleCell))
      {
        int navigationCost = this.navigator.GetNavigationCost(mingleCell);
        if (navigationCost != -1)
        {
          if (mingleCell == Grid.InvalidCell || navigationCost < num1)
          {
            this.cell = mingleCell;
            num1 = navigationCost;
          }
          if (navigationCost < num2)
          {
            int cell1 = Grid.CellRight(mingleCell);
            int cell2 = Grid.CellRight(cell1);
            int cell3 = Grid.CellLeft(mingleCell);
            int cell4 = Grid.CellLeft(cell3);
            CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell(this.cell);
            CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(cell4);
            CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(cell2);
            if (cavityForCell1 != null)
            {
              if (cavityForCell3 != null && cavityForCell3.handle == cavityForCell1.handle && (this.navigator.NavGrid.NavTable.IsValid(cell1) && this.navigator.NavGrid.NavTable.IsValid(cell2)))
                pooledList.Add(new int[2]
                {
                  mingleCell,
                  cell2
                });
              if (cavityForCell2 != null && cavityForCell2.handle == cavityForCell1.handle && (this.navigator.NavGrid.NavTable.IsValid(cell3) && this.navigator.NavGrid.NavTable.IsValid(cell4)))
                pooledList.Add(new int[2]
                {
                  mingleCell,
                  cell4
                });
            }
          }
        }
      }
    }
    if (pooledList.Count > 0)
    {
      int[] numArray = pooledList[Random.Range(0, pooledList.Count)];
      this.cell = numArray[0];
      this.standCell = numArray[1];
    }
    else if (Components.Telepads.Count > 0)
    {
      Telepad telepad = Components.Telepads.Items[0];
      if ((Object) telepad == (Object) null || !telepad.GetComponent<Operational>().IsOperational)
        return;
      int cell1 = Grid.CellLeft(Grid.PosToCell(telepad.transform.GetPosition()));
      int cell2 = Grid.CellRight(cell1);
      int cell3 = Grid.CellRight(cell2);
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell(cell1);
      CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(cell3);
      if (cavityForCell1 != null && cavityForCell2 != null && (this.navigator.NavGrid.NavTable.IsValid(cell1) && this.navigator.NavGrid.NavTable.IsValid(cell2)) && this.navigator.NavGrid.NavTable.IsValid(cell3))
      {
        this.cell = cell1;
        this.standCell = cell3;
      }
    }
    pooledList.Recycle();
  }

  public int GetCell() => this.cell;

  public int GetStandCell() => this.standCell;
}
