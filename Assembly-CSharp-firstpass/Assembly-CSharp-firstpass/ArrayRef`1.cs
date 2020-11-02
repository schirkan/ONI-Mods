// Decompiled with JetBrains decompiler
// Type: ArrayRef`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public struct ArrayRef<T>
{
  [Serialize]
  private T[] elements;
  [Serialize]
  private int sizeImpl;
  [Serialize]
  private int capacityImpl;

  public T this[int i]
  {
    get
    {
      this.ValidateIndex(i);
      return this.elements[i];
    }
    set
    {
      this.ValidateIndex(i);
      this.elements[i] = value;
    }
  }

  public int size => this.sizeImpl;

  public int Count => this.size;

  public int capacity => this.capacityImpl;

  public ArrayRef(int initialCapacity)
  {
    this.capacityImpl = initialCapacity;
    this.elements = new T[initialCapacity];
    this.sizeImpl = 0;
  }

  public ArrayRef(T[] elements, int size)
  {
    Debug.Assert(size <= elements.Length);
    this.elements = elements;
    this.sizeImpl = size;
    this.capacityImpl = elements.Length;
  }

  public int Add(T item)
  {
    this.MaybeGrow(this.size);
    this.elements[this.size] = item;
    ++this.sizeImpl;
    return this.size;
  }

  public bool RemoveFirst(Predicate<T> match)
  {
    int index = this.FindIndex(match);
    if (index == -1)
      return false;
    this.RemoveAt(index);
    return true;
  }

  public bool RemoveFirstSwap(Predicate<T> match)
  {
    int index = this.FindIndex(match);
    if (index == -1)
      return false;
    this.RemoveAtSwap(index);
    return true;
  }

  public void RemoveAt(int index)
  {
    this.ValidateIndex(index);
    for (int index1 = index; index1 != this.size - 1; ++index1)
      this.elements[index1] = this.elements[index1 + 1];
    --this.sizeImpl;
    DebugUtil.Assert(this.sizeImpl >= 0);
  }

  public void RemoveAtSwap(int index)
  {
    this.ValidateIndex(index);
    this.elements[index] = this.elements[this.size - 1];
    --this.sizeImpl;
    DebugUtil.Assert(this.sizeImpl >= 0);
  }

  public void RemoveAll(Predicate<T> match)
  {
    for (int index = this.size - 1; index != -1; --index)
    {
      if (match(this.elements[index]))
        this.RemoveAt(index);
    }
  }

  public void RemoveAllSwap(Predicate<T> match)
  {
    int index = 0;
    while (index != this.size)
    {
      if (match(this.elements[index]))
      {
        this.elements[index] = this.elements[this.size - 1];
        --this.sizeImpl;
        DebugUtil.Assert(this.sizeImpl >= 0);
      }
      else
        ++index;
    }
  }

  public void Clear() => this.sizeImpl = 0;

  public int FindIndex(Predicate<T> match)
  {
    for (int index = 0; index != this.size; ++index)
    {
      if (match(this.elements[index]))
        return index;
    }
    return -1;
  }

  public void ShrinkToFit()
  {
    if (this.size == this.capacity)
      return;
    this.Reallocate(this.size);
  }

  private void ValidateIndex(int index)
  {
  }

  private void MaybeGrow(int index)
  {
    DebugUtil.Assert(this.capacity == 0 || this.capacity == this.elements.Length);
    DebugUtil.Assert(index >= 0);
    if (index < this.capacity)
      return;
    this.Reallocate(this.capacity == 0 ? 1 : this.capacity * 2);
    DebugUtil.Assert(this.capacity == 0 || this.capacity == this.elements.Length);
  }

  private void Reallocate(int newCapacity)
  {
    Debug.Assert(this.size <= newCapacity);
    this.capacityImpl = newCapacity;
    T[] objArray = new T[this.capacity];
    for (int index = 0; index != this.size; ++index)
      objArray[index] = this.elements[index];
    this.elements = objArray;
  }
}
