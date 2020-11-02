// Decompiled with JetBrains decompiler
// Type: BuildWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BuildWatermark : KScreen
{
  public LocText textDisplay;
  public ToolTip toolTip;
  public KButton button;
  public List<GameObject> archiveIcons;
  public static BuildWatermark Instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    BuildWatermark.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshText();
  }

  public void RefreshText()
  {
    string str1 = "AP-";
    bool flag1 = true;
    bool flag2 = DistributionPlatform.Initialized && DistributionPlatform.Inst.IsArchiveBranch;
    this.button.ClearOnClick();
    string str2;
    if (Application.isEditor)
    {
      str2 = str1 + "<EDITOR>";
    }
    else
    {
      str2 = str1 + 420700U.ToString();
      if (DebugHandler.enabled)
        str2 += "-D";
    }
    if (flag1)
    {
      this.textDisplay.SetText(string.Format((string) UI.DEVELOPMENTBUILDS.WATERMARK, (object) str2));
      this.toolTip.ClearMultiStringTooltip();
    }
    else
    {
      this.textDisplay.SetText(string.Format((string) UI.DEVELOPMENTBUILDS.TESTING_WATERMARK, (object) str2));
      this.toolTip.SetSimpleTooltip((string) UI.DEVELOPMENTBUILDS.TESTING_TOOLTIP);
      this.button.onClick += new System.Action(this.ShowTestingMessage);
    }
    foreach (GameObject archiveIcon in this.archiveIcons)
      archiveIcon.SetActive(flag1 & flag2);
  }

  private void ShowTestingMessage() => Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, Global.Instance.globalCanvas, true).PopupConfirmDialog((string) UI.DEVELOPMENTBUILDS.TESTING_MESSAGE, (System.Action) (() => Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/")), (System.Action) (() => {}), title_text: ((string) UI.DEVELOPMENTBUILDS.TESTING_MESSAGE_TITLE), confirm_text: ((string) UI.DEVELOPMENTBUILDS.TESTING_MORE_INFO));
}
