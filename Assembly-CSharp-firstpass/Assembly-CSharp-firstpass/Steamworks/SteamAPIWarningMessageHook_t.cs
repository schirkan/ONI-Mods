// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPIWarningMessageHook_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}
