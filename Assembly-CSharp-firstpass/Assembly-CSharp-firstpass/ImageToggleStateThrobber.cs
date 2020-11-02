// Decompiled with JetBrains decompiler
// Type: ImageToggleStateThrobber
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Plugins/ImageToggleStateThrobber")]
public class ImageToggleStateThrobber : KMonoBehaviour
{
  public ImageToggleState[] targetImageToggleStates;
  public ImageToggleState.State state1;
  public ImageToggleState.State state2;
  public float period = 2f;
  public bool useScaledTime;
  private float t;

  public void OnEnable() => this.t = 0.0f;

  public void OnDisable()
  {
    foreach (ImageToggleState imageToggleState in this.targetImageToggleStates)
      imageToggleState.ResetColor();
  }

  public void Update()
  {
    this.t = (this.t + (this.useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime)) % this.period;
    float t = (float) ((double) Mathf.Cos((float) ((double) this.t / (double) this.period * 2.0 * 3.14159274101257)) * 0.5 + 0.5);
    foreach (ImageToggleState imageToggleState in this.targetImageToggleStates)
    {
      Color color = Color.Lerp(this.ColorForState(imageToggleState, this.state1), this.ColorForState(imageToggleState, this.state2), t);
      imageToggleState.TargetImage.color = color;
    }
  }

  private Color ColorForState(ImageToggleState its, ImageToggleState.State state)
  {
    switch (state)
    {
      case ImageToggleState.State.Disabled:
        return its.DisabledColour;
      case ImageToggleState.State.Inactive:
        return its.InactiveColour;
      case ImageToggleState.State.DisabledActive:
        return its.DisabledActiveColour;
      default:
        return its.ActiveColour;
    }
  }
}
