// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackDispatcher
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Steamworks
{
  public static class CallbackDispatcher
  {
    public static void ExceptionHandler(Exception e) => Debug.LogException(e);
  }
}
