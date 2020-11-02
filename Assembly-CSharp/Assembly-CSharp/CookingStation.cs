// Decompiled with JetBrains decompiler
// Type: CookingStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class CookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
  [SerializeField]
  private int diseaseCountKillRate = 100;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.Cook;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_cookstation_kanim")
    };
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.workable.OnWorkTickActions += (System.Action<Worker, float>) ((worker, dt) =>
    {
      Debug.Assert((UnityEngine.Object) worker != (UnityEngine.Object) null, (object) "How did we get a null worker?");
      if (this.diseaseCountKillRate <= 0)
        return;
      this.GetComponent<PrimaryElement>().ModifyDiseaseCount(-Math.Max(1, (int) ((double) this.diseaseCountKillRate * (double) dt)), nameof (CookingStation));
    });
  }

  protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = base.SpawnOrderProduct(recipe);
    foreach (GameObject gameObject in gameObjectList)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      component.ModifyDiseaseCount(-component.DiseaseCount, "CookingStation.CompleteOrder");
    }
    this.GetComponent<Operational>().SetActive(false);
    return gameObjectList;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.REMOVES_DISEASE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE));
    return descriptors;
  }
}
