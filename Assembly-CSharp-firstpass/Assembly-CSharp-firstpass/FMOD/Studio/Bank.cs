// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.Bank
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  public struct Bank
  {
    public IntPtr handle;

    public RESULT getID(out Guid id) => Bank.FMOD_Studio_Bank_GetID(this.handle, out id);

    public RESULT getPath(out string path)
    {
      path = (string) null;
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
      {
        IntPtr num = Marshal.AllocHGlobal(256);
        int retrieved = 0;
        RESULT path1 = Bank.FMOD_Studio_Bank_GetPath(this.handle, num, 256, out retrieved);
        if (path1 == RESULT.ERR_TRUNCATED)
        {
          Marshal.FreeHGlobal(num);
          num = Marshal.AllocHGlobal(retrieved);
          path1 = Bank.FMOD_Studio_Bank_GetPath(this.handle, num, retrieved, out retrieved);
        }
        if (path1 == RESULT.OK)
          path = freeHelper.stringFromNative(num);
        Marshal.FreeHGlobal(num);
        return path1;
      }
    }

    public RESULT unload() => Bank.FMOD_Studio_Bank_Unload(this.handle);

    public RESULT loadSampleData() => Bank.FMOD_Studio_Bank_LoadSampleData(this.handle);

    public RESULT unloadSampleData() => Bank.FMOD_Studio_Bank_UnloadSampleData(this.handle);

    public RESULT getLoadingState(out LOADING_STATE state) => Bank.FMOD_Studio_Bank_GetLoadingState(this.handle, out state);

    public RESULT getSampleLoadingState(out LOADING_STATE state) => Bank.FMOD_Studio_Bank_GetSampleLoadingState(this.handle, out state);

    public RESULT getStringCount(out int count) => Bank.FMOD_Studio_Bank_GetStringCount(this.handle, out count);

    public RESULT getStringInfo(int index, out Guid id, out string path)
    {
      path = (string) null;
      id = Guid.Empty;
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
      {
        IntPtr num = Marshal.AllocHGlobal(256);
        int retrieved = 0;
        RESULT stringInfo = Bank.FMOD_Studio_Bank_GetStringInfo(this.handle, index, out id, num, 256, out retrieved);
        if (stringInfo == RESULT.ERR_TRUNCATED)
        {
          Marshal.FreeHGlobal(num);
          num = Marshal.AllocHGlobal(retrieved);
          stringInfo = Bank.FMOD_Studio_Bank_GetStringInfo(this.handle, index, out id, num, retrieved, out retrieved);
        }
        if (stringInfo == RESULT.OK)
          path = freeHelper.stringFromNative(num);
        Marshal.FreeHGlobal(num);
        return stringInfo;
      }
    }

    public RESULT getEventCount(out int count) => Bank.FMOD_Studio_Bank_GetEventCount(this.handle, out count);

    public RESULT getEventList(out EventDescription[] array)
    {
      array = (EventDescription[]) null;
      int count1;
      RESULT eventCount = Bank.FMOD_Studio_Bank_GetEventCount(this.handle, out count1);
      if (eventCount != RESULT.OK)
        return eventCount;
      if (count1 == 0)
      {
        array = new EventDescription[0];
        return eventCount;
      }
      IntPtr[] array1 = new IntPtr[count1];
      int count2;
      RESULT eventList = Bank.FMOD_Studio_Bank_GetEventList(this.handle, array1, count1, out count2);
      if (eventList != RESULT.OK)
        return eventList;
      if (count2 > count1)
        count2 = count1;
      array = new EventDescription[count2];
      for (int index = 0; index < count2; ++index)
        array[index].handle = array1[index];
      return RESULT.OK;
    }

    public RESULT getBusCount(out int count) => Bank.FMOD_Studio_Bank_GetBusCount(this.handle, out count);

    public RESULT getBusList(out Bus[] array)
    {
      array = (Bus[]) null;
      int count1;
      RESULT busCount = Bank.FMOD_Studio_Bank_GetBusCount(this.handle, out count1);
      if (busCount != RESULT.OK)
        return busCount;
      if (count1 == 0)
      {
        array = new Bus[0];
        return busCount;
      }
      IntPtr[] array1 = new IntPtr[count1];
      int count2;
      RESULT busList = Bank.FMOD_Studio_Bank_GetBusList(this.handle, array1, count1, out count2);
      if (busList != RESULT.OK)
        return busList;
      if (count2 > count1)
        count2 = count1;
      array = new Bus[count2];
      for (int index = 0; index < count2; ++index)
        array[index].handle = array1[index];
      return RESULT.OK;
    }

    public RESULT getVCACount(out int count) => Bank.FMOD_Studio_Bank_GetVCACount(this.handle, out count);

    public RESULT getVCAList(out VCA[] array)
    {
      array = (VCA[]) null;
      int count1;
      RESULT vcaCount = Bank.FMOD_Studio_Bank_GetVCACount(this.handle, out count1);
      if (vcaCount != RESULT.OK)
        return vcaCount;
      if (count1 == 0)
      {
        array = new VCA[0];
        return vcaCount;
      }
      IntPtr[] array1 = new IntPtr[count1];
      int count2;
      RESULT vcaList = Bank.FMOD_Studio_Bank_GetVCAList(this.handle, array1, count1, out count2);
      if (vcaList != RESULT.OK)
        return vcaList;
      if (count2 > count1)
        count2 = count1;
      array = new VCA[count2];
      for (int index = 0; index < count2; ++index)
        array[index].handle = array1[index];
      return RESULT.OK;
    }

    public RESULT getUserData(out IntPtr userdata) => Bank.FMOD_Studio_Bank_GetUserData(this.handle, out userdata);

    public RESULT setUserData(IntPtr userdata) => Bank.FMOD_Studio_Bank_SetUserData(this.handle, userdata);

    [DllImport("fmodstudio")]
    private static extern bool FMOD_Studio_Bank_IsValid(IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetID(IntPtr bank, out Guid id);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetPath(
      IntPtr bank,
      IntPtr path,
      int size,
      out int retrieved);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_Unload(IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_LoadSampleData(IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_UnloadSampleData(IntPtr bank);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetLoadingState(
      IntPtr bank,
      out LOADING_STATE state);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetSampleLoadingState(
      IntPtr bank,
      out LOADING_STATE state);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetStringCount(IntPtr bank, out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetStringInfo(
      IntPtr bank,
      int index,
      out Guid id,
      IntPtr path,
      int size,
      out int retrieved);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetEventCount(IntPtr bank, out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetEventList(
      IntPtr bank,
      IntPtr[] array,
      int capacity,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetBusCount(IntPtr bank, out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetBusList(
      IntPtr bank,
      IntPtr[] array,
      int capacity,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetVCACount(IntPtr bank, out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetVCAList(
      IntPtr bank,
      IntPtr[] array,
      int capacity,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_GetUserData(
      IntPtr bank,
      out IntPtr userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_Bank_SetUserData(IntPtr bank, IntPtr userdata);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;

    public bool isValid() => this.hasHandle() && Bank.FMOD_Studio_Bank_IsValid(this.handle);
  }
}
