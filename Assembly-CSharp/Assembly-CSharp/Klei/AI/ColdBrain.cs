﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.ColdBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Klei.AI
{
  public class ColdBrain : Sickness
  {
    public const string ID = "ColdSickness";

    public ColdBrain()
      : base("ColdSickness", Sickness.SicknessType.Ailment, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Inhalation
      }, 180f)
    {
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[5]
      {
        new AttributeModifier("Learning", -5f, (string) DUPLICANTS.DISEASES.COLDSICKNESS.NAME),
        new AttributeModifier("Machinery", -5f, (string) DUPLICANTS.DISEASES.COLDSICKNESS.NAME),
        new AttributeModifier("Construction", -5f, (string) DUPLICANTS.DISEASES.COLDSICKNESS.NAME),
        new AttributeModifier("Cooking", -5f, (string) DUPLICANTS.DISEASES.COLDSICKNESS.NAME),
        new AttributeModifier("Sneezyness", 1f, (string) DUPLICANTS.DISEASES.COLDSICKNESS.NAME)
      }));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[3]
      {
        (HashedString) "anim_idle_cold_kanim",
        (HashedString) "anim_loco_run_cold_kanim",
        (HashedString) "anim_loco_walk_cold_kanim"
      }, Db.Get().Expressions.SickCold));
      this.AddSicknessComponent((Sickness.SicknessComponent) new PeriodicEmoteSickness((HashedString) "anim_idle_cold_kanim", new HashedString[3]
      {
        (HashedString) "idle_pre",
        (HashedString) "idle_default",
        (HashedString) "idle_pst"
      }, 15f));
    }
  }
}
