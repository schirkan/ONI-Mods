// Decompiled with JetBrains decompiler
// Type: ProcGenGame.River
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGenGame
{
  public class River : ProcGen.River, SymbolicMapElement
  {
    public River(ProcGen.River other)
      : base(other)
    {
    }

    public void ConvertToMap(
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      Element elementByName1 = ElementLoader.FindElementByName(this.backgroundElement);
      Sim.PhysicsData defaultValues1 = elementByName1.defaultValues;
      Element elementByName2 = ElementLoader.FindElementByName(this.element);
      Sim.PhysicsData defaultValues2 = elementByName2.defaultValues;
      defaultValues2.temperature = this.temperature;
      Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
      for (int index1 = 0; index1 < this.pathElements.Count; ++index1)
      {
        Segment pathElement = this.pathElements[index1];
        Vector2 vector2_1 = pathElement.e1 - pathElement.e0;
        Vector2 normalized = new Vector2(-vector2_1.y, vector2_1.x).normalized;
        List<Vector2I> line = ProcGen.Util.GetLine(pathElement.e0, pathElement.e1);
        for (int index2 = 0; index2 < line.Count; ++index2)
        {
          for (float num = 0.5f; (double) num <= (double) this.widthCenter; ++num)
          {
            Vector2 vector2_2 = (Vector2) line[index2] + normalized * num;
            int cell1 = Grid.XYToCell((int) vector2_2.x, (int) vector2_2.y);
            if (Grid.IsValidCell(cell1))
              SetValues(cell1, (object) elementByName2, defaultValues2, invalid);
            Vector2 vector2_3 = (Vector2) line[index2] - normalized * num;
            int cell2 = Grid.XYToCell((int) vector2_3.x, (int) vector2_3.y);
            if (Grid.IsValidCell(cell2))
              SetValues(cell2, (object) elementByName2, defaultValues2, invalid);
          }
          for (float num = 0.5f; (double) num <= (double) this.widthBorder; ++num)
          {
            Vector2 vector2_2 = (Vector2) line[index2] + normalized * (this.widthCenter + num);
            int cell1 = Grid.XYToCell((int) vector2_2.x, (int) vector2_2.y);
            if (Grid.IsValidCell(cell1))
            {
              defaultValues1.temperature = temperatureMin + world.heatOffset[cell1] * temperatureRange;
              SetValues(cell1, (object) elementByName1, defaultValues1, invalid);
            }
            Vector2 vector2_3 = (Vector2) line[index2] - normalized * (this.widthCenter + num);
            int cell2 = Grid.XYToCell((int) vector2_3.x, (int) vector2_3.y);
            if (Grid.IsValidCell(cell2))
            {
              defaultValues1.temperature = temperatureMin + world.heatOffset[cell2] * temperatureRange;
              SetValues(cell2, (object) elementByName1, defaultValues1, invalid);
            }
          }
        }
      }
    }

    public static void ProcessRivers(
      Chunk world,
      List<River> rivers,
      Sim.Cell[] cells,
      Sim.DiseaseCell[] dcs)
    {
      TerrainCell.SetValuesFunction SetValues = (TerrainCell.SetValuesFunction) ((index, elem, pd, dc) =>
      {
        if (Grid.IsValidCell(index))
        {
          cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
          dcs[index] = dc;
        }
        else
          Debug.LogError((object) ("Process::SetValuesFunction Index [" + (object) index + "] is not valid. cells.Length [" + (object) cells.Length + "]"));
      });
      float temperatureMin = 265f;
      float temperatureRange = 30f;
      for (int index = 0; index < rivers.Count; ++index)
        rivers[index].ConvertToMap(world, SetValues, temperatureMin, temperatureRange, (SeededRandom) null);
    }

    public static River GetRiverForCell(List<ProcGen.River> rivers, int cell) => new River(rivers.Find((Predicate<ProcGen.River>) (river => Grid.PosToCell(river.SourcePosition()) == cell || Grid.PosToCell(river.SinkPosition()) == cell)));

    private static void GetRiverLocation(List<River> rivers, ref GameSpawnData gsd)
    {
      for (int index = 0; index < rivers.Count; ++index)
      {
        if ((double) rivers[index].SourcePosition().y < (double) rivers[index].SinkPosition().y)
          ;
      }
    }
  }
}
