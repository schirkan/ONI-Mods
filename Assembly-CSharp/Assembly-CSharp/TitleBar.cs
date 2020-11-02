// Decompiled with JetBrains decompiler
// Type: TitleBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
  public LocText titleText;
  public LocText subtextText;
  public GameObject WarningNotification;
  public Text NotificationText;
  public Image NotificationIcon;
  public Sprite techIcon;
  public Sprite materialIcon;
  public TitleBarPortrait portrait;
  public bool userEditable;
  public bool setCameraControllerState = true;

  public void SetTitle(string Name) => this.titleText.text = Name;

  public void SetSubText(string subtext, string tooltip = "")
  {
    this.subtextText.text = subtext;
    this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
  }

  public void SetWarningActve(bool state) => this.WarningNotification.SetActive(state);

  public void SetWarning(Sprite icon, string label)
  {
    this.SetWarningActve(true);
    this.NotificationIcon.sprite = icon;
    this.NotificationText.text = label;
  }

  public void SetPortrait(GameObject target) => this.portrait.SetPortrait(target);
}
