// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_LinkAtPosition_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4513)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_LinkAtPosition_t
  {
    public const int k_iCallback = 4513;
    public HHTMLBrowser unBrowserHandle;
    public uint x;
    public uint y;
    public string pchURL;
    [MarshalAs(UnmanagedType.I1)]
    public bool bInput;
    [MarshalAs(UnmanagedType.I1)]
    public bool bLiveLink;
  }
}
