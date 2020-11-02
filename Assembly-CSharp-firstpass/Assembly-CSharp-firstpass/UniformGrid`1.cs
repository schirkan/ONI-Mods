// Decompiled with JetBrains decompiler
// Type: UniformGrid`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformGrid<T> where T : IUniformGridObject
{
  private List<T>[] cells;
  private List<T> items;
  private int cellWidth;
  private int cellHeight;
  private int numXCells;
  private int numYCells;

  public UniformGrid()
  {
  }

  public UniformGrid(int width, int height, int cellWidth, int cellHeight) => this.Reset(width, height, cellWidth, cellHeight);

  public void Reset(int width, int height, int cellWidth, int cellHeight)
  {
    this.cellWidth = cellWidth;
    this.cellHeight = cellHeight;
    this.numXCells = (int) Math.Ceiling((double) width / (double) cellWidth);
    this.numYCells = (int) Math.Ceiling((double) height / (double) cellHeight);
    this.cells = new List<T>[this.numXCells * this.numYCells];
    this.items = new List<T>();
  }

  public void Clear()
  {
    this.cellWidth = 0;
    this.cellHeight = 0;
    this.numXCells = 0;
    this.numYCells = 0;
    this.cells = (List<T>[]) null;
  }

  public void Add(T item)
  {
    Vector2 vector2_1 = item.PosMin();
    Vector2 vector2_2 = item.PosMax();
    int num1 = (int) Math.Max(vector2_1.x / (float) this.cellWidth, 0.0f);
    int num2 = (int) Math.Max(vector2_2.y / (float) this.cellHeight, 0.0f);
    int num3 = Math.Min(this.numXCells - 1, (int) Math.Ceiling((double) vector2_2.x / (double) this.cellWidth));
    int num4 = Math.Min(this.numYCells - 1, (int) Math.Ceiling((double) vector2_2.y / (double) this.cellHeight));
    for (int index1 = num2; index1 <= num4; ++index1)
    {
      for (int index2 = num1; index2 <= num3; ++index2)
      {
        int index3 = index1 * this.numXCells + index2;
        List<T> objList = this.cells[index3];
        if (objList == null)
        {
          objList = new List<T>();
          this.cells[index3] = objList;
        }
        objList.Add(item);
        this.items.Add(item);
      }
    }
  }

  public void Remove(T item)
  {
    Vector2 vector2_1 = item.PosMin();
    Vector2 vector2_2 = item.PosMax();
    int num1 = (int) Math.Max(vector2_1.x / (float) this.cellWidth, 0.0f);
    int num2 = (int) Math.Max(vector2_2.y / (float) this.cellHeight, 0.0f);
    int num3 = Math.Min(this.numXCells - 1, (int) Math.Ceiling((double) vector2_2.x / (double) this.cellWidth));
    int num4 = Math.Min(this.numYCells - 1, (int) Math.Ceiling((double) vector2_2.y / (double) this.cellHeight));
    for (int index1 = num2; index1 <= num4; ++index1)
    {
      for (int index2 = num1; index2 <= num3; ++index2)
      {
        List<T> cell = this.cells[index1 * this.numXCells + index2];
        if (cell != null && cell.IndexOf(item) != -1)
        {
          cell.Remove(item);
          this.items.Remove(item);
        }
      }
    }
  }

  public IEnumerable GetAllIntersecting(IUniformGridObject item) => this.GetAllIntersecting(item.PosMin(), item.PosMax());

  public IEnumerable GetAllIntersecting(Vector2 pos) => this.GetAllIntersecting(pos, pos);

  public IEnumerable GetAllIntersecting(Vector2 min, Vector2 max)
  {
    HashSet<T> objSet = new HashSet<T>();
    this.GetAllIntersecting(min, max, (ICollection<T>) objSet);
    return (IEnumerable) objSet;
  }

  public void GetAllIntersecting(Vector2 min, Vector2 max, ICollection<T> results)
  {
    int num1 = Math.Max(0, Math.Min((int) ((double) min.x / (double) this.cellWidth), this.numXCells - 1));
    int num2 = Math.Max(0, Math.Min((int) Math.Ceiling((double) max.x / (double) this.cellWidth), this.numXCells - 1));
    int num3 = Math.Max(0, Math.Min((int) ((double) min.y / (double) this.cellHeight), this.numYCells - 1));
    int num4 = Math.Max(0, Math.Min((int) Math.Ceiling((double) max.y / (double) this.cellHeight), this.numYCells - 1));
    for (int index1 = num3; index1 <= num4; ++index1)
    {
      for (int index2 = num1; index2 <= num2; ++index2)
      {
        List<T> cell = this.cells[index1 * this.numXCells + index2];
        if (cell != null)
        {
          for (int index3 = 0; index3 < cell.Count; ++index3)
            results.Add(cell[index3]);
        }
      }
    }
  }

  public ICollection<T> GetAllItems() => (ICollection<T>) this.items;
}
