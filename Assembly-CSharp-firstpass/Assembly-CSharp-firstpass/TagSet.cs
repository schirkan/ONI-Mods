// Decompiled with JetBrains decompiler
// Type: TagSet
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class TagSet : ICollection<Tag>, IEnumerable<Tag>, IEnumerable, ICollection
{
  [Serialize]
  [SerializeField]
  private List<Tag> tags = new List<Tag>();

  public TagSet() => this.tags = new List<Tag>();

  public TagSet(TagSet other) => this.tags = new List<Tag>((IEnumerable<Tag>) other.tags);

  public TagSet(Tag[] other) => this.tags = new List<Tag>((IEnumerable<Tag>) other);

  public TagSet(IEnumerable<string> others)
  {
    this.tags = new List<Tag>();
    IEnumerator<string> enumerator = others.GetEnumerator();
    while (enumerator.MoveNext())
      this.tags.Add(new Tag(enumerator.Current));
  }

  public TagSet(params TagSet[] others)
  {
    this.tags = new List<Tag>();
    for (int index = 0; index < others.Length; ++index)
      this.tags.AddRange((IEnumerable<Tag>) others[index]);
  }

  public TagSet(params string[] others)
  {
    this.tags = new List<Tag>();
    for (int index = 0; index < others.Length; ++index)
      this.tags.Add(new Tag(others[index]));
  }

  public int Count => this.tags.Count;

  public bool IsReadOnly => false;

  public void Add(Tag item)
  {
    if (this.tags.Contains(item))
      return;
    this.tags.Add(item);
  }

  public void Union(TagSet others)
  {
    for (int index = 0; index < others.tags.Count; ++index)
    {
      if (!this.tags.Contains(others.tags[index]))
        this.tags.Add(others.tags[index]);
    }
  }

  public void Clear() => this.tags.Clear();

  public bool Contains(Tag item) => this.tags.Contains(item);

  public bool ContainsAll(TagSet others)
  {
    for (int index = 0; index < others.tags.Count; ++index)
    {
      if (!this.tags.Contains(others.tags[index]))
        return false;
    }
    return true;
  }

  public bool ContainsOne(TagSet others)
  {
    for (int index = 0; index < others.tags.Count; ++index)
    {
      if (this.tags.Contains(others.tags[index]))
        return true;
    }
    return false;
  }

  public void CopyTo(Tag[] array, int arrayIndex) => this.tags.CopyTo(array, arrayIndex);

  public bool Remove(Tag item) => this.tags.Remove(item);

  public void Remove(TagSet other)
  {
    for (int index = 0; index < other.tags.Count; ++index)
    {
      if (this.tags.Contains(other.tags[index]))
        this.tags.Remove(other.tags[index]);
    }
  }

  public IEnumerator<Tag> GetEnumerator() => (IEnumerator<Tag>) this.tags.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public Tag this[int i] => this.tags[i];

  public override string ToString()
  {
    if (this.tags.Count <= 0)
      return "";
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.tags[0].Name);
    for (int index = 1; index < this.tags.Count; ++index)
    {
      stringBuilder.Append(", ");
      stringBuilder.Append(this.tags[index].Name);
    }
    return stringBuilder.ToString();
  }

  public string GetTagDescription()
  {
    if (this.tags.Count <= 0)
      return "";
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(TagDescriptions.GetDescription(this.tags[0].ToString()));
    for (int index = 1; index < this.tags.Count; ++index)
    {
      stringBuilder.Append(", ");
      stringBuilder.Append(TagDescriptions.GetDescription(this.tags[index].ToString()));
    }
    return stringBuilder.ToString();
  }

  public bool IsSynchronized => throw new NotImplementedException();

  public object SyncRoot => throw new NotImplementedException();

  public void CopyTo(Array array, int index) => throw new NotImplementedException();
}
