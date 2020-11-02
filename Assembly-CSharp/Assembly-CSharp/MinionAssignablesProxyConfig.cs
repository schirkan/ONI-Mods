// Decompiled with JetBrains decompiler
// Type: MinionAssignablesProxyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MinionAssignablesProxyConfig : IEntityConfig
{
  public static string ID = "MinionAssignablesProxy";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<Ownables>();
    entity.AddOrGet<Equipment>();
    entity.AddOrGet<MinionAssignablesProxy>();
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
