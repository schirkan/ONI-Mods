// Decompiled with JetBrains decompiler
// Type: UIUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class UIUtil
{
  public static Vector3[] corners = new Vector3[4];

  public static float worldHeight(this RectTransform rt)
  {
    rt.GetWorldCorners(UIUtil.corners);
    return UIUtil.corners[2].y - UIUtil.corners[0].y;
  }
}
