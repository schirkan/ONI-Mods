// Decompiled with JetBrains decompiler
// Type: TargetLocator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TargetLocator : IEntityConfig
{
  public static readonly string ID = nameof (TargetLocator);

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
    entity.AddTag(GameTags.NotConversationTopic);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
