// Decompiled with JetBrains decompiler
// Type: Matrix2x3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public struct Matrix2x3
{
  public float m00;
  public float m01;
  public float m02;
  public float m10;
  public float m11;
  public float m12;
  public static readonly Matrix2x3 identity = new Matrix2x3(1f, 0.0f, 0.0f, 0.0f, 1f, 0.0f);

  public Matrix2x3(float e00, float e01, float e02, float e10, float e11, float e12)
  {
    this.m00 = e00;
    this.m01 = e01;
    this.m02 = e02;
    this.m10 = e10;
    this.m11 = e11;
    this.m12 = e12;
  }

  public override bool Equals(object obj) => this == (Matrix2x3) obj;

  public override int GetHashCode() => base.GetHashCode();

  public static Vector3 operator *(Matrix2x3 m, Vector3 v) => new Vector3((float) ((double) v.x * (double) m.m00 + (double) v.y * (double) m.m01) + m.m02, (float) ((double) v.x * (double) m.m10 + (double) v.y * (double) m.m11) + m.m12, v.z);

  public static Matrix2x3 operator *(Matrix2x3 m, Matrix2x3 n) => new Matrix2x3((float) ((double) m.m00 * (double) n.m00 + (double) m.m01 * (double) n.m10), (float) ((double) m.m00 * (double) n.m01 + (double) m.m01 * (double) n.m11), (float) ((double) m.m00 * (double) n.m02 + (double) m.m01 * (double) n.m12 + (double) m.m02 * 1.0), (float) ((double) m.m10 * (double) n.m00 + (double) m.m11 * (double) n.m10), (float) ((double) m.m10 * (double) n.m01 + (double) m.m11 * (double) n.m11), (float) ((double) m.m10 * (double) n.m02 + (double) m.m11 * (double) n.m12 + (double) m.m12 * 1.0));

  public static bool operator ==(Matrix2x3 m, Matrix2x3 n) => (double) m.m00 == (double) n.m00 && (double) m.m01 == (double) n.m01 && ((double) m.m02 == (double) n.m02 && (double) m.m10 == (double) n.m10) && (double) m.m11 == (double) n.m11 && (double) m.m12 == (double) n.m12;

  public static bool operator !=(Matrix2x3 m, Matrix2x3 n) => !(m == n);

  public Vector3 MultiplyPoint(Vector3 v) => new Vector3((float) ((double) v.x * (double) this.m00 + (double) v.y * (double) this.m01) + this.m02, (float) ((double) v.x * (double) this.m10 + (double) v.y * (double) this.m11) + this.m12, v.z);

  public Vector3 MultiplyVector(Vector3 v) => new Vector3((float) ((double) v.x * (double) this.m00 + (double) v.y * (double) this.m01), (float) ((double) v.x * (double) this.m10 + (double) v.y * (double) this.m11), v.z);

  public static implicit operator Matrix4x4(Matrix2x3 m)
  {
    Matrix4x4 identity = Matrix4x4.identity;
    identity.m00 = m.m00;
    identity.m01 = m.m01;
    identity.m03 = m.m02;
    identity.m10 = m.m10;
    identity.m11 = m.m11;
    identity.m13 = m.m12;
    return identity;
  }

  public static Matrix2x3 Scale(Vector2 scale)
  {
    Matrix2x3 identity = Matrix2x3.identity;
    identity.m00 = scale.x;
    identity.m11 = scale.y;
    return identity;
  }

  public static Matrix2x3 Translate(Vector2 translation)
  {
    Matrix2x3 identity = Matrix2x3.identity;
    identity.m02 = translation.x;
    identity.m12 = translation.y;
    return identity;
  }

  public static Matrix2x3 Rotate(float angle_in_radians)
  {
    Matrix2x3 identity = Matrix2x3.identity;
    float num1 = Mathf.Cos(angle_in_radians);
    float num2 = Mathf.Sin(angle_in_radians);
    identity.m00 = num1;
    identity.m01 = -num2;
    identity.m10 = num2;
    identity.m11 = num1;
    return identity;
  }

  public static Matrix2x3 Rotate(Quaternion quaternion)
  {
    Matrix2x3 identity = Matrix2x3.identity;
    float num1 = quaternion.x * quaternion.x;
    float num2 = quaternion.y * quaternion.y;
    float num3 = quaternion.z * quaternion.z;
    float num4 = quaternion.x * quaternion.y;
    float num5 = quaternion.x * quaternion.z;
    float num6 = quaternion.y * quaternion.z;
    float num7 = quaternion.w * quaternion.x;
    float num8 = quaternion.w * quaternion.y;
    float num9 = quaternion.w * quaternion.z;
    identity.m00 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
    identity.m01 = (float) (2.0 * ((double) num4 - (double) num9));
    identity.m02 = (float) (2.0 * ((double) num5 + (double) num8));
    identity.m10 = (float) (2.0 * ((double) num4 + (double) num9));
    identity.m11 = (float) (1.0 - 2.0 * ((double) num1 + (double) num3));
    identity.m12 = (float) (2.0 * ((double) num6 - (double) num7));
    return identity;
  }

  public static Matrix2x3 TRS(Vector2 translation, Quaternion quaternion, Vector2 scale)
  {
    Matrix2x3 matrix2x3 = Matrix2x3.Rotate(quaternion);
    matrix2x3.m00 *= scale.x;
    matrix2x3.m11 *= scale.y;
    matrix2x3.m02 = translation.x;
    matrix2x3.m12 = translation.y;
    return matrix2x3;
  }

  public static Matrix2x3 TRS(Vector2 translation, float angle_in_radians, Vector2 scale)
  {
    Matrix2x3 matrix2x3 = Matrix2x3.Rotate(angle_in_radians);
    matrix2x3.m00 *= scale.x;
    matrix2x3.m11 *= scale.y;
    matrix2x3.m02 = translation.x;
    matrix2x3.m12 = translation.y;
    return matrix2x3;
  }

  public override string ToString() => string.Format("[{0}, {1}, {2}]  [{3}, {4}, {5}]", (object) this.m00, (object) this.m01, (object) this.m02, (object) this.m10, (object) this.m11, (object) this.m12);
}
