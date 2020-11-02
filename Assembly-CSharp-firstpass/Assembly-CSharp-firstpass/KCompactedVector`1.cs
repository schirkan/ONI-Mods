// Decompiled with JetBrains decompiler
// Type: KCompactedVector`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class KCompactedVector<T> : KCompactedVectorBase, ICollection, IEnumerable
{
  protected List<T> data;

  public KCompactedVector(int initial_count = 0)
    : base(initial_count)
    => this.data = new List<T>(initial_count);

  public HandleVector<int>.Handle Allocate(T initial_data)
  {
    this.data.Add(initial_data);
    return this.Allocate(this.data.Count - 1);
  }

  public HandleVector<int>.Handle Free(HandleVector<int>.Handle handle)
  {
    int num1 = this.data.Count - 1;
    int free_component_idx;
    int num2 = this.Free(handle, num1, out free_component_idx) ? 1 : 0;
    if (num2 != 0)
    {
      if (free_component_idx < num1)
        this.data[free_component_idx] = this.data[num1];
      this.data.RemoveAt(num1);
    }
    return num2 == 0 ? handle : HandleVector<int>.InvalidHandle;
  }

  public T GetData(HandleVector<int>.Handle handle) => this.data[this.ComputeIndex(handle)];

  public void SetData(HandleVector<int>.Handle handle, T new_data) => this.data[this.ComputeIndex(handle)] = new_data;

  public new virtual void Clear()
  {
    base.Clear();
    this.data.Clear();
  }

  public int Count => this.data.Count;

  public List<T> GetDataList() => this.data;

  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  public void CopyTo(Array array, int index) => throw new NotImplementedException();

  public IEnumerator GetEnumerator() => (IEnumerator) this.data.GetEnumerator();
}
