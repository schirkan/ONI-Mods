// Decompiled with JetBrains decompiler
// Type: FilterSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : KMonoBehaviour
{
  [SerializeField]
  private LocText labelText;
  [SerializeField]
  private Image BG;
  [SerializeField]
  private Image outline;
  [SerializeField]
  private Color outlineHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, byte.MaxValue);
  [SerializeField]
  private Color BGHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, (byte) 80);
  [SerializeField]
  private Color outlineDefaultColor = (Color) new Color32((byte) 204, (byte) 204, (byte) 204, byte.MaxValue);
  private Color regularColor = Color.white;
  [SerializeField]
  public KButton button;

  public Tag tag { get; private set; }

  public bool isSelected { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.regularColor = this.outline.color;
    if (!((UnityEngine.Object) this.button != (UnityEngine.Object) null))
      return;
    this.button.onPointerEnter += (System.Action) (() =>
    {
      if (this.isSelected)
        return;
      this.outline.color = this.outlineHighLightColor;
    });
    this.button.onPointerExit += (System.Action) (() =>
    {
      if (this.isSelected)
        return;
      this.outline.color = this.regularColor;
    });
  }

  public void SetTag(Tag tag)
  {
    this.tag = tag;
    this.SetText(tag == GameTags.Void ? STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION.text : tag.ProperName());
  }

  private void SetText(string assignmentStr) => this.labelText.text = !string.IsNullOrEmpty(assignmentStr) ? assignmentStr : "-";

  public void SetSelected(bool selected)
  {
    this.isSelected = selected;
    this.outline.color = selected ? this.outlineHighLightColor : this.outlineDefaultColor;
    this.BG.color = selected ? this.BGHighLightColor : Color.white;
  }
}
