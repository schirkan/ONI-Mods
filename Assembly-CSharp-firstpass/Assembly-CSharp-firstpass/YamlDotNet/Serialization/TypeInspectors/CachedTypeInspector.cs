// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TypeInspectors.CachedTypeInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization.TypeInspectors
{
  public sealed class CachedTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor;
    private readonly Dictionary<Type, List<IPropertyDescriptor>> cache = new Dictionary<Type, List<IPropertyDescriptor>>();

    public CachedTypeInspector(ITypeInspector innerTypeDescriptor) => this.innerTypeDescriptor = innerTypeDescriptor != null ? innerTypeDescriptor : throw new ArgumentNullException(nameof (innerTypeDescriptor));

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      List<IPropertyDescriptor> propertyDescriptorList;
      if (!this.cache.TryGetValue(type, out propertyDescriptorList))
        propertyDescriptorList = new List<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container));
      return (IEnumerable<IPropertyDescriptor>) propertyDescriptorList;
    }
  }
}
