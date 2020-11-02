// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventEmitters.WriterEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
  public sealed class WriterEventEmitter : IEventEmitter
  {
    void IEventEmitter.Emit(AliasEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new AnchorAlias(eventInfo.Alias));

    void IEventEmitter.Emit(ScalarEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));

    void IEventEmitter.Emit(MappingStartEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));

    void IEventEmitter.Emit(MappingEndEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new MappingEnd());

    void IEventEmitter.Emit(SequenceStartEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));

    void IEventEmitter.Emit(SequenceEndEventInfo eventInfo, IEmitter emitter) => emitter.Emit((ParsingEvent) new SequenceEnd());
  }
}
