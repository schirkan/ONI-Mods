// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.YamlConvertibleNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class YamlConvertibleNodeDeserializer : INodeDeserializer
  {
    private readonly IObjectFactory objectFactory;

    public YamlConvertibleNodeDeserializer(IObjectFactory objectFactory) => this.objectFactory = objectFactory;

    public bool Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      if (typeof (IYamlConvertible).IsAssignableFrom(expectedType))
      {
        IYamlConvertible yamlConvertible = (IYamlConvertible) this.objectFactory.Create(expectedType);
        yamlConvertible.Read(parser, expectedType, (ObjectDeserializer) (type => nestedObjectDeserializer(parser, type)));
        value = (object) yamlConvertible;
        return true;
      }
      value = (object) null;
      return false;
    }
  }
}
