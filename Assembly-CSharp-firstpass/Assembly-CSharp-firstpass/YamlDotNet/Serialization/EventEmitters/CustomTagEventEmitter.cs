// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventEmitters.CustomTagEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
  internal class CustomTagEventEmitter : ChainedEventEmitter
  {
    private IDictionary<Type, string> tagMappings;

    public CustomTagEventEmitter(IEventEmitter inner, IDictionary<Type, string> tagMappings)
      : base(inner)
      => this.tagMappings = tagMappings;

    public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
    {
      if (this.tagMappings.ContainsKey(eventInfo.Source.Type))
        eventInfo.Tag = this.tagMappings[eventInfo.Source.Type];
      base.Emit(eventInfo, emitter);
    }
  }
}
