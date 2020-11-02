// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TypeInspectors.ReadableAndWritablePropertiesTypeInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.TypeInspectors
{
  public sealed class ReadableAndWritablePropertiesTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector _innerTypeDescriptor;

    public ReadableAndWritablePropertiesTypeInspector(ITypeInspector innerTypeDescriptor) => this._innerTypeDescriptor = innerTypeDescriptor;

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      return this._innerTypeDescriptor.GetProperties(type, container).Where<IPropertyDescriptor>((Func<IPropertyDescriptor, bool>) (p => p.CanWrite));
    }
  }
}
