// Decompiled with JetBrains decompiler
// Type: FMODUnity.StudioParameterTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace FMODUnity
{
  [AddComponentMenu("FMOD Studio/FMOD Studio Parameter Trigger")]
  public class StudioParameterTrigger : MonoBehaviour
  {
    public EmitterRef[] Emitters;
    public EmitterGameEvent TriggerEvent;
    public string CollisionTag;

    private void Start() => this.HandleGameEvent(EmitterGameEvent.ObjectStart);

    private void OnDestroy() => this.HandleGameEvent(EmitterGameEvent.ObjectDestroy);

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
      if (this.TriggerEvent != gameEvent)
        return;
      this.TriggerParameters();
    }

    public void TriggerParameters()
    {
      for (int index1 = 0; index1 < this.Emitters.Length; ++index1)
      {
        EmitterRef emitter = this.Emitters[index1];
        if ((Object) emitter.Target != (Object) null)
        {
          for (int index2 = 0; index2 < this.Emitters[index1].Params.Length; ++index2)
            emitter.Target.SetParameter(this.Emitters[index1].Params[index2].Name, this.Emitters[index1].Params[index2].Value);
        }
      }
    }
  }
}
