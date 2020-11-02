// Decompiled with JetBrains decompiler
// Type: Vector3I
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

[DebuggerDisplay("{x}, {y}, {z}")]
public struct Vector3I
{
  public int x;
  public int y;
  public int z;

  public Vector3I(int a, int b, int c)
  {
    this.x = a;
    this.y = b;
    this.z = c;
  }

  public static bool operator ==(Vector3I v1, Vector3I v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;

  public static bool operator !=(Vector3I v1, Vector3I v2) => !(v1 == v2);

  public override bool Equals(object o) => base.Equals(o);

  public override int GetHashCode() => base.GetHashCode();

  public override string ToString() => string.Format("{0}, {1}, {2}", (object) this.x, (object) this.y, (object) this.z);
}
