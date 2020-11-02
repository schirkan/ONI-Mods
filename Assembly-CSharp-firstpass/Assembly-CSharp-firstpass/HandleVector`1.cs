// Decompiled with JetBrains decompiler
// Type: HandleVector`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class HandleVector<T>
{
  public static readonly HandleVector<T>.Handle InvalidHandle = HandleVector<T>.Handle.InvalidHandle;
  protected Stack<HandleVector<T>.Handle> freeHandles;
  protected List<T> items;
  protected List<byte> versions;

  public List<T> Items => this.items;

  public Stack<HandleVector<T>.Handle> Handles => this.freeHandles;

  public virtual void Clear()
  {
    this.items.Clear();
    this.freeHandles.Clear();
    this.versions.Clear();
  }

  public HandleVector(int initial_size)
  {
    this.freeHandles = new Stack<HandleVector<T>.Handle>(initial_size);
    this.items = new List<T>(initial_size);
    this.versions = new List<byte>(initial_size);
    this.Initialize(initial_size);
  }

  private void Initialize(int size)
  {
    for (int index = size - 1; index >= 0; --index)
    {
      this.freeHandles.Push(new HandleVector<T>.Handle()
      {
        index = index
      });
      this.items.Add(default (T));
      this.versions.Add((byte) 0);
    }
  }

  public virtual HandleVector<T>.Handle Add(T item)
  {
    HandleVector<T>.Handle handle;
    if (this.freeHandles.Count > 0)
    {
      handle = this.freeHandles.Pop();
      int index;
      this.UnpackHandle(handle, out byte _, out index);
      this.items[index] = item;
    }
    else
    {
      this.versions.Add((byte) 0);
      handle = this.PackHandle(this.items.Count);
      this.items.Add(item);
    }
    return handle;
  }

  public virtual T Release(HandleVector<T>.Handle handle)
  {
    if (!handle.IsValid())
      return default (T);
    byte version;
    int index;
    this.UnpackHandle(handle, out version, out index);
    ++version;
    this.versions[index] = version;
    Debug.Assert(index >= 0);
    Debug.Assert(index < 16777216);
    handle = this.PackHandle(index);
    this.freeHandles.Push(handle);
    T obj = this.items[index];
    this.items[index] = default (T);
    return obj;
  }

  public T GetItem(HandleVector<T>.Handle handle)
  {
    int index;
    this.UnpackHandle(handle, out byte _, out index);
    return this.items[index];
  }

  private HandleVector<T>.Handle PackHandle(int index)
  {
    Debug.Assert(index < 16777216);
    byte version = this.versions[index];
    this.versions[index] = version;
    HandleVector<T>.Handle invalidHandle = HandleVector<T>.InvalidHandle;
    invalidHandle.index = (int) version << 24 | index;
    return invalidHandle;
  }

  public void UnpackHandle(HandleVector<T>.Handle handle, out byte version, out int index)
  {
    version = (byte) (handle.index >> 24);
    index = handle.index & 16777215;
    if ((int) this.versions[index] != (int) version)
      throw new ArgumentException("Accessing mismatched handle version. Expected version=" + this.versions[index].ToString() + " but got version=" + version.ToString());
  }

  public void UnpackHandleUnchecked(HandleVector<T>.Handle handle, out byte version, out int index)
  {
    version = (byte) (handle.index >> 24);
    index = handle.index & 16777215;
  }

  public bool IsValid(HandleVector<T>.Handle handle) => (handle.index & 16777215) != 16777215;

  public bool IsVersionValid(HandleVector<T>.Handle handle) => (int) (byte) (handle.index >> 24) == (int) this.versions[handle.index & 16777215];

  [DebuggerDisplay("{index}")]
  public struct Handle : IComparable<HandleVector<T>.Handle>, IEquatable<HandleVector<T>.Handle>
  {
    private const int InvalidIndex = 0;
    private int _index;
    public static readonly HandleVector<T>.Handle InvalidHandle = new HandleVector<T>.Handle()
    {
      _index = 0
    };

    public int index
    {
      get => this._index - 1;
      set => this._index = value + 1;
    }

    public bool IsValid() => (uint) this._index > 0U;

    public void Clear() => this._index = 0;

    public int CompareTo(HandleVector<T>.Handle obj) => this._index - obj._index;

    public override bool Equals(object obj) => this._index == ((HandleVector<T>.Handle) obj)._index;

    public bool Equals(HandleVector<T>.Handle other) => this._index == other._index;

    public override int GetHashCode() => this._index;

    public static bool operator ==(HandleVector<T>.Handle x, HandleVector<T>.Handle y) => x._index == y._index;

    public static bool operator !=(HandleVector<T>.Handle x, HandleVector<T>.Handle y) => x._index != y._index;
  }
}
