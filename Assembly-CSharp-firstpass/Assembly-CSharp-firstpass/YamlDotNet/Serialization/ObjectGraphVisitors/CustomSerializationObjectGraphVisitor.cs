// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphVisitors.CustomSerializationObjectGraphVisitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
  public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
  {
    private readonly IEnumerable<IYamlTypeConverter> typeConverters;
    private readonly ObjectSerializer nestedObjectSerializer;

    public CustomSerializationObjectGraphVisitor(
      IObjectGraphVisitor<IEmitter> nextVisitor,
      IEnumerable<IYamlTypeConverter> typeConverters,
      ObjectSerializer nestedObjectSerializer)
      : base(nextVisitor)
    {
      this.typeConverters = typeConverters != null ? (IEnumerable<IYamlTypeConverter>) typeConverters.ToList<IYamlTypeConverter>() : Enumerable.Empty<IYamlTypeConverter>();
      this.nestedObjectSerializer = nestedObjectSerializer;
    }

    public override bool Enter(IObjectDescriptor value, IEmitter context)
    {
      IYamlTypeConverter yamlTypeConverter = this.typeConverters.FirstOrDefault<IYamlTypeConverter>((Func<IYamlTypeConverter, bool>) (t => t.Accepts(value.Type)));
      if (yamlTypeConverter != null)
      {
        yamlTypeConverter.WriteYaml(context, value.Value, value.Type);
        return false;
      }
      if (value.Value is IYamlConvertible yamlConvertible)
      {
        yamlConvertible.Write(context, this.nestedObjectSerializer);
        return false;
      }
      if (!(value.Value is IYamlSerializable yamlSerializable))
        return base.Enter(value, context);
      yamlSerializable.WriteYaml(context);
      return false;
    }
  }
}
