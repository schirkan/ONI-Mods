// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
  public sealed class NodeValueDeserializer : IValueDeserializer
  {
    private readonly IList<INodeDeserializer> deserializers;
    private readonly IList<INodeTypeResolver> typeResolvers;

    public NodeValueDeserializer(
      IList<INodeDeserializer> deserializers,
      IList<INodeTypeResolver> typeResolvers)
    {
      this.deserializers = deserializers != null ? deserializers : throw new ArgumentNullException(nameof (deserializers));
      this.typeResolvers = typeResolvers != null ? typeResolvers : throw new ArgumentNullException(nameof (typeResolvers));
    }

    public object DeserializeValue(
      IParser parser,
      Type expectedType,
      SerializerState state,
      IValueDeserializer nestedObjectDeserializer)
    {
      NodeEvent nodeEvent = parser.Peek<NodeEvent>();
      Type typeFromEvent = this.GetTypeFromEvent(nodeEvent, expectedType);
      try
      {
        foreach (INodeDeserializer deserializer in (IEnumerable<INodeDeserializer>) this.deserializers)
        {
          object obj;
          if (deserializer.Deserialize(parser, typeFromEvent, (Func<IParser, Type, object>) ((r, t) => nestedObjectDeserializer.DeserializeValue(r, t, state, nestedObjectDeserializer)), out obj))
            return obj;
        }
      }
      catch (YamlException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new YamlException(nodeEvent.Start, nodeEvent.End, "Exception during deserialization", ex);
      }
      throw new YamlException(nodeEvent.Start, nodeEvent.End, string.Format("No node deserializer was able to deserialize the node into type {0}", (object) expectedType.AssemblyQualifiedName));
    }

    private Type GetTypeFromEvent(NodeEvent nodeEvent, Type currentType)
    {
      foreach (INodeTypeResolver typeResolver in (IEnumerable<INodeTypeResolver>) this.typeResolvers)
      {
        if (typeResolver.Resolve(nodeEvent, ref currentType))
          break;
      }
      return currentType;
    }
  }
}
