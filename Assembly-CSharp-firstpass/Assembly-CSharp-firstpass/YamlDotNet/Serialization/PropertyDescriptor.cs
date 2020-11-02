// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.PropertyDescriptor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public sealed class PropertyDescriptor : IPropertyDescriptor
  {
    private readonly IPropertyDescriptor baseDescriptor;

    public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
    {
      this.baseDescriptor = baseDescriptor;
      this.Name = baseDescriptor.Name;
    }

    public string Name { get; set; }

    public Type Type => this.baseDescriptor.Type;

    public Type TypeOverride
    {
      get => this.baseDescriptor.TypeOverride;
      set => this.baseDescriptor.TypeOverride = value;
    }

    public int Order { get; set; }

    public ScalarStyle ScalarStyle
    {
      get => this.baseDescriptor.ScalarStyle;
      set => this.baseDescriptor.ScalarStyle = value;
    }

    public bool CanWrite => this.baseDescriptor.CanWrite;

    public void Write(object target, object value) => this.baseDescriptor.Write(target, value);

    public T GetCustomAttribute<T>() where T : Attribute => this.baseDescriptor.GetCustomAttribute<T>();

    public IObjectDescriptor Read(object target) => this.baseDescriptor.Read(target);
  }
}
