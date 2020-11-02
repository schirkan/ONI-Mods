// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.YamlAttributesTypeInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
  public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor;

    public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor) => this.innerTypeDescriptor = innerTypeDescriptor;

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      return (IEnumerable<IPropertyDescriptor>) this.innerTypeDescriptor.GetProperties(type, container).Where<IPropertyDescriptor>((Func<IPropertyDescriptor, bool>) (p => p.GetCustomAttribute<YamlIgnoreAttribute>() == null)).Select<IPropertyDescriptor, IPropertyDescriptor>((Func<IPropertyDescriptor, IPropertyDescriptor>) (p =>
      {
        PropertyDescriptor propertyDescriptor = new PropertyDescriptor(p);
        YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
        if (customAttribute != null)
        {
          if (customAttribute.SerializeAs != (Type) null)
            propertyDescriptor.TypeOverride = customAttribute.SerializeAs;
          propertyDescriptor.Order = customAttribute.Order;
          propertyDescriptor.ScalarStyle = customAttribute.ScalarStyle;
          if (customAttribute.Alias != null)
            propertyDescriptor.Name = customAttribute.Alias;
        }
        return (IPropertyDescriptor) propertyDescriptor;
      })).OrderBy<IPropertyDescriptor, int>((Func<IPropertyDescriptor, int>) (p => p.Order));
    }
  }
}
