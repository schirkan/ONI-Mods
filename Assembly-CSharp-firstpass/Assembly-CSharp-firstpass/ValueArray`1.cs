// Decompiled with JetBrains decompiler
// Type: ValueArray`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class ValueArray<T>
{
  public int Count;
  public T[] Values;

  public ValueArray(int reserve_size) => this.Values = new T[reserve_size];

  public T this[int idx] => this.Values[idx];

  public void Add(ref T value)
  {
    if (this.Count == this.Values.Length)
      this.Resize(this.Values.Length * 2);
    this.Values[this.Count] = value;
    ++this.Count;
  }

  public void Resize(int new_size)
  {
    T[] objArray = new T[new_size];
    for (int index = 0; index < this.Values.Length; ++index)
      objArray[index] = this.Values[index];
    this.Values = objArray;
  }

  public void Remove(int index)
  {
    if (this.Count > 0)
      this.Values[index] = this.Values[this.Count - 1];
    --this.Count;
  }

  public void Clear() => this.Count = 0;

  public bool IsEqual(ValueArray<T> array)
  {
    if (this.Count != array.Count)
      return false;
    for (int index = 0; index < this.Count; ++index)
    {
      if (!this.Values[index].Equals((object) array.Values[index]))
        return false;
    }
    return true;
  }

  public void CopyFrom(ValueArray<T> array)
  {
    this.Clear();
    for (int idx = 0; idx < array.Count; ++idx)
    {
      T obj = array[idx];
      this.Add(ref obj);
    }
  }
}
