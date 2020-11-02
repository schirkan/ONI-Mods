// Decompiled with JetBrains decompiler
// Type: DoctorChoreWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/DoctorChoreWorkable")]
public class DoctorChoreWorkable : Workable
{
  private DoctorChoreWorkable() => this.synchronizeAnims = false;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
  }
}
