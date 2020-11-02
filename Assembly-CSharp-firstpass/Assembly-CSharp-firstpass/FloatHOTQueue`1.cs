// Decompiled with JetBrains decompiler
// Type: FloatHOTQueue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class FloatHOTQueue<TValue>
{
  private FloatHOTQueue<TValue>.PriorityQueue hotQueue = new FloatHOTQueue<TValue>.PriorityQueue();
  private FloatHOTQueue<TValue>.PriorityQueue coldQueue = new FloatHOTQueue<TValue>.PriorityQueue();
  private float hotThreshold = float.MinValue;
  private float coldThreshold = float.MinValue;
  private int count;

  public KeyValuePair<float, TValue> Dequeue()
  {
    if (this.hotQueue.Count == 0)
    {
      FloatHOTQueue<TValue>.PriorityQueue hotQueue = this.hotQueue;
      this.hotQueue = this.coldQueue;
      this.coldQueue = hotQueue;
      this.hotThreshold = this.coldThreshold;
    }
    --this.count;
    return this.hotQueue.Dequeue();
  }

  public void Enqueue(float priority, TValue value)
  {
    if ((double) priority <= (double) this.hotThreshold)
    {
      this.hotQueue.Enqueue(priority, value);
    }
    else
    {
      this.coldQueue.Enqueue(priority, value);
      this.coldThreshold = Math.Max(this.coldThreshold, priority);
    }
    ++this.count;
  }

  public KeyValuePair<float, TValue> Peek()
  {
    if (this.hotQueue.Count == 0)
    {
      FloatHOTQueue<TValue>.PriorityQueue hotQueue = this.hotQueue;
      this.hotQueue = this.coldQueue;
      this.coldQueue = hotQueue;
      this.hotThreshold = this.coldThreshold;
    }
    return this.hotQueue.Peek();
  }

  public void Clear()
  {
    this.count = 0;
    this.hotThreshold = float.MinValue;
    this.hotQueue.Clear();
    this.coldThreshold = float.MinValue;
    this.coldQueue.Clear();
  }

  public int Count => this.count;

  private class PriorityQueue
  {
    private List<KeyValuePair<float, TValue>> _baseHeap;

    public PriorityQueue() => this._baseHeap = new List<KeyValuePair<float, TValue>>();

    public void Enqueue(float priority, TValue value) => this.Insert(priority, value);

    public KeyValuePair<float, TValue> Dequeue()
    {
      KeyValuePair<float, TValue> keyValuePair = this._baseHeap[0];
      this.DeleteRoot();
      return keyValuePair;
    }

    public KeyValuePair<float, TValue> Peek()
    {
      if (this.Count > 0)
        return this._baseHeap[0];
      throw new InvalidOperationException("Priority queue is empty");
    }

    private void ExchangeElements(int pos1, int pos2)
    {
      KeyValuePair<float, TValue> keyValuePair = this._baseHeap[pos1];
      this._baseHeap[pos1] = this._baseHeap[pos2];
      this._baseHeap[pos2] = keyValuePair;
    }

    private void Insert(float priority, TValue value)
    {
      this._baseHeap.Add(new KeyValuePair<float, TValue>(priority, value));
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
        KeyValuePair<float, TValue> keyValuePair = this._baseHeap[num];
        double key1 = (double) keyValuePair.Key;
        keyValuePair = this._baseHeap[pos];
        double key2 = (double) keyValuePair.Key;
        if (key1 - key2 > 0.0)
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
      int count = this._baseHeap.Count;
      if (pos >= count)
        return;
      while (true)
      {
        int num = pos;
        int index1 = 2 * pos + 1;
        int index2 = 2 * pos + 2;
        if (index1 < count && (double) this._baseHeap[num].Key - (double) this._baseHeap[index1].Key > 0.0)
          num = index1;
        if (index2 < count && (double) this._baseHeap[num].Key - (double) this._baseHeap[index2].Key > 0.0)
          num = index2;
        if (num != pos)
        {
          this.ExchangeElements(num, pos);
          pos = num;
        }
        else
          break;
      }
    }

    public void Clear() => this._baseHeap.Clear();

    public int Count => this._baseHeap.Count;
  }
}
