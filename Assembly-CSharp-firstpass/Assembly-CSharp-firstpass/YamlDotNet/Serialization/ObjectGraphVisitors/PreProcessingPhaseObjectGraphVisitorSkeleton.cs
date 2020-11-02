// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphVisitors.PreProcessingPhaseObjectGraphVisitorSkeleton
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
  public abstract class PreProcessingPhaseObjectGraphVisitorSkeleton : IObjectGraphVisitor<Nothing>
  {
    protected readonly IEnumerable<IYamlTypeConverter> typeConverters;

    public PreProcessingPhaseObjectGraphVisitorSkeleton(
      IEnumerable<IYamlTypeConverter> typeConverters)
    {
      this.typeConverters = typeConverters != null ? (IEnumerable<IYamlTypeConverter>) typeConverters.ToList<IYamlTypeConverter>() : Enumerable.Empty<IYamlTypeConverter>();
    }

    bool IObjectGraphVisitor<Nothing>.Enter(
      IObjectDescriptor value,
      Nothing context)
    {
      return this.typeConverters.FirstOrDefault<IYamlTypeConverter>((Func<IYamlTypeConverter, bool>) (t => t.Accepts(value.Type))) == null && !(value.Value is IYamlConvertible) && !(value.Value is IYamlSerializable) && this.Enter(value);
    }

    bool IObjectGraphVisitor<Nothing>.EnterMapping(
      IPropertyDescriptor key,
      IObjectDescriptor value,
      Nothing context)
    {
      return this.EnterMapping(key, value);
    }

    bool IObjectGraphVisitor<Nothing>.EnterMapping(
      IObjectDescriptor key,
      IObjectDescriptor value,
      Nothing context)
    {
      return this.EnterMapping(key, value);
    }

    void IObjectGraphVisitor<Nothing>.VisitMappingEnd(
      IObjectDescriptor mapping,
      Nothing context)
    {
      this.VisitMappingEnd(mapping);
    }

    void IObjectGraphVisitor<Nothing>.VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType,
      Nothing context)
    {
      this.VisitMappingStart(mapping, keyType, valueType);
    }

    void IObjectGraphVisitor<Nothing>.VisitScalar(
      IObjectDescriptor scalar,
      Nothing context)
    {
      this.VisitScalar(scalar);
    }

    void IObjectGraphVisitor<Nothing>.VisitSequenceEnd(
      IObjectDescriptor sequence,
      Nothing context)
    {
      this.VisitSequenceEnd(sequence);
    }

    void IObjectGraphVisitor<Nothing>.VisitSequenceStart(
      IObjectDescriptor sequence,
      Type elementType,
      Nothing context)
    {
      this.VisitSequenceStart(sequence, elementType);
    }

    protected abstract bool Enter(IObjectDescriptor value);

    protected abstract bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);

    protected abstract bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);

    protected abstract void VisitMappingEnd(IObjectDescriptor mapping);

    protected abstract void VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType);

    protected abstract void VisitScalar(IObjectDescriptor scalar);

    protected abstract void VisitSequenceEnd(IObjectDescriptor sequence);

    protected abstract void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);
  }
}
