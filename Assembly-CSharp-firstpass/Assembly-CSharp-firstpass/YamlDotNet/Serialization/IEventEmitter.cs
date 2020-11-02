// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.IEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public interface IEventEmitter
  {
    void Emit(AliasEventInfo eventInfo, IEmitter emitter);

    void Emit(ScalarEventInfo eventInfo, IEmitter emitter);

    void Emit(MappingStartEventInfo eventInfo, IEmitter emitter);

    void Emit(MappingEndEventInfo eventInfo, IEmitter emitter);

    void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter);

    void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter);
  }
}
