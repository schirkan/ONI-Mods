// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Utilities.SerializerState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.Utilities
{
  public sealed class SerializerState : IDisposable
  {
    private readonly IDictionary<Type, object> items = (IDictionary<Type, object>) new Dictionary<Type, object>();

    public T Get<T>() where T : class, new()
    {
      object obj;
      if (!this.items.TryGetValue(typeof (T), out obj))
      {
        obj = (object) new T();
        this.items.Add(typeof (T), obj);
      }
      return (T) obj;
    }

    public void OnDeserialization()
    {
      foreach (IPostDeserializationCallback deserializationCallback in this.items.Values.OfType<IPostDeserializationCallback>())
        deserializationCallback.OnDeserialization();
    }

    public void Dispose()
    {
      foreach (IDisposable disposable in this.items.Values.OfType<IDisposable>())
        disposable.Dispose();
    }
  }
}
