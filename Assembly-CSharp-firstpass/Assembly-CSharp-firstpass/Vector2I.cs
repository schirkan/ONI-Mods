﻿// Decompiled with JetBrains decompiler
// Type: Vector2I
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{x}, {y}")]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public struct Vector2I : IComparable<Vector2I>, IEquatable<Vector2I>
{
  public static readonly Vector2I zero = new Vector2I(0, 0);
  public static readonly Vector2I one = new Vector2I(1, 1);
  public static readonly Vector2I minusone = new Vector2I(-1, -1);
  [Serialize]
  public int x;
  [Serialize]
  public int y;

  public int X
  {
    get => this.x;
    set => this.x = value;
  }

  public int Y
  {
    get => this.y;
    set => this.y = value;
  }

  public Vector2I(int a, int b)
  {
    this.x = a;
    this.y = b;
  }

  public static Vector2I operator +(Vector2I u, Vector2I v) => new Vector2I(u.x + v.x, u.y + v.y);

  public static Vector2I operator -(Vector2I u, Vector2I v) => new Vector2I(u.x - v.x, u.y - v.y);

  public static Vector2I operator *(Vector2I u, Vector2I v) => new Vector2I(u.x * v.x, u.y * v.y);

  public static Vector2I operator /(Vector2I u, Vector2I v) => new Vector2I(u.x / v.x, u.y / v.y);

  public static Vector2I operator *(Vector2I v, int s) => new Vector2I(v.x * s, v.y * s);

  public static Vector2I operator /(Vector2I v, int s) => new Vector2I(v.x / s, v.y / s);

  public static Vector2I operator +(Vector2I u, int scalar) => new Vector2I(u.x + scalar, u.y + scalar);

  public static Vector2I operator -(Vector2I u, int scalar) => new Vector2I(u.x - scalar, u.y - scalar);

  public static bool operator ==(Vector2I u, Vector2I v) => u.x == v.x && u.y == v.y;

  public static bool operator !=(Vector2I u, Vector2I v) => u.x != v.x || u.y != v.y;

  public static Vector2I Min(Vector2I v, Vector2I w) => new Vector2I(v.x < w.x ? v.x : w.x, v.y < w.y ? v.y : w.y);

  public static Vector2I Max(Vector2I v, Vector2I w) => new Vector2I(v.x > w.x ? v.x : w.x, v.y > w.y ? v.y : w.y);

  public static bool operator <(Vector2I u, Vector2I v) => u.x < v.x && u.y < v.y;

  public static bool operator >(Vector2I u, Vector2I v) => u.x > v.x && u.y > v.y;

  public static bool operator <=(Vector2I u, Vector2I v) => u.x <= v.x && u.y <= v.y;

  public static bool operator >=(Vector2I u, Vector2I v) => u.x >= v.x && u.y >= v.y;

  public int magnitudeSqr => this.x * this.x + this.y * this.y;

  public static implicit operator Vector2(Vector2I v) => new Vector2((float) v.x, (float) v.y);

  public override bool Equals(object obj)
  {
    try
    {
      Vector2I vector2I = (Vector2I) obj;
      return vector2I.x == this.x && vector2I.y == this.y;
    }
    catch
    {
      return false;
    }
  }

  public bool Equals(Vector2I v) => v.x == this.x && v.y == this.y;

  public override int GetHashCode() => this.x ^ this.y;

  public static bool operator <=(Vector2I u, Vector2 v) => (double) u.x <= (double) v.x && (double) u.y <= (double) v.y;

  public static bool operator >=(Vector2I u, Vector2 v) => (double) u.x >= (double) v.x && (double) u.y >= (double) v.y;

  public static bool operator <=(Vector2 u, Vector2I v) => (double) u.x <= (double) v.x && (double) u.y <= (double) v.y;

  public static bool operator >=(Vector2 u, Vector2I v) => (double) u.x >= (double) v.x && (double) u.y >= (double) v.y;

  public override string ToString() => string.Format("{0}, {1}", (object) this.x, (object) this.y);

  public int CompareTo(Vector2I other)
  {
    int num = this.y - other.y;
    return other.y == 0 ? this.x - other.x : num;
  }
}
