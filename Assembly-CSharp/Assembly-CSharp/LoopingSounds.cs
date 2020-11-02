﻿// Decompiled with JetBrains decompiler
// Type: LoopingSounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSounds")]
public class LoopingSounds : KMonoBehaviour
{
  private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();
  private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();
  [SerializeField]
  public bool updatePosition;
  public float vol = 1f;
  public bool objectIsSelectedAndVisible;
  public Vector3 sound_pos;

  public bool IsSoundPlaying(string path)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == path)
        return true;
    }
    return false;
  }

  public bool StartSound(
    string asset,
    AnimEventManager.EventPlayerData behaviour,
    EffectorValues noiseValues,
    bool ignore_pause = false,
    bool enable_camera_scaled_position = true)
  {
    switch (asset)
    {
      case "":
      case null:
        Debug.LogWarning((object) "Missing sound");
        return false;
      default:
        if (!this.IsSoundPlaying(asset))
        {
          LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
          {
            asset = asset
          };
          this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(this.gameObject);
          if (this.objectIsSelectedAndVisible)
          {
            this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.transform.GetPosition());
            this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
          }
          else
          {
            this.sound_pos = behaviour.GetComponent<Transform>().GetPosition();
            this.sound_pos.z = 0.0f;
          }
          loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, this.transform, !ignore_pause, enable_camera_scaled_position: enable_camera_scaled_position, vol: this.vol, objectIsSelectedAndVisible: this.objectIsSelectedAndVisible);
          this.loopingSounds.Add(loopingSoundEvent);
        }
        return true;
    }
  }

  public bool StartSound(string asset)
  {
    switch (asset)
    {
      case "":
      case null:
        Debug.LogWarning((object) "Missing sound");
        return false;
      default:
        if (!this.IsSoundPlaying(asset))
        {
          LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
          {
            asset = asset
          };
          this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(this.gameObject);
          if (this.objectIsSelectedAndVisible)
          {
            this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.transform.GetPosition());
            this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
          }
          else
          {
            this.sound_pos = this.transform.GetPosition();
            this.sound_pos.z = 0.0f;
          }
          loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, this.transform, vol: this.vol, objectIsSelectedAndVisible: this.objectIsSelectedAndVisible);
          this.loopingSounds.Add(loopingSoundEvent);
        }
        return true;
    }
  }

  public bool StartSound(
    string asset,
    bool pause_on_game_pause = true,
    bool enable_culling = true,
    bool enable_camera_scaled_position = true)
  {
    switch (asset)
    {
      case "":
      case null:
        Debug.LogWarning((object) "Missing sound");
        return false;
      default:
        if (!this.IsSoundPlaying(asset))
        {
          LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
          {
            asset = asset
          };
          this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(this.gameObject);
          if (this.objectIsSelectedAndVisible)
          {
            this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.transform.GetPosition());
            this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
          }
          else
          {
            this.sound_pos = this.transform.GetPosition();
            this.sound_pos.z = 0.0f;
          }
          loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, this.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
          this.loopingSounds.Add(loopingSoundEvent);
        }
        return true;
    }
  }

  public void UpdateVelocity(string asset, Vector2 value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateVelocity(loopingSound.handle, value);
        break;
      }
    }
  }

  public void UpdateFirstParameter(string asset, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateFirstParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  public void UpdateSecondParameter(string asset, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateSecondParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  private void StopSoundAtIndex(int i) => LoopingSoundManager.StopSound(this.loopingSounds[i].handle);

  public void StopSound(string asset)
  {
    for (int index = 0; index < this.loopingSounds.Count; ++index)
    {
      if (this.loopingSounds[index].asset == asset)
      {
        this.StopSoundAtIndex(index);
        this.loopingSounds.RemoveAt(index);
        break;
      }
    }
  }

  public void StopAllSounds()
  {
    for (int i = 0; i < this.loopingSounds.Count; ++i)
      this.StopSoundAtIndex(i);
    this.loopingSounds.Clear();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.StopAllSounds();
  }

  public void SetParameter(string path, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == path)
      {
        LoopingSoundManager.Get().UpdateFirstParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  public void PlayEvent(GameSoundEvents.Event ev)
  {
    if (AudioDebug.Get().debugGameEventSounds)
      Debug.Log((object) ("GameSoundEvent: " + (object) ev.Name));
    List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
    if (events == null)
      return;
    Vector2 position = (Vector2) this.transform.GetPosition();
    for (int index = 0; index < events.Count && (events[index] is SoundEvent soundEvent && soundEvent.sound != null); ++index)
    {
      if (CameraController.Instance.IsAudibleSound((Vector3) position, (HashedString) soundEvent.sound))
      {
        if (AudioDebug.Get().debugGameEventSounds)
          Debug.Log((object) ("GameSound: " + soundEvent.sound));
        float num = 0.0f;
        if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
        {
          if ((double) Time.time - (double) num > (double) soundEvent.minInterval)
            SoundEvent.PlayOneShot(soundEvent.sound, (Vector3) position);
        }
        else
          SoundEvent.PlayOneShot(soundEvent.sound, (Vector3) position);
        this.lastTimePlayed[soundEvent.soundHash] = Time.time;
      }
    }
  }

  public void UpdateObjectSelection(bool selected)
  {
    GameObject gameObject = this.gameObject;
    if (selected && (Object) gameObject != (Object) null && CameraController.Instance.IsVisiblePos(gameObject.transform.position))
    {
      this.objectIsSelectedAndVisible = true;
      this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.sound_pos);
      this.vol = 1f;
    }
    else
    {
      this.objectIsSelectedAndVisible = false;
      this.sound_pos = this.transform.GetPosition();
      this.sound_pos.z = 0.0f;
      this.vol = 1f;
    }
    for (int index = 0; index < this.loopingSounds.Count; ++index)
      LoopingSoundManager.Get().UpdateObjectSelection(this.loopingSounds[index].handle, this.sound_pos, this.vol, this.objectIsSelectedAndVisible);
  }

  private struct LoopingSoundEvent
  {
    public string asset;
    public HandleVector<int>.Handle handle;
  }
}
