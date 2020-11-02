// Decompiled with JetBrains decompiler
// Type: PixelPackSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelPackSideScreen : SideScreenContent
{
  public PixelPack targetPixelPack;
  public KButton copyActiveToStandbyButton;
  public KButton copyStandbyToActiveButton;
  public KButton swapColorsButton;
  public GameObject colorSwatchContainer;
  public GameObject swatchEntry;
  public GameObject activeColorsContainer;
  public GameObject standbyColorsContainer;
  public List<GameObject> activeColors = new List<GameObject>();
  public List<GameObject> standbyColors = new List<GameObject>();
  public Color paintingColor;
  public GameObject selectedSwatchEntry;
  private Dictionary<Color, GameObject> swatch_object_by_color = new Dictionary<Color, GameObject>();
  private List<GameObject> highlightedSwatchGameObjects = new List<GameObject>();
  private List<Color> colorSwatch = new List<Color>()
  {
    new Color(0.4862745f, 0.4862745f, 0.4862745f),
    new Color(0.0f, 0.0f, 0.9882353f),
    new Color(0.0f, 0.0f, 0.7372549f),
    new Color(0.2666667f, 0.1568628f, 0.7372549f),
    new Color(0.5803922f, 0.0f, 0.5176471f),
    new Color(0.6588235f, 0.0f, 0.1254902f),
    new Color(0.6588235f, 0.0627451f, 0.0f),
    new Color(0.5333334f, 0.07843138f, 0.0f),
    new Color(0.3137255f, 0.1882353f, 0.0f),
    new Color(0.0f, 0.4705882f, 0.0f),
    new Color(0.0f, 0.4078431f, 0.0f),
    new Color(0.0f, 0.345098f, 0.0f),
    new Color(0.0f, 0.2509804f, 0.345098f),
    new Color(0.0f, 0.0f, 0.0f),
    new Color(0.7372549f, 0.7372549f, 0.7372549f),
    new Color(0.0f, 0.4705882f, 0.972549f),
    new Color(0.0f, 0.345098f, 0.972549f),
    new Color(0.4078431f, 0.2666667f, 0.9882353f),
    new Color(0.8470588f, 0.0f, 0.8f),
    new Color(0.8941177f, 0.0f, 0.345098f),
    new Color(0.972549f, 0.2196078f, 0.0f),
    new Color(0.8941177f, 0.3607843f, 0.0627451f),
    new Color(0.6745098f, 0.4862745f, 0.0f),
    new Color(0.0f, 0.7215686f, 0.0f),
    new Color(0.0f, 0.6588235f, 0.0f),
    new Color(0.0f, 0.6588235f, 0.2666667f),
    new Color(0.0f, 0.5333334f, 0.5333334f),
    new Color(0.0f, 0.0f, 0.0f),
    new Color(0.972549f, 0.972549f, 0.972549f),
    new Color(0.2352941f, 0.7372549f, 0.9882353f),
    new Color(0.4078431f, 0.5333334f, 0.9882353f),
    new Color(0.5960785f, 0.4705882f, 0.972549f),
    new Color(0.972549f, 0.4705882f, 0.972549f),
    new Color(0.972549f, 0.345098f, 0.5960785f),
    new Color(0.972549f, 0.4705882f, 0.345098f),
    new Color(0.9882353f, 0.627451f, 0.2666667f),
    new Color(0.972549f, 0.7215686f, 0.0f),
    new Color(0.7215686f, 0.972549f, 0.09411765f),
    new Color(0.345098f, 0.8470588f, 0.3294118f),
    new Color(0.345098f, 0.972549f, 0.5960785f),
    new Color(0.0f, 0.9098039f, 0.8470588f),
    new Color(0.4705882f, 0.4705882f, 0.4705882f),
    new Color(0.9882353f, 0.9882353f, 0.9882353f),
    new Color(0.6431373f, 0.8941177f, 0.9882353f),
    new Color(0.7215686f, 0.7215686f, 0.972549f),
    new Color(0.8470588f, 0.7215686f, 0.972549f),
    new Color(0.972549f, 0.7215686f, 0.972549f),
    new Color(0.972549f, 0.7215686f, 0.7529412f),
    new Color(0.9411765f, 0.8156863f, 0.6901961f),
    new Color(0.9882353f, 0.8784314f, 0.6588235f),
    new Color(0.972549f, 0.8470588f, 0.4705882f),
    new Color(0.8470588f, 0.972549f, 0.4705882f),
    new Color(0.7215686f, 0.972549f, 0.7215686f),
    new Color(0.7215686f, 0.972549f, 0.8470588f),
    new Color(0.0f, 0.9882353f, 0.9882353f),
    new Color(0.8470588f, 0.8470588f, 0.8470588f)
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.swatch_object_by_color.Count == 0)
      this.InitializeColorSwatch();
    this.PopulateColorSelections();
    this.copyActiveToStandbyButton.onClick += new System.Action(this.CopyActiveToStandby);
    this.copyStandbyToActiveButton.onClick += new System.Action(this.CopyStandbyToActive);
    this.swapColorsButton.onClick += new System.Action(this.SwapColors);
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<PixelPack>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetPixelPack = target.GetComponent<PixelPack>();
    this.PopulateColorSelections();
    this.HighlightUsedColors();
  }

  private void HighlightUsedColors()
  {
    if (this.swatch_object_by_color.Count == 0)
      this.InitializeColorSwatch();
    for (int index = 0; index < this.highlightedSwatchGameObjects.Count; ++index)
      this.highlightedSwatchGameObjects[index].GetComponent<HierarchyReferences>().GetReference("used").GetComponentInChildren<Image>().gameObject.SetActive(false);
    this.highlightedSwatchGameObjects.Clear();
    for (int index = 0; index < this.targetPixelPack.colorSettings.Count; ++index)
    {
      this.swatch_object_by_color[this.targetPixelPack.colorSettings[index].activeColor].GetComponent<HierarchyReferences>().GetReference("used").gameObject.SetActive(true);
      this.swatch_object_by_color[this.targetPixelPack.colorSettings[index].standbyColor].GetComponent<HierarchyReferences>().GetReference("used").gameObject.SetActive(true);
      this.highlightedSwatchGameObjects.Add(this.swatch_object_by_color[this.targetPixelPack.colorSettings[index].activeColor]);
      this.highlightedSwatchGameObjects.Add(this.swatch_object_by_color[this.targetPixelPack.colorSettings[index].standbyColor]);
    }
  }

  private void PopulateColorSelections()
  {
    for (int index = 0; index < this.targetPixelPack.colorSettings.Count; ++index)
    {
      int current_id = index;
      this.activeColors[index].GetComponent<Image>().color = this.targetPixelPack.colorSettings[index].activeColor;
      this.activeColors[index].GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        PixelPack.ColorPair colorSetting = this.targetPixelPack.colorSettings[current_id];
        this.activeColors[current_id].GetComponent<Image>().color = this.paintingColor;
        colorSetting.activeColor = this.paintingColor;
        this.targetPixelPack.colorSettings[current_id] = colorSetting;
        this.targetPixelPack.UpdateColors();
        this.HighlightUsedColors();
      });
      this.standbyColors[index].GetComponent<Image>().color = this.targetPixelPack.colorSettings[index].standbyColor;
      this.standbyColors[index].GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        PixelPack.ColorPair colorSetting = this.targetPixelPack.colorSettings[current_id];
        this.standbyColors[current_id].GetComponent<Image>().color = this.paintingColor;
        colorSetting.standbyColor = this.paintingColor;
        this.targetPixelPack.colorSettings[current_id] = colorSetting;
        this.targetPixelPack.UpdateColors();
        this.HighlightUsedColors();
      });
    }
  }

  private void InitializeColorSwatch()
  {
    bool flag = false;
    for (int index = 0; index < this.colorSwatch.Count; ++index)
    {
      GameObject swatch = Util.KInstantiateUI(this.swatchEntry, this.colorSwatchContainer, true);
      Image component1 = swatch.GetComponent<Image>();
      component1.color = this.colorSwatch[index];
      KButton component2 = swatch.GetComponent<KButton>();
      Color color = this.colorSwatch[index];
      if (component1.color == Color.black)
        swatch.GetComponent<HierarchyReferences>().GetReference("selected").GetComponentInChildren<Image>().color = Color.white;
      if (!flag)
      {
        this.SelectColor(color, swatch);
        flag = true;
      }
      component2.onClick += (System.Action) (() => this.SelectColor(color, swatch));
      this.swatch_object_by_color[color] = swatch;
    }
  }

  private void SelectColor(Color color, GameObject swatchEntry)
  {
    this.paintingColor = color;
    swatchEntry.GetComponent<HierarchyReferences>().GetReference("selected").gameObject.SetActive(true);
    if ((UnityEngine.Object) this.selectedSwatchEntry != (UnityEngine.Object) null && (UnityEngine.Object) this.selectedSwatchEntry != (UnityEngine.Object) swatchEntry)
      this.selectedSwatchEntry.GetComponent<HierarchyReferences>().GetReference("selected").gameObject.SetActive(false);
    this.selectedSwatchEntry = swatchEntry;
  }

  private void CopyActiveToStandby()
  {
    for (int index = 0; index < this.targetPixelPack.colorSettings.Count; ++index)
    {
      PixelPack.ColorPair colorSetting = this.targetPixelPack.colorSettings[index];
      colorSetting.standbyColor = colorSetting.activeColor;
      this.targetPixelPack.colorSettings[index] = colorSetting;
      this.standbyColors[index].GetComponent<Image>().color = colorSetting.activeColor;
    }
    this.HighlightUsedColors();
    this.targetPixelPack.UpdateColors();
  }

  private void CopyStandbyToActive()
  {
    for (int index = 0; index < this.targetPixelPack.colorSettings.Count; ++index)
    {
      PixelPack.ColorPair colorSetting = this.targetPixelPack.colorSettings[index];
      colorSetting.activeColor = colorSetting.standbyColor;
      this.targetPixelPack.colorSettings[index] = colorSetting;
      this.activeColors[index].GetComponent<Image>().color = colorSetting.standbyColor;
    }
    this.HighlightUsedColors();
    this.targetPixelPack.UpdateColors();
  }

  private void SwapColors()
  {
    for (int index = 0; index < this.targetPixelPack.colorSettings.Count; ++index)
    {
      PixelPack.ColorPair colorPair = new PixelPack.ColorPair();
      colorPair.activeColor = this.targetPixelPack.colorSettings[index].standbyColor;
      colorPair.standbyColor = this.targetPixelPack.colorSettings[index].activeColor;
      this.targetPixelPack.colorSettings[index] = colorPair;
      this.activeColors[index].GetComponent<Image>().color = colorPair.activeColor;
      this.standbyColors[index].GetComponent<Image>().color = colorPair.standbyColor;
    }
    this.HighlightUsedColors();
    this.targetPixelPack.UpdateColors();
  }
}
