// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.Bus
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  public struct Bus
  {
    public IntPtr handle;

    public RESULT getID(out Guid id) => Bus.FMOD_Studio_Bus_GetID(this.handle, out id);

    public RESULT getPath(out string path)
    {
      path = (string) null;
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
      {
        IntPtr num = Marshal.AllocHGlobal(256);
        int retrieved = 0;
        RESULT path1 = Bus.FMOD_Studio_Bus_GetPath(this.handle, num, 256, out retrieved);
        if (path1 == RESULT.ERR_TRUNCATED)
        {
          Marshal.FreeHGlobal(num);
          num = Marshal.AllocHGlobal(retrieved);
          path1 = Bus.FMOD_Studio_Bus_GetPath(this.handle, num, retrieved, out retrieved);
        }
        if (path1 == RESULT.OK)
          path = freeHelper.stringFromNative(num);
        Marshal.FreeHGlobal(num);
        return path1;
      }
    }

    public RESULT getVolume(out float volume, out float finalvolume) => Bus.FMOD_Studio_Bus_GetVolume(this.handle, out volume, out finalvolume);

    public RESULT setVolume(float volume) => Bus.FMOD_Studio_Bus_SetVolume(this.handle, volume);

    public RESULT getPaused(out bool paused) => Bus.FMOD_Studio_Bus_GetPaused(this.handle, out paused);

    public RESULT setPaused(bool paused) => Bus.FMOD_Studio_Bus_SetPaused(this.handle, paused);

    public RESULT getMute(out bool mute) => Bus.FMOD_Studio_Bus_GetMute(this.handle, out mute);

    public RESULT setMute(bool mute) => Bus.FMOD_Studio_Bus_SetMute(this.handle, mute);

    public RESULT stopAllEvents(STOP_MODE mode) => Bus.FMOD_Studio_Bus_StopAllEvents(this.handle, mode);

    public RESULT lockChannelGroup() => Bus.FMOD_Studio_Bus_LockChannelGroup(this.handle);

    public RESULT unlockChannelGroup() => Bus.FMOD_Studio_Bus_UnlockChannelGroup(this.handle);

    public RESULT getChannelGroup(out ChannelGroup group) => Bus.FMOD_Studio_Bus_GetChannelGroup(this.handle, out group.handle);

    [DllImport("fmodstudio")]
    private static extern bool FMOD_Studio_Bus_IsValid(IntPtr bus);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetID(IntPtr bus, out Guid id);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetPath(
      IntPtr bus,
      IntPtr path,
      int size,
      out int retrieved);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetVolume(
      IntPtr bus,
      out float volume,
      out float finalvolume);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_SetVolume(IntPtr bus, float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetPaused(IntPtr bus, out bool paused);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_SetPaused(IntPtr bus, bool paused);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetMute(IntPtr bus, out bool mute);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_SetMute(IntPtr bus, bool mute);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_StopAllEvents(IntPtr bus, STOP_MODE mode);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_LockChannelGroup(IntPtr bus);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_UnlockChannelGroup(IntPtr bus);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bus_GetChannelGroup(IntPtr bus, out IntPtr group);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;

    public bool isValid() => this.hasHandle() && Bus.FMOD_Studio_Bus_IsValid(this.handle);
  }
}
