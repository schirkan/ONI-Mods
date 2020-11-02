// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.SerializationOptions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Serialization
{
  [Flags]
  public enum SerializationOptions
  {
    None = 0,
    Roundtrip = 1,
    DisableAliases = 2,
    EmitDefaults = 4,
    JsonCompatible = 8,
    DefaultToStaticType = 16, // 0x00000010
  }
}
