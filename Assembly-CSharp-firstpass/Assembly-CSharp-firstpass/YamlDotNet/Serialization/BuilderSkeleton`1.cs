// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.BuilderSkeleton`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
  public abstract class BuilderSkeleton<TBuilder> where TBuilder : BuilderSkeleton<TBuilder>
  {
    internal INamingConvention namingConvention;
    internal ITypeResolver typeResolver;
    internal readonly YamlAttributeOverrides overrides;
    internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;
    internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

    internal BuilderSkeleton()
    {
      this.overrides = new YamlAttributeOverrides();
      this.typeConverterFactories = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
      this.typeConverterFactories.Add(typeof (GuidConverter), (Func<Nothing, IYamlTypeConverter>) (_ => (IYamlTypeConverter) new GuidConverter(false)));
      this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
    }

    protected abstract TBuilder Self { get; }

    internal ITypeInspector BuildTypeInspector() => this.typeInspectorFactories.BuildComponentChain<ITypeInspector>((ITypeInspector) new ReadablePropertiesTypeInspector(this.typeResolver));

    public TBuilder WithNamingConvention(INamingConvention namingConvention)
    {
      this.namingConvention = namingConvention != null ? namingConvention : throw new ArgumentNullException(nameof (namingConvention));
      return this.Self;
    }

    public TBuilder WithTypeResolver(ITypeResolver typeResolver)
    {
      this.typeResolver = typeResolver != null ? typeResolver : throw new ArgumentNullException(nameof (typeResolver));
      return this.Self;
    }

    public TBuilder WithAttributeOverride<TClass>(
      Expression<Func<TClass, object>> propertyAccessor,
      Attribute attribute)
    {
      this.overrides.Add<TClass>(propertyAccessor, attribute);
      return this.Self;
    }

    public TBuilder WithAttributeOverride(Type type, string member, Attribute attribute)
    {
      this.overrides.Add(type, member, attribute);
      return this.Self;
    }

    public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter) => this.WithTypeConverter(typeConverter, (Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>) (w => w.OnTop()));

    public TBuilder WithTypeConverter(
      IYamlTypeConverter typeConverter,
      Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
    {
      if (typeConverter == null)
        throw new ArgumentNullException(nameof (typeConverter));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.typeConverterFactories.CreateRegistrationLocationSelector(typeConverter.GetType(), (Func<Nothing, IYamlTypeConverter>) (_ => typeConverter)));
      return this.Self;
    }

    public TBuilder WithTypeConverter<TYamlTypeConverter>(
      WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory,
      Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
      where TYamlTypeConverter : IYamlTypeConverter
    {
      if (typeConverterFactory == null)
        throw new ArgumentNullException(nameof (typeConverterFactory));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.typeConverterFactories.CreateTrackingRegistrationLocationSelector(typeof (TYamlTypeConverter), (Func<IYamlTypeConverter, Nothing, IYamlTypeConverter>) ((wrapped, _) => typeConverterFactory(wrapped))));
      return this.Self;
    }

    public TBuilder WithoutTypeConverter<TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter => this.WithoutTypeConverter(typeof (TYamlTypeConverter));

    public TBuilder WithoutTypeConverter(Type converterType)
    {
      if (converterType == (Type) null)
        throw new ArgumentNullException(nameof (converterType));
      this.typeConverterFactories.Remove(converterType);
      return this.Self;
    }

    public TBuilder WithTypeInspector<TTypeInspector>(
      Func<ITypeInspector, TTypeInspector> typeInspectorFactory)
      where TTypeInspector : ITypeInspector
    {
      return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, (Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>) (w => w.OnTop()));
    }

    public TBuilder WithTypeInspector<TTypeInspector>(
      Func<ITypeInspector, TTypeInspector> typeInspectorFactory,
      Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where)
      where TTypeInspector : ITypeInspector
    {
      if (typeInspectorFactory == null)
        throw new ArgumentNullException(nameof (typeInspectorFactory));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.typeInspectorFactories.CreateRegistrationLocationSelector(typeof (TTypeInspector), (Func<ITypeInspector, ITypeInspector>) (inner => (ITypeInspector) typeInspectorFactory(inner))));
      return this.Self;
    }

    public TBuilder WithTypeInspector<TTypeInspector>(
      WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory,
      Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where)
      where TTypeInspector : ITypeInspector
    {
      if (typeInspectorFactory == null)
        throw new ArgumentNullException(nameof (typeInspectorFactory));
      if (where == null)
        throw new ArgumentNullException(nameof (where));
      where(this.typeInspectorFactories.CreateTrackingRegistrationLocationSelector(typeof (TTypeInspector), (Func<ITypeInspector, ITypeInspector, ITypeInspector>) ((wrapped, inner) => (ITypeInspector) typeInspectorFactory(wrapped, inner))));
      return this.Self;
    }

    public TBuilder WithoutTypeInspector<TTypeInspector>() where TTypeInspector : ITypeInspector => this.WithoutTypeInspector(typeof (ITypeInspector));

    public TBuilder WithoutTypeInspector(Type inspectorType)
    {
      if (inspectorType == (Type) null)
        throw new ArgumentNullException(nameof (inspectorType));
      this.typeInspectorFactories.Remove(inspectorType);
      return this.Self;
    }

    protected IEnumerable<IYamlTypeConverter> BuildTypeConverters() => (IEnumerable<IYamlTypeConverter>) this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
  }
}
