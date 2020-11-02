﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.TypeInspectors.ReadablePropertiesTypeInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
  public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton
  {
    private readonly ITypeResolver _typeResolver;

    public ReadablePropertiesTypeInspector(ITypeResolver typeResolver) => this._typeResolver = typeResolver != null ? typeResolver : throw new ArgumentNullException(nameof (typeResolver));

    private static bool IsValidProperty(PropertyInfo property) => property.CanRead && property.GetGetMethod().GetParameters().Length == 0;

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      return type.GetPublicProperties().Where<PropertyInfo>(new Func<PropertyInfo, bool>(ReadablePropertiesTypeInspector.IsValidProperty)).Select<PropertyInfo, IPropertyDescriptor>((Func<PropertyInfo, IPropertyDescriptor>) (p => (IPropertyDescriptor) new ReadablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this._typeResolver)));
    }

    private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
    {
      private readonly PropertyInfo _propertyInfo;
      private readonly ITypeResolver _typeResolver;

      public ReflectionPropertyDescriptor(PropertyInfo propertyInfo, ITypeResolver typeResolver)
      {
        this._propertyInfo = propertyInfo;
        this._typeResolver = typeResolver;
        this.ScalarStyle = ScalarStyle.Any;
      }

      public string Name => this._propertyInfo.Name;

      public Type Type => this._propertyInfo.PropertyType;

      public Type TypeOverride { get; set; }

      public int Order { get; set; }

      public bool CanWrite => this._propertyInfo.CanWrite;

      public ScalarStyle ScalarStyle { get; set; }

      public void Write(object target, object value) => this._propertyInfo.SetValue(target, value, (object[]) null);

      public T GetCustomAttribute<T>() where T : Attribute => (T) ((IEnumerable<object>) this._propertyInfo.GetCustomAttributes(typeof (T), true)).FirstOrDefault<object>();

      public IObjectDescriptor Read(object target)
      {
        object actualValue = this._propertyInfo.ReadValue(target);
        Type type1 = this.TypeOverride;
        if ((object) type1 == null)
          type1 = this._typeResolver.Resolve(this.Type, actualValue);
        Type type2 = type1;
        return (IObjectDescriptor) new ObjectDescriptor(actualValue, type2, this.Type, this.ScalarStyle);
      }
    }
  }
}
