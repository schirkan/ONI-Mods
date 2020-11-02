﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeModifierSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  public class AttributeModifierSickness : Sickness.SicknessComponent
  {
    private AttributeModifier[] attributeModifiers;

    public AttributeModifierSickness(AttributeModifier[] attribute_modifiers) => this.attributeModifiers = attribute_modifiers;

    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      Attributes attributes = go.GetAttributes();
      for (int index = 0; index < this.attributeModifiers.Length; ++index)
      {
        AttributeModifier attributeModifier = this.attributeModifiers[index];
        attributes.Add(attributeModifier);
      }
      return (object) null;
    }

    public override void OnCure(GameObject go, object instance_data)
    {
      Attributes attributes = go.GetAttributes();
      for (int index = 0; index < this.attributeModifiers.Length; ++index)
      {
        AttributeModifier attributeModifier = this.attributeModifiers[index];
        attributes.Remove(attributeModifier);
      }
    }

    public AttributeModifier[] Modifers => this.attributeModifiers;

    public override List<Descriptor> GetSymptoms()
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      foreach (AttributeModifier attributeModifier in this.attributeModifiers)
      {
        Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, (object) attribute.Name, (object) attributeModifier.GetFormattedString((GameObject) null)), string.Format((string) DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, (object) attribute.Name, (object) attributeModifier.GetFormattedString((GameObject) null)), Descriptor.DescriptorType.Symptom));
      }
      return descriptorList;
    }
  }
}
