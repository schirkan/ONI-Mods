// Decompiled with JetBrains decompiler
// Type: FMODUnity.OneshotList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using FMOD.Studio;
using System;
using System.Collections.Generic;

namespace FMODUnity
{
  public class OneshotList
  {
    private List<EventInstance> instances = new List<EventInstance>();

    public void Add(EventInstance instance) => this.instances.Add(instance);

    public void Update(ATTRIBUTES_3D attributes)
    {
      PLAYBACK_STATE state;
      foreach (EventInstance eventInstance in this.instances.FindAll((Predicate<EventInstance>) (x =>
      {
        int playbackState = (int) x.getPlaybackState(out state);
        return state == PLAYBACK_STATE.STOPPED;
      })))
      {
        int num = (int) eventInstance.release();
      }
      this.instances.RemoveAll((Predicate<EventInstance>) (x => !x.isValid()));
      foreach (EventInstance instance in this.instances)
      {
        int num = (int) instance.set3DAttributes(attributes);
      }
    }

    public void SetParameterValue(string name, float value)
    {
      foreach (EventInstance instance in this.instances)
      {
        int num = (int) instance.setParameterValue(name, value);
      }
    }

    public void StopAll(STOP_MODE stopMode)
    {
      foreach (EventInstance instance in this.instances)
      {
        int num1 = (int) instance.stop(stopMode);
        int num2 = (int) instance.release();
      }
      this.instances.Clear();
    }
  }
}
