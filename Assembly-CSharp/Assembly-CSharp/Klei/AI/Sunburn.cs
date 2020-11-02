﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.Sunburn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Klei.AI
{
  public class Sunburn : Sickness
  {
    public const string ID = "SunburnSickness";

    public Sunburn()
      : base("SunburnSickness", Sickness.SicknessType.Ailment, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Exposure
      }, 1020f)
    {
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[1]
      {
        new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.03333334f, (string) DUPLICANTS.DISEASES.SUNBURNSICKNESS.NAME)
      }));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[3]
      {
        (HashedString) "anim_idle_hot_kanim",
        (HashedString) "anim_loco_run_hot_kanim",
        (HashedString) "anim_loco_walk_hot_kanim"
      }, Db.Get().Expressions.SickFierySkin));
      this.AddSicknessComponent((Sickness.SicknessComponent) new PeriodicEmoteSickness((HashedString) "anim_idle_hot_kanim", new HashedString[3]
      {
        (HashedString) "idle_pre",
        (HashedString) "idle_default",
        (HashedString) "idle_pst"
      }, 5f));
    }
  }
}
