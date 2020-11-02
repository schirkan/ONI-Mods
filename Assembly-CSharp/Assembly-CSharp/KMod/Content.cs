// Decompiled with JetBrains decompiler
// Type: KMod.Content
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace KMod
{
  [Flags]
  public enum Content : byte
  {
    LayerableFiles = 1,
    Strings = 2,
    DLL = 4,
    Translation = 8,
    Animation = 16, // 0x10
  }
}
