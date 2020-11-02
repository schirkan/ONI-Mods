// Decompiled with JetBrains decompiler
// Type: HandleValueArray`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class HandleValueArray<T> : HandleValueArrayBase
{
  public int Count;
  public HandleValueArray<T>.Entry[] Entries;
  private int[] Indices;

  public HandleValueArray(int reserve_size)
  {
    this.Entries = new HandleValueArray<T>.Entry[reserve_size];
    this.Indices = new int[reserve_size];
    for (int index = 0; index < this.Entries.Length; ++index)
      this.Entries[index].Handle = index;
  }

  public ValueArrayHandle Add(ref T value)
  {
    if (this.Count == this.Entries.Length)
    {
      HandleValueArray<T>.Entry[] entryArray = new HandleValueArray<T>.Entry[this.Entries.Length * 2];
      int[] numArray = new int[this.Entries.Length * 2];
      for (int index = 0; index < this.Entries.Length; ++index)
      {
        entryArray[index] = this.Entries[index];
        numArray[index] = this.Indices[index];
      }
      for (int length = this.Entries.Length; length < entryArray.Length; ++length)
        entryArray[length].Handle = length;
      this.Entries = entryArray;
      this.Indices = numArray;
    }
    int handle = this.Entries[this.Count].Handle;
    int count = this.Count;
    this.Entries[count].Value = value;
    this.Indices[handle] = count;
    ++this.Count;
    return new ValueArrayHandle(handle);
  }

  public int GetIndex(ref ValueArrayHandle handle) => this.Indices[handle.handle];

  public void Remove(ref ValueArrayHandle handle)
  {
    int index = this.Indices[handle.handle];
    --this.Count;
    int handle1 = this.Entries[this.Count].Handle;
    this.Entries[index] = this.Entries[this.Count];
    this.Entries[this.Count].Handle = handle.handle;
    this.Indices[handle1] = index;
  }

  public struct Entry
  {
    public T Value;
    public int Handle;
  }
}
