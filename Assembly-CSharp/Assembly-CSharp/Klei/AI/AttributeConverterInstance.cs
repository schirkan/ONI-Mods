﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeConverterInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
  {
    public AttributeConverter converter;
    public AttributeInstance attributeInstance;

    public AttributeConverterInstance(
      GameObject game_object,
      AttributeConverter converter,
      AttributeInstance attribute_instance)
      : base(game_object, converter)
    {
      this.converter = converter;
      this.attributeInstance = attribute_instance;
    }

    public float Evaluate() => this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;

    public string DescriptionFromAttribute(float value, GameObject go) => this.converter.DescriptionFromAttribute(this.Evaluate(), go);
  }
}
