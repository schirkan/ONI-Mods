// Decompiled with JetBrains decompiler
// Type: KFMOD
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class KFMOD
{
  private static Dictionary<HashedString, SoundDescription> soundDescriptions = new Dictionary<HashedString, SoundDescription>();
  public static bool didFmodInitializeSuccessfully = true;
  private static Dictionary<HashedString, OneShotSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, OneShotSoundParameterUpdater>();
  public static KFMOD.AudioDevice currentDevice;

  public static SoundDescription GetSoundEventDescription(HashedString path) => !KFMOD.soundDescriptions.ContainsKey(path) ? new SoundDescription() : KFMOD.soundDescriptions[path];

  public static void Initialize()
  {
    try
    {
      FMOD.Studio.System studioSystem = RuntimeManager.StudioSystem;
      KFMOD.didFmodInitializeSuccessfully = RuntimeManager.IsInitialized;
    }
    catch (Exception ex)
    {
      KFMOD.didFmodInitializeSuccessfully = false;
      if (!(ex.GetType() == typeof (SystemNotInitializedException)))
        throw ex;
      Debug.LogWarning((object) ex);
    }
    KFMOD.CollectParameterUpdaters();
    KFMOD.CollectSoundDescriptions();
  }

  public static void PlayOneShot(string sound, Vector3 position, float volume = 1f) => KFMOD.EndOneShot(KFMOD.BeginOneShot(sound, position, volume));

  public static void PlayUISound(string sound) => KFMOD.PlayOneShot(sound, Vector3.zero);

  public static EventInstance BeginOneShot(
    string sound,
    Vector3 position,
    float volume = 1f)
  {
    if (string.IsNullOrEmpty(sound) || App.IsExiting || !RuntimeManager.IsInitialized)
      return new EventInstance();
    EventInstance instance = KFMOD.CreateInstance(sound);
    if (!instance.isValid())
    {
      int num = (UnityEngine.Object) KFMODDebugger.instance != (UnityEngine.Object) null ? 1 : 0;
      return instance;
    }
    Vector3 pos = new Vector3(position.x, position.y, position.z);
    int num1 = (UnityEngine.Object) KFMODDebugger.instance != (UnityEngine.Object) null ? 1 : 0;
    ATTRIBUTES_3D attributes = pos.To3DAttributes();
    int num2 = (int) instance.set3DAttributes(attributes);
    int num3 = (int) instance.setVolume(volume);
    return instance;
  }

  public static bool EndOneShot(EventInstance instance)
  {
    if (!instance.isValid())
      return false;
    int num1 = (int) instance.start();
    int num2 = (int) instance.release();
    return true;
  }

  public static EventInstance CreateInstance(string path)
  {
    if (!RuntimeManager.IsInitialized)
      return new EventInstance();
    EventInstance instance;
    try
    {
      instance = RuntimeManager.CreateInstance(path);
    }
    catch (EventNotFoundException ex)
    {
      Debug.LogWarning((object) ex);
      return new EventInstance();
    }
    HashedString path1 = (HashedString) path;
    SoundDescription eventDescription = KFMOD.GetSoundEventDescription(path1);
    OneShotSoundParameterUpdater.Sound sound = new OneShotSoundParameterUpdater.Sound()
    {
      ev = instance,
      path = path1,
      description = eventDescription
    };
    foreach (OneShotSoundParameterUpdater parameterUpdater in eventDescription.oneShotParameterUpdaters)
      parameterUpdater.Play(sound);
    return instance;
  }

  private static void CollectSoundDescriptions()
  {
    Bank[] array1 = (Bank[]) null;
    int bankList = (int) RuntimeManager.StudioSystem.getBankList(out array1);
    foreach (Bank bank in array1)
    {
      EventDescription[] array2;
      int eventList = (int) bank.getEventList(out array2);
      for (int index1 = 0; index1 < array2.Length; ++index1)
      {
        EventDescription eventDescription = array2[index1];
        string path1;
        int path2 = (int) eventDescription.getPath(out path1);
        HashedString key = (HashedString) path1;
        SoundDescription soundDescription = new SoundDescription();
        soundDescription.path = path1;
        float distance = 0.0f;
        int maximumDistance = (int) eventDescription.getMaximumDistance(out distance);
        if ((double) distance == 0.0)
          distance = 60f;
        soundDescription.falloffDistanceSq = distance * distance;
        List<OneShotSoundParameterUpdater> parameterUpdaterList = new List<OneShotSoundParameterUpdater>();
        int count = 0;
        int parameterCount = (int) eventDescription.getParameterCount(out count);
        SoundDescription.Parameter[] parameterArray = new SoundDescription.Parameter[count];
        for (int index2 = 0; index2 < count; ++index2)
        {
          PARAMETER_DESCRIPTION parameter;
          int parameterByIndex = (int) eventDescription.getParameterByIndex(index2, out parameter);
          string name = (string) parameter.name;
          parameterArray[index2] = new SoundDescription.Parameter()
          {
            name = new HashedString(name),
            idx = index2
          };
          OneShotSoundParameterUpdater parameterUpdater = (OneShotSoundParameterUpdater) null;
          if (KFMOD.parameterUpdaters.TryGetValue((HashedString) name, out parameterUpdater))
            parameterUpdaterList.Add(parameterUpdater);
        }
        soundDescription.parameters = parameterArray;
        soundDescription.oneShotParameterUpdaters = parameterUpdaterList.ToArray();
        KFMOD.soundDescriptions[key] = soundDescription;
      }
    }
  }

  private static void CollectParameterUpdaters()
  {
    foreach (System.Type currentDomainType in App.GetCurrentDomainTypes())
    {
      if (!currentDomainType.IsAbstract)
      {
        bool flag = false;
        for (System.Type baseType = currentDomainType.BaseType; baseType != (System.Type) null; baseType = baseType.BaseType)
        {
          if (baseType == typeof (OneShotSoundParameterUpdater))
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          OneShotSoundParameterUpdater instance = (OneShotSoundParameterUpdater) Activator.CreateInstance(currentDomainType);
          DebugUtil.Assert(!KFMOD.parameterUpdaters.ContainsKey(instance.parameter));
          KFMOD.parameterUpdaters[instance.parameter] = instance;
        }
      }
    }
  }

  public static void RenderEveryTick(float dt)
  {
    foreach (KeyValuePair<HashedString, OneShotSoundParameterUpdater> parameterUpdater in KFMOD.parameterUpdaters)
      parameterUpdater.Value.Update(dt);
  }

  private struct SoundCountEntry
  {
    public int count;
    public float minObjects;
    public float maxObjects;
  }

  public struct AudioDevice
  {
    public int fmod_id;
    public string name;
    public Guid guid;
    public int systemRate;
    public SPEAKERMODE speakerMode;
    public int speakerModeChannels;
    public bool selected;
  }
}
