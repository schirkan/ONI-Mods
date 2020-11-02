// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_NeedsPaint_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_NeedsPaint_t
  {
    public const int k_iCallback = 4502;
    public HHTMLBrowser unBrowserHandle;
    public IntPtr pBGRA;
    public uint unWide;
    public uint unTall;
    public uint unUpdateX;
    public uint unUpdateY;
    public uint unUpdateWide;
    public uint unUpdateTall;
    public uint unScrollX;
    public uint unScrollY;
    public float flPageScale;
    public uint unPageSerial;
  }
}
