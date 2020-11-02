// Decompiled with JetBrains decompiler
// Type: BreakdownListRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
  private static Color[] statusColour = new Color[4]
  {
    new Color(0.3411765f, 0.3686275f, 0.4588235f, 1f),
    new Color(0.7215686f, 0.3843137f, 0.0f, 1f),
    new Color(0.3843137f, 0.7215686f, 0.0f, 1f),
    new Color(0.7215686f, 0.7215686f, 0.0f, 1f)
  };
  public Image dotOutlineImage;
  public Image dotInsideImage;
  public Image iconImage;
  public Image checkmarkImage;
  public LocText nameLabel;
  public LocText valueLabel;
  private bool isHighlighted;
  private bool isDisabled;
  private bool isImportant;
  private ToolTip tooltip;
  [SerializeField]
  private Sprite statusSuccessIcon;
  [SerializeField]
  private Sprite statusWarningIcon;
  [SerializeField]
  private Sprite statusFailureIcon;

  public void ShowData(string name, string value)
  {
    this.gameObject.transform.localScale = Vector3.one;
    this.nameLabel.text = name;
    this.valueLabel.text = value;
    this.dotOutlineImage.gameObject.SetActive(true);
    Vector2 vector2 = Vector2.one * 0.6f;
    this.dotOutlineImage.rectTransform.localScale.Set(vector2.x, vector2.y, 1f);
    this.dotInsideImage.gameObject.SetActive(true);
    this.dotInsideImage.color = BreakdownListRow.statusColour[0];
    this.iconImage.gameObject.SetActive(false);
    this.checkmarkImage.gameObject.SetActive(false);
    this.SetHighlighted(false);
    this.SetImportant(false);
  }

  public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
  {
    this.ShowData(name, value);
    this.dotOutlineImage.gameObject.SetActive(true);
    this.dotInsideImage.gameObject.SetActive(true);
    this.iconImage.gameObject.SetActive(false);
    this.checkmarkImage.gameObject.SetActive(false);
    this.SetStatusColor(dotColor);
  }

  public void SetStatusColor(BreakdownListRow.Status dotColor)
  {
    this.checkmarkImage.gameObject.SetActive((uint) dotColor > 0U);
    this.checkmarkImage.color = BreakdownListRow.statusColour[(int) dotColor];
    switch (dotColor)
    {
      case BreakdownListRow.Status.Red:
        this.checkmarkImage.sprite = this.statusFailureIcon;
        break;
      case BreakdownListRow.Status.Green:
        this.checkmarkImage.sprite = this.statusSuccessIcon;
        break;
      case BreakdownListRow.Status.Yellow:
        this.checkmarkImage.sprite = this.statusWarningIcon;
        break;
    }
  }

  public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
  {
    this.ShowData(name, value);
    this.dotOutlineImage.gameObject.SetActive(true);
    this.dotOutlineImage.rectTransform.localScale = Vector3.one;
    this.dotInsideImage.gameObject.SetActive(true);
    this.iconImage.gameObject.SetActive(false);
    this.SetStatusColor(status);
  }

  public void ShowIconData(string name, string value, Sprite sprite)
  {
    this.ShowData(name, value);
    this.dotOutlineImage.gameObject.SetActive(false);
    this.dotInsideImage.gameObject.SetActive(false);
    this.iconImage.gameObject.SetActive(true);
    this.checkmarkImage.gameObject.SetActive(false);
    this.iconImage.sprite = sprite;
    this.iconImage.color = Color.white;
  }

  public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
  {
    this.ShowIconData(name, value, sprite);
    this.iconImage.color = spriteColor;
  }

  public void SetHighlighted(bool highlighted)
  {
    this.isHighlighted = highlighted;
    Vector2 vector2 = Vector2.one * 0.8f;
    this.dotOutlineImage.rectTransform.localScale.Set(vector2.x, vector2.y, 1f);
    this.nameLabel.alpha = this.isHighlighted ? 0.9f : 0.5f;
    this.valueLabel.alpha = this.isHighlighted ? 0.9f : 0.5f;
  }

  public void SetDisabled(bool disabled)
  {
    this.isDisabled = disabled;
    this.nameLabel.alpha = this.isDisabled ? 0.4f : 0.5f;
    this.valueLabel.alpha = this.isDisabled ? 0.4f : 0.5f;
  }

  public void SetImportant(bool important)
  {
    this.isImportant = important;
    this.dotOutlineImage.rectTransform.localScale = Vector3.one;
    this.nameLabel.alpha = this.isImportant ? 1f : 0.5f;
    this.valueLabel.alpha = this.isImportant ? 1f : 0.5f;
    this.nameLabel.fontStyle = this.isImportant ? FontStyles.Bold : FontStyles.Normal;
    this.valueLabel.fontStyle = this.isImportant ? FontStyles.Bold : FontStyles.Normal;
  }

  public void HideIcon()
  {
    this.dotOutlineImage.gameObject.SetActive(false);
    this.dotInsideImage.gameObject.SetActive(false);
    this.iconImage.gameObject.SetActive(false);
    this.checkmarkImage.gameObject.SetActive(false);
  }

  public void AddTooltip(string tooltipText)
  {
    if ((Object) this.tooltip == (Object) null)
      this.tooltip = this.gameObject.AddComponent<ToolTip>();
    this.tooltip.SetSimpleTooltip(tooltipText);
  }

  public void ClearTooltip()
  {
    if (!((Object) this.tooltip != (Object) null))
      return;
    this.tooltip.ClearMultiStringTooltip();
  }

  public void SetValue(string value) => this.valueLabel.text = value;

  public enum Status
  {
    Default,
    Red,
    Green,
    Yellow,
  }
}
