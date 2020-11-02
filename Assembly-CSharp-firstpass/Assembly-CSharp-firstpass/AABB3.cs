// Decompiled with JetBrains decompiler
// Type: AABB3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

[Serializable]
public struct AABB3
{
  public Vector3 min;
  public Vector3 max;

  public AABB3(Vector3 pt)
  {
    this.min = pt;
    this.max = pt;
  }

  public AABB3(Vector3 min, Vector3 max)
  {
    this.min = min;
    this.max = max;
  }

  public bool IsValid() => this.min.Min(this.max) == this.min;

  public Vector3 Center => (this.min + this.max) * 0.5f;

  public Vector3 Range => this.max - this.min;

  public void Expand(float amount)
  {
    Vector3 vector3 = new Vector3(amount * 0.5f, amount * 0.5f, amount * 0.5f);
    this.min -= vector3;
    this.max += vector3;
  }

  public void ExpandToFit(Vector3 pt)
  {
    this.min = this.min.Min(pt);
    this.max = this.max.Max(pt);
  }

  public void ExpandToFit(AABB3 aabb)
  {
    this.min = this.min.Min(aabb.min);
    this.max = this.max.Max(aabb.max);
  }

  public bool Contains(Vector3 pt) => this.min.LessEqual(pt) && pt.Less(this.max);

  public bool Contains(AABB3 aabb) => this.Contains(aabb.min) && this.Contains(aabb.max);

  public bool Intersects(AABB3 aabb) => this.min.LessEqual(aabb.max) && aabb.min.Less(this.max);

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    AABB3 aabB3 = (AABB3) obj;
    return this.min == aabB3.min && this.max == aabB3.max;
  }

  public override int GetHashCode() => this.min.GetHashCode() ^ this.max.GetHashCode();

  public unsafe void Transform(Matrix4x4 t)
  {
    Vector3* vector3Ptr = stackalloc Vector3[8];
    vector3Ptr[0] = this.min;
    vector3Ptr[1] = new Vector3(this.min.x, this.min.y, this.max.z);
    vector3Ptr[2] = new Vector3(this.min.x, this.max.y, this.min.z);
    vector3Ptr[3] = new Vector3(this.max.x, this.min.y, this.min.z);
    vector3Ptr[4] = new Vector3(this.min.x, this.max.y, this.max.z);
    vector3Ptr[5] = new Vector3(this.max.x, this.min.y, this.max.z);
    vector3Ptr[6] = new Vector3(this.max.x, this.max.y, this.min.z);
    vector3Ptr[7] = this.max;
    this.min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    this.max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    for (int index = 0; index < 8; ++index)
      this.ExpandToFit((Vector3) (t * (Vector4) vector3Ptr[index]));
  }

  public float Width => this.max.x - this.min.x;

  public float Height => this.max.y - this.min.y;

  public float Depth => this.max.z - this.min.z;
}
