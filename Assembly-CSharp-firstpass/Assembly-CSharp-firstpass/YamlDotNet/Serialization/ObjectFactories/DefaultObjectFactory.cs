// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectFactories.DefaultObjectFactory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization.ObjectFactories
{
  public sealed class DefaultObjectFactory : IObjectFactory
  {
    private static readonly Dictionary<Type, Type> defaultInterfaceImplementations = new Dictionary<Type, Type>()
    {
      {
        typeof (IEnumerable<>),
        typeof (List<>)
      },
      {
        typeof (ICollection<>),
        typeof (List<>)
      },
      {
        typeof (IList<>),
        typeof (List<>)
      },
      {
        typeof (IDictionary<,>),
        typeof (Dictionary<,>)
      }
    };

    public object Create(Type type)
    {
      Type type1;
      if (type.IsInterface() && DefaultObjectFactory.defaultInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), out type1))
        type = type1.MakeGenericType(type.GetGenericArguments());
      try
      {
        return Activator.CreateInstance(type);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format("Failed to create an instance of type '{0}'.", (object) type), ex);
      }
    }
  }
}
