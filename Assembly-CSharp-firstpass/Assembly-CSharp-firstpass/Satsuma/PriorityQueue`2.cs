// Decompiled with JetBrains decompiler
// Type: Satsuma.PriorityQueue`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>, IReadOnlyPriorityQueue<TElement, TPriority>, IClearable
    where TPriority : IComparable<TPriority>
  {
    private List<TElement> payloads = new List<TElement>();
    private List<TPriority> priorities = new List<TPriority>();
    private Dictionary<TElement, int> positions = new Dictionary<TElement, int>();

    public void Clear()
    {
      this.payloads.Clear();
      this.priorities.Clear();
      this.positions.Clear();
    }

    public int Count => this.payloads.Count;

    public IEnumerable<KeyValuePair<TElement, TPriority>> Items
    {
      get
      {
        int i = 0;
        for (int n = this.Count; i < n; ++i)
          yield return new KeyValuePair<TElement, TPriority>(this.payloads[i], this.priorities[i]);
      }
    }

    public TPriority this[TElement element]
    {
      get => this.priorities[this.positions[element]];
      set
      {
        int index1;
        if (this.positions.TryGetValue(element, out index1))
        {
          TPriority priority = this.priorities[index1];
          this.priorities[index1] = value;
          int num = value.CompareTo(priority);
          if (num < 0)
          {
            this.MoveUp(index1);
          }
          else
          {
            if (num <= 0)
              return;
            this.MoveDown(index1);
          }
        }
        else
        {
          this.payloads.Add(element);
          this.priorities.Add(value);
          int index2 = this.Count - 1;
          this.positions[element] = index2;
          this.MoveUp(index2);
        }
      }
    }

    public bool Contains(TElement element) => this.positions.ContainsKey(element);

    public bool TryGetPriority(TElement element, out TPriority priority)
    {
      int index;
      if (!this.positions.TryGetValue(element, out index))
      {
        priority = default (TPriority);
        return false;
      }
      priority = this.priorities[index];
      return true;
    }

    private void RemoveAt(int pos)
    {
      int count = this.Count;
      TElement payload = this.payloads[pos];
      TPriority priority = this.priorities[pos];
      this.positions.Remove(payload);
      int num1 = count <= 1 ? 1 : 0;
      if (num1 == 0 && pos != count - 1)
      {
        this.payloads[pos] = this.payloads[count - 1];
        this.priorities[pos] = this.priorities[count - 1];
        this.positions[this.payloads[pos]] = pos;
      }
      this.payloads.RemoveAt(count - 1);
      this.priorities.RemoveAt(count - 1);
      if (num1 != 0 || pos == count - 1)
        return;
      int num2 = this.priorities[pos].CompareTo(priority);
      if (num2 > 0)
      {
        this.MoveDown(pos);
      }
      else
      {
        if (num2 >= 0)
          return;
        this.MoveUp(pos);
      }
    }

    public bool Remove(TElement element)
    {
      int pos;
      int num = this.positions.TryGetValue(element, out pos) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.RemoveAt(pos);
      return num != 0;
    }

    public TElement Peek() => this.payloads[0];

    public TElement Peek(out TPriority priority)
    {
      priority = this.priorities[0];
      return this.payloads[0];
    }

    public bool Pop()
    {
      if (this.Count == 0)
        return false;
      this.RemoveAt(0);
      return true;
    }

    private void MoveUp(int index)
    {
      TElement payload = this.payloads[index];
      TPriority priority = this.priorities[index];
      int index1;
      int index2;
      for (index1 = index; index1 > 0; index1 = index2)
      {
        index2 = index1 / 2;
        if (priority.CompareTo(this.priorities[index2]) < 0)
        {
          this.payloads[index1] = this.payloads[index2];
          this.priorities[index1] = this.priorities[index2];
          this.positions[this.payloads[index1]] = index1;
        }
        else
          break;
      }
      if (index1 == index)
        return;
      this.payloads[index1] = payload;
      this.priorities[index1] = priority;
      this.positions[payload] = index1;
    }

    private void MoveDown(int index)
    {
      TElement payload = this.payloads[index];
      TPriority priority = this.priorities[index];
      int index1;
      int index2;
      for (index1 = index; 2 * index1 < this.Count; index1 = index2)
      {
        index2 = index1;
        int index3 = 2 * index1;
        if (priority.CompareTo(this.priorities[index3]) >= 0)
          index2 = index3;
        int index4 = index3 + 1;
        if (index4 < this.Count && this.priorities[index2].CompareTo(this.priorities[index4]) >= 0)
          index2 = index4;
        if (index2 != index1)
        {
          this.payloads[index1] = this.payloads[index2];
          this.priorities[index1] = this.priorities[index2];
          this.positions[this.payloads[index1]] = index1;
        }
        else
          break;
      }
      if (index1 == index)
        return;
      this.payloads[index1] = payload;
      this.priorities[index1] = priority;
      this.positions[payload] = index1;
    }
  }
}
