// Decompiled with JetBrains decompiler
// Type: HashedString
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public struct HashedString : IComparable<HashedString>, IEquatable<HashedString>, ISerializationCallbackReceiver
{
  public static HashedString Invalid;
  [SerializeField]
  [Serialize]
  private int hash;

  public static implicit operator HashedString(string s) => new HashedString(s);

  public bool IsValid => (uint) this.HashValue > 0U;

  public int HashValue
  {
    get => this.hash;
    set => this.hash = value;
  }

  public HashedString(string name) => this.hash = global::Hash.SDBMLower(name);

  public static int Hash(string name) => global::Hash.SDBMLower(name);

  public HashedString(int initial_hash) => this.hash = initial_hash;

  public int CompareTo(HashedString obj) => this.hash - obj.hash;

  public override bool Equals(object obj) => this.hash == ((HashedString) obj).hash;

  public bool Equals(HashedString other) => this.hash == other.hash;

  public override int GetHashCode() => this.hash;

  public static bool operator ==(HashedString x, HashedString y) => x.hash == y.hash;

  public static bool operator !=(HashedString x, HashedString y) => x.hash != y.hash;

  public static implicit operator HashedString(KAnimHashedString hash) => new HashedString(hash.HashValue);

  public override string ToString() => "0x" + this.hash.ToString("X");

  public void OnAfterDeserialize()
  {
  }

  public void OnBeforeSerialize()
  {
  }
}
