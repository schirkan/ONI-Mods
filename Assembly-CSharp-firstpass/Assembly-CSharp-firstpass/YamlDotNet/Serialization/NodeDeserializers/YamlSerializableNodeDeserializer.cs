// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.YamlSerializableNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class YamlSerializableNodeDeserializer : INodeDeserializer
  {
    private readonly IObjectFactory objectFactory;

    public YamlSerializableNodeDeserializer(IObjectFactory objectFactory) => this.objectFactory = objectFactory;

    public bool Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      if (typeof (IYamlSerializable).IsAssignableFrom(expectedType))
      {
        IYamlSerializable yamlSerializable = (IYamlSerializable) this.objectFactory.Create(expectedType);
        yamlSerializable.ReadYaml(parser);
        value = (object) yamlSerializable;
        return true;
      }
      value = (object) null;
      return false;
    }
  }
}
