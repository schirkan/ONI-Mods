﻿// Decompiled with JetBrains decompiler
// Type: MinMaxSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
  public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;
  public bool lockRange;
  public bool interactable = true;
  public float minLimit;
  public float maxLimit = 100f;
  public float range = 50f;
  public float barWidth = 10f;
  public float barHeight = 100f;
  public float currentMinValue = 10f;
  public float currentMaxValue = 90f;
  public float currentExtraValue = 50f;
  public Slider.Direction direction;
  public bool wholeNumbers = true;
  public System.Action<MinMaxSlider> onMinChange;
  public System.Action<MinMaxSlider> onMaxChange;
  public Slider minSlider;
  public Slider maxSlider;
  public Slider extraSlider;
  public RectTransform minRect;
  public RectTransform maxRect;
  public RectTransform bgFill;
  public RectTransform mgFill;
  public RectTransform fgFill;
  public Text title;
  [MyCmpGet]
  public ToolTip toolTip;
  public Image icon;
  public Image isOverPowered;
  private Vector3 mousePos;

  public MinMaxSlider.Mode mode { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ToolTip component = this.transform.parent.gameObject.GetComponent<ToolTip>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.toolTip);
      this.toolTip = component;
    }
    this.minSlider.value = this.currentMinValue;
    this.maxSlider.value = this.currentMaxValue;
    this.minSlider.interactable = this.interactable;
    this.maxSlider.interactable = this.interactable;
    this.minSlider.maxValue = this.maxLimit;
    this.maxSlider.maxValue = this.maxLimit;
    this.minSlider.minValue = this.minLimit;
    this.maxSlider.minValue = this.minLimit;
    this.minSlider.direction = this.maxSlider.direction = this.direction;
    if ((UnityEngine.Object) this.isOverPowered != (UnityEngine.Object) null)
      this.isOverPowered.enabled = false;
    this.minSlider.gameObject.SetActive(false);
    if (this.mode != MinMaxSlider.Mode.Single)
      this.minSlider.gameObject.SetActive(true);
    if (!((UnityEngine.Object) this.extraSlider != (UnityEngine.Object) null))
      return;
    this.extraSlider.value = this.currentExtraValue;
    this.extraSlider.wholeNumbers = this.minSlider.wholeNumbers = this.maxSlider.wholeNumbers = this.wholeNumbers;
    this.extraSlider.direction = this.direction;
    this.extraSlider.interactable = this.interactable;
    this.extraSlider.maxValue = this.maxLimit;
    this.extraSlider.minValue = this.minLimit;
    this.extraSlider.gameObject.SetActive(false);
    if (this.mode != MinMaxSlider.Mode.Triple)
      return;
    this.extraSlider.gameObject.SetActive(true);
  }

  public void SetIcon(Image newIcon)
  {
    this.icon = newIcon;
    this.icon.gameObject.transform.SetParent(this.transform);
    this.icon.gameObject.transform.SetAsFirstSibling();
    this.icon.rectTransform().anchoredPosition = Vector2.zero;
  }

  public void SetMode(MinMaxSlider.Mode mode)
  {
    this.mode = mode;
    if (mode != MinMaxSlider.Mode.Single || !((UnityEngine.Object) this.extraSlider != (UnityEngine.Object) null))
      return;
    this.extraSlider.gameObject.SetActive(false);
    this.extraSlider.handleRect.gameObject.SetActive(false);
  }

  private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
  {
    trans.anchorMin = min;
    trans.anchorMax = max;
  }

  public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
  {
    this.currentMinValue = this.minSlider.value = currentMin;
    this.currentMaxValue = this.maxSlider.value = currentMax;
    this.minLimit = min;
    this.maxLimit = max;
    this.minSlider.minValue = this.minLimit;
    this.maxSlider.minValue = this.minLimit;
    this.minSlider.maxValue = this.maxLimit;
    this.maxSlider.maxValue = this.maxLimit;
    if (!((UnityEngine.Object) this.extraSlider != (UnityEngine.Object) null))
      return;
    this.extraSlider.minValue = this.minLimit;
    this.extraSlider.maxValue = this.maxLimit;
  }

  public void SetExtraValue(float current)
  {
    this.extraSlider.value = current;
    this.toolTip.toolTip = this.transform.parent.name + ": " + current.ToString("F2");
  }

  public void SetMaxValue(float current, float max)
  {
    float b = (float) ((double) current / (double) max * 100.0);
    if ((UnityEngine.Object) this.isOverPowered != (UnityEngine.Object) null)
      this.isOverPowered.enabled = (double) b > 100.0;
    this.maxSlider.value = Mathf.Min(100f, b);
    if (!((UnityEngine.Object) this.toolTip != (UnityEngine.Object) null))
      return;
    this.toolTip.toolTip = this.transform.parent.name + ": " + current.ToString("F2") + "/" + max.ToString("F2");
  }

  private void Update()
  {
    if (!this.interactable)
      return;
    this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
    this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
    if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
    {
      this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
      this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
      this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
    }
    else
    {
      this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
      this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
    }
  }

  public void OnMinValueChanged(float ignoreThis)
  {
    if (!this.interactable)
      return;
    if (this.lockRange)
    {
      this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
      this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
    }
    else
      this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
    if (this.onMinChange == null)
      return;
    this.onMinChange(this);
  }

  public void OnMaxValueChanged(float ignoreThis)
  {
    if (!this.interactable)
      return;
    if (this.lockRange)
    {
      this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
      this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
    }
    else
      this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
    if (this.onMaxChange == null)
      return;
    this.onMaxChange(this);
  }

  public void Lock(bool shouldLock)
  {
    if (!this.interactable || this.lockType != MinMaxSlider.LockingType.Drag)
      return;
    this.lockRange = shouldLock;
    this.range = this.maxSlider.value - this.minSlider.value;
    this.mousePos = KInputManager.GetMousePos();
  }

  public void ToggleLock()
  {
    if (!this.interactable || this.lockType != MinMaxSlider.LockingType.Toggle)
      return;
    this.lockRange = !this.lockRange;
    if (!this.lockRange)
      return;
    this.range = this.maxSlider.value - this.minSlider.value;
  }

  public void OnDrag()
  {
    if (!this.interactable || !this.lockRange || this.lockType != MinMaxSlider.LockingType.Drag)
      return;
    float num = KInputManager.GetMousePos().x - this.mousePos.x;
    if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
      num = KInputManager.GetMousePos().y - this.mousePos.y;
    this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
    this.mousePos = KInputManager.GetMousePos();
  }

  public enum LockingType
  {
    Toggle,
    Drag,
  }

  public enum Mode
  {
    Single,
    Double,
    Triple,
  }
}
