// Decompiled with JetBrains decompiler
// Type: KAnimHashedString
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

[System.Diagnostics.DebuggerDisplay("Name = {DebuggerDisplay}")]
[Serializable]
public struct KAnimHashedString : IComparable<KAnimHashedString>, IEquatable<KAnimHashedString>
{
  [SerializeField]
  private int hash;

  public int HashValue
  {
    get => this.hash;
    set => this.hash = value;
  }

  public KAnimHashedString(string name) => this.hash = Hash.SDBMLower(name);

  public KAnimHashedString(int hash) => this.hash = hash;

  public bool IsValid() => (uint) this.hash > 0U;

  public string DebuggerDisplay => HashCache.Get().Get(this.hash);

  public static implicit operator KAnimHashedString(HashedString hash) => new KAnimHashedString(hash.HashValue);

  public static implicit operator KAnimHashedString(string str) => new KAnimHashedString(str);

  public int CompareTo(KAnimHashedString obj)
  {
    if (this.hash < obj.hash)
      return -1;
    return this.hash > obj.hash ? 1 : 0;
  }

  public override bool Equals(object obj) => this.hash == ((KAnimHashedString) obj).hash;

  public bool Equals(KAnimHashedString other) => this.hash == other.hash;

  public override int GetHashCode() => this.hash;

  public static bool operator ==(KAnimHashedString x, HashedString y) => x.HashValue == y.HashValue;

  public static bool operator !=(KAnimHashedString x, HashedString y) => x.HashValue != y.HashValue;

  public static bool operator ==(KAnimHashedString x, KAnimHashedString y) => x.hash == y.hash;

  public static bool operator !=(KAnimHashedString x, KAnimHashedString y) => x.hash != y.hash;

  public override string ToString() => string.IsNullOrEmpty(this.DebuggerDisplay) ? "0x" + this.hash.ToString("X") : this.DebuggerDisplay;
}
