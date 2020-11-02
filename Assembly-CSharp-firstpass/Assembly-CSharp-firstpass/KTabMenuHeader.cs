// Decompiled with JetBrains decompiler
// Type: KTabMenuHeader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/Plugins/KTabMenuHeader")]
public class KTabMenuHeader : KMonoBehaviour
{
  [SerializeField]
  private RectTransform prefab;
  public TextStyleSetting TextStyle_Active;
  public TextStyleSetting TextStyle_Inactive;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ActivateTabArtwork(0);
  }

  public void Add(string name, KTabMenuHeader.OnClick onClick, int id)
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab.gameObject);
    gameObject.SetActive(true);
    RectTransform component = gameObject.GetComponent<RectTransform>();
    component.transform.SetParent(this.transform, false);
    component.name = name;
    Text componentInChildren = component.GetComponentInChildren<Text>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.text = name.ToUpper();
    this.ActivateTabArtwork(id);
    gameObject.GetComponent<KButton>().onClick += (System.Action) (() => onClick(id));
  }

  public void Add(
    Sprite icon,
    string name,
    KTabMenuHeader.OnClick onClick,
    int id,
    string tooltip = "")
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab.gameObject);
    RectTransform component1 = gameObject.GetComponent<RectTransform>();
    component1.transform.SetParent(this.transform, false);
    component1.name = name;
    if (tooltip == "")
      component1.GetComponent<ToolTip>().toolTip = name;
    else
      component1.GetComponent<ToolTip>().toolTip = tooltip;
    this.ActivateTabArtwork(id);
    TabHeaderIcon componentInChildren = component1.GetComponentInChildren<TabHeaderIcon>();
    if ((bool) (UnityEngine.Object) componentInChildren)
      componentInChildren.TitleText.text = name;
    KToggle component2 = gameObject.GetComponent<KToggle>();
    if ((bool) (UnityEngine.Object) component2 && (bool) (UnityEngine.Object) component2.fgImage)
      component2.fgImage.sprite = icon;
    component2.group = this.GetComponent<ToggleGroup>();
    component2.onClick += (System.Action) (() => onClick(id));
  }

  public void Activate(int itemIdx, int previouslyActiveTabIdx)
  {
    int childCount = this.transform.childCount;
    if (itemIdx >= childCount)
      return;
    for (int index = 0; index < childCount; ++index)
    {
      Transform child = this.transform.GetChild(index);
      if (child.gameObject.activeSelf)
      {
        KButton componentInChildren = child.GetComponentInChildren<KButton>();
        if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null && (UnityEngine.Object) componentInChildren.GetComponentInChildren<Text>() != (UnityEngine.Object) null && index == itemIdx)
          this.ActivateTabArtwork(itemIdx);
        KToggle component = child.GetComponent<KToggle>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          this.ActivateTabArtwork(itemIdx);
          if (index == itemIdx)
            component.Select();
          else
            component.Deselect();
        }
      }
    }
  }

  public void SetTabEnabled(int tabIdx, bool enabled)
  {
    if (tabIdx >= this.transform.childCount)
      return;
    this.transform.GetChild(tabIdx).gameObject.SetActive(enabled);
  }

  public virtual void ActivateTabArtwork(int tabIdx)
  {
    if (tabIdx >= this.transform.childCount)
      return;
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      ImageToggleState component = this.transform.GetChild(index).GetComponent<ImageToggleState>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (index == tabIdx)
          component.SetActive();
        else
          component.SetInactive();
      }
      Canvas componentInChildren1 = this.transform.GetChild(index).GetComponentInChildren<Canvas>(true);
      if ((UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) null)
        componentInChildren1.overrideSorting = tabIdx == index;
      SetTextStyleSetting componentInChildren2 = this.transform.GetChild(index).GetComponentInChildren<SetTextStyleSetting>();
      if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null && (UnityEngine.Object) this.TextStyle_Active != (UnityEngine.Object) null && (UnityEngine.Object) this.TextStyle_Inactive != (UnityEngine.Object) null)
      {
        if (index == tabIdx)
          componentInChildren2.SetStyle(this.TextStyle_Active);
        else
          componentInChildren2.SetStyle(this.TextStyle_Inactive);
      }
    }
  }

  public delegate void OnClick(int id);
}
