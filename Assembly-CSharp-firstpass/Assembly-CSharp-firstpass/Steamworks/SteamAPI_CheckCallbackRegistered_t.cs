// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI_CheckCallbackRegistered_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
