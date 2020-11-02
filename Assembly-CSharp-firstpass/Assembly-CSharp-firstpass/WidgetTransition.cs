// Decompiled with JetBrains decompiler
// Type: WidgetTransition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class WidgetTransition : MonoBehaviour
{
  private const float OFFSETX = 50f;
  private const float SLIDE_SPEED = 7f;
  private const float FADEIN_SPEED = 6f;
  private bool fadingIn;
  private CanvasGroup canvasGroup;

  private CanvasGroup CanvasGroup => !((Object) this.canvasGroup == (Object) null) ? this.canvasGroup : (this.canvasGroup = this.gameObject.FindOrAddUnityComponent<CanvasGroup>());

  public void SetTransitionType(WidgetTransition.TransitionType transitionType)
  {
  }

  public void StartTransition()
  {
    if (this.fadingIn)
      return;
    this.CanvasGroup.alpha = 0.0f;
    this.fadingIn = true;
    this.enabled = true;
  }

  public void StopTransition()
  {
    if (!this.fadingIn)
      return;
    this.fadingIn = false;
    this.enabled = false;
  }

  private void Update()
  {
    if (!this.fadingIn)
      return;
    float num = this.CanvasGroup.alpha + 6f * Time.unscaledDeltaTime;
    if ((double) num >= 1.0)
      num = 1f;
    if ((double) num == 1.0)
    {
      this.fadingIn = false;
      this.enabled = false;
    }
    this.CanvasGroup.alpha = num;
  }

  private void OnDisable() => this.StopTransition();

  public enum TransitionType
  {
    SlideFromRight,
    SlideFromLeft,
    FadeOnly,
    SlideFromTop,
  }
}
