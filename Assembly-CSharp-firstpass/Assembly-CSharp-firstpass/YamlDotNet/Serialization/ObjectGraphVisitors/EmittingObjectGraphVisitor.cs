// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphVisitors.EmittingObjectGraphVisitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
  public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
  {
    private readonly IEventEmitter eventEmitter;

    public EmittingObjectGraphVisitor(IEventEmitter eventEmitter) => this.eventEmitter = eventEmitter;

    bool IObjectGraphVisitor<IEmitter>.Enter(
      IObjectDescriptor value,
      IEmitter context)
    {
      return true;
    }

    bool IObjectGraphVisitor<IEmitter>.EnterMapping(
      IObjectDescriptor key,
      IObjectDescriptor value,
      IEmitter context)
    {
      return true;
    }

    bool IObjectGraphVisitor<IEmitter>.EnterMapping(
      IPropertyDescriptor key,
      IObjectDescriptor value,
      IEmitter context)
    {
      return true;
    }

    void IObjectGraphVisitor<IEmitter>.VisitScalar(
      IObjectDescriptor scalar,
      IEmitter context)
    {
      this.eventEmitter.Emit(new ScalarEventInfo(scalar), context);
    }

    void IObjectGraphVisitor<IEmitter>.VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType,
      IEmitter context)
    {
      this.eventEmitter.Emit(new MappingStartEventInfo(mapping), context);
    }

    void IObjectGraphVisitor<IEmitter>.VisitMappingEnd(
      IObjectDescriptor mapping,
      IEmitter context)
    {
      this.eventEmitter.Emit(new MappingEndEventInfo(mapping), context);
    }

    void IObjectGraphVisitor<IEmitter>.VisitSequenceStart(
      IObjectDescriptor sequence,
      Type elementType,
      IEmitter context)
    {
      this.eventEmitter.Emit(new SequenceStartEventInfo(sequence), context);
    }

    void IObjectGraphVisitor<IEmitter>.VisitSequenceEnd(
      IObjectDescriptor sequence,
      IEmitter context)
    {
      this.eventEmitter.Emit(new SequenceEndEventInfo(sequence), context);
    }
  }
}
