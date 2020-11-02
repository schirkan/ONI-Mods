// Decompiled with JetBrains decompiler
// Type: CodexScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CodexScreen : KScreen
{
  private string _activeEntryID;
  private Dictionary<System.Type, UIGameObjectPool> ContentUIPools = new Dictionary<System.Type, UIGameObjectPool>();
  private Dictionary<System.Type, GameObject> ContentPrefabs = new Dictionary<System.Type, GameObject>();
  private List<GameObject> categoryHeaders = new List<GameObject>();
  private Dictionary<CodexEntry, GameObject> entryButtons = new Dictionary<CodexEntry, GameObject>();
  private Dictionary<SubEntry, GameObject> subEntryButtons = new Dictionary<SubEntry, GameObject>();
  private UIGameObjectPool contentContainerPool;
  [SerializeField]
  private KScrollRect displayScrollRect;
  [SerializeField]
  private RectTransform scrollContentPane;
  private bool editingSearch;
  private List<CodexScreen.HistoryEntry> history = new List<CodexScreen.HistoryEntry>();
  private int currentHistoryIdx;
  [Header("Hierarchy")]
  [SerializeField]
  private Transform navigatorContent;
  [SerializeField]
  private Transform displayPane;
  [SerializeField]
  private Transform contentContainers;
  [SerializeField]
  private Transform widgetPool;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private TMP_InputField searchInputField;
  [SerializeField]
  private KButton clearSearchButton;
  [SerializeField]
  private LocText backButton;
  [SerializeField]
  private KButton backButtonButton;
  [SerializeField]
  private KButton fwdButtonButton;
  [SerializeField]
  private LocText currentLocationText;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject prefabNavigatorEntry;
  [SerializeField]
  private GameObject prefabCategoryHeader;
  [SerializeField]
  private GameObject prefabContentContainer;
  [SerializeField]
  private GameObject prefabTextWidget;
  [SerializeField]
  private GameObject prefabTextWithTooltipWidget;
  [SerializeField]
  private GameObject prefabImageWidget;
  [SerializeField]
  private GameObject prefabDividerLineWidget;
  [SerializeField]
  private GameObject prefabSpacer;
  [SerializeField]
  private GameObject prefabLargeSpacer;
  [SerializeField]
  private GameObject prefabLabelWithIcon;
  [SerializeField]
  private GameObject prefabLabelWithLargeIcon;
  [SerializeField]
  private GameObject prefabContentLocked;
  [SerializeField]
  private GameObject prefabVideoWidget;
  [SerializeField]
  private GameObject prefabIndentedLabelWithIcon;
  [SerializeField]
  private GameObject prefabRecipePanel;
  [Header("Text Styles")]
  [SerializeField]
  private TextStyleSetting textStyleTitle;
  [SerializeField]
  private TextStyleSetting textStyleSubtitle;
  [SerializeField]
  private TextStyleSetting textStyleBody;
  [SerializeField]
  private TextStyleSetting textStyleBodyWhite;
  private Dictionary<CodexTextStyle, TextStyleSetting> textStyles = new Dictionary<CodexTextStyle, TextStyleSetting>();
  private List<CodexEntry> searchResults = new List<CodexEntry>();
  private List<SubEntry> subEntrySearchResults = new List<SubEntry>();
  private Coroutine scrollToTargetRoutine;

  public string activeEntryID
  {
    get => this._activeEntryID;
    private set => this._activeEntryID = value;
  }

  protected override void OnActivate()
  {
    this.ConsumeMouseScroll = true;
    base.OnActivate();
    this.closeButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.clearSearchButton.onClick += (System.Action) (() => this.searchInputField.text = "");
    if (string.IsNullOrEmpty(this.activeEntryID))
      this.ChangeArticle("HOME");
    this.searchInputField.onValueChanged.AddListener((UnityAction<string>) (value => this.FilterSearch(value)));
    this.searchInputField.onFocus += (System.Action) (() => this.editingSearch = true);
    this.searchInputField.onEndEdit.AddListener((UnityAction<string>) (value => this.editingSearch = false));
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.editingSearch)
      e.Consumed = true;
    base.OnKeyDown(e);
  }

  public override float GetSortKey() => 10000f;

  private void CodexScreenInit()
  {
    this.textStyles[CodexTextStyle.Title] = this.textStyleTitle;
    this.textStyles[CodexTextStyle.Subtitle] = this.textStyleSubtitle;
    this.textStyles[CodexTextStyle.Body] = this.textStyleBody;
    this.textStyles[CodexTextStyle.BodyWhite] = this.textStyleBodyWhite;
    this.SetupPrefabs();
    this.PopulatePools();
    this.CategorizeEntries();
    this.FilterSearch("");
    this.backButtonButton.onClick += new System.Action(this.HistoryStepBack);
    this.backButtonButton.soundPlayer.AcceptClickCondition = (Func<bool>) (() => this.currentHistoryIdx > 0);
    this.fwdButtonButton.onClick += new System.Action(this.HistoryStepForward);
    this.fwdButtonButton.soundPlayer.AcceptClickCondition = (Func<bool>) (() => this.currentHistoryIdx < this.history.Count - 1);
    Game.Instance.Subscribe(1594320620, (System.Action<object>) (val =>
    {
      if (!this.gameObject.activeSelf)
        return;
      this.FilterSearch(this.searchInputField.text);
      if (string.IsNullOrEmpty(this.activeEntryID))
        return;
      this.ChangeArticle(this.activeEntryID);
    }));
  }

  private void SetupPrefabs()
  {
    this.contentContainerPool = new UIGameObjectPool(this.prefabContentContainer);
    this.contentContainerPool.disabledElementParent = this.widgetPool;
    this.ContentPrefabs[typeof (CodexText)] = this.prefabTextWidget;
    this.ContentPrefabs[typeof (CodexTextWithTooltip)] = this.prefabTextWithTooltipWidget;
    this.ContentPrefabs[typeof (CodexImage)] = this.prefabImageWidget;
    this.ContentPrefabs[typeof (CodexDividerLine)] = this.prefabDividerLineWidget;
    this.ContentPrefabs[typeof (CodexSpacer)] = this.prefabSpacer;
    this.ContentPrefabs[typeof (CodexLabelWithIcon)] = this.prefabLabelWithIcon;
    this.ContentPrefabs[typeof (CodexLabelWithLargeIcon)] = this.prefabLabelWithLargeIcon;
    this.ContentPrefabs[typeof (CodexContentLockedIndicator)] = this.prefabContentLocked;
    this.ContentPrefabs[typeof (CodexLargeSpacer)] = this.prefabLargeSpacer;
    this.ContentPrefabs[typeof (CodexVideo)] = this.prefabVideoWidget;
    this.ContentPrefabs[typeof (CodexIndentedLabelWithIcon)] = this.prefabIndentedLabelWithIcon;
    this.ContentPrefabs[typeof (CodexRecipePanel)] = this.prefabRecipePanel;
  }

  private List<CodexEntry> FilterSearch(string input)
  {
    this.searchResults.Clear();
    this.subEntrySearchResults.Clear();
    input = input.ToLower();
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      if (input == "")
      {
        if (!entry.Value.searchOnly)
          this.searchResults.Add(entry.Value);
      }
      else if (input == entry.Value.name.ToLower() || input.Contains(entry.Value.name.ToLower()) || entry.Value.name.ToLower().Contains(input))
        this.searchResults.Add(entry.Value);
    }
    foreach (KeyValuePair<string, SubEntry> subEntry in CodexCache.subEntries)
    {
      if (input == subEntry.Value.name.ToLower() || input.Contains(subEntry.Value.name.ToLower()) || subEntry.Value.name.ToLower().Contains(input))
        this.subEntrySearchResults.Add(subEntry.Value);
    }
    this.FilterEntries(input != "");
    return this.searchResults;
  }

  private bool HasUnlockedCategoryEntries(string entryID)
  {
    foreach (ContentContainer contentContainer in CodexCache.entries[entryID].contentContainers)
    {
      if (string.IsNullOrEmpty(contentContainer.lockID) || Game.Instance.unlocks.IsUnlocked(contentContainer.lockID))
        return true;
    }
    return false;
  }

  private void FilterEntries(bool allowOpenCategories = true)
  {
    foreach (KeyValuePair<CodexEntry, GameObject> entryButton in this.entryButtons)
      entryButton.Value.SetActive(this.searchResults.Contains(entryButton.Key) && this.HasUnlockedCategoryEntries(entryButton.Key.id));
    foreach (KeyValuePair<SubEntry, GameObject> subEntryButton in this.subEntryButtons)
      subEntryButton.Value.SetActive(this.subEntrySearchResults.Contains(subEntryButton.Key));
    foreach (GameObject categoryHeader in this.categoryHeaders)
    {
      bool flag = false;
      Transform transform = categoryHeader.transform.Find("Content");
      for (int index = 0; index < transform.childCount; ++index)
      {
        if (transform.GetChild(index).gameObject.activeSelf)
          flag = true;
      }
      categoryHeader.SetActive(flag);
      if (allowOpenCategories)
      {
        if (flag)
          this.ToggleCategoryOpen(categoryHeader, true);
      }
      else
        this.ToggleCategoryOpen(categoryHeader, false);
    }
  }

  private void ToggleCategoryOpen(GameObject header, bool open)
  {
    header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
    header.GetComponent<HierarchyReferences>().GetReference("Content").gameObject.SetActive(open);
  }

  private void PopulatePools()
  {
    foreach (KeyValuePair<System.Type, GameObject> contentPrefab in this.ContentPrefabs)
      this.ContentUIPools[contentPrefab.Key] = new UIGameObjectPool(contentPrefab.Value)
      {
        disabledElementParent = this.widgetPool
      };
  }

  private GameObject NewCategoryHeader(
    KeyValuePair<string, CodexEntry> entryKVP,
    Dictionary<string, GameObject> categories)
  {
    if (entryKVP.Value.category == "")
      entryKVP.Value.category = "Root";
    GameObject categoryHeader = Util.KInstantiateUI(this.prefabCategoryHeader, this.navigatorContent.gameObject, true);
    GameObject categoryContent = categoryHeader.GetComponent<HierarchyReferences>().GetReference("Content").gameObject;
    categories.Add(entryKVP.Value.category, categoryContent);
    LocText reference = categoryHeader.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
    if (CodexCache.entries.ContainsKey(entryKVP.Value.category))
      reference.text = CodexCache.entries[entryKVP.Value.category].name;
    else
      reference.text = (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES." + entryKVP.Value.category.ToUpper());
    this.categoryHeaders.Add(categoryHeader);
    categoryContent.SetActive(false);
    categoryHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").onClick = (System.Action) (() => this.ToggleCategoryOpen(categoryHeader, !categoryContent.activeSelf));
    return categoryHeader;
  }

  private void CategorizeEntries()
  {
    GameObject gameObject1 = this.navigatorContent.gameObject;
    Dictionary<string, GameObject> categories = new Dictionary<string, GameObject>();
    List<Tuple<string, CodexEntry>> tupleList = new List<Tuple<string, CodexEntry>>();
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      if (string.IsNullOrEmpty(entry.Value.sortString))
        entry.Value.sortString = STRINGS.UI.StripLinkFormatting((string) Strings.Get(entry.Value.title));
      tupleList.Add(new Tuple<string, CodexEntry>(entry.Key, entry.Value));
    }
    tupleList.Sort((Comparison<Tuple<string, CodexEntry>>) ((a, b) => string.Compare(a.second.sortString, b.second.sortString)));
    for (int index = 0; index < tupleList.Count; ++index)
    {
      Tuple<string, CodexEntry> tuple = tupleList[index];
      string key = tuple.second.category;
      if (key == "" || key == "Root")
        key = "Root";
      if (!categories.ContainsKey(key))
        this.NewCategoryHeader(new KeyValuePair<string, CodexEntry>(tuple.first, tuple.second), categories);
      GameObject gameObject2 = Util.KInstantiateUI(this.prefabNavigatorEntry, categories[key], true);
      string id = tuple.second.id;
      gameObject2.GetComponent<KButton>().onClick += (System.Action) (() => this.ChangeArticle(id));
      if (string.IsNullOrEmpty(tuple.second.name))
        tuple.second.name = (string) Strings.Get(tuple.second.title);
      gameObject2.GetComponentInChildren<LocText>().text = tuple.second.name;
      this.entryButtons.Add(tuple.second, gameObject2);
      foreach (SubEntry subEntry in tuple.second.subEntries)
      {
        GameObject gameObject3 = Util.KInstantiateUI(this.prefabNavigatorEntry, categories[key], true);
        string subEntryId = subEntry.id;
        gameObject3.GetComponent<KButton>().onClick += (System.Action) (() => this.ChangeArticle(subEntryId));
        if (string.IsNullOrEmpty(subEntry.name))
          subEntry.name = (string) Strings.Get(subEntry.title);
        gameObject3.GetComponentInChildren<LocText>().text = subEntry.name;
        this.subEntryButtons.Add(subEntry, gameObject3);
        CodexCache.subEntries.Add(subEntry.id, subEntry);
      }
    }
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      if (CodexCache.entries.ContainsKey(entry.Value.category) && CodexCache.entries.ContainsKey(CodexCache.entries[entry.Value.category].category))
        entry.Value.searchOnly = true;
    }
    List<KeyValuePair<string, GameObject>> keyValuePairList = new List<KeyValuePair<string, GameObject>>();
    foreach (KeyValuePair<string, GameObject> keyValuePair in categories)
      keyValuePairList.Add(keyValuePair);
    keyValuePairList.Sort((Comparison<KeyValuePair<string, GameObject>>) ((a, b) => string.Compare(a.Value.name, b.Value.name)));
    for (int index = 0; index < keyValuePairList.Count; ++index)
      keyValuePairList[index].Value.transform.parent.SetSiblingIndex(index);
    CodexScreen.SetupCategory(categories, "PLANTS");
    CodexScreen.SetupCategory(categories, "CREATURES");
    CodexScreen.SetupCategory(categories, "NOTICES");
    CodexScreen.SetupCategory(categories, "RESEARCHNOTES");
    CodexScreen.SetupCategory(categories, "JOURNALS");
    CodexScreen.SetupCategory(categories, "EMAILS");
    CodexScreen.SetupCategory(categories, "INVESTIGATIONS");
    CodexScreen.SetupCategory(categories, "MYLOG");
    CodexScreen.SetupCategory(categories, "LESSONS");
    CodexScreen.SetupCategory(categories, "Root");
  }

  private static void SetupCategory(Dictionary<string, GameObject> categories, string category_name)
  {
    if (!categories.ContainsKey(category_name))
      return;
    categories[category_name].transform.parent.SetAsFirstSibling();
  }

  public void ChangeArticle(
    string id,
    bool playClickSound = false,
    Vector3 targetPosition = default (Vector3),
    CodexScreen.HistoryDirection historyMovement = CodexScreen.HistoryDirection.NewArticle)
  {
    Debug.Assert(id != null);
    if (playClickSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
    if (this.contentContainerPool == null)
      this.CodexScreenInit();
    string articleName = "";
    SubEntry subEntry = (SubEntry) null;
    if (!CodexCache.entries.ContainsKey(id))
    {
      subEntry = CodexCache.FindSubEntry(id);
      if (subEntry != null && !subEntry.disabled)
      {
        id = subEntry.parentEntryID.ToUpper();
        articleName = STRINGS.UI.StripLinkFormatting(subEntry.name);
      }
      else
        id = "PAGENOTFOUND";
    }
    if (CodexCache.entries[id].disabled)
      id = "PAGENOTFOUND";
    if (string.IsNullOrEmpty(articleName))
      articleName = STRINGS.UI.StripLinkFormatting(CodexCache.entries[id].name);
    ICodexWidget codexWidget1 = (ICodexWidget) null;
    CodexCache.entries[id].GetFirstWidget();
    RectTransform targetWidgetTransform = (RectTransform) null;
    if (subEntry != null)
    {
      foreach (ContentContainer contentContainer in CodexCache.entries[id].contentContainers)
      {
        if (contentContainer == subEntry.contentContainers[0])
        {
          codexWidget1 = contentContainer.content[0];
          break;
        }
      }
    }
    int index1 = 0;
    string str1 = "";
    while (this.contentContainers.transform.childCount > 0)
    {
      while (!string.IsNullOrEmpty(str1) && CodexCache.entries[this.activeEntryID].contentContainers[index1].lockID == str1)
        ++index1;
      GameObject gameObject1 = this.contentContainers.transform.GetChild(0).gameObject;
      int index2 = 0;
      while (gameObject1.transform.childCount > 0)
      {
        GameObject gameObject2 = gameObject1.transform.GetChild(0).gameObject;
        System.Type key;
        if (gameObject2.name == "PrefabContentLocked")
        {
          str1 = CodexCache.entries[this.activeEntryID].contentContainers[index1].lockID;
          key = typeof (CodexContentLockedIndicator);
        }
        else
          key = CodexCache.entries[this.activeEntryID].contentContainers[index1].content[index2].GetType();
        this.ContentUIPools[key].ClearElement(gameObject2);
        ++index2;
      }
      this.contentContainerPool.ClearElement(this.contentContainers.transform.GetChild(0).gameObject);
      ++index1;
    }
    bool flag1 = CodexCache.entries[id] is CategoryEntry;
    this.activeEntryID = id;
    if (CodexCache.entries[id].contentContainers == null)
      CodexCache.entries[id].CreateContentContainerCollection();
    bool flag2 = false;
    string str2 = "";
    for (int index2 = 0; index2 < CodexCache.entries[id].contentContainers.Count; ++index2)
    {
      ContentContainer contentContainer = CodexCache.entries[id].contentContainers[index2];
      if (!string.IsNullOrEmpty(contentContainer.lockID) && !Game.Instance.unlocks.IsUnlocked(contentContainer.lockID))
      {
        if (str2 != contentContainer.lockID)
        {
          GameObject gameObject1 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
          this.ConfigureContentContainer(contentContainer, gameObject1, flag1 & flag2);
          str2 = contentContainer.lockID;
          GameObject gameObject2 = this.ContentUIPools[typeof (CodexContentLockedIndicator)].GetFreeElement(gameObject1, true).gameObject;
        }
      }
      else
      {
        GameObject gameObject1 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
        this.ConfigureContentContainer(contentContainer, gameObject1, flag1 & flag2);
        flag2 = !flag2;
        if (contentContainer.content != null)
        {
          foreach (ICodexWidget codexWidget2 in contentContainer.content)
          {
            GameObject gameObject2 = this.ContentUIPools[codexWidget2.GetType()].GetFreeElement(gameObject1, true).gameObject;
            codexWidget2.Configure(gameObject2, this.displayPane, this.textStyles);
            if (codexWidget2 == codexWidget1)
              targetWidgetTransform = gameObject2.rectTransform();
          }
        }
      }
    }
    string str3 = "";
    string key1 = id;
    int num = 0;
    while (key1 != CodexCache.FormatLinkID("HOME") && num < 10)
    {
      ++num;
      if (key1 != null)
      {
        str3 = !(key1 != id) ? str3.Insert(0, CodexCache.entries[key1].name) : str3.Insert(0, CodexCache.entries[key1].name + " > ");
        key1 = CodexCache.entries[key1].parentId;
      }
      else
      {
        key1 = CodexCache.entries[CodexCache.FormatLinkID("HOME")].id;
        str3 = str3.Insert(0, CodexCache.entries[key1].name + " > ");
      }
    }
    this.currentLocationText.text = str3 == "" ? "<b>" + STRINGS.UI.StripLinkFormatting(CodexCache.entries["HOME"].name) + "</b>" : str3;
    if (this.history.Count == 0)
    {
      this.history.Add(new CodexScreen.HistoryEntry(id, Vector3.zero, articleName));
      this.currentHistoryIdx = 0;
    }
    else
    {
      switch (historyMovement)
      {
        case CodexScreen.HistoryDirection.Back:
          this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
          --this.currentHistoryIdx;
          break;
        case CodexScreen.HistoryDirection.Forward:
          this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
          ++this.currentHistoryIdx;
          break;
        case CodexScreen.HistoryDirection.Up:
        case CodexScreen.HistoryDirection.NewArticle:
          if (this.currentHistoryIdx == this.history.Count - 1)
          {
            this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, articleName));
            this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
            ++this.currentHistoryIdx;
            break;
          }
          for (int index2 = this.history.Count - 1; index2 > this.currentHistoryIdx; --index2)
            this.history.RemoveAt(index2);
          this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, articleName));
          this.history[this.history.Count - 2].position = this.displayPane.transform.localPosition;
          ++this.currentHistoryIdx;
          break;
      }
    }
    if (this.currentHistoryIdx > 0)
    {
      this.backButtonButton.GetComponent<Image>().color = Color.black;
      this.backButton.text = STRINGS.UI.FormatAsLink(string.Format((string) STRINGS.UI.CODEX.BACK_BUTTON, (object) STRINGS.UI.StripLinkFormatting(CodexCache.entries[this.history[this.history.Count - 2].id].name)), CodexCache.entries[this.history[this.history.Count - 2].id].id);
      this.backButtonButton.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.CODEX.BACK_BUTTON_TOOLTIP, (object) this.history[this.currentHistoryIdx - 1].name);
    }
    else
    {
      this.backButtonButton.GetComponent<Image>().color = Color.grey;
      this.backButton.text = STRINGS.UI.StripLinkFormatting(GameUtil.ColourizeString((Color32) Color.grey, string.Format((string) STRINGS.UI.CODEX.BACK_BUTTON, (object) CodexCache.entries["HOME"].name)));
      this.backButtonButton.GetComponent<ToolTip>().toolTip = (string) STRINGS.UI.CODEX.BACK_BUTTON_NO_HISTORY_TOOLTIP;
    }
    if (this.currentHistoryIdx < this.history.Count - 1)
    {
      this.fwdButtonButton.GetComponent<Image>().color = Color.black;
      this.fwdButtonButton.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.CODEX.FORWARD_BUTTON_TOOLTIP, (object) this.history[this.currentHistoryIdx + 1].name);
    }
    else
    {
      this.fwdButtonButton.GetComponent<Image>().color = Color.grey;
      this.fwdButtonButton.GetComponent<ToolTip>().toolTip = (string) STRINGS.UI.CODEX.FORWARD_BUTTON_NO_HISTORY_TOOLTIP;
    }
    if (targetPosition != Vector3.zero)
    {
      if (this.scrollToTargetRoutine != null)
        this.StopCoroutine(this.scrollToTargetRoutine);
      this.scrollToTargetRoutine = this.StartCoroutine(this.ScrollToTarget(targetPosition));
    }
    else if ((UnityEngine.Object) targetWidgetTransform != (UnityEngine.Object) null)
    {
      if (this.scrollToTargetRoutine != null)
        this.StopCoroutine(this.scrollToTargetRoutine);
      this.scrollToTargetRoutine = this.StartCoroutine(this.ScrollToTarget(targetWidgetTransform));
    }
    else
      this.displayScrollRect.content.SetLocalPosition(Vector3.zero);
  }

  private void HistoryStepBack()
  {
    if (this.currentHistoryIdx == 0)
      return;
    this.ChangeArticle(this.history[this.currentHistoryIdx - 1].id, targetPosition: this.history[this.currentHistoryIdx - 1].position, historyMovement: CodexScreen.HistoryDirection.Back);
  }

  private void HistoryStepForward()
  {
    if (this.currentHistoryIdx == this.history.Count - 1)
      return;
    this.ChangeArticle(this.history[this.currentHistoryIdx + 1].id, targetPosition: this.history[this.currentHistoryIdx + 1].position, historyMovement: CodexScreen.HistoryDirection.Forward);
  }

  private void HistoryStepUp()
  {
    if (string.IsNullOrEmpty(CodexCache.entries[this.activeEntryID].parentId))
      return;
    this.ChangeArticle(CodexCache.entries[this.activeEntryID].parentId, historyMovement: CodexScreen.HistoryDirection.Up);
  }

  private IEnumerator ScrollToTarget(RectTransform targetWidgetTransform)
  {
    yield return (object) 0;
    this.displayScrollRect.content.SetLocalPosition(Vector3.down * (this.displayScrollRect.content.InverseTransformPoint(targetWidgetTransform.GetPosition()).y + 12f));
  }

  private IEnumerator ScrollToTarget(Vector3 position)
  {
    yield return (object) 0;
    this.displayScrollRect.content.SetLocalPosition(position);
  }

  private void ConfigureContentContainer(
    ContentContainer container,
    GameObject containerGameObject,
    bool bgColor = false)
  {
    container.go = containerGameObject;
    LayoutGroup component = containerGameObject.GetComponent<LayoutGroup>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component);
    if (Game.Instance.unlocks.IsUnlocked(container.lockID) || string.IsNullOrEmpty(container.lockID))
    {
      switch (container.contentLayout)
      {
        case ContentContainer.ContentLayout.Vertical:
          LayoutGroup layoutGroup1 = (LayoutGroup) containerGameObject.AddComponent<VerticalLayoutGroup>();
          (layoutGroup1 as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = (layoutGroup1 as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false;
          (layoutGroup1 as HorizontalOrVerticalLayoutGroup).spacing = 8f;
          break;
        case ContentContainer.ContentLayout.Horizontal:
          LayoutGroup layoutGroup2 = (LayoutGroup) containerGameObject.AddComponent<HorizontalLayoutGroup>();
          layoutGroup2.childAlignment = TextAnchor.MiddleLeft;
          (layoutGroup2 as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = (layoutGroup2 as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false;
          (layoutGroup2 as HorizontalOrVerticalLayoutGroup).spacing = 8f;
          break;
        case ContentContainer.ContentLayout.Grid:
          LayoutGroup layoutGroup3 = (LayoutGroup) containerGameObject.AddComponent<GridLayoutGroup>();
          (layoutGroup3 as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
          (layoutGroup3 as GridLayoutGroup).constraintCount = 4;
          (layoutGroup3 as GridLayoutGroup).cellSize = new Vector2(128f, 180f);
          (layoutGroup3 as GridLayoutGroup).spacing = new Vector2(6f, 6f);
          break;
        case ContentContainer.ContentLayout.GridTwoColumn:
          LayoutGroup layoutGroup4 = (LayoutGroup) containerGameObject.AddComponent<GridLayoutGroup>();
          (layoutGroup4 as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
          (layoutGroup4 as GridLayoutGroup).constraintCount = 2;
          (layoutGroup4 as GridLayoutGroup).cellSize = new Vector2(264f, 32f);
          (layoutGroup4 as GridLayoutGroup).spacing = new Vector2(0.0f, 12f);
          break;
        case ContentContainer.ContentLayout.GridTwoColumnTall:
          LayoutGroup layoutGroup5 = (LayoutGroup) containerGameObject.AddComponent<GridLayoutGroup>();
          (layoutGroup5 as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
          (layoutGroup5 as GridLayoutGroup).constraintCount = 2;
          (layoutGroup5 as GridLayoutGroup).cellSize = new Vector2(264f, 64f);
          (layoutGroup5 as GridLayoutGroup).spacing = new Vector2(0.0f, 12f);
          break;
      }
    }
    else
    {
      LayoutGroup layoutGroup6 = (LayoutGroup) containerGameObject.AddComponent<VerticalLayoutGroup>();
      (layoutGroup6 as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = (layoutGroup6 as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false;
      (layoutGroup6 as HorizontalOrVerticalLayoutGroup).spacing = 8f;
    }
  }

  public enum PlanCategory
  {
    Home,
    Tips,
    MyLog,
    Investigations,
    Emails,
    Journals,
    ResearchNotes,
    Creatures,
    Plants,
    Food,
    Tech,
    Diseases,
    Roles,
    Buildings,
    Elements,
  }

  public enum HistoryDirection
  {
    Back,
    Forward,
    Up,
    NewArticle,
  }

  public class HistoryEntry
  {
    public string id;
    public Vector3 position;
    public string name;

    public HistoryEntry(string entry, Vector3 pos, string articleName)
    {
      this.id = entry;
      this.position = pos;
      this.name = articleName;
    }
  }
}
