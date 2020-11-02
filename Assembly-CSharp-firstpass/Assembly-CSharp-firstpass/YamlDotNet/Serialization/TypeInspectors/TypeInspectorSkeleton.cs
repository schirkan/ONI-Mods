// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TypeInspectors.TypeInspectorSkeleton
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace YamlDotNet.Serialization.TypeInspectors
{
  public abstract class TypeInspectorSkeleton : ITypeInspector
  {
    public abstract IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container);

    public IPropertyDescriptor GetProperty(
      Type type,
      object container,
      string name,
      bool ignoreUnmatched)
    {
      IEnumerable<IPropertyDescriptor> source = this.GetProperties(type, container).Where<IPropertyDescriptor>((Func<IPropertyDescriptor, bool>) (p => p.Name == name));
      using (IEnumerator<IPropertyDescriptor> enumerator = source.GetEnumerator())
      {
        if (!enumerator.MoveNext())
        {
          if (ignoreUnmatched)
            return (IPropertyDescriptor) null;
          throw new SerializationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Property '{0}' not found on type '{1}'.", (object) name, (object) type.FullName));
        }
        IPropertyDescriptor current = enumerator.Current;
        if (enumerator.MoveNext())
          throw new SerializationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Multiple properties with the name/alias '{0}' already exists on type '{1}', maybe you're misusing YamlAlias or maybe you are using the wrong naming convention? The matching properties are: {2}", (object) name, (object) type.FullName, (object) string.Join(", ", source.Select<IPropertyDescriptor, string>((Func<IPropertyDescriptor, string>) (p => p.Name)).ToArray<string>())));
        return current;
      }
    }
  }
}
