// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.DictionaryNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class DictionaryNodeDeserializer : INodeDeserializer
  {
    private readonly IObjectFactory _objectFactory;

    public DictionaryNodeDeserializer(IObjectFactory objectFactory) => this._objectFactory = objectFactory;

    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      Type genericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof (IDictionary<,>));
      Type tKey;
      Type tValue;
      if (genericInterface != (Type) null)
      {
        Type[] genericArguments = genericInterface.GetGenericArguments();
        tKey = genericArguments[0];
        tValue = genericArguments[1];
        value = this._objectFactory.Create(expectedType);
        if (!(value is IDictionary result))
          result = (IDictionary) new GenericDictionaryToNonGenericAdapter(value, genericInterface);
      }
      else if (typeof (IDictionary).IsAssignableFrom(expectedType))
      {
        tKey = typeof (object);
        tValue = typeof (object);
        value = this._objectFactory.Create(expectedType);
        result = (IDictionary) value;
      }
      else
      {
        value = (object) null;
        return false;
      }
      DictionaryNodeDeserializer.DeserializeHelper(tKey, tValue, parser, nestedObjectDeserializer, result);
      return true;
    }

    private static void DeserializeHelper(
      Type tKey,
      Type tValue,
      IParser parser,
      Func<IParser, Type, object> nestedObjectDeserializer,
      IDictionary result)
    {
      parser.Expect<MappingStart>();
      while (!parser.Accept<MappingEnd>())
      {
        object key = nestedObjectDeserializer(parser, tKey);
        IValuePromise valuePromise1 = key as IValuePromise;
        object value = nestedObjectDeserializer(parser, tValue);
        IValuePromise valuePromise2 = value as IValuePromise;
        if (valuePromise1 == null)
        {
          if (valuePromise2 == null)
            result[key] = value;
          else
            valuePromise2.ValueAvailable += (Action<object>) (v => result[key] = v);
        }
        else if (valuePromise2 == null)
        {
          valuePromise1.ValueAvailable += (Action<object>) (v => result[v] = value);
        }
        else
        {
          bool hasFirstPart = false;
          valuePromise1.ValueAvailable += (Action<object>) (v =>
          {
            if (hasFirstPart)
            {
              result[v] = value;
            }
            else
            {
              key = v;
              hasFirstPart = true;
            }
          });
          valuePromise2.ValueAvailable += (Action<object>) (v =>
          {
            if (hasFirstPart)
            {
              result[key] = v;
            }
            else
            {
              value = v;
              hasFirstPart = true;
            }
          });
        }
      }
      parser.Expect<MappingEnd>();
    }
  }
}
