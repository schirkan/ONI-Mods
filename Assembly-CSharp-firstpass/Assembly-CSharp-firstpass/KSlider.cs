// Decompiled with JetBrains decompiler
// Type: KSlider
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KSlider : Slider
{
  public AnimationCurve sliderWeightCurve;
  public static string[] DefaultSounds = new string[5];
  private string[] currentSounds;
  private bool playSounds = true;
  private float lastMoveTime;
  private float movePlayRate = 0.025f;
  private float lastMoveValue;
  public bool playedBoundaryBump;
  private ToolTip tooltip;

  public event System.Action onReleaseHandle;

  public event System.Action onDrag;

  public event System.Action onPointerDown;

  public event System.Action onMove;

  private new void Awake()
  {
    this.currentSounds = new string[KSlider.DefaultSounds.Length];
    for (int index = 0; index < KSlider.DefaultSounds.Length; ++index)
      this.currentSounds[index] = KSlider.DefaultSounds[index];
    this.lastMoveTime = Time.unscaledTime;
    this.lastMoveValue = -1f;
    this.tooltip = this.handleRect.gameObject.GetComponent<ToolTip>();
  }

  public override void OnPointerUp(PointerEventData eventData)
  {
    base.OnPointerUp(eventData);
    this.PlayEndSound();
    if ((UnityEngine.Object) this.tooltip != (UnityEngine.Object) null)
    {
      this.tooltip.enabled = true;
      this.tooltip.OnPointerEnter(eventData);
    }
    if (this.onReleaseHandle == null)
      return;
    this.onReleaseHandle();
  }

  public override void OnPointerDown(PointerEventData eventData)
  {
    base.OnPointerDown(eventData);
    this.PlayStartSound();
    if ((double) this.value != (double) this.lastMoveValue)
      this.PlayMoveSound(KSlider.MoveSource.MouseClick);
    if ((UnityEngine.Object) this.tooltip != (UnityEngine.Object) null)
      this.tooltip.enabled = false;
    if (this.onPointerDown == null)
      return;
    this.onPointerDown();
  }

  public override void OnDrag(PointerEventData eventData)
  {
    base.OnDrag(eventData);
    this.PlayMoveSound(KSlider.MoveSource.MouseDrag);
    if (this.onDrag == null)
      return;
    this.onDrag();
  }

  public override void OnMove(AxisEventData eventData)
  {
    base.OnMove(eventData);
    this.PlayMoveSound(KSlider.MoveSource.Keyboard);
    if (this.onMove == null)
      return;
    this.onMove();
  }

  public void ClearReleaseHandleEvent() => this.onReleaseHandle = (System.Action) null;

  public void SetTooltipText(string tooltipText)
  {
    if (!((UnityEngine.Object) this.tooltip != (UnityEngine.Object) null))
      return;
    this.tooltip.SetSimpleTooltip(tooltipText);
  }

  public void PlayStartSound()
  {
    if (!KInputManager.isFocused || !this.playSounds)
      return;
    string currentSound = this.currentSounds[0];
    if (currentSound == null || currentSound.Length <= 0)
      return;
    KFMOD.PlayUISound(currentSound);
  }

  public void PlayMoveSound(KSlider.MoveSource moveSource)
  {
    if (!KInputManager.isFocused || !this.playSounds)
      return;
    float num1 = Time.unscaledTime - this.lastMoveTime;
    if ((double) num1 < (double) this.movePlayRate)
      return;
    if (moveSource != KSlider.MoveSource.MouseDrag)
      this.playedBoundaryBump = false;
    float num2 = Mathf.InverseLerp(this.minValue, this.maxValue, this.value);
    string sound = (string) null;
    if ((double) num2 == 1.0 && (double) this.lastMoveValue == 1.0)
    {
      if (!this.playedBoundaryBump)
      {
        sound = this.currentSounds[4];
        this.playedBoundaryBump = true;
      }
    }
    else if ((double) num2 == 0.0 && (double) this.lastMoveValue == 0.0)
    {
      if (!this.playedBoundaryBump)
      {
        sound = this.currentSounds[3];
        this.playedBoundaryBump = true;
      }
    }
    else if ((double) num2 >= 0.0 && (double) num2 <= 1.0)
    {
      sound = this.currentSounds[1];
      this.playedBoundaryBump = false;
    }
    if (sound == null || sound.Length <= 0)
      return;
    this.lastMoveTime = Time.unscaledTime;
    this.lastMoveValue = num2;
    EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero);
    int num3 = (int) instance.setParameterValue("sliderValue", num2);
    int num4 = (int) instance.setParameterValue("timeSinceLast", num1);
    KFMOD.EndOneShot(instance);
  }

  public void PlayEndSound()
  {
    if (!KInputManager.isFocused || !this.playSounds)
      return;
    string currentSound = this.currentSounds[2];
    if (currentSound == null || currentSound.Length <= 0)
      return;
    EventInstance instance = KFMOD.BeginOneShot(currentSound, Vector3.zero);
    int num = (int) instance.setParameterValue("sliderValue", this.value);
    KFMOD.EndOneShot(instance);
  }

  public enum SoundType
  {
    Start,
    Move,
    End,
    BoundaryLow,
    BoundaryHigh,
    Num,
  }

  public enum MoveSource
  {
    Keyboard,
    MouseDrag,
    MouseClick,
    Num,
  }
}
