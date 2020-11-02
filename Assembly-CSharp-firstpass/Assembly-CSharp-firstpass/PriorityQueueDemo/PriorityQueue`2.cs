// Decompiled with JetBrains decompiler
// Type: PriorityQueueDemo.PriorityQueue`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace PriorityQueueDemo
{
  public class PriorityQueue<TPriority, TValue> : ICollection<KeyValuePair<TPriority, TValue>>, IEnumerable<KeyValuePair<TPriority, TValue>>, IEnumerable
  {
    private List<KeyValuePair<TPriority, TValue>> _baseHeap;
    private IComparer<TPriority> _comparer;

    public PriorityQueue()
      : this((IComparer<TPriority>) Comparer<TPriority>.Default)
    {
    }

    public PriorityQueue(int capacity)
      : this(capacity, (IComparer<TPriority>) Comparer<TPriority>.Default)
    {
    }

    public PriorityQueue(int capacity, IComparer<TPriority> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException();
      this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(capacity);
      this._comparer = comparer;
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException();
      this._baseHeap = new List<KeyValuePair<TPriority, TValue>>();
      this._comparer = comparer;
    }

    public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
      : this(data, (IComparer<TPriority>) Comparer<TPriority>.Default)
    {
    }

    public PriorityQueue(
      IEnumerable<KeyValuePair<TPriority, TValue>> data,
      IComparer<TPriority> comparer)
    {
      this._comparer = data != null && comparer != null ? comparer : throw new ArgumentNullException();
      this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(data);
      for (int pos = this._baseHeap.Count / 2 - 1; pos >= 0; --pos)
        this.HeapifyFromBeginningToEnd(pos);
    }

    public static PriorityQueue<TPriority, TValue> MergeQueues(
      PriorityQueue<TPriority, TValue> pq1,
      PriorityQueue<TPriority, TValue> pq2)
    {
      if (pq1 == null || pq2 == null)
        throw new ArgumentNullException();
      if (pq1._comparer != pq2._comparer)
        throw new InvalidOperationException("Priority queues to be merged must have equal comparers");
      return PriorityQueue<TPriority, TValue>.MergeQueues(pq1, pq2, pq1._comparer);
    }

    public static PriorityQueue<TPriority, TValue> MergeQueues(
      PriorityQueue<TPriority, TValue> pq1,
      PriorityQueue<TPriority, TValue> pq2,
      IComparer<TPriority> comparer)
    {
      if (pq1 == null || pq2 == null || comparer == null)
        throw new ArgumentNullException();
      PriorityQueue<TPriority, TValue> priorityQueue = new PriorityQueue<TPriority, TValue>(pq1.Count + pq2.Count, pq1._comparer);
      priorityQueue._baseHeap.AddRange((IEnumerable<KeyValuePair<TPriority, TValue>>) pq1._baseHeap);
      priorityQueue._baseHeap.AddRange((IEnumerable<KeyValuePair<TPriority, TValue>>) pq2._baseHeap);
      for (int pos = priorityQueue._baseHeap.Count / 2 - 1; pos >= 0; --pos)
        priorityQueue.HeapifyFromBeginningToEnd(pos);
      return priorityQueue;
    }

    public void Enqueue(TPriority priority, TValue value) => this.Insert(priority, value);

    public KeyValuePair<TPriority, TValue> Dequeue()
    {
      if (this.IsEmpty)
        throw new InvalidOperationException("Priority queue is empty");
      KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[0];
      this.DeleteRoot();
      return keyValuePair;
    }

    public TValue DequeueValue() => this.Dequeue().Value;

    public KeyValuePair<TPriority, TValue> Peek()
    {
      if (!this.IsEmpty)
        return this._baseHeap[0];
      throw new InvalidOperationException("Priority queue is empty");
    }

    public TValue PeekValue() => this.Peek().Value;

    public bool IsEmpty => this._baseHeap.Count == 0;

    private void ExchangeElements(int pos1, int pos2)
    {
      KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[pos1];
      this._baseHeap[pos1] = this._baseHeap[pos2];
      this._baseHeap[pos2] = keyValuePair;
    }

    private void Insert(TPriority priority, TValue value)
    {
      this._baseHeap.Add(new KeyValuePair<TPriority, TValue>(priority, value));
      this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
    }

    private int HeapifyFromEndToBeginning(int pos)
    {
      if (pos >= this._baseHeap.Count)
        return -1;
      int num;
      for (; pos > 0; pos = num)
      {
        num = (pos - 1) / 2;
        IComparer<TPriority> comparer = this._comparer;
        KeyValuePair<TPriority, TValue> keyValuePair = this._baseHeap[num];
        TPriority key1 = keyValuePair.Key;
        keyValuePair = this._baseHeap[pos];
        TPriority key2 = keyValuePair.Key;
        if (comparer.Compare(key1, key2) > 0)
          this.ExchangeElements(num, pos);
        else
          break;
      }
      return pos;
    }

    private void DeleteRoot()
    {
      if (this._baseHeap.Count <= 1)
      {
        this._baseHeap.Clear();
      }
      else
      {
        this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
        this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
        this.HeapifyFromBeginningToEnd(0);
      }
    }

    private void HeapifyFromBeginningToEnd(int pos)
    {
      if (pos >= this._baseHeap.Count)
        return;
      while (true)
      {
        int num = pos;
        int index1 = 2 * pos + 1;
        int index2 = 2 * pos + 2;
        KeyValuePair<TPriority, TValue> keyValuePair;
        if (index1 < this._baseHeap.Count)
        {
          IComparer<TPriority> comparer = this._comparer;
          keyValuePair = this._baseHeap[num];
          TPriority key1 = keyValuePair.Key;
          keyValuePair = this._baseHeap[index1];
          TPriority key2 = keyValuePair.Key;
          if (comparer.Compare(key1, key2) > 0)
            num = index1;
        }
        if (index2 < this._baseHeap.Count)
        {
          IComparer<TPriority> comparer = this._comparer;
          keyValuePair = this._baseHeap[num];
          TPriority key1 = keyValuePair.Key;
          keyValuePair = this._baseHeap[index2];
          TPriority key2 = keyValuePair.Key;
          if (comparer.Compare(key1, key2) > 0)
            num = index2;
        }
        if (num != pos)
        {
          this.ExchangeElements(num, pos);
          pos = num;
        }
        else
          break;
      }
    }

    public void Add(KeyValuePair<TPriority, TValue> item) => this.Enqueue(item.Key, item.Value);

    public void Clear() => this._baseHeap.Clear();

    public bool Contains(KeyValuePair<TPriority, TValue> item) => this._baseHeap.Contains(item);

    public int Count => this._baseHeap.Count;

    public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int arrayIndex) => this._baseHeap.CopyTo(array, arrayIndex);

    public bool IsReadOnly => false;

    public bool Remove(KeyValuePair<TPriority, TValue> item)
    {
      int num = this._baseHeap.IndexOf(item);
      if (num < 0)
        return false;
      this._baseHeap[num] = this._baseHeap[this._baseHeap.Count - 1];
      this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
      if (this.HeapifyFromEndToBeginning(num) == num)
        this.HeapifyFromBeginningToEnd(num);
      return true;
    }

    public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator() => (IEnumerator<KeyValuePair<TPriority, TValue>>) this._baseHeap.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
