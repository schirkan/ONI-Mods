// Decompiled with JetBrains decompiler
// Type: CellOffset
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

[Serializable]
public struct CellOffset : IEquatable<CellOffset>
{
  public int x;
  public int y;

  public static CellOffset none => new CellOffset(0, 0);

  public CellOffset(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public CellOffset(Vector2 offset)
  {
    this.x = Mathf.RoundToInt(offset.x);
    this.y = Mathf.RoundToInt(offset.y);
  }

  public Vector2I ToVector2I() => new Vector2I(this.x, this.y);

  public Vector3 ToVector3() => new Vector3((float) this.x, (float) this.y, 0.0f);

  public CellOffset Offset(CellOffset offset) => new CellOffset(this.x + offset.x, this.y + offset.y);

  public int GetOffsetDistance() => Math.Abs(this.x) + Math.Abs(this.y);

  public static CellOffset operator +(CellOffset a, CellOffset b) => new CellOffset(a.x + b.x, a.y + b.y);

  public static CellOffset operator -(CellOffset a, CellOffset b) => new CellOffset(a.x - b.x, a.y - b.y);

  public static CellOffset operator *(CellOffset offset, int value) => new CellOffset(offset.x * value, offset.y * value);

  public static CellOffset operator *(int value, CellOffset offset) => new CellOffset(offset.x * value, offset.y * value);

  public override bool Equals(object obj)
  {
    CellOffset cellOffset = (CellOffset) obj;
    return this.x == cellOffset.x && this.y == cellOffset.y;
  }

  public bool Equals(CellOffset offset) => this.x == offset.x && this.y == offset.y;

  public override int GetHashCode() => this.x + this.y * 8192;

  public static bool operator ==(CellOffset a, CellOffset b) => a.x == b.x && a.y == b.y;

  public static bool operator !=(CellOffset a, CellOffset b) => a.x != b.x || a.y != b.y;

  public override string ToString() => "(" + (object) this.x + "," + (object) this.y + ")";
}
