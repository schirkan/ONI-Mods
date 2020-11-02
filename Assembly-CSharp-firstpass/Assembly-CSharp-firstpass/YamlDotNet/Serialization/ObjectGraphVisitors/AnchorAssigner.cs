// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphVisitors.AnchorAssigner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
  public sealed class AnchorAssigner : PreProcessingPhaseObjectGraphVisitorSkeleton, IAliasProvider
  {
    private readonly IDictionary<object, AnchorAssigner.AnchorAssignment> assignments = (IDictionary<object, AnchorAssigner.AnchorAssignment>) new Dictionary<object, AnchorAssigner.AnchorAssignment>();
    private uint nextId;

    public AnchorAssigner(IEnumerable<IYamlTypeConverter> typeConverters)
      : base(typeConverters)
    {
    }

    protected override bool Enter(IObjectDescriptor value)
    {
      AnchorAssigner.AnchorAssignment anchorAssignment;
      if (value.Value == null || !this.assignments.TryGetValue(value.Value, out anchorAssignment))
        return true;
      if (anchorAssignment.Anchor == null)
      {
        anchorAssignment.Anchor = "o" + this.nextId.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        ++this.nextId;
      }
      return false;
    }

    protected override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => true;

    protected override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value) => true;

    protected override void VisitScalar(IObjectDescriptor scalar)
    {
    }

    protected override void VisitMappingStart(
      IObjectDescriptor mapping,
      Type keyType,
      Type valueType)
    {
      this.VisitObject(mapping);
    }

    protected override void VisitMappingEnd(IObjectDescriptor mapping)
    {
    }

    protected override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType) => this.VisitObject(sequence);

    protected override void VisitSequenceEnd(IObjectDescriptor sequence)
    {
    }

    private void VisitObject(IObjectDescriptor value)
    {
      if (value.Value == null)
        return;
      this.assignments.Add(value.Value, new AnchorAssigner.AnchorAssignment());
    }

    string IAliasProvider.GetAlias(object target)
    {
      AnchorAssigner.AnchorAssignment anchorAssignment;
      return target != null && this.assignments.TryGetValue(target, out anchorAssignment) ? anchorAssignment.Anchor : (string) null;
    }

    private class AnchorAssignment
    {
      public string Anchor;
    }
  }
}
