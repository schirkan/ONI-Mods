// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.YamlAttributeOverrides
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using YamlDotNet.Core;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization
{
  public sealed class YamlAttributeOverrides
  {
    private readonly Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> overrides = new Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>>();

    public T GetAttribute<T>(Type type, string member) where T : Attribute
    {
      List<YamlAttributeOverrides.AttributeMapping> attributeMappingList;
      if (this.overrides.TryGetValue(new YamlAttributeOverrides.AttributeKey(typeof (T), member), out attributeMappingList))
      {
        int num1 = 0;
        YamlAttributeOverrides.AttributeMapping attributeMapping1 = (YamlAttributeOverrides.AttributeMapping) null;
        foreach (YamlAttributeOverrides.AttributeMapping attributeMapping2 in attributeMappingList)
        {
          int num2 = attributeMapping2.Matches(type);
          if (num2 > num1)
          {
            num1 = num2;
            attributeMapping1 = attributeMapping2;
          }
        }
        if (num1 > 0)
          return (T) attributeMapping1.Attribute;
      }
      return default (T);
    }

    public void Add(Type type, string member, Attribute attribute)
    {
      YamlAttributeOverrides.AttributeMapping attributeMapping = new YamlAttributeOverrides.AttributeMapping(type, attribute);
      YamlAttributeOverrides.AttributeKey key = new YamlAttributeOverrides.AttributeKey(attribute.GetType(), member);
      List<YamlAttributeOverrides.AttributeMapping> attributeMappingList;
      if (!this.overrides.TryGetValue(key, out attributeMappingList))
      {
        attributeMappingList = new List<YamlAttributeOverrides.AttributeMapping>();
        this.overrides.Add(key, attributeMappingList);
      }
      else if (attributeMappingList.Contains(attributeMapping))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Attribute ({2}) already set for Type {0}, Member {1}", (object) type.FullName, (object) member, (object) attribute));
      attributeMappingList.Add(attributeMapping);
    }

    public void Add<TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute) => this.Add(typeof (TClass), propertyAccessor.AsProperty().Name, attribute);

    public YamlAttributeOverrides Clone()
    {
      YamlAttributeOverrides attributeOverrides = new YamlAttributeOverrides();
      foreach (KeyValuePair<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> keyValuePair in this.overrides)
      {
        foreach (YamlAttributeOverrides.AttributeMapping attributeMapping in keyValuePair.Value)
          attributeOverrides.Add(attributeMapping.RegisteredType, keyValuePair.Key.PropertyName, attributeMapping.Attribute);
      }
      return attributeOverrides;
    }

    private struct AttributeKey
    {
      public readonly Type AttributeType;
      public readonly string PropertyName;

      public AttributeKey(Type attributeType, string propertyName)
      {
        this.AttributeType = attributeType;
        this.PropertyName = propertyName;
      }

      public override bool Equals(object obj)
      {
        YamlAttributeOverrides.AttributeKey attributeKey = (YamlAttributeOverrides.AttributeKey) obj;
        return this.AttributeType.Equals(attributeKey.AttributeType) && this.PropertyName.Equals(attributeKey.PropertyName);
      }

      public override int GetHashCode() => HashCode.CombineHashCodes(this.AttributeType.GetHashCode(), this.PropertyName.GetHashCode());
    }

    private sealed class AttributeMapping
    {
      public readonly Type RegisteredType;
      public readonly Attribute Attribute;

      public AttributeMapping(Type registeredType, Attribute attribute)
      {
        this.RegisteredType = registeredType;
        this.Attribute = attribute;
      }

      public override bool Equals(object obj) => obj is YamlAttributeOverrides.AttributeMapping attributeMapping && this.RegisteredType.Equals(attributeMapping.RegisteredType) && this.Attribute.Equals((object) attributeMapping.Attribute);

      public override int GetHashCode() => HashCode.CombineHashCodes(this.RegisteredType.GetHashCode(), this.Attribute.GetHashCode());

      public int Matches(Type matchType)
      {
        int num = 0;
        for (Type type = matchType; type != (Type) null; type = type.BaseType())
        {
          ++num;
          if (type == this.RegisteredType)
            return num;
        }
        return ((IEnumerable<Type>) matchType.GetInterfaces()).Contains<Type>(this.RegisteredType) ? num : 0;
      }
    }
  }
}
