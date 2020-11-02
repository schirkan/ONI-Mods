// Decompiled with JetBrains decompiler
// Type: ResourceCategoryScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceCategoryScreen : KScreen
{
  public static ResourceCategoryScreen Instance;
  public GameObject Prefab_CategoryBar;
  public Transform CategoryContainer;
  public MultiToggle HiderButton;
  public KLayoutElement HideTarget;
  private float HideSpeedFactor = 12f;
  private float maxHeightPadding = 280f;
  private float targetContentHideHeight;
  public Dictionary<Tag, ResourceCategoryHeader> DisplayedCategories = new Dictionary<Tag, ResourceCategoryHeader>();
  private Tag[] DisplayedCategoryKeys;
  private int categoryUpdatePacer;

  public static void DestroyInstance() => ResourceCategoryScreen.Instance = (ResourceCategoryScreen) null;

  protected override void OnActivate()
  {
    base.OnActivate();
    ResourceCategoryScreen.Instance = this;
    this.ConsumeMouseScroll = true;
    this.HiderButton.onClick += new System.Action(this.OnHiderClick);
    this.OnHiderClick();
    this.CreateTagSetHeaders((IEnumerable<Tag>) GameTags.MaterialCategories, GameUtil.MeasureUnit.mass);
    this.CreateTagSetHeaders((IEnumerable<Tag>) GameTags.CalorieCategories, GameUtil.MeasureUnit.kcal);
    this.CreateTagSetHeaders((IEnumerable<Tag>) GameTags.UnitCategories, GameUtil.MeasureUnit.quantity);
    if (!this.DisplayedCategories.ContainsKey(GameTags.Miscellaneous))
    {
      ResourceCategoryHeader resourceCategoryHeader = this.NewCategoryHeader(GameTags.Miscellaneous, GameUtil.MeasureUnit.mass);
      this.DisplayedCategories.Add(GameTags.Miscellaneous, resourceCategoryHeader);
    }
    this.DisplayedCategoryKeys = this.DisplayedCategories.Keys.ToArray<Tag>();
  }

  private void CreateTagSetHeaders(IEnumerable<Tag> set, GameUtil.MeasureUnit measure)
  {
    foreach (Tag tag in set)
    {
      ResourceCategoryHeader resourceCategoryHeader = this.NewCategoryHeader(tag, measure);
      this.DisplayedCategories.Add(tag, resourceCategoryHeader);
    }
  }

  private void OnHiderClick()
  {
    this.HiderButton.NextState();
    if (this.HiderButton.CurrentState == 0)
      this.targetContentHideHeight = 0.0f;
    else
      this.targetContentHideHeight = Mathf.Min(((float) Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
  }

  private void Update()
  {
    if ((UnityEngine.Object) WorldInventory.Instance == (UnityEngine.Object) null)
      return;
    if ((double) this.HideTarget.minHeight != (double) this.targetContentHideHeight)
    {
      float minHeight = this.HideTarget.minHeight;
      float num1 = this.targetContentHideHeight - minHeight;
      float num2 = Mathf.Clamp(num1 * this.HideSpeedFactor * Time.unscaledDeltaTime, (double) num1 > 0.0 ? -num1 : num1, (double) num1 > 0.0 ? num1 : -num1);
      this.HideTarget.minHeight = minHeight + num2;
    }
    for (int index = 0; index < 1; ++index)
    {
      Tag displayedCategoryKey = this.DisplayedCategoryKeys[this.categoryUpdatePacer];
      ResourceCategoryHeader displayedCategory = this.DisplayedCategories[displayedCategoryKey];
      if (WorldInventory.Instance.IsDiscovered(displayedCategoryKey) && !displayedCategory.gameObject.activeInHierarchy)
        displayedCategory.gameObject.SetActive(true);
      displayedCategory.UpdateContents();
      this.categoryUpdatePacer = (this.categoryUpdatePacer + 1) % this.DisplayedCategoryKeys.Length;
    }
    if (this.HiderButton.CurrentState != 0)
      this.targetContentHideHeight = Mathf.Min(((float) Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
    if (!((UnityEngine.Object) MeterScreen.Instance != (UnityEngine.Object) null) || MeterScreen.Instance.StartValuesSet)
      return;
    MeterScreen.Instance.InitializeValues();
  }

  private ResourceCategoryHeader NewCategoryHeader(
    Tag categoryTag,
    GameUtil.MeasureUnit measure)
  {
    GameObject gameObject = Util.KInstantiateUI(this.Prefab_CategoryBar, this.CategoryContainer.gameObject);
    gameObject.name = "CategoryHeader_" + categoryTag.Name;
    ResourceCategoryHeader component = gameObject.GetComponent<ResourceCategoryHeader>();
    component.SetTag(categoryTag, measure);
    return component;
  }

  public static string QuantityTextForMeasure(float quantity, GameUtil.MeasureUnit measure)
  {
    switch (measure)
    {
      case GameUtil.MeasureUnit.mass:
        return GameUtil.GetFormattedMass(quantity);
      case GameUtil.MeasureUnit.kcal:
        return GameUtil.GetFormattedCalories(quantity);
      default:
        return quantity.ToString();
    }
  }
}
