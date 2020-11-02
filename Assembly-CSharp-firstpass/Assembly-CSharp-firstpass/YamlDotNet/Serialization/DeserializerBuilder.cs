// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.DeserializerBuilder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
  public sealed class DeserializerBuilder : BuilderSkeleton<DeserializerBuilder>
  {
    private IObjectFactory objectFactory = (IObjectFactory) new DefaultObjectFactory();
    private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;
    private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;
    private readonly Dictionary<string, Type> tagMappings;
    private bool ignoreUnmatched;
    private Action<string> unmatchedLogFn;

    public DeserializerBuilder()
    {
      this.tagMappings = new Dictionary<string, Type>()
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
      this.typeInspectorFactories.Add(typeof (CachedTypeInspector), (Func<ITypeInspector, ITypeInspector>) (inner => (ITypeInspector) new CachedTypeInspector(inner)));
      this.typeInspectorFactories.Add(typeof (NamingConventionTypeInspector), (Func<ITypeInspector, ITypeInspector>) (inner => this.namingConvention == null ? inner : (ITypeInspector) new NamingConventionTypeInspector(inner, this.namingConvention)));
      this.typeInspectorFactories.Add(typeof (YamlAttributesTypeInspector), (Func<ITypeInspector, ITypeInspector>) (inner => (ITypeInspector) new YamlAttributesTypeInspector(inner)));
      this.typeInspectorFactories.Add(typeof (YamlAttributeOverridesInspector), (Func<ITypeInspector, ITypeInspector>) (inner => this.overrides == null ? inner : (ITypeInspector) new YamlAttributeOverridesInspector(inner, this.overrides.Clone())));
      this.typeInspectorFactories.Add(typeof (ReadableAndWritablePropertiesTypeInspector), (Func<ITypeInspector, ITypeInspector>) (inner => (ITypeInspector) new ReadableAndWritablePropertiesTypeInspector(inner)));
      this.nodeDeserializerFactories = new LazyComponentRegistrationList<Nothing, INodeDeserializer>()
      {
        {
          typeof (YamlConvertibleNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new YamlConvertibleNodeDeserializer(this.objectFactory))
        },
        {
          typeof (YamlSerializableNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new YamlSerializableNodeDeserializer(this.objectFactory))
        },
        {
          typeof (TypeConverterNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new TypeConverterNodeDeserializer(this.BuildTypeConverters()))
        },
        {
          typeof (NullNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new NullNodeDeserializer())
        },
        {
          typeof (ScalarNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new ScalarNodeDeserializer())
        },
        {
          typeof (ArrayNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new ArrayNodeDeserializer())
        },
        {
          typeof (DictionaryNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new DictionaryNodeDeserializer(this.objectFactory))
        },
        {
          typeof (CollectionNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new CollectionNodeDeserializer(this.objectFactory))
        },
        {
          typeof (EnumerableNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new EnumerableNodeDeserializer())
        },
        {
          typeof (ObjectNodeDeserializer),
          (Func<Nothing, INodeDeserializer>) (_ => (INodeDeserializer) new ObjectNodeDeserializer(this.objectFactory, this.BuildTypeInspector(), this.ignoreUnmatched, this.unmatchedLogFn))
        }
      };
      this.nodeTypeResolverFactories = new LazyComponentRegistrationList<Nothing, INodeTypeResolver>()
      {
        {
          typeof (YamlConvertibleTypeResolver),
          (Func<Nothing, INodeTypeResolver>) (_ => (INodeTypeResolver) new YamlConvertibleTypeResolver())
        },
        {
          typeof (YamlSerializableTypeResolver),
          (Func<Nothing, INodeTypeResolver>) (_ => (INodeTypeResolver) new YamlSerializableTypeResolver())
        },
        {
          typeof (TagNodeTypeResolver),
          (Func<Nothing, INodeTypeResolver>) (_ => (INodeTypeResolver) new TagNodeTypeResolver((IDictionary<string, Type>) this.tagMappings))
        },
        {
          typeof (TypeNameInTagNodeTypeResolver),
          (Func<Nothing, INodeTypeResolver>) (_ => (INodeTypeResolver) new TypeNameInTagNodeTypeResolver())
        },
        {
          typeof (DefaultContainersNodeTypeResolver),
          (Func<Nothing, INodeTypeResolver>) (_ => (INodeTypeResolver) new DefaultContainersNodeTypeResolver())
        }
      };
      this.WithTypeResolver((ITypeResolver) new StaticTypeResolver());
    }

    protected override DeserializerBuilder Self => this;

    public DeserializerBuilder WithObjectFactory(IObjectFactory objectFactory)
    {
      this.objectFactory = objectFactory != null ? objectFactory : throw new ArgumentNullException(nameof (objectFactory));
      return this;
    }

    public DeserializerBuilder WithObjectFactory(Func<Type, object> objectFactory) => objectFactory != null ? this.WithObjectFactory((IObjectFactory) new LambdaObjectFactory(objectFactory)) : throw new ArgumentNullException(nameof (objectFactory));

    public DeserializerBuilder WithNodeDeserializer(
      INodeDeserializer nodeDeserializer)
    {
      return this.WithNodeDeserializer(nodeDeserializer, (Action<IRegistrationLocationSelectionSyntax<INodeDeserializer>>) (w => w.OnTop()));
    }

    public DeserializerBuilder WithNodeDeserializer(
      INodeDeserializer nodeDeserializer,
      Action<IRegistrationLocationSelectionSyntax<INodeDeserializer>> where)
    {
      if (nodeDeserializer == null)
        throw new ArgumentNullException(nameof (nodeDeserializer));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.nodeDeserializerFactories.CreateRegistrationLocationSelector(nodeDeserializer.GetType(), (Func<Nothing, INodeDeserializer>) (_ => nodeDeserializer)));
      return this;
    }

    public DeserializerBuilder WithNodeDeserializer<TNodeDeserializer>(
      WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory,
      Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where)
      where TNodeDeserializer : INodeDeserializer
    {
      if (nodeDeserializerFactory == null)
        throw new ArgumentNullException(nameof (nodeDeserializerFactory));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.nodeDeserializerFactories.CreateTrackingRegistrationLocationSelector(typeof (TNodeDeserializer), (Func<INodeDeserializer, Nothing, INodeDeserializer>) ((wrapped, _) => (INodeDeserializer) nodeDeserializerFactory(wrapped))));
      return this;
    }

    public DeserializerBuilder WithoutNodeDeserializer<TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer => this.WithoutNodeDeserializer(typeof (TNodeDeserializer));

    public DeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
    {
      if (nodeDeserializerType == (Type) null)
        throw new ArgumentNullException(nameof (nodeDeserializerType));
      this.nodeDeserializerFactories.Remove(nodeDeserializerType);
      return this;
    }

    public DeserializerBuilder WithNodeTypeResolver(
      INodeTypeResolver nodeTypeResolver)
    {
      return this.WithNodeTypeResolver(nodeTypeResolver, (Action<IRegistrationLocationSelectionSyntax<INodeTypeResolver>>) (w => w.OnTop()));
    }

    public DeserializerBuilder WithNodeTypeResolver(
      INodeTypeResolver nodeTypeResolver,
      Action<IRegistrationLocationSelectionSyntax<INodeTypeResolver>> where)
    {
      if (nodeTypeResolver == null)
        throw new ArgumentNullException(nameof (nodeTypeResolver));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.nodeTypeResolverFactories.CreateRegistrationLocationSelector(nodeTypeResolver.GetType(), (Func<Nothing, INodeTypeResolver>) (_ => nodeTypeResolver)));
      return this;
    }

    public DeserializerBuilder WithNodeTypeResolver<TNodeTypeResolver>(
      WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory,
      Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where)
      where TNodeTypeResolver : INodeTypeResolver
    {
      if (nodeTypeResolverFactory == null)
        throw new ArgumentNullException(nameof (nodeTypeResolverFactory));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.nodeTypeResolverFactories.CreateTrackingRegistrationLocationSelector(typeof (TNodeTypeResolver), (Func<INodeTypeResolver, Nothing, INodeTypeResolver>) ((wrapped, _) => (INodeTypeResolver) nodeTypeResolverFactory(wrapped))));
      return this;
    }

    public DeserializerBuilder WithoutNodeTypeResolver<TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver => this.WithoutNodeTypeResolver(typeof (TNodeTypeResolver));

    public DeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
    {
      if (nodeTypeResolverType == (Type) null)
        throw new ArgumentNullException(nameof (nodeTypeResolverType));
      this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
      return this;
    }

    public DeserializerBuilder WithTagMapping(string tag, Type type)
    {
      if (tag == null)
        throw new ArgumentNullException(nameof (tag));
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      Type type1;
      if (this.tagMappings.TryGetValue(tag, out type1))
        throw new ArgumentException(string.Format("Type already has a registered type '{0}' for tag '{1}'", (object) type1.FullName, (object) tag), nameof (tag));
      this.tagMappings.Add(tag, type);
      return this;
    }

    public DeserializerBuilder WithoutTagMapping(string tag)
    {
      if (tag == null)
        throw new ArgumentNullException(nameof (tag));
      if (!this.tagMappings.Remove(tag))
        throw new KeyNotFoundException(string.Format("Tag '{0}' is not registered", (object) tag));
      return this;
    }

    public DeserializerBuilder IgnoreUnmatchedProperties(
      Action<string> unmatchedLogFn = null)
    {
      this.ignoreUnmatched = true;
      this.unmatchedLogFn = unmatchedLogFn;
      return this;
    }

    public Deserializer Build() => Deserializer.FromValueDeserializer(this.BuildValueDeserializer());

    public IValueDeserializer BuildValueDeserializer() => (IValueDeserializer) new AliasValueDeserializer((IValueDeserializer) new NodeValueDeserializer((IList<INodeDeserializer>) this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), (IList<INodeTypeResolver>) this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>()));
  }
}
