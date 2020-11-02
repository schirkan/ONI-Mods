// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.System
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  public struct System
  {
    public IntPtr handle;

    public static RESULT create(out FMOD.Studio.System studiosystem) => FMOD.Studio.System.FMOD_Studio_System_Create(out studiosystem.handle, 69637U);

    public RESULT setAdvancedSettings(ADVANCEDSETTINGS settings)
    {
      settings.cbsize = Marshal.SizeOf(typeof (ADVANCEDSETTINGS));
      return FMOD.Studio.System.FMOD_Studio_System_SetAdvancedSettings(this.handle, ref settings);
    }

    public RESULT getAdvancedSettings(out ADVANCEDSETTINGS settings)
    {
      settings.cbsize = Marshal.SizeOf(typeof (ADVANCEDSETTINGS));
      return FMOD.Studio.System.FMOD_Studio_System_GetAdvancedSettings(this.handle, out settings);
    }

    public RESULT initialize(
      int maxchannels,
      INITFLAGS studioFlags,
      FMOD.INITFLAGS flags,
      IntPtr extradriverdata)
    {
      return FMOD.Studio.System.FMOD_Studio_System_Initialize(this.handle, maxchannels, studioFlags, flags, extradriverdata);
    }

    public RESULT release() => FMOD.Studio.System.FMOD_Studio_System_Release(this.handle);

    public RESULT update() => FMOD.Studio.System.FMOD_Studio_System_Update(this.handle);

    public RESULT getLowLevelSystem(out FMOD.System system) => FMOD.Studio.System.FMOD_Studio_System_GetLowLevelSystem(this.handle, out system.handle);

    public RESULT getEvent(string path, out EventDescription _event)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_GetEvent(this.handle, freeHelper.byteFromStringUTF8(path), out _event.handle);
    }

    public RESULT getBus(string path, out Bus bus)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_GetBus(this.handle, freeHelper.byteFromStringUTF8(path), out bus.handle);
    }

    public RESULT getVCA(string path, out VCA vca)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_GetVCA(this.handle, freeHelper.byteFromStringUTF8(path), out vca.handle);
    }

    public RESULT getBank(string path, out Bank bank)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_GetBank(this.handle, freeHelper.byteFromStringUTF8(path), out bank.handle);
    }

    public RESULT getEventByID(Guid guid, out EventDescription _event) => FMOD.Studio.System.FMOD_Studio_System_GetEventByID(this.handle, ref guid, out _event.handle);

    public RESULT getBusByID(Guid guid, out Bus bus) => FMOD.Studio.System.FMOD_Studio_System_GetBusByID(this.handle, ref guid, out bus.handle);

    public RESULT getVCAByID(Guid guid, out VCA vca) => FMOD.Studio.System.FMOD_Studio_System_GetVCAByID(this.handle, ref guid, out vca.handle);

    public RESULT getBankByID(Guid guid, out Bank bank) => FMOD.Studio.System.FMOD_Studio_System_GetBankByID(this.handle, ref guid, out bank.handle);

    public RESULT getSoundInfo(string key, out SOUND_INFO info)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_GetSoundInfo(this.handle, freeHelper.byteFromStringUTF8(key), out info);
    }

    public RESULT lookupID(string path, out Guid guid)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_LookupID(this.handle, freeHelper.byteFromStringUTF8(path), out guid);
    }

    public RESULT lookupPath(Guid guid, out string path)
    {
      path = (string) null;
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
      {
        IntPtr num = Marshal.AllocHGlobal(256);
        int retrieved = 0;
        RESULT result = FMOD.Studio.System.FMOD_Studio_System_LookupPath(this.handle, ref guid, num, 256, out retrieved);
        if (result == RESULT.ERR_TRUNCATED)
        {
          Marshal.FreeHGlobal(num);
          num = Marshal.AllocHGlobal(retrieved);
          result = FMOD.Studio.System.FMOD_Studio_System_LookupPath(this.handle, ref guid, num, retrieved, out retrieved);
        }
        if (result == RESULT.OK)
          path = freeHelper.stringFromNative(num);
        Marshal.FreeHGlobal(num);
        return result;
      }
    }

    public RESULT getNumListeners(out int numlisteners) => FMOD.Studio.System.FMOD_Studio_System_GetNumListeners(this.handle, out numlisteners);

    public RESULT setNumListeners(int numlisteners) => FMOD.Studio.System.FMOD_Studio_System_SetNumListeners(this.handle, numlisteners);

    public RESULT getListenerAttributes(int listener, out ATTRIBUTES_3D attributes) => FMOD.Studio.System.FMOD_Studio_System_GetListenerAttributes(this.handle, listener, out attributes);

    public RESULT setListenerAttributes(int listener, ATTRIBUTES_3D attributes) => FMOD.Studio.System.FMOD_Studio_System_SetListenerAttributes(this.handle, listener, ref attributes);

    public RESULT getListenerWeight(int listener, out float weight) => FMOD.Studio.System.FMOD_Studio_System_GetListenerWeight(this.handle, listener, out weight);

    public RESULT setListenerWeight(int listener, float weight) => FMOD.Studio.System.FMOD_Studio_System_SetListenerWeight(this.handle, listener, weight);

    public RESULT loadBankFile(string name, LOAD_BANK_FLAGS flags, out Bank bank)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_LoadBankFile(this.handle, freeHelper.byteFromStringUTF8(name), flags, out bank.handle);
    }

    public RESULT loadBankMemory(byte[] buffer, LOAD_BANK_FLAGS flags, out Bank bank)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      int num = (int) FMOD.Studio.System.FMOD_Studio_System_LoadBankMemory(this.handle, gcHandle.AddrOfPinnedObject(), buffer.Length, LOAD_MEMORY_MODE.LOAD_MEMORY, flags, out bank.handle);
      gcHandle.Free();
      return (RESULT) num;
    }

    public RESULT loadBankCustom(BANK_INFO info, LOAD_BANK_FLAGS flags, out Bank bank)
    {
      info.size = Marshal.SizeOf<BANK_INFO>((M0) info);
      return FMOD.Studio.System.FMOD_Studio_System_LoadBankCustom(this.handle, ref info, flags, out bank.handle);
    }

    public RESULT unloadAll() => FMOD.Studio.System.FMOD_Studio_System_UnloadAll(this.handle);

    public RESULT flushCommands() => FMOD.Studio.System.FMOD_Studio_System_FlushCommands(this.handle);

    public RESULT flushSampleLoading() => FMOD.Studio.System.FMOD_Studio_System_FlushSampleLoading(this.handle);

    public RESULT startCommandCapture(string path, COMMANDCAPTURE_FLAGS flags)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_StartCommandCapture(this.handle, freeHelper.byteFromStringUTF8(path), flags);
    }

    public RESULT stopCommandCapture() => FMOD.Studio.System.FMOD_Studio_System_StopCommandCapture(this.handle);

    public RESULT loadCommandReplay(
      string path,
      COMMANDREPLAY_FLAGS flags,
      out CommandReplay replay)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return FMOD.Studio.System.FMOD_Studio_System_LoadCommandReplay(this.handle, freeHelper.byteFromStringUTF8(path), flags, out replay.handle);
    }

    public RESULT getBankCount(out int count) => FMOD.Studio.System.FMOD_Studio_System_GetBankCount(this.handle, out count);

    public RESULT getBankList(out Bank[] array)
    {
      array = (Bank[]) null;
      int count1;
      RESULT bankCount = FMOD.Studio.System.FMOD_Studio_System_GetBankCount(this.handle, out count1);
      if (bankCount != RESULT.OK)
        return bankCount;
      if (count1 == 0)
      {
        array = new Bank[0];
        return bankCount;
      }
      IntPtr[] array1 = new IntPtr[count1];
      int count2;
      RESULT bankList = FMOD.Studio.System.FMOD_Studio_System_GetBankList(this.handle, array1, count1, out count2);
      if (bankList != RESULT.OK)
        return bankList;
      if (count2 > count1)
        count2 = count1;
      array = new Bank[count2];
      for (int index = 0; index < count2; ++index)
        array[index].handle = array1[index];
      return RESULT.OK;
    }

    public RESULT getCPUUsage(out CPU_USAGE usage) => FMOD.Studio.System.FMOD_Studio_System_GetCPUUsage(this.handle, out usage);

    public RESULT getBufferUsage(out BUFFER_USAGE usage) => FMOD.Studio.System.FMOD_Studio_System_GetBufferUsage(this.handle, out usage);

    public RESULT resetBufferUsage() => FMOD.Studio.System.FMOD_Studio_System_ResetBufferUsage(this.handle);

    public RESULT setCallback(SYSTEM_CALLBACK callback, SYSTEM_CALLBACK_TYPE callbackmask = SYSTEM_CALLBACK_TYPE.ALL) => FMOD.Studio.System.FMOD_Studio_System_SetCallback(this.handle, callback, callbackmask);

    public RESULT getUserData(out IntPtr userdata) => FMOD.Studio.System.FMOD_Studio_System_GetUserData(this.handle, out userdata);

    public RESULT setUserData(IntPtr userdata) => FMOD.Studio.System.FMOD_Studio_System_SetUserData(this.handle, userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_Create(
      out IntPtr studiosystem,
      uint headerversion);

    [DllImport("fmodstudio")]
    private static extern bool FMOD_Studio_System_IsValid(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetAdvancedSettings(
      IntPtr studiosystem,
      ref ADVANCEDSETTINGS settings);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetAdvancedSettings(
      IntPtr studiosystem,
      out ADVANCEDSETTINGS settings);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_Initialize(
      IntPtr studiosystem,
      int maxchannels,
      INITFLAGS studioFlags,
      FMOD.INITFLAGS flags,
      IntPtr extradriverdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_Release(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_Update(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetLowLevelSystem(
      IntPtr studiosystem,
      out IntPtr system);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetEvent(
      IntPtr studiosystem,
      byte[] path,
      out IntPtr description);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBus(
      IntPtr studiosystem,
      byte[] path,
      out IntPtr bus);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetVCA(
      IntPtr studiosystem,
      byte[] path,
      out IntPtr vca);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBank(
      IntPtr studiosystem,
      byte[] path,
      out IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetEventByID(
      IntPtr studiosystem,
      ref Guid guid,
      out IntPtr description);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBusByID(
      IntPtr studiosystem,
      ref Guid guid,
      out IntPtr bus);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetVCAByID(
      IntPtr studiosystem,
      ref Guid guid,
      out IntPtr vca);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBankByID(
      IntPtr studiosystem,
      ref Guid guid,
      out IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetSoundInfo(
      IntPtr studiosystem,
      byte[] key,
      out SOUND_INFO info);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LookupID(
      IntPtr studiosystem,
      byte[] path,
      out Guid guid);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LookupPath(
      IntPtr studiosystem,
      ref Guid guid,
      IntPtr path,
      int size,
      out int retrieved);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetNumListeners(
      IntPtr studiosystem,
      out int numlisteners);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetNumListeners(
      IntPtr studiosystem,
      int numlisteners);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetListenerAttributes(
      IntPtr studiosystem,
      int listener,
      out ATTRIBUTES_3D attributes);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetListenerAttributes(
      IntPtr studiosystem,
      int listener,
      ref ATTRIBUTES_3D attributes);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetListenerWeight(
      IntPtr studiosystem,
      int listener,
      out float weight);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetListenerWeight(
      IntPtr studiosystem,
      int listener,
      float weight);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LoadBankFile(
      IntPtr studiosystem,
      byte[] filename,
      LOAD_BANK_FLAGS flags,
      out IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LoadBankMemory(
      IntPtr studiosystem,
      IntPtr buffer,
      int length,
      LOAD_MEMORY_MODE mode,
      LOAD_BANK_FLAGS flags,
      out IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LoadBankCustom(
      IntPtr studiosystem,
      ref BANK_INFO info,
      LOAD_BANK_FLAGS flags,
      out IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_UnloadAll(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_FlushCommands(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_FlushSampleLoading(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_StartCommandCapture(
      IntPtr studiosystem,
      byte[] path,
      COMMANDCAPTURE_FLAGS flags);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_StopCommandCapture(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_LoadCommandReplay(
      IntPtr studiosystem,
      byte[] path,
      COMMANDREPLAY_FLAGS flags,
      out IntPtr commandReplay);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBankCount(
      IntPtr studiosystem,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBankList(
      IntPtr studiosystem,
      IntPtr[] array,
      int capacity,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetCPUUsage(
      IntPtr studiosystem,
      out CPU_USAGE usage);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetBufferUsage(
      IntPtr studiosystem,
      out BUFFER_USAGE usage);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_ResetBufferUsage(IntPtr studiosystem);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetCallback(
      IntPtr studiosystem,
      SYSTEM_CALLBACK callback,
      SYSTEM_CALLBACK_TYPE callbackmask);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_GetUserData(
      IntPtr studiosystem,
      out IntPtr userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_System_SetUserData(
      IntPtr studiosystem,
      IntPtr userdata);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;

    public bool isValid() => this.hasHandle() && FMOD.Studio.System.FMOD_Studio_System_IsValid(this.handle);
  }
}
