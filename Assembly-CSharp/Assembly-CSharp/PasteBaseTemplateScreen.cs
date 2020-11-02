// Decompiled with JetBrains decompiler
// Type: PasteBaseTemplateScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PasteBaseTemplateScreen : KScreen
{
  public static PasteBaseTemplateScreen Instance;
  public GameObject button_list_container;
  public GameObject prefab_paste_button;
  private List<string> base_template_assets;
  private List<GameObject> template_buttons = new List<GameObject>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PasteBaseTemplateScreen.Instance = this;
    TemplateCache.Init();
    this.ConsumeMouseScroll = true;
    this.RefreshStampButtons();
  }

  public void RefreshStampButtons()
  {
    foreach (UnityEngine.Object templateButton in this.template_buttons)
      UnityEngine.Object.Destroy(templateButton);
    this.template_buttons.Clear();
    this.base_template_assets = TemplateCache.CollectBaseTemplateNames();
    this.base_template_assets.AddRange((IEnumerable<string>) TemplateCache.CollectBaseTemplateNames("poi"));
    this.base_template_assets.AddRange((IEnumerable<string>) TemplateCache.CollectBaseTemplateNames(""));
    foreach (string baseTemplateAsset in this.base_template_assets)
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
      KButton component = gameObject.GetComponent<KButton>();
      string template_name = baseTemplateAsset;
      System.Action action = (System.Action) (() => this.OnClickPasteButton(template_name));
      component.onClick += action;
      gameObject.GetComponentInChildren<LocText>().text = template_name;
      this.template_buttons.Add(gameObject);
    }
  }

  private void OnClickPasteButton(string template_name)
  {
    if (template_name == null)
      return;
    DebugTool.Instance.DeactivateTool();
    DebugBaseTemplateButton.Instance.ClearSelection();
    DebugBaseTemplateButton.Instance.nameField.text = template_name;
    TemplateContainer template = TemplateCache.GetTemplate(template_name);
    StampTool.Instance.Activate(template, true);
  }
}
