// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectGraphTraversalStrategies.RoundtripObjectGraphTraversalStrategy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
  public class RoundtripObjectGraphTraversalStrategy : FullObjectGraphTraversalStrategy
  {
    private readonly IEnumerable<IYamlTypeConverter> converters;

    public RoundtripObjectGraphTraversalStrategy(
      IEnumerable<IYamlTypeConverter> converters,
      ITypeInspector typeDescriptor,
      ITypeResolver typeResolver,
      int maxRecursion)
      : base(typeDescriptor, typeResolver, maxRecursion, (INamingConvention) null)
    {
      this.converters = converters;
    }

    protected override void TraverseProperties<TContext>(
      IObjectDescriptor value,
      IObjectGraphVisitor<TContext> visitor,
      int currentDepth,
      TContext context)
    {
      if (!value.Type.HasDefaultConstructor() && !this.converters.Any<IYamlTypeConverter>((Func<IYamlTypeConverter, bool>) (c => c.Accepts(value.Type))))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Type '{0}' cannot be deserialized because it does not have a default constructor or a type converter.", (object) value.Type));
      base.TraverseProperties<TContext>(value, visitor, currentDepth, context);
    }
  }
}
