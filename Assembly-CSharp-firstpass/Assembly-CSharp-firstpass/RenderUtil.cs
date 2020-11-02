// Decompiled with JetBrains decompiler
// Type: RenderUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class RenderUtil
{
  public static void EnableRenderer(Transform node, bool is_enabled)
  {
    if (!((Object) node != (Object) null))
      return;
    Renderer component = node.GetComponent<Renderer>();
    if ((Object) component != (Object) null)
      component.enabled = is_enabled;
    for (int index = 0; index < node.childCount; ++index)
      RenderUtil.EnableRenderer(node.GetChild(index), is_enabled);
  }
}
