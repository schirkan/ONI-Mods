// Decompiled with JetBrains decompiler
// Type: KSplitCompactedVector`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class KSplitCompactedVector<Header, Payload> : KCompactedVectorBase, ICollection, IEnumerable
{
  protected List<Header> headers;
  protected List<Payload> payloads;

  public KSplitCompactedVector(int initial_count = 0)
    : base(initial_count)
  {
    this.headers = new List<Header>(initial_count);
    this.payloads = new List<Payload>(initial_count);
  }

  public HandleVector<int>.Handle Allocate(Header header, ref Payload payload)
  {
    this.headers.Add(header);
    this.payloads.Add(payload);
    return this.Allocate(this.headers.Count - 1);
  }

  public HandleVector<int>.Handle Free(HandleVector<int>.Handle handle)
  {
    int num1 = this.headers.Count - 1;
    int free_component_idx;
    int num2 = this.Free(handle, num1, out free_component_idx) ? 1 : 0;
    if (num2 != 0)
    {
      if (free_component_idx < num1)
      {
        this.headers[free_component_idx] = this.headers[num1];
        this.payloads[free_component_idx] = this.payloads[num1];
      }
      this.headers.RemoveAt(num1);
      this.payloads.RemoveAt(num1);
    }
    return num2 == 0 ? handle : HandleVector<int>.InvalidHandle;
  }

  public void GetData(HandleVector<int>.Handle handle, out Header header, out Payload payload)
  {
    int index = this.ComputeIndex(handle);
    header = this.headers[index];
    payload = this.payloads[index];
  }

  public Header GetHeader(HandleVector<int>.Handle handle) => this.headers[this.ComputeIndex(handle)];

  public Payload GetPayload(HandleVector<int>.Handle handle) => this.payloads[this.ComputeIndex(handle)];

  public void SetData(HandleVector<int>.Handle handle, Header new_data0, ref Payload new_data1)
  {
    int index = this.ComputeIndex(handle);
    this.headers[index] = new_data0;
    this.payloads[index] = new_data1;
  }

  public void SetHeader(HandleVector<int>.Handle handle, Header new_data) => this.headers[this.ComputeIndex(handle)] = new_data;

  public void SetPayload(HandleVector<int>.Handle handle, ref Payload new_data) => this.payloads[this.ComputeIndex(handle)] = new_data;

  public new virtual void Clear()
  {
    base.Clear();
    this.headers.Clear();
    this.payloads.Clear();
  }

  public int Count => this.headers.Count;

  public void GetDataLists(out List<Header> headers, out List<Payload> payloads)
  {
    headers = this.headers;
    payloads = this.payloads;
  }

  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  public void CopyTo(Array array, int index) => throw new NotImplementedException();

  public IEnumerator GetEnumerator() => (IEnumerator) new KSplitCompactedVector<Header, Payload>.Enumerator(this.headers.GetEnumerator(), this.payloads.GetEnumerator());

  private struct Enumerator : IEnumerator
  {
    private readonly List<Header>.Enumerator headerBegin;
    private readonly List<Payload>.Enumerator payloadBegin;
    private List<Header>.Enumerator headerCurrent;
    private List<Payload>.Enumerator payloadCurrent;

    public object Current => (object) new KSplitCompactedVector<Header, Payload>.Enumerator.Value()
    {
      header = this.headerCurrent.Current,
      payload = this.payloadCurrent.Current
    };

    public Enumerator(
      List<Header>.Enumerator headerEnumerator,
      List<Payload>.Enumerator payloadEnumerator)
    {
      this.headerBegin = headerEnumerator;
      this.payloadBegin = payloadEnumerator;
      this.Reset();
    }

    public bool MoveNext() => this.headerCurrent.MoveNext() && this.payloadCurrent.MoveNext();

    public void Reset()
    {
      this.headerCurrent = this.headerBegin;
      this.payloadCurrent = this.payloadBegin;
    }

    public struct Value
    {
      public Header header;
      public Payload payload;
    }
  }
}
