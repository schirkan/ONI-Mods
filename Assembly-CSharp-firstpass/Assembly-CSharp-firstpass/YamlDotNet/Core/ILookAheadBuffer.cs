﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.ILookAheadBuffer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core
{
  internal interface ILookAheadBuffer
  {
    bool EndOfInput { get; }

    char Peek(int offset);

    void Skip(int length);
  }
}
