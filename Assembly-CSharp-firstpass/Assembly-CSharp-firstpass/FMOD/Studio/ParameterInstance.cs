// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.ParameterInstance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  public struct ParameterInstance
  {
    public IntPtr handle;

    public RESULT getDescription(out PARAMETER_DESCRIPTION description) => ParameterInstance.FMOD_Studio_ParameterInstance_GetDescription(this.handle, out description);

    public RESULT getValue(out float value) => ParameterInstance.FMOD_Studio_ParameterInstance_GetValue(this.handle, out value);

    public RESULT setValue(float value) => ParameterInstance.FMOD_Studio_ParameterInstance_SetValue(this.handle, value);

    [DllImport("fmodstudio")]
    private static extern bool FMOD_Studio_ParameterInstance_IsValid(IntPtr parameter);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_ParameterInstance_GetDescription(
      IntPtr parameter,
      out PARAMETER_DESCRIPTION description);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_ParameterInstance_GetValue(
      IntPtr parameter,
      out float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_ParameterInstance_SetValue(
      IntPtr parameter,
      float value);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;

    public bool isValid() => this.hasHandle() && ParameterInstance.FMOD_Studio_ParameterInstance_IsValid(this.handle);
  }
}
