// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerAnalogActionData_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ControllerAnalogActionData_t
  {
    public EControllerSourceMode eMode;
    public float x;
    public float y;
    public byte bActive;
  }
}
