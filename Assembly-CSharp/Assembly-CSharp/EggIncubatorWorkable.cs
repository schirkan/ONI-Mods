// Decompiled with JetBrains decompiler
// Type: EggIncubatorWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_incubator_kanim")
    };
    this.SetWorkTime(15f);
    this.showProgressBar = true;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    EggIncubator component = this.GetComponent<EggIncubator>();
    if (!(bool) (Object) component || !(bool) (Object) component.Occupant)
      return;
    component.Occupant.GetSMI<IncubationMonitor.Instance>()?.ApplySongBuff();
  }
}
