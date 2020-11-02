// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.YamlAttributeOverridesInspector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
  public sealed class YamlAttributeOverridesInspector : TypeInspectorSkeleton
  {
    private readonly ITypeInspector innerTypeDescriptor;
    private readonly YamlAttributeOverrides overrides;

    public YamlAttributeOverridesInspector(
      ITypeInspector innerTypeDescriptor,
      YamlAttributeOverrides overrides)
    {
      this.innerTypeDescriptor = innerTypeDescriptor;
      this.overrides = overrides;
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(
      Type type,
      object container)
    {
      return this.overrides == null ? this.innerTypeDescriptor.GetProperties(type, container) : this.innerTypeDescriptor.GetProperties(type, container).Select<IPropertyDescriptor, IPropertyDescriptor>((Func<IPropertyDescriptor, IPropertyDescriptor>) (p => (IPropertyDescriptor) new YamlAttributeOverridesInspector.OverridePropertyDescriptor(p, this.overrides, type)));
    }

    public sealed class OverridePropertyDescriptor : IPropertyDescriptor
    {
      private readonly IPropertyDescriptor baseDescriptor;
      private readonly YamlAttributeOverrides overrides;
      private readonly Type classType;

      public OverridePropertyDescriptor(
        IPropertyDescriptor baseDescriptor,
        YamlAttributeOverrides overrides,
        Type classType)
      {
        this.baseDescriptor = baseDescriptor;
        this.overrides = overrides;
        this.classType = classType;
      }

      public string Name => this.baseDescriptor.Name;

      public bool CanWrite => this.baseDescriptor.CanWrite;

      public Type Type => this.baseDescriptor.Type;

      public Type TypeOverride
      {
        get => this.baseDescriptor.TypeOverride;
        set => this.baseDescriptor.TypeOverride = value;
      }

      public int Order
      {
        get => this.baseDescriptor.Order;
        set => this.baseDescriptor.Order = value;
      }

      public ScalarStyle ScalarStyle
      {
        get => this.baseDescriptor.ScalarStyle;
        set => this.baseDescriptor.ScalarStyle = value;
      }

      public void Write(object target, object value) => this.baseDescriptor.Write(target, value);

      public T GetCustomAttribute<T>() where T : Attribute
      {
        T attribute = this.overrides.GetAttribute<T>(this.classType, this.Name);
        return (object) attribute != null ? attribute : this.baseDescriptor.GetCustomAttribute<T>();
      }

      public IObjectDescriptor Read(object target) => this.baseDescriptor.Read(target);
    }
  }
}
