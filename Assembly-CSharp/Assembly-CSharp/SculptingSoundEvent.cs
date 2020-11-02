// Decompiled with JetBrains decompiler
// Type: SculptingSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

public class SculptingSoundEvent : SoundEvent
{
  private const int COUNTER_MODULUS_INVALID = -2147483648;
  private const int COUNTER_MODULUS_CLEAR = -1;
  private int counterModulus = int.MinValue;

  private static string BaseSoundName(string sound_name)
  {
    int length = sound_name.IndexOf(":");
    return length > 0 ? sound_name.Substring(0, length) : sound_name;
  }

  public SculptingSoundEvent(
    string file_name,
    string sound_name,
    int frame,
    bool do_load,
    bool is_looping,
    float min_interval,
    bool is_dynamic)
    : base(file_name, SculptingSoundEvent.BaseSoundName(sound_name), frame, do_load, is_looping, min_interval, is_dynamic)
  {
    if (sound_name.Contains(":"))
    {
      string[] strArray = sound_name.Split(':');
      if (strArray.Length != 2)
        DebugUtil.LogErrorArgs((object) "Invalid CountedSoundEvent parameter for", (object) (file_name + "." + sound_name + "." + frame.ToString() + ":"), (object) ("'" + sound_name + "'"));
      for (int index = 1; index < strArray.Length; ++index)
        this.ParseParameter(strArray[index]);
    }
    else
      DebugUtil.LogErrorArgs((object) "CountedSoundEvent for", (object) (file_name + "." + sound_name + "." + frame.ToString()), (object) (" - Must specify max number of steps on event: '" + sound_name + "'"));
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    if (string.IsNullOrEmpty(this.sound))
      return;
    GameObject gameObject = behaviour.controller.gameObject;
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
      return;
    int num1 = -1;
    if (this.counterModulus >= -1)
    {
      HandleVector<int>.Handle h = GameComps.WhiteBoards.GetHandle(gameObject);
      if (!h.IsValid())
        h = GameComps.WhiteBoards.Add(gameObject);
      num1 = GameComps.WhiteBoards.HasValue(h, this.soundHash) ? (int) GameComps.WhiteBoards.GetValue(h, this.soundHash) : 0;
      int num2 = this.counterModulus == -1 ? 0 : (num1 + 1) % this.counterModulus;
      GameComps.WhiteBoards.SetValue(h, this.soundHash, (object) num2);
    }
    Vector3 vector3 = behaviour.GetComponent<Transform>().GetPosition();
    float volume = 1f;
    if (this.objectIsSelectedAndVisible)
    {
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
      volume = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
    }
    else
      vector3.z = 0.0f;
    string sound = GlobalAssets.GetSound("Hammer_sculpture");
    Worker component1 = behaviour.GetComponent<Worker>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Workable workable = component1.workable;
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
      {
        Building component2 = workable.GetComponent<Building>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          string name = component2.Def.name;
          if (!(name == "MetalSculpture"))
          {
            if (name == "MarbleSculpture")
              sound = GlobalAssets.GetSound("Hammer_sculpture_marble");
          }
          else
            sound = GlobalAssets.GetSound("Hammer_sculpture_metal");
        }
      }
    }
    EventInstance instance = SoundEvent.BeginOneShot(sound, vector3, volume);
    if (!instance.isValid())
      return;
    if (num1 >= 0)
    {
      int num3 = (int) instance.setParameterValue("eventCount", (float) num1);
    }
    SoundEvent.EndOneShot(instance);
  }

  private void ParseParameter(string param)
  {
    this.counterModulus = int.Parse(param);
    if (this.counterModulus != -1 && this.counterModulus < 2)
      throw new ArgumentException("CountedSoundEvent modulus must be 2 or larger");
  }
}
