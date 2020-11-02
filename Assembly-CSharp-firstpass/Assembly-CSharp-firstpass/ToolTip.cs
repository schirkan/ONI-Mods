// Decompiled with JetBrains decompiler
// Type: ToolTip
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("KMonoBehaviour/Plugins/ToolTip")]
public class ToolTip : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public bool UseFixedStringKey;
  public string FixedStringKey = "";
  private List<string> multiStringToolTips = new List<string>();
  private List<ScriptableObject> styleSettings = new List<ScriptableObject>();
  public bool worldSpace;
  public bool forceRefresh;
  public bool refreshWhileHovering;
  private bool isHovering;
  private float lastUpdateTime;
  public ToolTip.TooltipPosition toolTipPosition = ToolTip.TooltipPosition.BottomCenter;
  public Vector2 tooltipPivot = new Vector2(0.0f, 1f);
  public Vector2 tooltipPositionOffset = new Vector2(0.0f, -25f);
  public Vector2 parentPositionAnchor = new Vector2(0.5f, 0.5f);
  public RectTransform overrideParentObject;
  public ToolTip.ToolTipSizeSetting SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
  public float WrapWidth = 256f;
  private Func<string> _OnToolTip;
  public Func<List<Tuple<string, ScriptableObject>>> OnComplexToolTip;
  private static readonly EventSystem.IntraObjectHandler<ToolTip> OnClickDelegate = new EventSystem.IntraObjectHandler<ToolTip>((System.Action<ToolTip, object>) ((component, data) => component.OnClick(data)));

  public string toolTip
  {
    set => this.SetSimpleTooltip(value);
  }

  public int multiStringCount => this.multiStringToolTips.Count;

  public Func<string> OnToolTip
  {
    get => this._OnToolTip;
    set => this._OnToolTip = value;
  }

  protected override void OnPrefabInit()
  {
    if (this.gameObject.GetComponents<ToolTip>().Length > 1)
      Debug.LogError((object) ("The object " + this.gameObject.name + " has more than one ToolTip, it conflict when displaying this tooltip."));
    this.Subscribe<ToolTip>(2098165161, ToolTip.OnClickDelegate);
    if (this.UseFixedStringKey)
      this.toolTip = (string) Strings.Get(new StringKey(this.FixedStringKey));
    switch (this.toolTipPosition)
    {
      case ToolTip.TooltipPosition.TopLeft:
        this.tooltipPivot = new Vector2(1f, 0.0f);
        this.tooltipPositionOffset = new Vector2(0.0f, 20f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
      case ToolTip.TooltipPosition.TopCenter:
        this.tooltipPivot = new Vector2(0.5f, 0.0f);
        this.tooltipPositionOffset = new Vector2(0.0f, 20f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
      case ToolTip.TooltipPosition.TopRight:
        this.tooltipPivot = new Vector2(0.0f, 0.0f);
        this.tooltipPositionOffset = new Vector2(0.0f, 20f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
      case ToolTip.TooltipPosition.BottomLeft:
        this.tooltipPivot = new Vector2(1f, 1f);
        this.tooltipPositionOffset = new Vector2(0.0f, -25f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
      case ToolTip.TooltipPosition.BottomCenter:
        this.tooltipPivot = new Vector2(0.5f, 1f);
        this.tooltipPositionOffset = new Vector2(0.0f, -25f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
      case ToolTip.TooltipPosition.BottomRight:
        this.tooltipPivot = new Vector2(0.0f, 1f);
        this.tooltipPositionOffset = new Vector2(0.0f, -25f);
        this.parentPositionAnchor = new Vector2(0.5f, 0.5f);
        break;
    }
  }

  protected override void OnSpawn()
  {
    if (this.worldSpace)
      return;
    Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
    this.worldSpace = (UnityEngine.Object) componentInParent != (UnityEngine.Object) null && (UnityEngine.Object) componentInParent.worldCamera != (UnityEngine.Object) null;
  }

  public void SetSimpleTooltip(string message)
  {
    this.ClearMultiStringTooltip();
    this.AddMultiStringTooltip(message, (ScriptableObject) PluginAssets.Instance.defaultTextStyleSetting);
  }

  public void AddMultiStringTooltip(string newString, ScriptableObject styleSetting)
  {
    this.multiStringToolTips.Add(newString);
    this.styleSettings.Add(styleSetting);
  }

  public void ClearMultiStringTooltip()
  {
    this.multiStringToolTips.Clear();
    this.styleSettings.Clear();
  }

  public string GetMultiString(int idx) => this.multiStringToolTips[idx];

  public ScriptableObject GetStyleSetting(int idx) => this.styleSettings[idx];

  public void SetFixedStringKey(string newKey)
  {
    this.FixedStringKey = newKey;
    this.toolTip = (string) Strings.Get(new StringKey(this.FixedStringKey));
  }

  public void RebuildDynamicTooltip()
  {
    if (this.OnToolTip != null)
    {
      this.ClearMultiStringTooltip();
      string newString = this.OnToolTip();
      if (string.IsNullOrEmpty(newString))
        return;
      this.AddMultiStringTooltip(newString, (ScriptableObject) PluginAssets.Instance.defaultTextStyleSetting);
    }
    else
    {
      if (this.OnComplexToolTip == null)
        return;
      this.ClearMultiStringTooltip();
      foreach (Tuple<string, ScriptableObject> tuple in this.OnComplexToolTip())
        this.AddMultiStringTooltip(tuple.first, tuple.second);
    }
  }

  public void OnPointerEnter(PointerEventData data)
  {
    this.OnHoverStateChanged(true);
    this.isHovering = true;
  }

  public void OnPointerExit(PointerEventData data)
  {
    this.OnHoverStateChanged(false);
    this.isHovering = false;
  }

  private void OnClick(object data) => ToolTipScreen.Instance.ClearToolTip(this);

  private void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) ToolTipScreen.Instance)
      return;
    ToolTipScreen.Instance.MarkTooltipDirty(this);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!(bool) (UnityEngine.Object) ToolTipScreen.Instance)
      return;
    ToolTipScreen.Instance.MarkTooltipDirty(this);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!(bool) (UnityEngine.Object) ToolTipScreen.Instance)
      return;
    ToolTipScreen.Instance.MakeDirtyTooltipClean(this);
  }

  private void OnHoverStateChanged(bool is_over)
  {
    if ((UnityEngine.Object) ToolTipScreen.Instance == (UnityEngine.Object) null)
      return;
    if (is_over)
      ToolTipScreen.Instance.SetToolTip(this);
    else
      ToolTipScreen.Instance.ClearToolTip(this);
  }

  protected override void OnCleanUp()
  {
    if (!((UnityEngine.Object) ToolTipScreen.Instance != (UnityEngine.Object) null))
      return;
    ToolTipScreen.Instance.ClearToolTip(this);
  }

  public void UpdateWhileHovered()
  {
    if (!this.forceRefresh && !this.refreshWhileHovering || (double) Time.unscaledTime - (double) this.lastUpdateTime <= 0.200000002980232)
      return;
    this.lastUpdateTime = Time.unscaledTime;
    if (!this.isHovering)
      return;
    this.RebuildDynamicTooltip();
    for (int index = 0; index < this.multiStringToolTips.Count; ++index)
      ToolTipScreen.Instance.HotSwapTooltipString(this.multiStringToolTips[index], index);
  }

  public enum TooltipPosition
  {
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
    Custom,
  }

  public enum ToolTipSizeSetting
  {
    MaxWidthWrapContent,
    DynamicWidthNoWrap,
  }
}
