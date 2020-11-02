// Decompiled with JetBrains decompiler
// Type: RemoteSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

[Serializable]
public class RemoteSoundEvent : SoundEvent
{
  private const string STATE_PARAMETER = "State";

  public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, false, min_interval, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 vector3 = behaviour.GetComponent<Transform>().GetPosition();
    vector3.z = 0.0f;
    if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
    Workable workable = behaviour.GetComponent<Worker>().workable;
    if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
      return;
    Toggleable component = workable.GetComponent<Toggleable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    IToggleHandler handlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<Worker>());
    float num1 = 1f;
    if (handlerForWorker != null && handlerForWorker.IsHandlerOn())
      num1 = 0.0f;
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
      return;
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, vector3, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
    int num2 = (int) instance.setParameterValue("State", num1);
    SoundEvent.EndOneShot(instance);
  }
}
