// Decompiled with JetBrains decompiler
// Type: RepairableStorageProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RepairableStorageProxy : IEntityConfig
{
  public static string ID = nameof (RepairableStorageProxy);

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID);
    entity.AddOrGet<Storage>();
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
