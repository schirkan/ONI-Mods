// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeTypeResolvers.TagNodeTypeResolver
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
  public sealed class TagNodeTypeResolver : INodeTypeResolver
  {
    private readonly IDictionary<string, Type> tagMappings;

    public TagNodeTypeResolver(IDictionary<string, Type> tagMappings) => this.tagMappings = tagMappings != null ? tagMappings : throw new ArgumentNullException(nameof (tagMappings));

    bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
    {
      Type type;
      if (string.IsNullOrEmpty(nodeEvent.Tag) || !this.tagMappings.TryGetValue(nodeEvent.Tag, out type))
        return false;
      currentType = type;
      return true;
    }
  }
}
