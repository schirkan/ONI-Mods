// Decompiled with JetBrains decompiler
// Type: SoundListenerController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SoundListenerController : MonoBehaviour
{
  private VCA loopingVCA;

  public static SoundListenerController Instance { get; private set; }

  private void Awake() => SoundListenerController.Instance = this;

  private void OnDestroy() => SoundListenerController.Instance = (SoundListenerController) null;

  private void Start()
  {
    if (RuntimeManager.IsInitialized)
    {
      int vca = (int) RuntimeManager.StudioSystem.getVCA("vca:/Looping", out this.loopingVCA);
    }
    else
      this.enabled = false;
  }

  public void SetLoopingVolume(float volume)
  {
    int num = (int) this.loopingVCA.setVolume(volume);
  }

  private void Update()
  {
    Audio audio = Audio.Get();
    Vector3 position = Camera.main.transform.GetPosition();
    float num1 = Mathf.Max((float) (((double) Camera.main.orthographicSize - (double) audio.listenerMinOrthographicSize) / ((double) audio.listenerReferenceOrthographicSize - (double) audio.listenerMinOrthographicSize)), 0.0f);
    float num2 = (float) (-(double) audio.listenerMinZ - (double) num1 * ((double) audio.listenerReferenceZ - (double) audio.listenerMinZ));
    position.z = num2;
    this.transform.SetPosition(position);
  }
}
