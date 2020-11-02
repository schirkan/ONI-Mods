// Decompiled with JetBrains decompiler
// Type: StoredMinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class StoredMinionConfig : IEntityConfig
{
  public static string ID = "StoredMinion";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(StoredMinionConfig.ID, StoredMinionConfig.ID);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<KPrefabID>().AddTag((Tag) StoredMinionConfig.ID);
    entity.AddOrGet<Traits>();
    entity.AddOrGet<Schedulable>();
    entity.AddOrGet<StoredMinionIdentity>();
    entity.AddOrGet<KSelectable>().IsSelectable = false;
    entity.AddOrGet<MinionModifiers>().addBaseTraits = false;
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
