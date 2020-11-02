// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_NewWindow_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4521)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_NewWindow_t
  {
    public const int k_iCallback = 4521;
    public HHTMLBrowser unBrowserHandle;
    public string pchURL;
    public uint unX;
    public uint unY;
    public uint unWide;
    public uint unTall;
    public HHTMLBrowser unNewWindow_BrowserHandle_IGNORE;
  }
}
