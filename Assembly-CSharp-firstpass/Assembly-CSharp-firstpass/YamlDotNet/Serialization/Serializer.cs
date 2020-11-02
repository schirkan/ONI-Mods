// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Serializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace YamlDotNet.Serialization
{
  public sealed class Serializer
  {
    private readonly IValueSerializer valueSerializer;
    private readonly Serializer.BackwardsCompatibleConfiguration backwardsCompatibleConfiguration;

    private void ThrowUnlessInBackwardsCompatibleMode()
    {
      if (this.backwardsCompatibleConfiguration == null)
        throw new InvalidOperationException("This method / property exists for backwards compatibility reasons, but the Serializer was created using the new configuration mechanism. To configure the Serializer, use the SerializerBuilder.");
    }

    [Obsolete("Please use SerializerBuilder to customize the Serializer. This constructor will be removed in future releases.")]
    public Serializer(
      SerializationOptions options = SerializationOptions.None,
      INamingConvention namingConvention = null,
      YamlAttributeOverrides overrides = null)
    {
      this.backwardsCompatibleConfiguration = new Serializer.BackwardsCompatibleConfiguration(options, namingConvention, overrides);
    }

    [Obsolete("Please use SerializerBuilder to customize the Serializer. This method will be removed in future releases.")]
    public void RegisterTypeConverter(IYamlTypeConverter converter)
    {
      this.ThrowUnlessInBackwardsCompatibleMode();
      this.backwardsCompatibleConfiguration.Converters.Insert(0, converter);
    }

    public Serializer() => this.backwardsCompatibleConfiguration = new Serializer.BackwardsCompatibleConfiguration(SerializationOptions.None, (INamingConvention) null, (YamlAttributeOverrides) null);

    private Serializer(IValueSerializer valueSerializer) => this.valueSerializer = valueSerializer != null ? valueSerializer : throw new ArgumentNullException(nameof (valueSerializer));

    public static Serializer FromValueSerializer(IValueSerializer valueSerializer) => new Serializer(valueSerializer);

    public void Serialize(TextWriter writer, object graph) => this.Serialize((IEmitter) new Emitter(writer), graph);

    public string Serialize(object graph)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        this.Serialize((TextWriter) stringWriter, graph);
        return stringWriter.ToString();
      }
    }

    public void Serialize(TextWriter writer, object graph, Type type) => this.Serialize((IEmitter) new Emitter(writer), graph, type);

    public void Serialize(IEmitter emitter, object graph)
    {
      if (emitter == null)
        throw new ArgumentNullException(nameof (emitter));
      this.EmitDocument(emitter, graph, (Type) null);
    }

    public void Serialize(IEmitter emitter, object graph, Type type)
    {
      if (emitter == null)
        throw new ArgumentNullException(nameof (emitter));
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      this.EmitDocument(emitter, graph, type);
    }

    private void EmitDocument(IEmitter emitter, object graph, Type type)
    {
      emitter.Emit((ParsingEvent) new StreamStart());
      emitter.Emit((ParsingEvent) new DocumentStart());
      ((IValueSerializer) this.backwardsCompatibleConfiguration ?? this.valueSerializer).SerializeValue(emitter, graph, type);
      emitter.Emit((ParsingEvent) new DocumentEnd(true));
      emitter.Emit((ParsingEvent) new StreamEnd());
    }

    private class BackwardsCompatibleConfiguration : IValueSerializer
    {
      private readonly SerializationOptions options;
      private readonly INamingConvention namingConvention;
      private readonly ITypeResolver typeResolver;
      private readonly YamlAttributeOverrides overrides;

      public IList<IYamlTypeConverter> Converters { get; private set; }

      public BackwardsCompatibleConfiguration(
        SerializationOptions options,
        INamingConvention namingConvention,
        YamlAttributeOverrides overrides)
      {
        this.options = options;
        this.namingConvention = namingConvention ?? (INamingConvention) new NullNamingConvention();
        this.overrides = overrides;
        this.Converters = (IList<IYamlTypeConverter>) new List<IYamlTypeConverter>();
        this.Converters.Add((IYamlTypeConverter) new GuidConverter(this.IsOptionSet(SerializationOptions.JsonCompatible)));
        this.typeResolver = this.IsOptionSet(SerializationOptions.DefaultToStaticType) ? (ITypeResolver) new StaticTypeResolver() : (ITypeResolver) new DynamicTypeResolver();
      }

      public bool IsOptionSet(SerializationOptions option) => (uint) (this.options & option) > 0U;

      private IObjectGraphVisitor<IEmitter> CreateEmittingVisitor(
        IEmitter emitter,
        IObjectGraphTraversalStrategy traversalStrategy,
        IEventEmitter eventEmitter,
        IObjectDescriptor graph)
      {
        IObjectGraphVisitor<IEmitter> nextVisitor = (IObjectGraphVisitor<IEmitter>) new CustomSerializationObjectGraphVisitor((IObjectGraphVisitor<IEmitter>) new EmittingObjectGraphVisitor(eventEmitter), (IEnumerable<IYamlTypeConverter>) this.Converters, (ObjectSerializer) ((v, t) => this.SerializeValue(emitter, v, t)));
        if (!this.IsOptionSet(SerializationOptions.DisableAliases))
        {
          AnchorAssigner anchorAssigner = new AnchorAssigner((IEnumerable<IYamlTypeConverter>) this.Converters);
          traversalStrategy.Traverse<Nothing>(graph, (IObjectGraphVisitor<Nothing>) anchorAssigner, (Nothing) null);
          nextVisitor = (IObjectGraphVisitor<IEmitter>) new AnchorAssigningObjectGraphVisitor(nextVisitor, eventEmitter, (IAliasProvider) anchorAssigner);
        }
        if (!this.IsOptionSet(SerializationOptions.EmitDefaults))
          nextVisitor = (IObjectGraphVisitor<IEmitter>) new DefaultExclusiveObjectGraphVisitor(nextVisitor);
        return nextVisitor;
      }

      private IEventEmitter CreateEventEmitter()
      {
        WriterEventEmitter writerEventEmitter = new WriterEventEmitter();
        return this.IsOptionSet(SerializationOptions.JsonCompatible) ? (IEventEmitter) new JsonEventEmitter((IEventEmitter) writerEventEmitter) : (IEventEmitter) new TypeAssigningEventEmitter((IEventEmitter) writerEventEmitter, this.IsOptionSet(SerializationOptions.Roundtrip));
      }

      private IObjectGraphTraversalStrategy CreateTraversalStrategy()
      {
        ITypeInspector innerTypeDescriptor = (ITypeInspector) new ReadablePropertiesTypeInspector(this.typeResolver);
        if (this.IsOptionSet(SerializationOptions.Roundtrip))
          innerTypeDescriptor = (ITypeInspector) new ReadableAndWritablePropertiesTypeInspector(innerTypeDescriptor);
        ITypeInspector typeInspector = (ITypeInspector) new NamingConventionTypeInspector((ITypeInspector) new YamlAttributesTypeInspector((ITypeInspector) new YamlAttributeOverridesInspector(innerTypeDescriptor, this.overrides)), this.namingConvention);
        if (this.IsOptionSet(SerializationOptions.DefaultToStaticType))
          typeInspector = (ITypeInspector) new CachedTypeInspector(typeInspector);
        return this.IsOptionSet(SerializationOptions.Roundtrip) ? (IObjectGraphTraversalStrategy) new RoundtripObjectGraphTraversalStrategy((IEnumerable<IYamlTypeConverter>) this.Converters, typeInspector, this.typeResolver, 50) : (IObjectGraphTraversalStrategy) new FullObjectGraphTraversalStrategy(typeInspector, this.typeResolver, 50, this.namingConvention);
      }

      public void SerializeValue(IEmitter emitter, object value, Type type)
      {
        ObjectDescriptor objectDescriptor = type != (Type) null ? new ObjectDescriptor(value, type, type) : new ObjectDescriptor(value, value != null ? value.GetType() : typeof (object), typeof (object));
        IObjectGraphTraversalStrategy traversalStrategy = this.CreateTraversalStrategy();
        IObjectGraphVisitor<IEmitter> emittingVisitor = this.CreateEmittingVisitor(emitter, traversalStrategy, this.CreateEventEmitter(), (IObjectDescriptor) objectDescriptor);
        traversalStrategy.Traverse<IEmitter>((IObjectDescriptor) objectDescriptor, emittingVisitor, emitter);
      }
    }
  }
}
