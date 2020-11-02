// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectDescriptor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public sealed class ObjectDescriptor : IObjectDescriptor
  {
    public object Value { get; private set; }

    public Type Type { get; private set; }

    public Type StaticType { get; private set; }

    public ScalarStyle ScalarStyle { get; private set; }

    public ObjectDescriptor(object value, Type type, Type staticType)
      : this(value, type, staticType, ScalarStyle.Any)
    {
    }

    public ObjectDescriptor(object value, Type type, Type staticType, ScalarStyle scalarStyle)
    {
      this.Value = value;
      this.Type = !(type == (Type) null) ? type : throw new ArgumentNullException(nameof (type));
      this.StaticType = !(staticType == (Type) null) ? staticType : throw new ArgumentNullException(nameof (staticType));
      this.ScalarStyle = scalarStyle;
    }
  }
}
