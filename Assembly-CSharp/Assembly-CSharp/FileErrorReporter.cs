// Decompiled with JetBrains decompiler
// Type: FileErrorReporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/FileErrorReporter")]
public class FileErrorReporter : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    this.OnFileError();
    FileUtil.onErrorMessage += new System.Action(this.OnFileError);
  }

  private void OnFileError()
  {
    if (FileUtil.errorType == FileUtil.ErrorType.None)
      return;
    string text;
    switch (FileUtil.errorType)
    {
      case FileUtil.ErrorType.UnauthorizedAccess:
        text = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, (object) FileUtil.errorSubject);
        break;
      case FileUtil.ErrorType.IOError:
        text = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, (object) FileUtil.errorSubject);
        break;
      default:
        text = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_UNKNOWN, (object) FileUtil.errorSubject);
        break;
    }
    GameObject parent;
    if ((UnityEngine.Object) FrontEndManager.Instance != (UnityEngine.Object) null)
      parent = FrontEndManager.Instance.gameObject;
    else if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) GameScreenManager.Instance.ssOverlayCanvas != (UnityEngine.Object) null)
    {
      parent = GameScreenManager.Instance.ssOverlayCanvas;
    }
    else
    {
      parent = new GameObject();
      parent.name = "FileErrorCanvas";
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) parent);
      Canvas canvas = parent.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
      canvas.sortingOrder = 10;
      parent.AddComponent<GraphicRaycaster>();
    }
    if ((FileUtil.exceptionMessage != null || FileUtil.exceptionStackTrace != null) && !KCrashReporter.hasReportedError)
      KCrashReporter.ReportError(FileUtil.exceptionMessage, FileUtil.exceptionStackTrace, (string) null, (ConfirmDialogScreen) null, (GameObject) null);
    ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>();
    component.PopupConfirmDialog(text, (System.Action) null, (System.Action) null);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) component.gameObject);
  }

  private void OpenMoreInfo()
  {
  }
}
