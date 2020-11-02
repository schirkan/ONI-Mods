// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.NullNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class NullNodeDeserializer : INodeDeserializer
  {
    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      value = (object) null;
      NodeEvent nodeEvent = parser.Peek<NodeEvent>();
      int num = nodeEvent == null ? 0 : (this.NodeIsNull(nodeEvent) ? 1 : 0);
      if (num == 0)
        return num != 0;
      parser.SkipThisAndNestedEvents();
      return num != 0;
    }

    private bool NodeIsNull(NodeEvent nodeEvent)
    {
      if (nodeEvent.Tag == "tag:yaml.org,2002:null")
        return true;
      if (!(nodeEvent is Scalar scalar) || scalar.Style != ScalarStyle.Plain)
        return false;
      string str = scalar.Value;
      return str == "" || str == "~" || (str == "null" || str == "Null") || str == "NULL";
    }
  }
}
