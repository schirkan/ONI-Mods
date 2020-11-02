// Decompiled with JetBrains decompiler
// Type: TransformExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class TransformExtensions
{
  public static Vector3 GetPosition(this Transform transform) => transform.position;

  public static Vector3 SetPosition(this Transform transform, Vector3 position)
  {
    transform.position = position;
    if (Singleton<CellChangeMonitor>.Instance != null)
      Singleton<CellChangeMonitor>.Instance.MarkDirty(transform);
    return position;
  }

  public static Vector3 GetLocalPosition(this Transform transform) => transform.localPosition;

  public static Vector3 SetLocalPosition(this Transform transform, Vector3 position)
  {
    transform.localPosition = position;
    if (Singleton<CellChangeMonitor>.Instance != null)
      Singleton<CellChangeMonitor>.Instance.MarkDirty(transform);
    return position;
  }
}
