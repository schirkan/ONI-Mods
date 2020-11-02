// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Deserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;
using YamlDotNet.Serialization.Utilities;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
  public sealed class Deserializer
  {
    private readonly Deserializer.BackwardsCompatibleConfiguration backwardsCompatibleConfiguration;
    private readonly IValueDeserializer valueDeserializer;

    private void ThrowUnlessInBackwardsCompatibleMode()
    {
      if (this.backwardsCompatibleConfiguration == null)
        throw new InvalidOperationException("This method / property exists for backwards compatibility reasons, but the Deserializer was created using the new configuration mechanism. To configure the Deserializer, use the DeserializerBuilder.");
    }

    [Obsolete("Please use DeserializerBuilder to customize the Deserializer. This property will be removed in future releases.")]
    public IList<INodeDeserializer> NodeDeserializers
    {
      get
      {
        this.ThrowUnlessInBackwardsCompatibleMode();
        return this.backwardsCompatibleConfiguration.NodeDeserializers;
      }
    }

    [Obsolete("Please use DeserializerBuilder to customize the Deserializer. This property will be removed in future releases.")]
    public IList<INodeTypeResolver> TypeResolvers
    {
      get
      {
        this.ThrowUnlessInBackwardsCompatibleMode();
        return this.backwardsCompatibleConfiguration.TypeResolvers;
      }
    }

    [Obsolete("Please use DeserializerBuilder to customize the Deserializer. This constructor will be removed in future releases.")]
    public Deserializer(
      IObjectFactory objectFactory = null,
      INamingConvention namingConvention = null,
      bool ignoreUnmatched = false,
      YamlAttributeOverrides overrides = null)
    {
      this.backwardsCompatibleConfiguration = new Deserializer.BackwardsCompatibleConfiguration(objectFactory, namingConvention, ignoreUnmatched, overrides);
      this.valueDeserializer = this.backwardsCompatibleConfiguration.valueDeserializer;
    }

    [Obsolete("Please use DeserializerBuilder to customize the Deserializer. This method will be removed in future releases.")]
    public void RegisterTagMapping(string tag, Type type)
    {
      this.ThrowUnlessInBackwardsCompatibleMode();
      this.backwardsCompatibleConfiguration.RegisterTagMapping(tag, type);
    }

    [Obsolete("Please use DeserializerBuilder to customize the Deserializer. This method will be removed in future releases.")]
    public void RegisterTypeConverter(IYamlTypeConverter typeConverter)
    {
      this.ThrowUnlessInBackwardsCompatibleMode();
      this.backwardsCompatibleConfiguration.RegisterTypeConverter(typeConverter);
    }

    public Deserializer()
    {
      this.backwardsCompatibleConfiguration = new Deserializer.BackwardsCompatibleConfiguration((IObjectFactory) null, (INamingConvention) null, false, (YamlAttributeOverrides) null);
      this.valueDeserializer = this.backwardsCompatibleConfiguration.valueDeserializer;
    }

    private Deserializer(IValueDeserializer valueDeserializer) => this.valueDeserializer = valueDeserializer != null ? valueDeserializer : throw new ArgumentNullException(nameof (valueDeserializer));

    public static Deserializer FromValueDeserializer(
      IValueDeserializer valueDeserializer)
    {
      return new Deserializer(valueDeserializer);
    }

    public T Deserialize<T>(string input)
    {
      using (StringReader stringReader = new StringReader(input))
        return (T) this.Deserialize((TextReader) stringReader, typeof (T));
    }

    public T Deserialize<T>(TextReader input) => (T) this.Deserialize(input, typeof (T));

    public object Deserialize(TextReader input) => this.Deserialize(input, typeof (object));

    public object Deserialize(string input, Type type)
    {
      using (StringReader stringReader = new StringReader(input))
        return this.Deserialize((TextReader) stringReader, type);
    }

    public object Deserialize(TextReader input, Type type) => this.Deserialize((IParser) new Parser(input), type);

    public T Deserialize<T>(IParser parser) => (T) this.Deserialize(parser, typeof (T));

    public object Deserialize(IParser parser) => this.Deserialize(parser, typeof (object));

    public object Deserialize(IParser parser, Type type)
    {
      if (parser == null)
        throw new ArgumentNullException("reader");
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      bool flag1 = parser.Allow<StreamStart>() != null;
      bool flag2 = parser.Allow<DocumentStart>() != null;
      object obj = (object) null;
      if (!parser.Accept<DocumentEnd>() && !parser.Accept<StreamEnd>())
      {
        using (SerializerState state = new SerializerState())
        {
          obj = this.valueDeserializer.DeserializeValue(parser, type, state, this.valueDeserializer);
          state.OnDeserialization();
        }
      }
      if (flag2)
        parser.Expect<DocumentEnd>();
      if (flag1)
        parser.Expect<StreamEnd>();
      return obj;
    }

    private class BackwardsCompatibleConfiguration
    {
      private static readonly Dictionary<string, Type> predefinedTagMappings = new Dictionary<string, Type>()
      {
        {
          "tag:yaml.org,2002:map",
          typeof (Dictionary<object, object>)
        },
        {
          "tag:yaml.org,2002:bool",
          typeof (bool)
        },
        {
          "tag:yaml.org,2002:float",
          typeof (double)
        },
        {
          "tag:yaml.org,2002:int",
          typeof (int)
        },
        {
          "tag:yaml.org,2002:str",
          typeof (string)
        },
        {
          "tag:yaml.org,2002:timestamp",
          typeof (DateTime)
        }
      };
      private readonly Dictionary<string, Type> tagMappings;
      private readonly List<IYamlTypeConverter> converters;
      private Deserializer.BackwardsCompatibleConfiguration.TypeDescriptorProxy typeDescriptor = new Deserializer.BackwardsCompatibleConfiguration.TypeDescriptorProxy();
      public IValueDeserializer valueDeserializer;

      public IList<INodeDeserializer> NodeDeserializers { get; private set; }

      public IList<INodeTypeResolver> TypeResolvers { get; private set; }

      public BackwardsCompatibleConfiguration(
        IObjectFactory objectFactory,
        INamingConvention namingConvention,
        bool ignoreUnmatched,
        YamlAttributeOverrides overrides)
      {
        objectFactory = objectFactory ?? (IObjectFactory) new DefaultObjectFactory();
        namingConvention = namingConvention ?? (INamingConvention) new NullNamingConvention();
        this.typeDescriptor.TypeDescriptor = (ITypeInspector) new CachedTypeInspector((ITypeInspector) new NamingConventionTypeInspector((ITypeInspector) new YamlAttributesTypeInspector((ITypeInspector) new YamlAttributeOverridesInspector((ITypeInspector) new ReadableAndWritablePropertiesTypeInspector((ITypeInspector) new ReadablePropertiesTypeInspector((ITypeResolver) new StaticTypeResolver())), overrides)), namingConvention));
        this.converters = new List<IYamlTypeConverter>();
        this.converters.Add((IYamlTypeConverter) new GuidConverter(false));
        this.NodeDeserializers = (IList<INodeDeserializer>) new List<INodeDeserializer>();
        this.NodeDeserializers.Add((INodeDeserializer) new YamlConvertibleNodeDeserializer(objectFactory));
        this.NodeDeserializers.Add((INodeDeserializer) new YamlSerializableNodeDeserializer(objectFactory));
        this.NodeDeserializers.Add((INodeDeserializer) new TypeConverterNodeDeserializer((IEnumerable<IYamlTypeConverter>) this.converters));
        this.NodeDeserializers.Add((INodeDeserializer) new NullNodeDeserializer());
        this.NodeDeserializers.Add((INodeDeserializer) new ScalarNodeDeserializer());
        this.NodeDeserializers.Add((INodeDeserializer) new ArrayNodeDeserializer());
        this.NodeDeserializers.Add((INodeDeserializer) new DictionaryNodeDeserializer(objectFactory));
        this.NodeDeserializers.Add((INodeDeserializer) new CollectionNodeDeserializer(objectFactory));
        this.NodeDeserializers.Add((INodeDeserializer) new EnumerableNodeDeserializer());
        this.NodeDeserializers.Add((INodeDeserializer) new ObjectNodeDeserializer(objectFactory, (ITypeInspector) this.typeDescriptor, ignoreUnmatched));
        this.tagMappings = new Dictionary<string, Type>((IDictionary<string, Type>) Deserializer.BackwardsCompatibleConfiguration.predefinedTagMappings);
        this.TypeResolvers = (IList<INodeTypeResolver>) new List<INodeTypeResolver>();
        this.TypeResolvers.Add((INodeTypeResolver) new YamlConvertibleTypeResolver());
        this.TypeResolvers.Add((INodeTypeResolver) new YamlSerializableTypeResolver());
        this.TypeResolvers.Add((INodeTypeResolver) new TagNodeTypeResolver((IDictionary<string, Type>) this.tagMappings));
        this.TypeResolvers.Add((INodeTypeResolver) new TypeNameInTagNodeTypeResolver());
        this.TypeResolvers.Add((INodeTypeResolver) new DefaultContainersNodeTypeResolver());
        this.valueDeserializer = (IValueDeserializer) new AliasValueDeserializer((IValueDeserializer) new NodeValueDeserializer(this.NodeDeserializers, this.TypeResolvers));
      }

      public void RegisterTagMapping(string tag, Type type) => this.tagMappings.Add(tag, type);

      public void RegisterTypeConverter(IYamlTypeConverter typeConverter) => this.converters.Insert(0, typeConverter);

      private class TypeDescriptorProxy : ITypeInspector
      {
        public ITypeInspector TypeDescriptor;

        public IEnumerable<IPropertyDescriptor> GetProperties(
          Type type,
          object container)
        {
          return this.TypeDescriptor.GetProperties(type, container);
        }

        public IPropertyDescriptor GetProperty(
          Type type,
          object container,
          string name,
          bool ignoreUnmatched)
        {
          return this.TypeDescriptor.GetProperty(type, container, name, ignoreUnmatched);
        }
      }
    }
  }
}
