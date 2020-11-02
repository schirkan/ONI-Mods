// Decompiled with JetBrains decompiler
// Type: VectorUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class VectorUtil
{
  public static bool Less(this Vector2 v, Vector2 other) => (double) v.x < (double) other.x && (double) v.y < (double) other.y;

  public static bool LessEqual(this Vector2 v, Vector2 other) => (double) v.x <= (double) other.x && (double) v.y <= (double) other.y;

  public static bool Less(this Vector3 v, Vector3 other) => (double) v.x < (double) other.x && (double) v.y < (double) other.y && (double) v.z < (double) other.z;

  public static bool LessEqual(this Vector3 v, Vector3 other) => (double) v.x <= (double) other.x && (double) v.y <= (double) other.y && (double) v.z <= (double) other.z;
}
