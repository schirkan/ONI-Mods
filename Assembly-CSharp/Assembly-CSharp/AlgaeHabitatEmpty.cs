// Decompiled with JetBrains decompiler
// Type: AlgaeHabitatEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
  private static readonly HashedString[] CLEAN_ANIMS = new HashedString[2]
  {
    (HashedString) "sponge_pre",
    (HashedString) "sponge_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
    this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
    this.workingPstComplete = new HashedString[1]
    {
      AlgaeHabitatEmpty.PST_ANIM
    };
    this.workingPstFailed = new HashedString[1]
    {
      AlgaeHabitatEmpty.PST_ANIM
    };
    this.synchronizeAnims = false;
  }
}
