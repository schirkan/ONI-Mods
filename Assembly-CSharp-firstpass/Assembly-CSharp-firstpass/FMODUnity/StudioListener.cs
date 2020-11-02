// Decompiled with JetBrains decompiler
// Type: FMODUnity.StudioListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace FMODUnity
{
  [AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
  public class StudioListener : MonoBehaviour
  {
    private Rigidbody rigidBody;
    private Rigidbody2D rigidBody2D;
    public int ListenerNumber;

    private void OnEnable()
    {
      RuntimeUtils.EnforceLibraryOrder();
      if (RuntimeManager.IsInitialized)
      {
        this.rigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.rigidBody2D = this.gameObject.GetComponent<Rigidbody2D>();
        RuntimeManager.HasListener[this.ListenerNumber] = true;
        this.SetListenerLocation();
      }
      else
        this.enabled = false;
    }

    private void OnDisable() => RuntimeManager.HasListener[this.ListenerNumber] = false;

    private void Update() => this.SetListenerLocation();

    private void SetListenerLocation()
    {
      if ((bool) (Object) this.rigidBody)
        RuntimeManager.SetListenerLocation(this.ListenerNumber, this.gameObject, this.rigidBody);
      else
        RuntimeManager.SetListenerLocation(this.ListenerNumber, this.gameObject, this.rigidBody2D);
    }
  }
}
