// Decompiled with JetBrains decompiler
// Type: TracesExtesions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class TracesExtesions
{
  public static void DeleteObject(this GameObject go)
  {
    KMonoBehaviour component = go.GetComponent<KMonoBehaviour>();
    if ((Object) component != (Object) null)
      component.Trigger(1502190696, (object) go);
    Object.Destroy((Object) go);
  }

  public static void DeleteObject(this Component cmp) => cmp.gameObject.DeleteObject();
}
