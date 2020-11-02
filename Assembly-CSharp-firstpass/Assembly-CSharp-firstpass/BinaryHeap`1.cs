// Decompiled with JetBrains decompiler
// Type: BinaryHeap`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class BinaryHeap<T> : IEnumerable<T>, IEnumerable
{
  private IComparer<T> Comparer;
  private List<T> Items = new List<T>();

  public BinaryHeap()
    : this((IComparer<T>) System.Collections.Generic.Comparer<T>.Default)
  {
  }

  public BinaryHeap(IComparer<T> comp) => this.Comparer = comp;

  public int Count => this.Items.Count;

  public void Clear() => this.Items.Clear();

  public void TrimExcess() => this.Items.TrimExcess();

  public void Insert(T newItem)
  {
    int index = this.Count;
    this.Items.Add(newItem);
    for (; index > 0 && this.Comparer.Compare(this.Items[(index - 1) / 2], newItem) > 0; index = (index - 1) / 2)
      this.Items[index] = this.Items[(index - 1) / 2];
    this.Items[index] = newItem;
  }

  public T Peek() => this.Items.Count != 0 ? this.Items[0] : throw new InvalidOperationException("The heap is empty.");

  public T RemoveRoot()
  {
    T obj = this.Items.Count != 0 ? this.Items[0] : throw new InvalidOperationException("The heap is empty.");
    T y = this.Items[this.Items.Count - 1];
    this.Items.RemoveAt(this.Items.Count - 1);
    if (this.Items.Count > 0)
    {
      int index1;
      int index2;
      for (index1 = 0; index1 < this.Items.Count / 2; index1 = index2)
      {
        index2 = 2 * index1 + 1;
        if (index2 < this.Items.Count - 1 && this.Comparer.Compare(this.Items[index2], this.Items[index2 + 1]) > 0)
          ++index2;
        if (this.Comparer.Compare(this.Items[index2], y) < 0)
          this.Items[index1] = this.Items[index2];
        else
          break;
      }
      this.Items[index1] = y;
    }
    return obj;
  }

  IEnumerator<T> IEnumerable<T>.GetEnumerator()
  {
    foreach (T obj in this.Items)
      yield return obj;
  }

  public IEnumerator GetEnumerator() => this.GetEnumerator();
}
