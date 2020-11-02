// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphVisitors.ChainedObjectGraphVisitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
  public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
  {
    private readonly IObjectGraphVisitor<IEmitter> nextVisitor;

    protected ChainedObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) => this.nextVisitor = nextVisitor;

    public virtual bool Enter(IObjectDescriptor value, IEmitter context) => this.nextVisitor.Enter(value, context);

    public virtual bool EnterMapping(
      IObjectDescriptor key,
      IObjectDescriptor value,
      IEmitter context)
    {
      return this.nextVisitor.EnterMapping(key, value, context);
    }

    public virtual bool EnterMapping(
      IPropertyDescriptor key,
      IObjectDescriptor value,
      IEmitter context)
    {
      return this.nextVisitor.EnterMapping(key, value, context);
    }

    public virtual void VisitScalar(IObjectDescriptor scalar, IEmitter context) => this.nextVisitor.VisitScalar(scalar, context);

    public virtual void VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType,
      IEmitter context)
    {
      this.nextVisitor.VisitMappingStart(mapping, keyType, valueType, context);
    }

    public virtual void VisitMappingEnd(IObjectDescriptor mapping, IEmitter context) => this.nextVisitor.VisitMappingEnd(mapping, context);

    public virtual void VisitSequenceStart(
      IObjectDescriptor sequence,
      Type elementType,
      IEmitter context)
    {
      this.nextVisitor.VisitSequenceStart(sequence, elementType, context);
    }

    public virtual void VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context) => this.nextVisitor.VisitSequenceEnd(sequence, context);
  }
}
