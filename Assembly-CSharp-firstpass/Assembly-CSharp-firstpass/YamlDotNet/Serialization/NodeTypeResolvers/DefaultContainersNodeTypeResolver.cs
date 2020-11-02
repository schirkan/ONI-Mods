// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeTypeResolvers.DefaultContainersNodeTypeResolver
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
  public sealed class DefaultContainersNodeTypeResolver : INodeTypeResolver
  {
    bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
    {
      if (currentType == typeof (object))
      {
        switch (nodeEvent)
        {
          case SequenceStart _:
            currentType = typeof (List<object>);
            return true;
          case MappingStart _:
            currentType = typeof (Dictionary<object, object>);
            return true;
        }
      }
      return false;
    }
  }
}
