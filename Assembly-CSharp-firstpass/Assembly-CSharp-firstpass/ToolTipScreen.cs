// Decompiled with JetBrains decompiler
// Type: ToolTipScreen
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScreen : KScreen
{
  public GameObject ToolTipPrefab;
  public RectTransform anchorRoot;
  private GameObject toolTipWidget;
  private ToolTip prevTooltip;
  private ToolTip tooltipSetting;
  public GameObject labelPrefab;
  private GameObject multiTooltipContainer;
  public TextStyleSetting defaultTooltipHeaderStyle;
  public TextStyleSetting defaultTooltipBodyStyle;
  private bool toolTipIsBlank;
  private Vector2 ScreenEdgePadding = new Vector2(8f, 8f);
  private ToolTip dirtyHoverTooltip;
  private bool tooltipIncubating = true;

  public static ToolTipScreen Instance { get; private set; }

  protected override void OnActivate()
  {
    ToolTipScreen.Instance = this;
    this.toolTipWidget = Util.KInstantiate(this.ToolTipPrefab, this.gameObject);
    this.toolTipWidget.transform.SetParent(this.gameObject.transform, false);
    Util.Reset(this.toolTipWidget.transform);
    this.toolTipWidget.SetActive(false);
  }

  protected override void OnCleanUp() => ToolTipScreen.Instance = (ToolTipScreen) null;

  public void SetToolTip(ToolTip tool_tip)
  {
    this.tooltipSetting = tool_tip;
    this.multiTooltipContainer = this.toolTipWidget.transform.Find("MultitooltipContainer").gameObject;
    this.ConfigureTooltip();
  }

  private void ConfigureTooltip()
  {
    if ((Object) this.tooltipSetting == (Object) null)
      this.prevTooltip = (ToolTip) null;
    if ((Object) this.tooltipSetting != (Object) null && (Object) this.dirtyHoverTooltip != (Object) null && (Object) this.tooltipSetting == (Object) this.dirtyHoverTooltip)
      this.ClearToolTip(this.dirtyHoverTooltip);
    if ((Object) this.tooltipSetting != (Object) null)
    {
      this.tooltipSetting.RebuildDynamicTooltip();
      if (this.tooltipSetting.multiStringCount == 0)
        this.clearMultiStringTooltip();
      else if ((Object) this.prevTooltip != (Object) this.tooltipSetting || !this.multiTooltipContainer.activeInHierarchy)
      {
        this.prepareMultiStringTooltip(this.tooltipSetting);
        this.prevTooltip = this.tooltipSetting;
      }
      bool flag = (uint) this.multiTooltipContainer.transform.childCount > 0U;
      this.toolTipWidget.SetActive(flag);
      if (flag)
      {
        RectTransform cmp = !((Object) this.tooltipSetting.overrideParentObject == (Object) null) ? this.tooltipSetting.overrideParentObject : this.tooltipSetting.GetComponent<RectTransform>();
        RectTransform component1 = this.toolTipWidget.GetComponent<RectTransform>();
        component1.transform.SetParent(this.anchorRoot.transform);
        this.anchorRoot.anchoredPosition = this.tooltipSetting.worldSpace ? (Vector2) (this.WorldToScreen(cmp.transform.GetPosition()) + new Vector3((float) (Screen.width / 2), (float) (Screen.height / 2), 0.0f)) : (Vector2) cmp.transform.GetPosition();
        this.anchorRoot.anchoredPosition -= Vector2.up * (cmp.rectTransform().pivot.y * cmp.rectTransform().sizeDelta.y);
        this.anchorRoot.anchoredPosition -= Vector2.right * (cmp.rectTransform().pivot.x * cmp.rectTransform().sizeDelta.x);
        this.anchorRoot.anchoredPosition += Vector2.right * (cmp.sizeDelta.x * this.tooltipSetting.parentPositionAnchor.x);
        this.anchorRoot.anchoredPosition += Vector2.up * (cmp.sizeDelta.y * this.tooltipSetting.parentPositionAnchor.y);
        float b = 1f;
        CanvasScaler component2 = this.transform.parent.GetComponent<CanvasScaler>();
        if ((Object) component2 == (Object) null)
          component2 = this.transform.parent.parent.GetComponent<CanvasScaler>();
        if ((Object) component2 != (Object) null)
          b = component2.scaleFactor;
        this.anchorRoot.anchoredPosition = new Vector2(this.anchorRoot.anchoredPosition.x / b, this.anchorRoot.anchoredPosition.y / b);
        component1.pivot = this.tooltipSetting.tooltipPivot;
        RectTransform rectTransform1 = component1;
        RectTransform rectTransform2 = component1;
        Vector2 vector2_1 = new Vector2(0.0f, 0.0f);
        Vector2 vector2_2 = vector2_1;
        rectTransform2.anchorMax = vector2_2;
        Vector2 vector2_3 = vector2_1;
        rectTransform1.anchorMin = vector2_3;
        component1.anchoredPosition = this.tooltipSetting.tooltipPositionOffset * b;
        if (!this.tooltipSetting.worldSpace)
        {
          Rect rect = ((RectTransform) this.transform).rect;
          Vector2 vector2_4 = new Vector2(this.transform.GetPosition().x, this.transform.GetPosition().y) + this.ScreenEdgePadding;
          Vector2 vector2_5 = new Vector2(this.transform.GetPosition().x, this.transform.GetPosition().y) + rect.width * Vector2.right + rect.height * Vector2.up - this.ScreenEdgePadding * Mathf.Max(1f, b);
          vector2_5.x *= b;
          vector2_5.y *= b;
          Vector2 vector2_6;
          vector2_6.x = component1.GetPosition().x - component1.pivot.x * (component1.sizeDelta.x * b);
          vector2_6.y = component1.GetPosition().y - component1.pivot.y * (component1.sizeDelta.y * b);
          Vector2 vector2_7;
          vector2_7.x = component1.GetPosition().x + (float) ((1.0 - (double) component1.pivot.x) * ((double) component1.sizeDelta.x * (double) b));
          vector2_7.y = component1.GetPosition().y + (float) ((1.0 - (double) component1.pivot.y) * ((double) component1.sizeDelta.y * (double) b));
          Vector2 zero = Vector2.zero;
          if ((double) vector2_6.x < (double) vector2_4.x)
            zero.x = vector2_4.x - vector2_6.x;
          if ((double) vector2_7.x > (double) vector2_5.x)
            zero.x = vector2_5.x - vector2_7.x;
          if ((double) vector2_6.y < (double) vector2_4.y)
            zero.y = vector2_4.y - vector2_6.y;
          if ((double) vector2_7.y > (double) vector2_5.y)
            zero.y = vector2_5.y - vector2_7.y;
          Vector2 vector2_8 = zero / b;
          component1.anchoredPosition += vector2_8;
        }
      }
    }
    if (this.transform.GetSiblingIndex() == this.transform.parent.childCount - 1)
      return;
    this.transform.SetAsLastSibling();
  }

  private void prepareMultiStringTooltip(ToolTip setting)
  {
    int multiStringCount = this.tooltipSetting.multiStringCount;
    this.clearMultiStringTooltip();
    for (int index = 0; index < multiStringCount; ++index)
      Util.KInstantiateUI(this.labelPrefab, force_active: true).transform.SetParent(this.multiTooltipContainer.transform);
    for (int index = 0; index < this.tooltipSetting.multiStringCount; ++index)
    {
      Transform child = this.multiTooltipContainer.transform.GetChild(index);
      LayoutElement component1 = child.GetComponent<LayoutElement>();
      TextMeshProUGUI component2 = child.GetComponent<TextMeshProUGUI>();
      component2.text = this.tooltipSetting.GetMultiString(index);
      child.GetComponent<SetTextStyleSetting>().SetStyle((TextStyleSetting) this.tooltipSetting.GetStyleSetting(index));
      if (setting.SizingSetting == ToolTip.ToolTipSizeSetting.MaxWidthWrapContent)
      {
        component1.minWidth = component1.preferredWidth = setting.WrapWidth;
        component1.rectTransform().sizeDelta = new Vector2(setting.WrapWidth, 1000f);
        component1.minHeight = component1.preferredHeight = component2.preferredHeight;
        component1.minHeight = component1.preferredHeight = component2.preferredHeight;
        component1.rectTransform().sizeDelta = new Vector2(setting.WrapWidth, component1.minHeight);
        this.GetComponentInChildren<ContentSizeFitter>(true).horizontalFit = ContentSizeFitter.FitMode.MinSize;
        this.multiTooltipContainer.GetComponent<LayoutElement>().minWidth = setting.WrapWidth;
      }
      else if (setting.SizingSetting == ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap)
      {
        this.GetComponentInChildren<ContentSizeFitter>(true).horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        Vector2 preferredValues = component2.GetPreferredValues();
        this.multiTooltipContainer.GetComponent<LayoutElement>().minWidth = component1.minWidth = component1.preferredWidth = preferredValues.x;
        component1.minHeight = component1.preferredHeight = preferredValues.y;
        this.GetComponentInChildren<ContentSizeFitter>(true).SetLayoutHorizontal();
        this.GetComponentInChildren<ContentSizeFitter>(true).SetLayoutVertical();
        this.multiTooltipContainer.rectTransform().sizeDelta = new Vector2(component1.minWidth, component1.minHeight);
        this.multiTooltipContainer.transform.parent.rectTransform().sizeDelta = this.multiTooltipContainer.rectTransform().sizeDelta;
      }
      component2.ForceMeshUpdate();
    }
    this.tooltipIncubating = true;
  }

  private void Update()
  {
    if ((Object) this.tooltipSetting != (Object) null)
      this.tooltipSetting.UpdateWhileHovered();
    if ((Object) this.multiTooltipContainer == (Object) null || (Object) this.anchorRoot == (Object) null)
      return;
    if ((Object) this.dirtyHoverTooltip != (Object) null)
    {
      ToolTip dirtyHoverTooltip = this.dirtyHoverTooltip;
      this.MakeDirtyTooltipClean(dirtyHoverTooltip);
      this.ClearToolTip(dirtyHoverTooltip);
    }
    if (this.tooltipIncubating)
    {
      this.tooltipIncubating = false;
      if ((Object) this.anchorRoot.GetComponentInChildren<Image>() != (Object) null)
        this.anchorRoot.GetComponentInChildren<Image>(true).enabled = false;
      this.multiTooltipContainer.transform.localScale = Vector3.zero;
      this.toolTipIsBlank = true;
      for (int index = 0; index < this.multiTooltipContainer.transform.childCount; ++index)
      {
        if (this.multiTooltipContainer.transform.GetChild(index).transform.localScale != Vector3.one)
          this.multiTooltipContainer.transform.GetChild(index).transform.localScale = Vector3.one;
        LayoutElement component1 = this.multiTooltipContainer.transform.GetChild(index).GetComponent<LayoutElement>();
        TextMeshProUGUI component2 = component1.GetComponent<TextMeshProUGUI>();
        this.toolTipIsBlank = component2.text == "" && this.toolTipIsBlank;
        if ((double) component1.minHeight != (double) component2.preferredHeight)
          component1.minHeight = component2.preferredHeight;
      }
    }
    else
    {
      if (!(this.multiTooltipContainer.transform.localScale != Vector3.one) || this.toolTipIsBlank)
        return;
      if ((Object) this.anchorRoot.GetComponentInChildren<Image>() != (Object) null)
        this.anchorRoot.GetComponentInChildren<Image>(true).enabled = true;
      this.multiTooltipContainer.transform.localScale = Vector3.one;
    }
  }

  public void HotSwapTooltipString(string newString, int lineIndex)
  {
    if (this.multiTooltipContainer.transform.childCount <= lineIndex)
      return;
    this.multiTooltipContainer.transform.GetChild(lineIndex).GetComponent<TextMeshProUGUI>().text = newString;
  }

  private void clearMultiStringTooltip()
  {
    for (int index = this.multiTooltipContainer.transform.childCount - 1; index >= 0; --index)
      Object.DestroyImmediate((Object) this.multiTooltipContainer.transform.GetChild(index).gameObject);
  }

  public void ClearToolTip(ToolTip tt)
  {
    if (!((Object) tt == (Object) this.tooltipSetting))
      return;
    this.tooltipSetting = (ToolTip) null;
    if (!((Object) this.toolTipWidget != (Object) null))
      return;
    this.clearMultiStringTooltip();
    this.toolTipWidget.SetActive(false);
  }

  public void MarkTooltipDirty(ToolTip tt)
  {
    if (!((Object) tt == (Object) this.tooltipSetting))
      return;
    this.dirtyHoverTooltip = tt;
  }

  public void MakeDirtyTooltipClean(ToolTip tt)
  {
    if (!((Object) tt == (Object) this.dirtyHoverTooltip))
      return;
    this.dirtyHoverTooltip = (ToolTip) null;
  }
}
