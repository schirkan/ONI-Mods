// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.IPropertyDescriptor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public interface IPropertyDescriptor
  {
    string Name { get; }

    bool CanWrite { get; }

    Type Type { get; }

    Type TypeOverride { get; set; }

    int Order { get; set; }

    ScalarStyle ScalarStyle { get; set; }

    T GetCustomAttribute<T>() where T : Attribute;

    IObjectDescriptor Read(object target);

    void Write(object target, object value);
  }
}
