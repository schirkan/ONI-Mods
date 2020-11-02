// Decompiled with JetBrains decompiler
// Type: KSelectableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class KSelectableExtensions
{
  public static string GetProperName(this Component cmp) => (Object) cmp != (Object) null && (Object) cmp.gameObject != (Object) null ? cmp.gameObject.GetProperName() : "";

  public static string GetProperName(this GameObject go)
  {
    if ((Object) go != (Object) null)
    {
      KSelectable component = go.GetComponent<KSelectable>();
      if ((Object) component != (Object) null)
        return component.GetName();
    }
    return "";
  }

  public static string GetProperName(this KSelectable cmp) => (Object) cmp != (Object) null ? cmp.GetName() : "";
}
