// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TypeInspectors.NamingConventionTypeInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.TypeInspectors
{
  public sealed class NamingConventionTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor;
    private readonly INamingConvention namingConvention;

    public NamingConventionTypeInspector(
      ITypeInspector innerTypeDescriptor,
      INamingConvention namingConvention)
    {
      this.innerTypeDescriptor = innerTypeDescriptor != null ? innerTypeDescriptor : throw new ArgumentNullException(nameof (innerTypeDescriptor));
      this.namingConvention = namingConvention != null ? namingConvention : throw new ArgumentNullException(nameof (namingConvention));
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      return this.innerTypeDescriptor.GetProperties(type, container).Select<IPropertyDescriptor, IPropertyDescriptor>((Func<IPropertyDescriptor, IPropertyDescriptor>) (p =>
      {
        YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
        if (customAttribute != null && !customAttribute.ApplyNamingConventions)
          return p;
        return (IPropertyDescriptor) new PropertyDescriptor(p)
        {
          Name = this.namingConvention.Apply(p.Name)
        };
      }));
    }
  }
}
