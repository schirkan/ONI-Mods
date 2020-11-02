// Decompiled with JetBrains decompiler
// Type: Vector2f
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{x}, {y}")]
public struct Vector2f
{
  [Serialize]
  public float x;
  [Serialize]
  public float y;

  public float X
  {
    get => this.x;
    set => this.x = value;
  }

  public float Y
  {
    get => this.y;
    set => this.y = value;
  }

  public Vector2f(int a, int b)
  {
    this.x = (float) a;
    this.y = (float) b;
  }

  public Vector2f(float a, float b)
  {
    this.x = a;
    this.y = b;
  }

  public Vector2f(Vector2 src)
  {
    this.x = src.x;
    this.y = src.y;
  }

  public static bool operator ==(Vector2f u, Vector2f v) => (double) u.x == (double) v.x && (double) u.y == (double) v.y;

  public static bool operator !=(Vector2f u, Vector2f v) => (double) u.x != (double) v.x || (double) u.y != (double) v.y;

  public static implicit operator Vector2(Vector2f v) => new Vector2(v.x, v.y);

  public static implicit operator Vector2f(Vector2 v) => new Vector2f(v.x, v.y);

  public bool Equals(Vector2 v) => (double) v.x == (double) this.x && (double) v.y == (double) this.y;

  public override bool Equals(object obj)
  {
    try
    {
      Vector2f vector2f = (Vector2f) obj;
      return (double) vector2f.x == (double) this.x && (double) vector2f.y == (double) this.y;
    }
    catch
    {
      return false;
    }
  }

  public override int GetHashCode() => this.x.GetHashCode() ^ this.y.GetHashCode();

  public override string ToString() => string.Format("{0}, {1}", (object) this.x, (object) this.y);
}
