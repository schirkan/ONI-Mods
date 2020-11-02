// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Utilities.ReflectionUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization.Utilities
{
  internal static class ReflectionUtility
  {
    public static Type GetImplementedGenericInterface(Type type, Type genericInterfaceType)
    {
      foreach (Type implementedInterface in ReflectionUtility.GetImplementedInterfaces(type))
      {
        if (implementedInterface.IsGenericType() && implementedInterface.GetGenericTypeDefinition() == genericInterfaceType)
          return implementedInterface;
      }
      return (Type) null;
    }

    public static IEnumerable<Type> GetImplementedInterfaces(Type type)
    {
      if (type.IsInterface())
        yield return type;
      Type[] typeArray = type.GetInterfaces();
      for (int index = 0; index < typeArray.Length; ++index)
        yield return typeArray[index];
      typeArray = (Type[]) null;
    }
  }
}
