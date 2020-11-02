// Decompiled with JetBrains decompiler
// Type: FMOD.Reverb3D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD
{
  public struct Reverb3D
  {
    public IntPtr handle;

    public RESULT release() => Reverb3D.FMOD5_Reverb3D_Release(this.handle);

    public RESULT set3DAttributes(ref VECTOR position, float mindistance, float maxdistance) => Reverb3D.FMOD5_Reverb3D_Set3DAttributes(this.handle, ref position, mindistance, maxdistance);

    public RESULT get3DAttributes(
      ref VECTOR position,
      ref float mindistance,
      ref float maxdistance)
    {
      return Reverb3D.FMOD5_Reverb3D_Get3DAttributes(this.handle, ref position, ref mindistance, ref maxdistance);
    }

    public RESULT setProperties(ref REVERB_PROPERTIES properties) => Reverb3D.FMOD5_Reverb3D_SetProperties(this.handle, ref properties);

    public RESULT getProperties(ref REVERB_PROPERTIES properties) => Reverb3D.FMOD5_Reverb3D_GetProperties(this.handle, ref properties);

    public RESULT setActive(bool active) => Reverb3D.FMOD5_Reverb3D_SetActive(this.handle, active);

    public RESULT getActive(out bool active) => Reverb3D.FMOD5_Reverb3D_GetActive(this.handle, out active);

    public RESULT setUserData(IntPtr userdata) => Reverb3D.FMOD5_Reverb3D_SetUserData(this.handle, userdata);

    public RESULT getUserData(out IntPtr userdata) => Reverb3D.FMOD5_Reverb3D_GetUserData(this.handle, out userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_Release(IntPtr reverb);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_Set3DAttributes(
      IntPtr reverb,
      ref VECTOR position,
      float mindistance,
      float maxdistance);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_Get3DAttributes(
      IntPtr reverb,
      ref VECTOR position,
      ref float mindistance,
      ref float maxdistance);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_SetProperties(
      IntPtr reverb,
      ref REVERB_PROPERTIES properties);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_GetProperties(
      IntPtr reverb,
      ref REVERB_PROPERTIES properties);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_SetActive(IntPtr reverb, bool active);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_GetActive(IntPtr reverb, out bool active);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_SetUserData(IntPtr reverb, IntPtr userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD5_Reverb3D_GetUserData(
      IntPtr reverb,
      out IntPtr userdata);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;
  }
}
