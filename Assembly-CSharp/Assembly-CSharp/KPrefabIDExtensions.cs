// Decompiled with JetBrains decompiler
// Type: KPrefabIDExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class KPrefabIDExtensions
{
  public static Tag PrefabID(this Component cmp) => cmp.gameObject.PrefabID();

  public static Tag PrefabID(this GameObject go) => go.GetComponent<KPrefabID>().PrefabTag;

  public static bool HasTag(this Component cmp, Tag tag) => cmp.gameObject.HasTag(tag);

  public static bool HasTag(this GameObject go, Tag tag) => go.GetComponent<KPrefabID>().HasTag(tag);

  public static void AddTag(this GameObject go, Tag tag) => go.GetComponent<KPrefabID>().AddTag(tag);

  public static void AddTag(this Component cmp, Tag tag) => cmp.gameObject.AddTag(tag);

  public static void RemoveTag(this GameObject go, Tag tag) => go.GetComponent<KPrefabID>().RemoveTag(tag);

  public static void RemoveTag(this Component cmp, Tag tag) => cmp.gameObject.RemoveTag(tag);
}
