// Decompiled with JetBrains decompiler
// Type: ResourceEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public Tag Resource;
  public GameUtil.MeasureUnit Measure;
  public LocText NameLabel;
  public LocText QuantityLabel;
  public Image image;
  [SerializeField]
  private Color AvailableColor;
  [SerializeField]
  private Color UnavailableColor;
  [SerializeField]
  private Color OverdrawnColor;
  [SerializeField]
  private Color HighlightColor;
  [SerializeField]
  private Color BackgroundHoverColor;
  [SerializeField]
  private Image Background;
  [MyCmpGet]
  private ToolTip tooltip;
  [MyCmpReq]
  private UnityEngine.UI.Button button;
  private const float CLICK_RESET_TIME_THRESHOLD = 10f;
  private int selectionIdx;
  private float lastClickTime;
  private List<Pickupable> cachedPickupables;
  private float currentQuantity = float.MinValue;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.QuantityLabel.color = this.AvailableColor;
    this.NameLabel.color = this.AvailableColor;
    this.button.onClick.AddListener(new UnityAction(this.OnClick));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
  }

  private void OnClick()
  {
    this.lastClickTime = Time.unscaledTime;
    if (this.cachedPickupables == null)
    {
      this.cachedPickupables = WorldInventory.Instance.CreatePickupablesList(this.Resource);
      this.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
    }
    if (this.cachedPickupables == null)
      return;
    Pickupable cmp = (Pickupable) null;
    for (int index = 0; index < this.cachedPickupables.Count; ++index)
    {
      ++this.selectionIdx;
      cmp = this.cachedPickupables[this.selectionIdx % this.cachedPickupables.Count];
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null && !cmp.HasTag(GameTags.StoredPrivate))
        break;
    }
    if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
      return;
    Transform transform = cmp.transform;
    if ((UnityEngine.Object) cmp.storage != (UnityEngine.Object) null)
      transform = cmp.storage.transform;
    SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
    for (int index = 0; index < this.cachedPickupables.Count; ++index)
    {
      Pickupable cachedPickupable = this.cachedPickupables[index];
      if ((UnityEngine.Object) cachedPickupable != (UnityEngine.Object) null)
      {
        KAnimControllerBase component = cachedPickupable.GetComponent<KAnimControllerBase>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.HighlightColour = (Color32) this.HighlightColor;
      }
    }
  }

  private IEnumerator ClearCachedPickupablesAfterThreshold()
  {
    while (this.cachedPickupables != null && (double) this.lastClickTime != 0.0 && (double) Time.unscaledTime - (double) this.lastClickTime < 10.0)
      yield return (object) new WaitForSeconds(1f);
    this.cachedPickupables = (List<Pickupable>) null;
  }

  public void GetAmounts(
    EdiblesManager.FoodInfo food_info,
    bool doExtras,
    out float available,
    out float total,
    out float reserved)
  {
    available = WorldInventory.Instance.GetAmount(this.Resource);
    total = doExtras ? WorldInventory.Instance.GetTotalAmount(this.Resource) : 0.0f;
    reserved = doExtras ? MaterialNeeds.Instance.GetAmount(this.Resource) : 0.0f;
    if (food_info == null)
      return;
    available *= food_info.CaloriesPerUnit;
    total *= food_info.CaloriesPerUnit;
    reserved *= food_info.CaloriesPerUnit;
  }

  private void GetAmounts(bool doExtras, out float available, out float total, out float reserved) => this.GetAmounts(this.Measure == GameUtil.MeasureUnit.kcal ? EdiblesManager.GetFoodInfo(this.Resource.Name) : (EdiblesManager.FoodInfo) null, doExtras, out available, out total, out reserved);

  public void UpdateValue()
  {
    this.SetName(this.Resource.ProperName());
    float available;
    float total;
    float reserved;
    this.GetAmounts(GenericGameSettings.instance.allowInsufficientMaterialBuild, out available, out total, out reserved);
    if ((double) this.currentQuantity != (double) available)
    {
      this.currentQuantity = available;
      this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(available, this.Measure);
    }
    Color color = this.AvailableColor;
    if ((double) reserved > (double) total)
      color = this.OverdrawnColor;
    else if ((double) available == 0.0)
      color = this.UnavailableColor;
    if (this.QuantityLabel.color != color)
      this.QuantityLabel.color = color;
    if (!(this.NameLabel.color != color))
      return;
    this.NameLabel.color = color;
  }

  private string OnToolTip()
  {
    float available;
    float total;
    float reserved;
    this.GetAmounts(true, out available, out total, out reserved);
    return this.NameLabel.text + "\n" + string.Format((string) STRINGS.UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, (object) ResourceCategoryScreen.QuantityTextForMeasure(available, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(reserved, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(total, this.Measure));
  }

  public void SetName(string name) => this.NameLabel.text = name;

  public void SetTag(Tag t, GameUtil.MeasureUnit measure)
  {
    this.Resource = t;
    this.Measure = measure;
    this.cachedPickupables = (List<Pickupable>) null;
  }

  private void Hover(bool is_hovering)
  {
    if ((UnityEngine.Object) WorldInventory.Instance == (UnityEngine.Object) null)
      return;
    if (is_hovering)
      this.Background.color = this.BackgroundHoverColor;
    else
      this.Background.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    ICollection<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(this.Resource);
    if (pickupables == null)
      return;
    foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
    {
      if (!((UnityEngine.Object) pickupable == (UnityEngine.Object) null))
      {
        KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
          component.HighlightColour = !is_hovering ? (Color32) Color.black : (Color32) this.HighlightColor;
      }
    }
  }

  public void OnPointerEnter(PointerEventData eventData) => this.Hover(true);

  public void OnPointerExit(PointerEventData eventData) => this.Hover(false);

  public void SetSprite(Tag t)
  {
    Element elementByName = ElementLoader.FindElementByName(this.Resource.Name);
    if (elementByName == null)
      return;
    Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(elementByName.substance.anim);
    if (!((UnityEngine.Object) fromMultiObjectAnim != (UnityEngine.Object) null))
      return;
    this.image.sprite = fromMultiObjectAnim;
  }

  public void SetSprite(Sprite sprite) => this.image.sprite = sprite;
}
