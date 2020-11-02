// Decompiled with JetBrains decompiler
// Type: KBasicToggle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("KMonoBehaviour/Plugins/KBasicToggle")]
public class KBasicToggle : KMonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
  private const float DoubleClickTime = 0.15f;
  private bool _isOn;
  private bool didDoubleClick;
  private IEnumerator doubleClickCoroutine;

  public event System.Action onClick;

  public event System.Action onDoubleClick;

  public event System.Action onPointerEnter;

  public event System.Action onPointerExit;

  public event System.Action<bool> onValueChanged;

  public bool isOn
  {
    get => this._isOn;
    set
    {
      this._isOn = value;
      if (this.onValueChanged == null)
        return;
      this.onValueChanged(value);
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (this.doubleClickCoroutine != null && this.onDoubleClick != null)
    {
      this.onDoubleClick();
      this.didDoubleClick = true;
    }
    else
    {
      this.doubleClickCoroutine = this.DoubleClickTimer(eventData);
      this.StartCoroutine(this.doubleClickCoroutine);
    }
  }

  private IEnumerator DoubleClickTimer(PointerEventData eventData)
  {
    float startTime = Time.unscaledTime;
    while ((double) Time.unscaledTime - (double) startTime < 0.150000005960464 && !this.didDoubleClick)
      yield return (object) null;
    if (!this.didDoubleClick && this.onClick != null)
    {
      this.isOn = !this.isOn;
      this.onClick();
      this.onValueChanged(this.isOn);
    }
    this.doubleClickCoroutine = (IEnumerator) null;
    this.didDoubleClick = false;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (this.onPointerEnter == null)
      return;
    this.onPointerEnter();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (this.onPointerExit == null)
      return;
    this.onPointerExit();
  }
}
