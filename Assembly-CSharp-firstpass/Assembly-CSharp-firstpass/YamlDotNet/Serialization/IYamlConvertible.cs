﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.IYamlConvertible
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public interface IYamlConvertible
  {
    void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer);

    void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer);
  }
}
