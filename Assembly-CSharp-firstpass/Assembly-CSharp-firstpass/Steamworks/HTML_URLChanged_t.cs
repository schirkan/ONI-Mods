﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_URLChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4505)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_URLChanged_t
  {
    public const int k_iCallback = 4505;
    public HHTMLBrowser unBrowserHandle;
    public string pchURL;
    public string pchPostData;
    [MarshalAs(UnmanagedType.I1)]
    public bool bIsRedirect;
    public string pchPageTitle;
    [MarshalAs(UnmanagedType.I1)]
    public bool bNewNavigation;
  }
}