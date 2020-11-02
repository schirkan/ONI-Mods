// Decompiled with JetBrains decompiler
// Type: Database.SkillAttributePerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

namespace Database
{
  public class SkillAttributePerk : SkillPerk
  {
    public AttributeModifier modifier;

    public SkillAttributePerk(
      string id,
      string attributeId,
      float modifierBonus,
      string modifierDesc)
      : base(id, "", (System.Action<MinionResume>) null, (System.Action<MinionResume>) null, (System.Action<MinionResume>) (identity => {}))
    {
      Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
      this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc);
      this.Name = string.Format((string) UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, (object) this.modifier.GetFormattedString((GameObject) null), (object) attribute.Name);
      this.OnApply = (System.Action<MinionResume>) (identity =>
      {
        if (identity.GetAttributes().Get(this.modifier.AttributeId).Modifiers.FindIndex((Predicate<AttributeModifier>) (mod => mod == this.modifier)) != -1)
          return;
        identity.GetAttributes().Add(this.modifier);
      });
      this.OnRemove = (System.Action<MinionResume>) (identity => identity.GetAttributes().Remove(this.modifier));
    }
  }
}
