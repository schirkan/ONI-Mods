// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectFactories.LambdaObjectFactory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Serialization.ObjectFactories
{
  public sealed class LambdaObjectFactory : IObjectFactory
  {
    private readonly Func<Type, object> _factory;

    public LambdaObjectFactory(Func<Type, object> factory) => this._factory = factory != null ? factory : throw new ArgumentNullException(nameof (factory));

    public object Create(Type type) => this._factory(type);
  }
}
