// Decompiled with JetBrains decompiler
// Type: FMODUnity.StudioEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD.Studio;
using System;
using System.Threading;
using UnityEngine;

namespace FMODUnity
{
  [AddComponentMenu("FMOD Studio/FMOD Studio Event Emitter")]
  public class StudioEventEmitter : MonoBehaviour
  {
    [EventRef]
    public string Event = "";
    public EmitterGameEvent PlayEvent;
    public EmitterGameEvent StopEvent;
    public string CollisionTag = "";
    public bool AllowFadeout = true;
    public bool TriggerOnce;
    public bool Preload;
    public ParamRef[] Params = new ParamRef[0];
    public bool OverrideAttenuation;
    public float OverrideMinDistance = -1f;
    public float OverrideMaxDistance = -1f;
    private EventDescription eventDescription;
    private EventInstance instance;
    private bool hasTriggered;
    private bool isQuitting;

    public EventDescription EventDescription => this.eventDescription;

    public EventInstance EventInstance => this.instance;

    private void Start()
    {
      RuntimeUtils.EnforceLibraryOrder();
      if (this.Preload)
      {
        this.Lookup();
        int num1 = (int) this.eventDescription.loadSampleData();
        int num2 = (int) RuntimeManager.StudioSystem.update();
        LOADING_STATE state;
        for (int sampleLoadingState1 = (int) this.eventDescription.getSampleLoadingState(out state); state == LOADING_STATE.LOADING; int sampleLoadingState2 = (int) this.eventDescription.getSampleLoadingState(out state))
          Thread.Sleep(1);
      }
      this.HandleGameEvent(EmitterGameEvent.ObjectStart);
    }

    private void OnApplicationQuit() => this.isQuitting = true;

    private void OnDestroy()
    {
      if (this.isQuitting)
        return;
      this.HandleGameEvent(EmitterGameEvent.ObjectDestroy);
      if (this.instance.isValid())
        RuntimeManager.DetachInstanceFromGameObject(this.instance);
      if (!this.Preload)
        return;
      int num = (int) this.eventDescription.unloadSampleData();
    }

    private void OnEnable() => this.HandleGameEvent(EmitterGameEvent.ObjectEnable);

    private void OnDisable() => this.HandleGameEvent(EmitterGameEvent.ObjectDisable);

    private void OnTriggerEnter(Collider other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(EmitterGameEvent.TriggerEnter);
    }

    private void OnTriggerExit(Collider other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(EmitterGameEvent.TriggerExit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(EmitterGameEvent.TriggerEnter2D);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(EmitterGameEvent.TriggerExit2D);
    }

    private void OnCollisionEnter() => this.HandleGameEvent(EmitterGameEvent.CollisionEnter);

    private void OnCollisionExit() => this.HandleGameEvent(EmitterGameEvent.CollisionExit);

    private void OnCollisionEnter2D() => this.HandleGameEvent(EmitterGameEvent.CollisionEnter2D);

    private void OnCollisionExit2D() => this.HandleGameEvent(EmitterGameEvent.CollisionExit2D);

    private void HandleGameEvent(EmitterGameEvent gameEvent)
    {
      if (this.PlayEvent == gameEvent)
        this.Play();
      if (this.StopEvent != gameEvent)
        return;
      this.Stop();
    }

    private void Lookup() => this.eventDescription = RuntimeManager.GetEventDescription(this.Event);

    public void Play()
    {
      if (this.TriggerOnce && this.hasTriggered || string.IsNullOrEmpty(this.Event))
        return;
      if (!this.eventDescription.isValid())
        this.Lookup();
      bool oneshot = false;
      if (!this.Event.StartsWith("snapshot", StringComparison.CurrentCultureIgnoreCase))
      {
        int num1 = (int) this.eventDescription.isOneshot(out oneshot);
      }
      bool is3D;
      int num2 = (int) this.eventDescription.is3D(out is3D);
      if (!this.instance.isValid())
        this.instance.clearHandle();
      if (oneshot && this.instance.isValid())
      {
        int num3 = (int) this.instance.release();
        this.instance.clearHandle();
      }
      if (!this.instance.isValid())
      {
        int instance = (int) this.eventDescription.createInstance(out this.instance);
        if (is3D)
        {
          Rigidbody component1 = this.GetComponent<Rigidbody>();
          Rigidbody2D component2 = this.GetComponent<Rigidbody2D>();
          Transform component3 = this.GetComponent<Transform>();
          if ((bool) (UnityEngine.Object) component1)
          {
            int num3 = (int) this.instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.gameObject, component1));
            RuntimeManager.AttachInstanceToGameObject(this.instance, component3, component1);
          }
          else
          {
            int num3 = (int) this.instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.gameObject, component2));
            RuntimeManager.AttachInstanceToGameObject(this.instance, component3, component2);
          }
        }
      }
      foreach (ParamRef paramRef in this.Params)
      {
        int num3 = (int) this.instance.setParameterValue(paramRef.Name, paramRef.Value);
      }
      if (is3D && this.OverrideAttenuation)
      {
        int num3 = (int) this.instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, this.OverrideMinDistance);
        int num4 = (int) this.instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, this.OverrideMaxDistance);
      }
      int num5 = (int) this.instance.start();
      this.hasTriggered = true;
    }

    public void Stop()
    {
      if (!this.instance.isValid())
        return;
      int num1 = (int) this.instance.stop(this.AllowFadeout ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
      int num2 = (int) this.instance.release();
      this.instance.clearHandle();
    }

    public void SetParameter(string name, float value)
    {
      if (!this.instance.isValid())
        return;
      int num = (int) this.instance.setParameterValue(name, value);
    }

    public bool IsPlaying()
    {
      if (!this.instance.isValid() || !this.instance.isValid())
        return false;
      PLAYBACK_STATE state;
      int playbackState = (int) this.instance.getPlaybackState(out state);
      return state != PLAYBACK_STATE.STOPPED;
    }
  }
}
