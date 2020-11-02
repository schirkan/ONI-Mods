// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.ObjectNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class ObjectNodeDeserializer : INodeDeserializer
  {
    private readonly IObjectFactory _objectFactory;
    private readonly ITypeInspector _typeDescriptor;
    private readonly bool _ignoreUnmatched;
    private readonly Action<string> _unmatchedLogFn;

    public ObjectNodeDeserializer(
      IObjectFactory objectFactory,
      ITypeInspector typeDescriptor,
      bool ignoreUnmatched,
      Action<string> unmatchedLogFn = null)
    {
      this._objectFactory = objectFactory;
      this._typeDescriptor = typeDescriptor;
      this._ignoreUnmatched = ignoreUnmatched;
      this._unmatchedLogFn = unmatchedLogFn;
    }

    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      if (parser.Allow<MappingStart>() == null)
      {
        value = (object) null;
        return false;
      }
      value = this._objectFactory.Create(expectedType);
      while (!parser.Accept<MappingEnd>())
      {
        Scalar scalar = parser.Expect<Scalar>();
        IPropertyDescriptor property = this._typeDescriptor.GetProperty(expectedType, (object) null, scalar.Value, this._ignoreUnmatched);
        if (property == null)
        {
          if (this._unmatchedLogFn != null)
            this._unmatchedLogFn(string.Format("Found a property '{0}' on a type '{1}', but that type doesn't have that property!", (object) scalar.Value, (object) expectedType.FullName));
          parser.SkipThisAndNestedEvents();
        }
        else
        {
          object obj1 = nestedObjectDeserializer(parser, property.Type);
          if (!(obj1 is IValuePromise valuePromise))
          {
            object obj2 = TypeConverter.ChangeType(obj1, property.Type);
            property.Write(value, obj2);
          }
          else
          {
            object valueRef = value;
            valuePromise.ValueAvailable += (Action<object>) (v => property.Write(valueRef, TypeConverter.ChangeType(v, property.Type)));
          }
        }
      }
      parser.Expect<MappingEnd>();
      return true;
    }
  }
}
