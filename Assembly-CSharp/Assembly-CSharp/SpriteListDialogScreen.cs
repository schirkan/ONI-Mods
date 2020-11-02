// Decompiled with JetBrains decompiler
// Type: SpriteListDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteListDialogScreen : KModalScreen
{
  public System.Action onDeactivateCB;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private GameObject buttonPanel;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText popupMessage;
  [SerializeField]
  private GameObject listPanel;
  [SerializeField]
  private GameObject listPrefab;
  private List<SpriteListDialogScreen.Button> buttons;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
    this.buttons = new List<SpriteListDialogScreen.Button>();
  }

  public override bool IsModal() => true;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  public void AddOption(string text, System.Action action)
  {
    GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
    this.buttons.Add(new SpriteListDialogScreen.Button()
    {
      label = text,
      action = action,
      gameObject = gameObject
    });
  }

  public void AddSprite(Sprite sprite, string text)
  {
    GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
    gameObject.GetComponentInChildren<LocText>().text = text;
    Image componentInChildren = gameObject.GetComponentInChildren<Image>();
    componentInChildren.sprite = sprite;
    AspectRatioFitter component = componentInChildren.gameObject.GetComponent<AspectRatioFitter>();
    Rect rect = sprite.rect;
    double width = (double) rect.width;
    rect = sprite.rect;
    double height = (double) rect.height;
    double num = width / height;
    component.aspectRatio = (float) num;
  }

  public void PopupConfirmDialog(string text, string title_text = null)
  {
    foreach (SpriteListDialogScreen.Button button in this.buttons)
    {
      button.gameObject.GetComponentInChildren<LocText>().text = button.label;
      button.gameObject.GetComponent<KButton>().onClick += button.action;
    }
    if (title_text != null)
      this.titleText.text = title_text;
    this.popupMessage.text = text;
  }

  protected override void OnDeactivate()
  {
    if (this.onDeactivateCB != null)
      this.onDeactivateCB();
    base.OnDeactivate();
  }

  private struct Button
  {
    public System.Action action;
    public GameObject gameObject;
    public string label;
  }
}
