// Decompiled with JetBrains decompiler
// Type: GameTagExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GameTagExtensions
{
  public static GameObject Prefab(this Tag tag) => Assets.GetPrefab(tag);

  public static string ProperName(this Tag tag) => TagManager.GetProperName(tag);

  public static string ProperNameStripLink(this Tag tag) => TagManager.GetProperName(tag, true);

  public static Tag Create(SimHashes id) => TagManager.Create(id.ToString());

  public static Tag CreateTag(this SimHashes id) => TagManager.Create(id.ToString());
}
