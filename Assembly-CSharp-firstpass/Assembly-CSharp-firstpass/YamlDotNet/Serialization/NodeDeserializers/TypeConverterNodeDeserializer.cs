// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class TypeConverterNodeDeserializer : INodeDeserializer
  {
    private readonly IEnumerable<IYamlTypeConverter> converters;

    public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters) => this.converters = converters != null ? converters : throw new ArgumentNullException(nameof (converters));

    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      IYamlTypeConverter yamlTypeConverter = this.converters.FirstOrDefault<IYamlTypeConverter>((Func<IYamlTypeConverter, bool>) (c => c.Accepts(expectedType)));
      if (yamlTypeConverter == null)
      {
        value = (object) null;
        return false;
      }
      value = yamlTypeConverter.ReadYaml(parser, expectedType);
      return true;
    }
  }
}
