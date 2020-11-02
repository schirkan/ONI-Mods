// Decompiled with JetBrains decompiler
// Type: Vector3F
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{x}, {y}, {z}")]
public struct Vector3F
{
  public float x;
  public float y;
  public float z;

  public Vector3F(float _x, float _y, float _z)
  {
    this.x = _x;
    this.y = _y;
    this.z = _z;
  }

  public static Vector3F operator -(Vector3F v1, Vector3F v2) => new Vector3F(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

  public static Vector3F operator +(Vector3F v1, Vector3F v2) => new Vector3F(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

  public static Vector3F operator *(Vector3F v, float scalar) => new Vector3F(v.x * scalar, v.y * scalar, v.z * scalar);

  public static Vector3F operator *(float scalar, Vector3F v) => new Vector3F(v.x * scalar, v.y * scalar, v.z * scalar);

  public static Vector3F operator /(Vector3F v, float scalar) => new Vector3F(v.x / scalar, v.y / scalar, v.z / scalar);

  public static Vector3F operator /(float scalar, Vector3F v) => new Vector3F(v.x / scalar, v.y / scalar, v.z / scalar);

  public static bool operator <(Vector3F v1, Vector3F v2) => (double) v1.x < (double) v2.x && (double) v1.y < (double) v2.y && (double) v1.z < (double) v2.z;

  public static bool operator >(Vector3F v1, Vector3F v2) => (double) v1.x > (double) v2.x && (double) v1.y > (double) v2.y && (double) v1.z > (double) v2.z;

  public static bool operator <=(Vector3F v1, Vector3F v2) => (double) v1.x <= (double) v2.x && (double) v1.y <= (double) v2.y && (double) v1.z <= (double) v2.z;

  public static bool operator >=(Vector3F v1, Vector3F v2) => (double) v1.x >= (double) v2.x && (double) v1.y >= (double) v2.y && (double) v1.z >= (double) v2.z;

  public static bool operator ==(Vector3F v1, Vector3F v2) => (double) v1.x == (double) v2.x && (double) v1.y == (double) v2.y && (double) v1.z == (double) v2.z;

  public static bool operator !=(Vector3F v1, Vector3F v2) => !(v1 == v2);

  public override bool Equals(object o) => base.Equals(o);

  public override int GetHashCode() => base.GetHashCode();

  public override string ToString() => string.Format("{0}, {1}, {2}", (object) this.x, (object) this.y, (object) this.z);

  public static float Dot(Vector3F v1, Vector3F v2) => (float) ((double) v1.x * (double) v2.x + (double) v1.y * (double) v2.y + (double) v1.z * (double) v2.z);

  public static implicit operator Vector3(Vector3F v) => new Vector3(v.x, v.y, v.z);
}
