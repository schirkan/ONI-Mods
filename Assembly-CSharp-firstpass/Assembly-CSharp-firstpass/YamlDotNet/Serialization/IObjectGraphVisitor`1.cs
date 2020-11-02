// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.IObjectGraphVisitor`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Serialization
{
  public interface IObjectGraphVisitor<TContext>
  {
    bool Enter(IObjectDescriptor value, TContext context);

    bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, TContext context);

    bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, TContext context);

    void VisitScalar(IObjectDescriptor scalar, TContext context);

    void VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType,
      TContext context);

    void VisitMappingEnd(IObjectDescriptor mapping, TContext context);

    void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, TContext context);

    void VisitSequenceEnd(IObjectDescriptor sequence, TContext context);
  }
}
