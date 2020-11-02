﻿// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.EventInstance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
  public struct EventInstance
  {
    public IntPtr handle;

    public RESULT getDescription(out EventDescription description) => EventInstance.FMOD_Studio_EventInstance_GetDescription(this.handle, out description.handle);

    public RESULT getVolume(out float volume, out float finalvolume) => EventInstance.FMOD_Studio_EventInstance_GetVolume(this.handle, out volume, out finalvolume);

    public RESULT setVolume(float volume) => EventInstance.FMOD_Studio_EventInstance_SetVolume(this.handle, volume);

    public RESULT getPitch(out float pitch, out float finalpitch) => EventInstance.FMOD_Studio_EventInstance_GetPitch(this.handle, out pitch, out finalpitch);

    public RESULT setPitch(float pitch) => EventInstance.FMOD_Studio_EventInstance_SetPitch(this.handle, pitch);

    public RESULT get3DAttributes(out ATTRIBUTES_3D attributes) => EventInstance.FMOD_Studio_EventInstance_Get3DAttributes(this.handle, out attributes);

    public RESULT set3DAttributes(ATTRIBUTES_3D attributes) => EventInstance.FMOD_Studio_EventInstance_Set3DAttributes(this.handle, ref attributes);

    public RESULT getListenerMask(out uint mask) => EventInstance.FMOD_Studio_EventInstance_GetListenerMask(this.handle, out mask);

    public RESULT setListenerMask(uint mask) => EventInstance.FMOD_Studio_EventInstance_SetListenerMask(this.handle, mask);

    public RESULT getProperty(EVENT_PROPERTY index, out float value) => EventInstance.FMOD_Studio_EventInstance_GetProperty(this.handle, index, out value);

    public RESULT setProperty(EVENT_PROPERTY index, float value) => EventInstance.FMOD_Studio_EventInstance_SetProperty(this.handle, index, value);

    public RESULT getReverbLevel(int index, out float level) => EventInstance.FMOD_Studio_EventInstance_GetReverbLevel(this.handle, index, out level);

    public RESULT setReverbLevel(int index, float level) => EventInstance.FMOD_Studio_EventInstance_SetReverbLevel(this.handle, index, level);

    public RESULT getPaused(out bool paused) => EventInstance.FMOD_Studio_EventInstance_GetPaused(this.handle, out paused);

    public RESULT setPaused(bool paused) => EventInstance.FMOD_Studio_EventInstance_SetPaused(this.handle, paused);

    public RESULT start() => EventInstance.FMOD_Studio_EventInstance_Start(this.handle);

    public RESULT stop(STOP_MODE mode) => EventInstance.FMOD_Studio_EventInstance_Stop(this.handle, mode);

    public RESULT getTimelinePosition(out int position) => EventInstance.FMOD_Studio_EventInstance_GetTimelinePosition(this.handle, out position);

    public RESULT setTimelinePosition(int position) => EventInstance.FMOD_Studio_EventInstance_SetTimelinePosition(this.handle, position);

    public RESULT getPlaybackState(out PLAYBACK_STATE state) => EventInstance.FMOD_Studio_EventInstance_GetPlaybackState(this.handle, out state);

    public RESULT getChannelGroup(out ChannelGroup group) => EventInstance.FMOD_Studio_EventInstance_GetChannelGroup(this.handle, out group.handle);

    public RESULT release() => EventInstance.FMOD_Studio_EventInstance_Release(this.handle);

    public RESULT isVirtual(out bool virtualState) => EventInstance.FMOD_Studio_EventInstance_IsVirtual(this.handle, out virtualState);

    public RESULT getParameter(string name, out ParameterInstance instance)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return EventInstance.FMOD_Studio_EventInstance_GetParameter(this.handle, freeHelper.byteFromStringUTF8(name), out instance.handle);
    }

    public RESULT getParameterCount(out int count) => EventInstance.FMOD_Studio_EventInstance_GetParameterCount(this.handle, out count);

    public RESULT getParameterByIndex(int index, out ParameterInstance instance) => EventInstance.FMOD_Studio_EventInstance_GetParameterByIndex(this.handle, index, out instance.handle);

    public RESULT getParameterValue(string name, out float value, out float finalvalue)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return EventInstance.FMOD_Studio_EventInstance_GetParameterValue(this.handle, freeHelper.byteFromStringUTF8(name), out value, out finalvalue);
    }

    public RESULT setParameterValue(string name, float value)
    {
      using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
        return EventInstance.FMOD_Studio_EventInstance_SetParameterValue(this.handle, freeHelper.byteFromStringUTF8(name), value);
    }

    public RESULT getParameterValueByIndex(int index, out float value, out float finalvalue) => EventInstance.FMOD_Studio_EventInstance_GetParameterValueByIndex(this.handle, index, out value, out finalvalue);

    public RESULT setParameterValueByIndex(int index, float value) => EventInstance.FMOD_Studio_EventInstance_SetParameterValueByIndex(this.handle, index, value);

    public RESULT setParameterValuesByIndices(int[] indices, float[] values, int count) => EventInstance.FMOD_Studio_EventInstance_SetParameterValuesByIndices(this.handle, indices, values, count);

    public RESULT triggerCue() => EventInstance.FMOD_Studio_EventInstance_TriggerCue(this.handle);

    public RESULT setCallback(EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask = EVENT_CALLBACK_TYPE.ALL) => EventInstance.FMOD_Studio_EventInstance_SetCallback(this.handle, callback, callbackmask);

    public RESULT getUserData(out IntPtr userdata) => EventInstance.FMOD_Studio_EventInstance_GetUserData(this.handle, out userdata);

    public RESULT setUserData(IntPtr userdata) => EventInstance.FMOD_Studio_EventInstance_SetUserData(this.handle, userdata);

    [DllImport("fmodstudio")]
    private static extern bool FMOD_Studio_EventInstance_IsValid(IntPtr _event);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetDescription(
      IntPtr _event,
      out IntPtr description);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetVolume(
      IntPtr _event,
      out float volume,
      out float finalvolume);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetVolume(
      IntPtr _event,
      float volume);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetPitch(
      IntPtr _event,
      out float pitch,
      out float finalpitch);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetPitch(
      IntPtr _event,
      float pitch);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_Get3DAttributes(
      IntPtr _event,
      out ATTRIBUTES_3D attributes);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_Set3DAttributes(
      IntPtr _event,
      ref ATTRIBUTES_3D attributes);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetListenerMask(
      IntPtr _event,
      out uint mask);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetListenerMask(
      IntPtr _event,
      uint mask);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetProperty(
      IntPtr _event,
      EVENT_PROPERTY index,
      out float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetProperty(
      IntPtr _event,
      EVENT_PROPERTY index,
      float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetReverbLevel(
      IntPtr _event,
      int index,
      out float level);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetReverbLevel(
      IntPtr _event,
      int index,
      float level);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetPaused(
      IntPtr _event,
      out bool paused);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetPaused(
      IntPtr _event,
      bool paused);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_Start(IntPtr _event);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_Stop(IntPtr _event, STOP_MODE mode);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetTimelinePosition(
      IntPtr _event,
      out int position);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetTimelinePosition(
      IntPtr _event,
      int position);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetPlaybackState(
      IntPtr _event,
      out PLAYBACK_STATE state);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetChannelGroup(
      IntPtr _event,
      out IntPtr group);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_Release(IntPtr _event);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_IsVirtual(
      IntPtr _event,
      out bool virtualState);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetParameter(
      IntPtr _event,
      byte[] name,
      out IntPtr parameter);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetParameterByIndex(
      IntPtr _event,
      int index,
      out IntPtr parameter);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetParameterCount(
      IntPtr _event,
      out int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetParameterValue(
      IntPtr _event,
      byte[] name,
      out float value,
      out float finalvalue);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetParameterValue(
      IntPtr _event,
      byte[] name,
      float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetParameterValueByIndex(
      IntPtr _event,
      int index,
      out float value,
      out float finalvalue);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetParameterValueByIndex(
      IntPtr _event,
      int index,
      float value);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetParameterValuesByIndices(
      IntPtr _event,
      int[] indices,
      float[] values,
      int count);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_TriggerCue(IntPtr _event);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetCallback(
      IntPtr _event,
      EVENT_CALLBACK callback,
      EVENT_CALLBACK_TYPE callbackmask);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_GetUserData(
      IntPtr _event,
      out IntPtr userdata);

    [DllImport("fmodstudio")]
    private static extern RESULT FMOD_Studio_EventInstance_SetUserData(
      IntPtr _event,
      IntPtr userdata);

    public bool hasHandle() => this.handle != IntPtr.Zero;

    public void clearHandle() => this.handle = IntPtr.Zero;

    public bool isValid() => this.hasHandle() && EventInstance.FMOD_Studio_EventInstance_IsValid(this.handle);
  }
}
