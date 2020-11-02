// Decompiled with JetBrains decompiler
// Type: EffectorValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct EffectorValues
{
  public int amount;
  public int radius;

  public EffectorValues(int amt, int rad)
  {
    this.amount = amt;
    this.radius = rad;
  }

  public override bool Equals(object obj) => obj is EffectorValues p && this.Equals(p);

  public bool Equals(EffectorValues p)
  {
    if ((ValueType) p == null)
      return false;
    if ((ValueType) this == (ValueType) p)
      return true;
    return !(this.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius;
  }

  public override int GetHashCode() => this.amount ^ this.radius;

  public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
  {
    if ((ValueType) lhs != null)
      return lhs.Equals(rhs);
    return (ValueType) rhs == null;
  }

  public static bool operator !=(EffectorValues lhs, EffectorValues rhs) => !(lhs == rhs);
}
