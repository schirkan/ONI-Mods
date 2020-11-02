// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.EnumerableNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class EnumerableNodeDeserializer : INodeDeserializer
  {
    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      Type type1;
      if (expectedType == typeof (IEnumerable))
      {
        type1 = typeof (object);
      }
      else
      {
        Type genericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof (IEnumerable<>));
        if (genericInterface != expectedType)
        {
          value = (object) null;
          return false;
        }
        type1 = genericInterface.GetGenericArguments()[0];
      }
      Type type2 = typeof (List<>).MakeGenericType(type1);
      value = nestedObjectDeserializer(parser, type2);
      return true;
    }
  }
}
