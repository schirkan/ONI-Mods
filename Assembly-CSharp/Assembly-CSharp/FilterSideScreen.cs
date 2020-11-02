// Decompiled with JetBrains decompiler
// Type: FilterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterSideScreen : SideScreenContent
{
  public HierarchyReferences categoryFoldoutPrefab;
  public FilterSideScreenRow elementEntryPrefab;
  public RectTransform elementEntryContainer;
  public Image outputIcon;
  public Image everythingElseIcon;
  public LocText outputElementHeaderLabel;
  public LocText everythingElseHeaderLabel;
  public LocText selectElementHeaderLabel;
  public LocText currentSelectionLabel;
  private static TagNameComparer comparer = new TagNameComparer(GameTags.Void);
  public Dictionary<Tag, HierarchyReferences> categoryToggles = new Dictionary<Tag, HierarchyReferences>();
  public SortedDictionary<Tag, SortedDictionary<Tag, FilterSideScreenRow>> filterRowMap = new SortedDictionary<Tag, SortedDictionary<Tag, FilterSideScreenRow>>((IComparer<Tag>) FilterSideScreen.comparer);
  public bool isLogicFilter;
  private Filterable targetFilterable;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.filterRowMap.Clear();
  }

  public override bool IsValidForTarget(GameObject target) => (!this.isLogicFilter ? (UnityEngine.Object) target.GetComponent<ElementFilter>() != (UnityEngine.Object) null : (UnityEngine.Object) target.GetComponent<ConduitElementSensor>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<LogicElementSensor>() != (UnityEngine.Object) null) && (UnityEngine.Object) target.GetComponent<Filterable>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetFilterable = target.GetComponent<Filterable>();
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    switch (this.targetFilterable.filterElementState)
    {
      case Filterable.ElementState.Solid:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
        break;
      case Filterable.ElementState.Gas:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
        break;
      default:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
        break;
    }
    this.Configure(this.targetFilterable);
    this.SetFilterTag(this.targetFilterable.SelectedTag);
  }

  private void ToggleCategory(Tag tag, bool forceOn = false)
  {
    HierarchyReferences categoryToggle = this.categoryToggles[tag];
    if (!((UnityEngine.Object) categoryToggle != (UnityEngine.Object) null))
      return;
    MultiToggle reference = categoryToggle.GetReference<MultiToggle>("Toggle");
    if (!forceOn)
      reference.NextState();
    else
      reference.ChangeState(1);
    categoryToggle.GetReference<RectTransform>("Entries").gameObject.SetActive((uint) reference.CurrentState > 0U);
  }

  private void Configure(Filterable filterable)
  {
    Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
    foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in tagOptions)
    {
      KeyValuePair<Tag, HashSet<Tag>> category_tags = keyValuePair;
      if (!this.filterRowMap.ContainsKey(category_tags.Key))
      {
        if (category_tags.Key != GameTags.Void)
        {
          HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.categoryFoldoutPrefab.gameObject, this.elementEntryContainer.gameObject);
          hierarchyReferences.GetReference<LocText>("Label").text = category_tags.Key.ProperName();
          hierarchyReferences.GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() => this.ToggleCategory(category_tags.Key));
          this.categoryToggles.Add(category_tags.Key, hierarchyReferences);
        }
        this.filterRowMap[category_tags.Key] = new SortedDictionary<Tag, FilterSideScreenRow>((IComparer<Tag>) FilterSideScreen.comparer);
      }
      else if (category_tags.Key == GameTags.Void && !this.filterRowMap.ContainsKey(category_tags.Key))
        this.filterRowMap[category_tags.Key] = new SortedDictionary<Tag, FilterSideScreenRow>((IComparer<Tag>) FilterSideScreen.comparer);
      foreach (Tag tag in category_tags.Value)
      {
        if (!this.filterRowMap[category_tags.Key].ContainsKey(tag))
        {
          FilterSideScreenRow row = Util.KInstantiateUI<FilterSideScreenRow>(this.elementEntryPrefab.gameObject, (category_tags.Key != GameTags.Void ? this.categoryToggles[category_tags.Key].GetReference<RectTransform>("Entries") : this.elementEntryContainer).gameObject);
          row.SetTag(tag);
          row.button.onClick += (System.Action) (() => this.SetFilterTag(row.tag));
          this.filterRowMap[category_tags.Key].Add(row.tag, row);
        }
      }
    }
    int num1 = 0;
    Transform transform = this.filterRowMap[GameTags.Void][GameTags.Void].transform;
    int index = num1;
    int num2 = index + 1;
    transform.SetSiblingIndex(index);
    foreach (KeyValuePair<Tag, SortedDictionary<Tag, FilterSideScreenRow>> filterRow in this.filterRowMap)
    {
      if (tagOptions.ContainsKey(filterRow.Key) && tagOptions[filterRow.Key].Count > 0)
      {
        if (filterRow.Key != GameTags.Void)
        {
          this.categoryToggles[filterRow.Key].name = "CATE " + num2.ToString();
          this.categoryToggles[filterRow.Key].transform.SetSiblingIndex(num2++);
          this.categoryToggles[filterRow.Key].gameObject.SetActive(true);
        }
        int num3 = 0;
        foreach (KeyValuePair<Tag, FilterSideScreenRow> keyValuePair in filterRow.Value)
        {
          keyValuePair.Value.name = "ELE " + (object) num3;
          keyValuePair.Value.transform.SetSiblingIndex(num3++);
          keyValuePair.Value.gameObject.SetActive(tagOptions[filterRow.Key].Contains(keyValuePair.Value.tag));
          if (keyValuePair.Key != GameTags.Void && keyValuePair.Key == this.targetFilterable.SelectedTag)
            this.ToggleCategory(filterRow.Key, true);
        }
      }
      else if (filterRow.Key != GameTags.Void)
        this.categoryToggles[filterRow.Key].gameObject.SetActive(false);
    }
    this.RefreshUI();
  }

  private void SetFilterTag(Tag tag)
  {
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    if (tag.IsValid)
      this.targetFilterable.SelectedTag = tag;
    this.RefreshUI();
  }

  private void RefreshUI()
  {
    LocString locString;
    switch (this.targetFilterable.filterElementState)
    {
      case Filterable.ElementState.Solid:
        locString = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
        break;
      case Filterable.ElementState.Gas:
        locString = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
        break;
      default:
        locString = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
        break;
    }
    this.currentSelectionLabel.text = string.Format((string) locString, (object) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
    foreach (KeyValuePair<Tag, SortedDictionary<Tag, FilterSideScreenRow>> filterRow in this.filterRowMap)
    {
      foreach (KeyValuePair<Tag, FilterSideScreenRow> keyValuePair in filterRow.Value)
      {
        bool selected = keyValuePair.Key == this.targetFilterable.SelectedTag;
        keyValuePair.Value.SetSelected(selected);
        if (selected)
        {
          if (keyValuePair.Value.tag != GameTags.Void)
            this.currentSelectionLabel.text = string.Format((string) locString, (object) this.targetFilterable.SelectedTag.ProperName());
          else
            this.currentSelectionLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
        }
      }
    }
  }
}
