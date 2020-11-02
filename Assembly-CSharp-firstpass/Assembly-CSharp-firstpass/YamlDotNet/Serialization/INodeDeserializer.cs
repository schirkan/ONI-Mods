// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.INodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public interface INodeDeserializer
  {
    bool Deserialize(
      IParser reader,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value);
  }
}
