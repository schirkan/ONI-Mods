// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventEmitters.ChainedEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
  public abstract class ChainedEventEmitter : IEventEmitter
  {
    protected readonly IEventEmitter nextEmitter;

    protected ChainedEventEmitter(IEventEmitter nextEmitter) => this.nextEmitter = nextEmitter != null ? nextEmitter : throw new ArgumentNullException(nameof (nextEmitter));

    public virtual void Emit(AliasEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);

    public virtual void Emit(ScalarEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);

    public virtual void Emit(MappingStartEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);

    public virtual void Emit(MappingEndEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);

    public virtual void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);

    public virtual void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter) => this.nextEmitter.Emit(eventInfo, emitter);
  }
}
