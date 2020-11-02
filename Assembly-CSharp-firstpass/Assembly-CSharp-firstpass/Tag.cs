// Decompiled with JetBrains decompiler
// Type: Tag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public struct Tag : ISerializationCallbackReceiver, IEquatable<Tag>, IComparable<Tag>
{
  public static readonly Tag Invalid;
  [Serialize]
  [SerializeField]
  private string name;
  [Serialize]
  private int hash;

  public Tag(int hash)
  {
    this.hash = hash;
    this.name = "";
  }

  public Tag(Tag orig)
  {
    this.name = orig.name;
    this.hash = orig.hash;
  }

  public Tag(string name)
  {
    this.name = name;
    this.hash = Hash.SDBMLower(name);
  }

  public string Name
  {
    get => this.name;
    set
    {
      this.name = string.Intern(value);
      this.hash = Hash.SDBMLower(this.name);
    }
  }

  public bool IsValid => (uint) this.hash > 0U;

  public void Clear()
  {
    this.name = (string) null;
    this.hash = 0;
  }

  public override int GetHashCode() => this.hash;

  public int GetHash() => this.hash;

  public override bool Equals(object obj) => this.hash == ((Tag) obj).hash;

  public bool Equals(Tag other) => this.hash == other.hash;

  public static bool operator ==(Tag a, Tag b) => a.hash == b.hash;

  public static bool operator !=(Tag a, Tag b) => a.hash != b.hash;

  public void OnBeforeSerialize()
  {
  }

  public void OnAfterDeserialize()
  {
    if (this.name != null)
      this.Name = this.name;
    else
      this.name = "";
  }

  public int CompareTo(Tag other) => this.hash - other.hash;

  public override string ToString() => this.name == null ? this.hash.ToString("X") : this.name;

  public static implicit operator Tag(string s) => new Tag(s);

  public static string ArrayToString(Tag[] tags) => string.Join(",", ((IEnumerable<Tag>) tags).Select<Tag, string>((Func<Tag, string>) (x => x.ToString())).ToArray<string>());
}
