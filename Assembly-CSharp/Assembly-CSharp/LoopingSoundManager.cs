// Decompiled with JetBrains decompiler
// Type: LoopingSoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LoopingSoundManager")]
public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
  private static LoopingSoundManager instance;
  private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();
  private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>();

  public static void DestroyInstance() => LoopingSoundManager.instance = (LoopingSoundManager) null;

  protected override void OnPrefabInit()
  {
    LoopingSoundManager.instance = this;
    this.CollectParameterUpdaters();
  }

  protected override void OnSpawn()
  {
    if (!((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Subscribe(-1788536802, new System.Action<object>(LoopingSoundManager.instance.OnPauseChanged));
  }

  private void CollectParameterUpdaters()
  {
    foreach (System.Type currentDomainType in App.GetCurrentDomainTypes())
    {
      if (!currentDomainType.IsAbstract)
      {
        bool flag = false;
        for (System.Type baseType = currentDomainType.BaseType; baseType != (System.Type) null; baseType = baseType.BaseType)
        {
          if (baseType == typeof (LoopingSoundParameterUpdater))
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          LoopingSoundParameterUpdater instance = (LoopingSoundParameterUpdater) Activator.CreateInstance(currentDomainType);
          DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(instance.parameter));
          this.parameterUpdaters[instance.parameter] = instance;
        }
      }
    }
  }

  public void UpdateFirstParameter(
    HandleVector<int>.Handle handle,
    HashedString parameter,
    float value)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.firstParameterValue = value;
    data.firstParameter = parameter;
    if (data.IsPlaying)
    {
      int num = (int) data.ev.setParameterValueByIndex(this.GetSoundDescription(data.path).GetParameterIdx(parameter), value);
    }
    this.sounds.SetData(handle, data);
  }

  public void UpdateSecondParameter(
    HandleVector<int>.Handle handle,
    HashedString parameter,
    float value)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.secondParameterValue = value;
    data.secondParameter = parameter;
    if (data.IsPlaying)
    {
      int num = (int) data.ev.setParameterValueByIndex(this.GetSoundDescription(data.path).GetParameterIdx(parameter), value);
    }
    this.sounds.SetData(handle, data);
  }

  public void UpdateObjectSelection(
    HandleVector<int>.Handle handle,
    Vector3 sound_pos,
    float vol,
    bool objectIsSelectedAndVisible)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.pos = sound_pos;
    data.vol = vol;
    data.objectIsSelectedAndVisible = objectIsSelectedAndVisible;
    ATTRIBUTES_3D attributes = sound_pos.To3DAttributes();
    if (data.IsPlaying)
    {
      int num1 = (int) data.ev.set3DAttributes(attributes);
      int num2 = (int) data.ev.setVolume(vol);
    }
    this.sounds.SetData(handle, data);
  }

  public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.velocity = velocity;
    this.sounds.SetData(handle, data);
  }

  public void RenderEveryTick(float dt)
  {
    ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList1 = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
    ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
    ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
    List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
    bool flag = (double) Time.timeScale == 0.0;
    SoundCuller soundCuller = CameraController.Instance.soundCuller;
    for (int index = 0; index < dataList.Count; ++index)
    {
      LoopingSoundManager.Sound sound = dataList[index];
      if (sound.objectIsSelectedAndVisible)
      {
        sound.pos = SoundEvent.AudioHighlightListenerPosition(sound.transform.GetPosition());
        sound.vol = 1f;
      }
      else if ((UnityEngine.Object) sound.transform != (UnityEngine.Object) null)
      {
        sound.pos = sound.transform.GetPosition();
        sound.pos.z = 0.0f;
      }
      if ((UnityEngine.Object) sound.animController != (UnityEngine.Object) null)
      {
        Vector3 offset = sound.animController.Offset;
        sound.pos.x += offset.x;
        sound.pos.y += offset.y;
      }
      int num = !sound.IsCullingEnabled || sound.ShouldCameraScalePosition && soundCuller.IsAudible((Vector2) sound.pos, sound.falloffDistanceSq) ? 1 : (soundCuller.IsAudibleNoCameraScaling((Vector2) sound.pos, sound.falloffDistanceSq) ? 1 : 0);
      bool isPlaying = sound.IsPlaying;
      if (num != 0)
      {
        pooledList1.Add(sound);
        if (!isPlaying)
        {
          SoundDescription soundDescription = this.GetSoundDescription(sound.path);
          sound.ev = KFMOD.CreateInstance(soundDescription.path);
          dataList[index] = sound;
          pooledList2.Add(index);
        }
      }
      else if (isPlaying)
        pooledList3.Add(index);
    }
    foreach (int index in (List<int>) pooledList2)
    {
      LoopingSoundManager.Sound sound1 = dataList[index];
      SoundDescription soundDescription = this.GetSoundDescription(sound1.path);
      int num1 = (int) sound1.ev.setPaused(flag && sound1.ShouldPauseOnGamePaused);
      sound1.pos.z = 0.0f;
      Vector3 pos = sound1.pos;
      if (sound1.objectIsSelectedAndVisible)
      {
        sound1.pos = SoundEvent.AudioHighlightListenerPosition(sound1.transform.GetPosition());
        sound1.vol = 1f;
      }
      else if ((UnityEngine.Object) sound1.transform != (UnityEngine.Object) null)
        sound1.pos = sound1.transform.GetPosition();
      int num2 = (int) sound1.ev.set3DAttributes(pos.To3DAttributes());
      int num3 = (int) sound1.ev.setVolume(sound1.vol);
      int num4 = (int) sound1.ev.start();
      sound1.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
      if (sound1.firstParameter != HashedString.Invalid)
      {
        int num5 = (int) sound1.ev.setParameterValueByIndex(soundDescription.GetParameterIdx(sound1.firstParameter), sound1.firstParameterValue);
      }
      if (sound1.secondParameter != HashedString.Invalid)
      {
        int num6 = (int) sound1.ev.setParameterValueByIndex(soundDescription.GetParameterIdx(sound1.secondParameter), sound1.secondParameterValue);
      }
      LoopingSoundParameterUpdater.Sound sound2 = new LoopingSoundParameterUpdater.Sound()
      {
        ev = sound1.ev,
        path = sound1.path,
        description = soundDescription,
        transform = sound1.transform,
        objectIsSelectedAndVisible = false
      };
      foreach (SoundDescription.Parameter parameter in soundDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (this.parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
          parameterUpdater.Add(sound2);
      }
      dataList[index] = sound1;
    }
    pooledList2.Recycle();
    foreach (int index in (List<int>) pooledList3)
    {
      LoopingSoundManager.Sound sound1 = dataList[index];
      SoundDescription soundDescription = this.GetSoundDescription(sound1.path);
      LoopingSoundParameterUpdater.Sound sound2 = new LoopingSoundParameterUpdater.Sound()
      {
        ev = sound1.ev,
        path = sound1.path,
        description = soundDescription,
        transform = sound1.transform,
        objectIsSelectedAndVisible = false
      };
      foreach (SoundDescription.Parameter parameter in soundDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (this.parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
          parameterUpdater.Remove(sound2);
      }
      if (sound1.ShouldCameraScalePosition)
      {
        int num1 = (int) sound1.ev.stop(STOP_MODE.IMMEDIATE);
      }
      else
      {
        int num2 = (int) sound1.ev.stop(STOP_MODE.ALLOWFADEOUT);
      }
      sound1.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
      int num3 = (int) sound1.ev.release();
      dataList[index] = sound1;
    }
    pooledList3.Recycle();
    float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
    foreach (LoopingSoundManager.Sound sound in (List<LoopingSoundManager.Sound>) pooledList1)
    {
      ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition(sound.pos, sound.objectIsSelectedAndVisible).To3DAttributes();
      attributes.velocity = ((Vector3) (sound.velocity * velocityScale)).ToFMODVector();
      int num = (int) sound.ev.set3DAttributes(attributes);
    }
    foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> parameterUpdater in this.parameterUpdaters)
      parameterUpdater.Value.Update(dt);
    pooledList1.Recycle();
  }

  public static LoopingSoundManager Get() => LoopingSoundManager.instance;

  public void StopAllSounds()
  {
    foreach (LoopingSoundManager.Sound data in this.sounds.GetDataList())
    {
      if (data.IsPlaying)
      {
        int num1 = (int) data.ev.stop(STOP_MODE.IMMEDIATE);
        int num2 = (int) data.ev.release();
      }
    }
  }

  private SoundDescription GetSoundDescription(HashedString path) => KFMOD.GetSoundEventDescription(path);

  public HandleVector<int>.Handle Add(
    string path,
    Vector3 pos,
    Transform transform = null,
    bool pause_on_game_pause = true,
    bool enable_culling = true,
    bool enable_camera_scaled_position = true,
    float vol = 1f,
    bool objectIsSelectedAndVisible = false)
  {
    SoundDescription eventDescription = KFMOD.GetSoundEventDescription((HashedString) path);
    LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags) 0;
    if (pause_on_game_pause)
      flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
    if (enable_culling)
      flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
    if (enable_camera_scaled_position)
      flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
    KBatchedAnimController kbatchedAnimController = (KBatchedAnimController) null;
    if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
      kbatchedAnimController = transform.GetComponent<KBatchedAnimController>();
    return this.sounds.Allocate(new LoopingSoundManager.Sound()
    {
      transform = transform,
      animController = kbatchedAnimController,
      falloffDistanceSq = eventDescription.falloffDistanceSq,
      path = (HashedString) path,
      pos = pos,
      flags = flags,
      firstParameter = HashedString.Invalid,
      secondParameter = HashedString.Invalid,
      vol = vol,
      objectIsSelectedAndVisible = objectIsSelectedAndVisible
    });
  }

  public static HandleVector<int>.Handle StartSound(
    string path,
    Vector3 pos,
    bool pause_on_game_pause = true,
    bool enable_culling = true)
  {
    if (!string.IsNullOrEmpty(path))
      return LoopingSoundManager.Get().Add(path, pos, pause_on_game_pause: pause_on_game_pause, enable_culling: enable_culling);
    Debug.LogWarning((object) "Missing sound");
    return HandleVector<int>.InvalidHandle;
  }

  public static void StopSound(HandleVector<int>.Handle handle)
  {
    if ((UnityEngine.Object) LoopingSoundManager.Get() == (UnityEngine.Object) null)
      return;
    LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
    if (data.IsPlaying)
    {
      int num1 = (int) data.ev.stop(STOP_MODE.ALLOWFADEOUT);
      int num2 = (int) data.ev.release();
      SoundDescription eventDescription = KFMOD.GetSoundEventDescription(data.path);
      foreach (SoundDescription.Parameter parameter in eventDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
        {
          LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound()
          {
            ev = data.ev,
            path = data.path,
            description = eventDescription,
            transform = data.transform,
            objectIsSelectedAndVisible = false
          };
          parameterUpdater.Remove(sound);
        }
      }
    }
    LoopingSoundManager.Get().sounds.Free(handle);
  }

  private void OnPauseChanged(object data)
  {
    bool flag = (bool) data;
    foreach (LoopingSoundManager.Sound data1 in this.sounds.GetDataList())
    {
      if (data1.IsPlaying)
      {
        int num = (int) data1.ev.setPaused(flag && data1.ShouldPauseOnGamePaused);
      }
    }
  }

  public class Tuning : TuningData<LoopingSoundManager.Tuning>
  {
    public float velocityScale;
  }

  public struct Sound
  {
    public EventInstance ev;
    public Transform transform;
    public KBatchedAnimController animController;
    public float falloffDistanceSq;
    public HashedString path;
    public Vector3 pos;
    public Vector2 velocity;
    public HashedString firstParameter;
    public HashedString secondParameter;
    public float firstParameterValue;
    public float secondParameterValue;
    public float vol;
    public bool objectIsSelectedAndVisible;
    public LoopingSoundManager.Sound.Flags flags;

    public bool IsPlaying => (uint) (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) > 0U;

    public bool ShouldPauseOnGamePaused => (uint) (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) > 0U;

    public bool IsCullingEnabled => (uint) (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) > 0U;

    public bool ShouldCameraScalePosition => (uint) (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) > 0U;

    [System.Flags]
    public enum Flags
    {
      PLAYING = 1,
      PAUSE_ON_GAME_PAUSED = 2,
      ENABLE_CULLING = 4,
      ENABLE_CAMERA_SCALED_POSITION = 8,
    }
  }
}
