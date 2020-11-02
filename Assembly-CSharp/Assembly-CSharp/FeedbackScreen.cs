// Decompiled with JetBrains decompiler
// Type: FeedbackScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class FeedbackScreen : KModalScreen
{
  public LocText title;
  public KButton dismissButton;
  public KButton closeButton;
  public KButton bugForumsButton;
  public KButton suggestionForumsButton;
  public KButton logsDirectoryButton;
  public KButton saveFilesDirectoryButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
    this.dismissButton.onClick += (System.Action) (() => this.Deactivate());
    this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    this.bugForumsButton.onClick += (System.Action) (() => Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/"));
    this.suggestionForumsButton.onClick += (System.Action) (() => Application.OpenURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/"));
    this.logsDirectoryButton.onClick += (System.Action) (() => Application.OpenURL(Util.LogsFolder()));
    this.saveFilesDirectoryButton.onClick += (System.Action) (() => Application.OpenURL(SaveLoader.GetSavePrefix()));
  }
}
