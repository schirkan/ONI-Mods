// Decompiled with JetBrains decompiler
// Type: EffectConfigs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EffectConfigs : IMultiEntityConfig
{
  public static string EffectTemplateId = "EffectTemplateFx";
  public static string AttackSplashId = "AttackSplashFx";
  public static string OreAbsorbId = "OreAbsorbFx";
  public static string PlantDeathId = "PlantDeathFx";
  public static string BuildSplashId = "BuildSplashFx";

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    \u003C\u003Ef__AnonymousType0<string, string[], string, KAnim.PlayMode, bool>[] dataArray = new \u003C\u003Ef__AnonymousType0<string, string[], string, KAnim.PlayMode, bool>[5]
    {
      new
      {
        id = EffectConfigs.EffectTemplateId,
        animFiles = new string[0],
        initialAnim = "",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = false
      },
      new
      {
        id = EffectConfigs.AttackSplashId,
        animFiles = new string[1]
        {
          "attack_beam_contact_fx_kanim"
        },
        initialAnim = "loop",
        initialMode = KAnim.PlayMode.Loop,
        destroyOnAnimComplete = false
      },
      new
      {
        id = EffectConfigs.OreAbsorbId,
        animFiles = new string[1]{ "ore_collision_kanim" },
        initialAnim = "idle",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = true
      },
      new
      {
        id = EffectConfigs.PlantDeathId,
        animFiles = new string[1]{ "plant_death_fx_kanim" },
        initialAnim = "plant_death",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = true
      },
      new
      {
        id = EffectConfigs.BuildSplashId,
        animFiles = new string[1]
        {
          "sparks_radial_build_kanim"
        },
        initialAnim = "loop",
        initialMode = KAnim.PlayMode.Loop,
        destroyOnAnimComplete = false
      }
    };
    foreach (var data in dataArray)
    {
      GameObject entity = EntityTemplates.CreateEntity(data.id, data.id, false);
      KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.Simple;
      kbatchedAnimController.initialAnim = data.initialAnim;
      kbatchedAnimController.initialMode = data.initialMode;
      kbatchedAnimController.isMovable = true;
      kbatchedAnimController.destroyOnAnimComplete = data.destroyOnAnimComplete;
      if (data.animFiles.Length != 0)
      {
        KAnimFile[] kanimFileArray = new KAnimFile[data.animFiles.Length];
        for (int index = 0; index < kanimFileArray.Length; ++index)
          kanimFileArray[index] = Assets.GetAnim((HashedString) data.animFiles[index]);
        kbatchedAnimController.AnimFiles = kanimFileArray;
      }
      entity.AddOrGet<LoopingSounds>();
      gameObjectList.Add(entity);
    }
    return gameObjectList;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
