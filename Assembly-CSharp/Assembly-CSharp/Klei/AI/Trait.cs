// Decompiled with JetBrains decompiler
// Type: Klei.AI.Trait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  public class Trait : Modifier
  {
    public float Rating;
    public bool ShouldSave;
    public bool PositiveTrait;
    public bool ValidStarterTrait;
    public System.Action<GameObject> OnAddTrait;
    public Func<string> ExtendedTooltip;
    public ChoreGroup[] disabledChoreGroups;
    public bool isTaskBeingRefused;
    public string[] ignoredEffects;

    public Trait(
      string id,
      string name,
      string description,
      float rating,
      bool should_save,
      ChoreGroup[] disallowed_chore_groups,
      bool positive_trait,
      bool is_valid_starter_trait)
      : base(id, name, description)
    {
      this.Rating = rating;
      this.ShouldSave = should_save;
      this.disabledChoreGroups = disallowed_chore_groups;
      this.PositiveTrait = positive_trait;
      this.ValidStarterTrait = is_valid_starter_trait;
      this.ignoredEffects = new string[0];
    }

    public void AddIgnoredEffects(string[] effects)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.ignoredEffects);
      stringList.AddRange((IEnumerable<string>) effects);
      this.ignoredEffects = stringList.ToArray();
    }

    public string GetTooltip() => this.description + this.GetAttributeModifiersString(true) + this.GetDisabledChoresString(true) + this.GetIgnoredEffectsString(true) + this.GetExtendedTooltipStr();

    public string GetAttributeModifiersString(bool list_entry)
    {
      string str = "";
      foreach (AttributeModifier selfModifier in this.SelfModifiers)
      {
        Attribute attribute = Db.Get().Attributes.Get(selfModifier.AttributeId);
        if (list_entry)
          str += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
        str += string.Format((string) DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, (object) attribute.Name, (object) selfModifier.GetFormattedString((GameObject) null));
      }
      return str;
    }

    public string GetDisabledChoresString(bool list_entry)
    {
      string str = "";
      if (this.disabledChoreGroups != null)
      {
        string format = (string) DUPLICANTS.TRAITS.CANNOT_DO_TASK;
        if (this.isTaskBeingRefused)
          format = (string) DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
        foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
        {
          if (list_entry)
            str += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
          str += string.Format(format, (object) disabledChoreGroup.Name);
        }
      }
      return str;
    }

    public string GetIgnoredEffectsString(bool list_entry)
    {
      string str1 = "";
      if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
      {
        foreach (string ignoredEffect in this.ignoredEffects)
        {
          if (list_entry)
            str1 += (string) DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
          string str2 = (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + ignoredEffect.ToUpper() + ".NAME");
          str1 += string.Format((string) DUPLICANTS.TRAITS.IGNORED_EFFECTS, (object) str2);
        }
      }
      return str1;
    }

    public string GetExtendedTooltipStr()
    {
      string str = "";
      if (this.ExtendedTooltip != null)
      {
        foreach (Func<string> invocation in this.ExtendedTooltip.GetInvocationList())
          str = str + "\n" + invocation();
      }
      return str;
    }

    public override void AddTo(Attributes attributes)
    {
      base.AddTo(attributes);
      ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.disabledChoreGroups == null)
        return;
      foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
        component.SetPermittedByTraits(disabledChoreGroup, false);
    }

    public override void RemoveFrom(Attributes attributes)
    {
      base.RemoveFrom(attributes);
      ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.disabledChoreGroups == null)
        return;
      foreach (ChoreGroup disabledChoreGroup in this.disabledChoreGroups)
        component.SetPermittedByTraits(disabledChoreGroup, true);
    }
  }
}
