// Decompiled with JetBrains decompiler
// Type: TextLinkHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
  private static TextLinkHandler hoveredText;
  [MyCmpGet]
  private LocText text;
  private bool hoverLink;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || !this.text.AllowLinks)
      return;
    int intersectingLink = TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null);
    if (intersectingLink == -1)
      return;
    string str = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[intersectingLink].GetLinkID());
    if (!CodexCache.entries.ContainsKey(str))
    {
      SubEntry subEntry = CodexCache.FindSubEntry(str);
      if (subEntry == null || subEntry.disabled)
        str = "PAGENOTFOUND";
    }
    else if (CodexCache.entries[str].disabled)
      str = "PAGENOTFOUND";
    if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
      ManagementMenu.Instance.ToggleCodex();
    ManagementMenu.Instance.codexScreen.ChangeArticle(str, true);
  }

  private void Update()
  {
    this.CheckMouseOver();
    if (!((Object) TextLinkHandler.hoveredText == (Object) this) || !this.text.AllowLinks)
      return;
    PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
  }

  private void OnEnable() => this.CheckMouseOver();

  private void OnDisable() => this.ClearState();

  private void Awake()
  {
    this.text = this.GetComponent<LocText>();
    if (!this.text.AllowLinks || this.text.raycastTarget)
      return;
    this.text.raycastTarget = true;
  }

  public void OnPointerEnter(PointerEventData eventData) => this.SetMouseOver();

  public void OnPointerExit(PointerEventData eventData) => this.ClearState();

  private void ClearState()
  {
    if ((Object) this == (Object) null || this.Equals((object) null) || !((Object) TextLinkHandler.hoveredText == (Object) this))
      return;
    if (this.hoverLink && (Object) PlayerController.Instance != (Object) null && (Object) PlayerController.Instance.ActiveTool != (Object) null)
      PlayerController.Instance.ActiveTool.SetLinkCursor(false);
    TextLinkHandler.hoveredText = (TextLinkHandler) null;
    this.hoverLink = false;
  }

  public void CheckMouseOver()
  {
    if ((Object) this.text == (Object) null)
      return;
    if (TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null) != -1)
    {
      this.SetMouseOver();
      this.hoverLink = true;
    }
    else
    {
      if (!((Object) TextLinkHandler.hoveredText == (Object) this))
        return;
      this.hoverLink = false;
    }
  }

  private void SetMouseOver()
  {
    if ((Object) TextLinkHandler.hoveredText != (Object) null && (Object) TextLinkHandler.hoveredText != (Object) this)
      TextLinkHandler.hoveredText.hoverLink = false;
    TextLinkHandler.hoveredText = this;
  }
}
