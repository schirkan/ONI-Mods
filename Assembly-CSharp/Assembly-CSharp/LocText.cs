// Decompiled with JetBrains decompiler
// Type: LocText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Text;
using TMPro;
using UnityEngine;

public class LocText : TextMeshProUGUI
{
  public string key;
  public TextStyleSetting textStyleSetting;
  public bool allowOverride;
  public bool staticLayout;
  private TextLinkHandler textLinkHandler;
  [SerializeField]
  private bool allowLinksInternal;
  private const string linkPrefix_open = "<link=\"";
  private const string linkSuffix = "</link>";
  private const string linkColorPrefix = "<b><style=\"KLink\">";
  private const string linkColorSuffix = "</style></b>";
  private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";
  private static readonly string combinedSuffix = "</style></b></link>";

  protected override void OnEnable() => base.OnEnable();

  public bool AllowLinks
  {
    get => this.allowLinksInternal;
    set
    {
      this.allowLinksInternal = value;
      this.RefreshLinkHandler();
      this.raycastTarget = this.raycastTarget || this.allowLinksInternal;
    }
  }

  [ContextMenu("Apply Settings")]
  public void ApplySettings()
  {
    if (this.key != "" && Application.isPlaying)
      this.text = (string) Strings.Get(new StringKey(this.key));
    if (!((Object) this.textStyleSetting != (Object) null))
      return;
    SetTextStyleSetting.ApplyStyle((TextMeshProUGUI) this, this.textStyleSetting);
  }

  private new void Awake()
  {
    base.Awake();
    if (!Application.isPlaying)
      return;
    if (this.key != "")
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = Localization.IsRightToLeft;
    SetTextStyleSetting textStyleSetting = this.gameObject.GetComponent<SetTextStyleSetting>();
    if ((Object) textStyleSetting == (Object) null)
      textStyleSetting = this.gameObject.AddComponent<SetTextStyleSetting>();
    if (!this.allowOverride)
      textStyleSetting.SetStyle(this.textStyleSetting);
    this.textLinkHandler = this.GetComponent<TextLinkHandler>();
  }

  private new void Start()
  {
    base.Start();
    this.RefreshLinkHandler();
  }

  public override void SetLayoutDirty()
  {
    if (this.staticLayout)
      return;
    base.SetLayoutDirty();
  }

  public override string text
  {
    get => base.text;
    set => base.text = this.FilterInput(value);
  }

  public override void SetText(string text)
  {
    text = this.FilterInput(text);
    base.SetText(text);
  }

  private string FilterInput(string input) => this.AllowLinks ? LocText.ModifyLinkStrings(input) : input;

  protected override void GenerateTextMesh() => base.GenerateTextMesh();

  internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
  {
    this.font = font;
    if (this.key != "")
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = isRightToLeft;
  }

  private static string ModifyLinkStrings(string input)
  {
    if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
      return input;
    StringBuilder stringBuilder = new StringBuilder(input);
    stringBuilder.Replace("<link=\"", LocText.combinedPrefix);
    stringBuilder.Replace("</link>", LocText.combinedSuffix);
    return stringBuilder.ToString();
  }

  private void RefreshLinkHandler()
  {
    if ((Object) this.textLinkHandler == (Object) null && this.allowLinksInternal)
    {
      this.textLinkHandler = this.GetComponent<TextLinkHandler>();
      if ((Object) this.textLinkHandler == (Object) null)
        this.textLinkHandler = this.gameObject.AddComponent<TextLinkHandler>();
    }
    else if (!this.allowLinksInternal && (Object) this.textLinkHandler != (Object) null)
    {
      Object.Destroy((Object) this.textLinkHandler);
      this.textLinkHandler = (TextLinkHandler) null;
    }
    if (!((Object) this.textLinkHandler != (Object) null))
      return;
    this.textLinkHandler.CheckMouseOver();
  }
}
