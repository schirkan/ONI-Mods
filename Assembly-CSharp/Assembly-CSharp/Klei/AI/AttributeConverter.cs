// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class AttributeConverter : Resource
  {
    public string description;
    public float multiplier;
    public float baseValue;
    public Attribute attribute;
    public IAttributeFormatter formatter;

    public AttributeConverter(
      string id,
      string name,
      string description,
      float multiplier,
      float base_value,
      Attribute attribute,
      IAttributeFormatter formatter = null)
      : base(id, name)
    {
      this.description = description;
      this.multiplier = multiplier;
      this.baseValue = base_value;
      this.attribute = attribute;
      this.formatter = formatter;
    }

    public AttributeConverterInstance Lookup(Component cmp) => this.Lookup(cmp.gameObject);

    public AttributeConverterInstance Lookup(GameObject go)
    {
      AttributeConverters component = go.GetComponent<AttributeConverters>();
      return (Object) component != (Object) null ? component.Get(this) : (AttributeConverterInstance) null;
    }

    public string DescriptionFromAttribute(float value, GameObject go)
    {
      string text = this.formatter == null ? (this.attribute.formatter == null ? GameUtil.GetFormattedSimple(value) : this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice, go)) : this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice, go);
      return text != null ? string.Format(this.description, (object) GameUtil.AddPositiveSign(text, (double) value > 0.0)) : (string) null;
    }
  }
}
