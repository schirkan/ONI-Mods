// Decompiled with JetBrains decompiler
// Type: GraphicsOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal class GraphicsOptionsScreen : KModalScreen
{
  [SerializeField]
  private Dropdown resolutionDropdown;
  [SerializeField]
  private MultiToggle lowResToggle;
  [SerializeField]
  private MultiToggle fullscreenToggle;
  [SerializeField]
  private KButton applyButton;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private ConfirmDialogScreen confirmPrefab;
  [SerializeField]
  private ConfirmDialogScreen feedbackPrefab;
  [SerializeField]
  private KSlider uiScaleSlider;
  [SerializeField]
  private LocText sliderLabel;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private Dropdown colorModeDropdown;
  [SerializeField]
  private KImage colorExampleLogicOn;
  [SerializeField]
  private KImage colorExampleLogicOff;
  [SerializeField]
  private KImage colorExampleCropHalted;
  [SerializeField]
  private KImage colorExampleCropGrowing;
  [SerializeField]
  private KImage colorExampleCropGrown;
  public static readonly string ResolutionWidthKey = "ResolutionWidth";
  public static readonly string ResolutionHeightKey = "ResolutionHeight";
  public static readonly string RefreshRateKey = "RefreshRate";
  public static readonly string FullScreenKey = "FullScreen";
  public static readonly string LowResKey = "LowResTextures";
  public static readonly string ColorModeKey = "ColorModeID";
  private KCanvasScaler[] CanvasScalers;
  private ConfirmDialogScreen confirmDialog;
  private ConfirmDialogScreen feedbackDialog;
  private List<Resolution> resolutions = new List<Resolution>();
  private List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
  private List<Dropdown.OptionData> colorModeOptions = new List<Dropdown.OptionData>();
  private int colorModeId;
  private bool colorModeChanged;
  private GraphicsOptionsScreen.Settings originalSettings;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.TITLE);
    this.originalSettings = this.CaptureSettings();
    this.applyButton.isInteractable = false;
    this.applyButton.onClick += new System.Action(this.OnApply);
    this.applyButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.APPLYBUTTON);
    this.doneButton.onClick += new System.Action(this.OnDone);
    this.closeButton.onClick += new System.Action(this.OnDone);
    this.doneButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.DONE_BUTTON);
    this.lowResToggle.ChangeState(QualitySettings.GetQualityLevel() == 1 ? 1 : 0);
    this.lowResToggle.onClick += new System.Action(this.OnLowResToggle);
    this.lowResToggle.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.LOWRES);
    this.resolutionDropdown.ClearOptions();
    this.BuildOptions();
    ScreenResize.Instance.OnResize += (System.Action) (() =>
    {
      this.BuildOptions();
      this.resolutionDropdown.options = this.options;
    });
    this.resolutionDropdown.options = this.options;
    this.resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionChanged));
    this.fullscreenToggle.ChangeState(Screen.fullScreen ? 1 : 0);
    this.fullscreenToggle.onClick += new System.Action(this.OnFullscreenToggle);
    this.fullscreenToggle.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.FULLSCREEN);
    this.resolutionDropdown.transform.parent.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.RESOLUTION);
    if (this.fullscreenToggle.CurrentState == 1)
    {
      int resolutionIndex = this.GetResolutionIndex(this.originalSettings.resolution);
      if (resolutionIndex != -1)
        this.resolutionDropdown.value = resolutionIndex;
    }
    this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>();
    this.UpdateSliderLabel();
    this.uiScaleSlider.onValueChanged.AddListener((UnityAction<float>) (data => this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%"));
    this.uiScaleSlider.onReleaseHandle += (System.Action) (() => this.UpdateUIScale(this.uiScaleSlider.value));
    this.BuildColorModeOptions();
    this.colorModeDropdown.options = this.colorModeOptions;
    this.colorModeDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnColorModeChanged));
    int num = 0;
    if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
      num = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
    this.colorModeDropdown.value = num;
    this.RefreshColorExamples(this.originalSettings.colorSetId);
  }

  public static void SetSettingsFromPrefs()
  {
    GraphicsOptionsScreen.SetResolutionFromPrefs();
    GraphicsOptionsScreen.SetLowResFromPrefs();
  }

  public static void SetLowResFromPrefs()
  {
    int index = 0;
    if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.LowResKey))
    {
      index = KPlayerPrefs.GetInt(GraphicsOptionsScreen.LowResKey);
      QualitySettings.SetQualityLevel(index, true);
    }
    else
      QualitySettings.SetQualityLevel(index, true);
    DebugUtil.LogArgs((object) string.Format("Low Res Textures? {0}", index == 1 ? (object) "Yes" : (object) "No"));
  }

  public static void SetResolutionFromPrefs()
  {
    int width = Screen.currentResolution.width;
    int height = Screen.currentResolution.height;
    int preferredRefreshRate = Screen.currentResolution.refreshRate;
    bool fullscreen = Screen.fullScreen;
    if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionWidthKey) && KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionHeightKey))
    {
      int num1 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionWidthKey);
      int num2 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionHeightKey);
      int num3 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.RefreshRateKey, Screen.currentResolution.refreshRate);
      bool flag = KPlayerPrefs.GetInt(GraphicsOptionsScreen.FullScreenKey, Screen.fullScreen ? 1 : 0) == 1;
      if (num2 <= 1 || num1 <= 1)
      {
        DebugUtil.LogArgs((object) "Saved resolution was invalid, ignoring...");
      }
      else
      {
        width = num1;
        height = num2;
        preferredRefreshRate = num3;
        fullscreen = flag;
      }
    }
    if (width <= 1 || height <= 1)
    {
      DebugUtil.LogWarningArgs((object) "Detected a degenerate resolution, attempting to fix...");
      foreach (Resolution resolution in Screen.resolutions)
      {
        if (resolution.width == 1920)
        {
          width = resolution.width;
          height = resolution.height;
          preferredRefreshRate = 0;
        }
      }
      if (width <= 1 || height <= 1)
      {
        foreach (Resolution resolution in Screen.resolutions)
        {
          if (resolution.width == 1280)
          {
            width = resolution.width;
            height = resolution.height;
            preferredRefreshRate = 0;
          }
        }
      }
      if (width <= 1 || height <= 1)
      {
        foreach (Resolution resolution in Screen.resolutions)
        {
          if (resolution.width > 1 && resolution.height > 1 && resolution.refreshRate > 0)
          {
            width = resolution.width;
            height = resolution.height;
            preferredRefreshRate = 0;
          }
        }
      }
      if (width <= 1 || height <= 1)
      {
        string str = "Could not find a suitable resolution for this screen! Reported available resolutions are:";
        foreach (Resolution resolution in Screen.resolutions)
          str += string.Format("\n{0}x{1} @ {2}hz", (object) resolution.width, (object) resolution.height, (object) resolution.refreshRate);
        Debug.LogError((object) str);
        width = 1280;
        height = 720;
        fullscreen = false;
        preferredRefreshRate = 0;
      }
    }
    DebugUtil.LogArgs((object) string.Format("Applying resolution {0}x{1} @{2}hz (fullscreen: {3})", (object) width, (object) height, (object) preferredRefreshRate, (object) fullscreen));
    Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
  }

  public static void SetColorModeFromPrefs()
  {
    int index = 0;
    if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
      index = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
    GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[index];
  }

  public static void OnResize()
  {
    GraphicsOptionsScreen.Settings settings = new GraphicsOptionsScreen.Settings()
    {
      resolution = Screen.currentResolution
    };
    settings.resolution.width = Screen.width;
    settings.resolution.height = Screen.height;
    settings.fullscreen = Screen.fullScreen;
    settings.lowRes = QualitySettings.GetQualityLevel();
    settings.colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet);
    GraphicsOptionsScreen.SaveSettingsToPrefs(settings);
  }

  private static void SaveSettingsToPrefs(GraphicsOptionsScreen.Settings settings)
  {
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.LowResKey, settings.lowRes);
    Debug.LogFormat("Screen resolution updated, saving values to prefs: {0}x{1} @ {2}, fullscreen: {3}", (object) settings.resolution.width, (object) settings.resolution.height, (object) settings.resolution.refreshRate, (object) settings.fullscreen);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionWidthKey, settings.resolution.width);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionHeightKey, settings.resolution.height);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.RefreshRateKey, settings.resolution.refreshRate);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.FullScreenKey, settings.fullscreen ? 1 : 0);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.ColorModeKey, settings.colorSetId);
  }

  private void UpdateUIScale(float value)
  {
    foreach (KCanvasScaler canvasScaler in this.CanvasScalers)
    {
      canvasScaler.SetUserScale(value / 100f);
      KPlayerPrefs.SetFloat(KCanvasScaler.UIScalePrefKey, value);
    }
    this.UpdateSliderLabel();
  }

  private void UpdateSliderLabel()
  {
    if (this.CanvasScalers == null || this.CanvasScalers.Length == 0 || !((UnityEngine.Object) this.CanvasScalers[0] != (UnityEngine.Object) null))
      return;
    this.uiScaleSlider.value = this.CanvasScalers[0].GetUserScale() * 100f;
    this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%";
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
    {
      this.resolutionDropdown.Hide();
      this.Deactivate();
    }
    else
      base.OnKeyDown(e);
  }

  private void BuildOptions()
  {
    this.options.Clear();
    this.resolutions.Clear();
    Resolution resolution1 = new Resolution();
    resolution1.width = Screen.width;
    resolution1.height = Screen.height;
    resolution1.refreshRate = Screen.currentResolution.refreshRate;
    this.options.Add(new Dropdown.OptionData(resolution1.ToString()));
    this.resolutions.Add(resolution1);
    foreach (Resolution resolution2 in Screen.resolutions)
    {
      if (resolution2.height >= 720)
      {
        this.options.Add(new Dropdown.OptionData(resolution2.ToString()));
        this.resolutions.Add(resolution2);
      }
    }
  }

  private void BuildColorModeOptions()
  {
    this.colorModeOptions.Clear();
    for (int index = 0; index < GlobalAssets.Instance.colorSetOptions.Length; ++index)
      this.colorModeOptions.Add(new Dropdown.OptionData((string) Strings.Get(GlobalAssets.Instance.colorSetOptions[index].settingName)));
  }

  private void RefreshColorExamples(int idx)
  {
    Color32 logicOn = GlobalAssets.Instance.colorSetOptions[idx].logicOn;
    Color32 logicOff = GlobalAssets.Instance.colorSetOptions[idx].logicOff;
    Color32 cropHalted = GlobalAssets.Instance.colorSetOptions[idx].cropHalted;
    Color32 cropGrowing = GlobalAssets.Instance.colorSetOptions[idx].cropGrowing;
    Color32 cropGrown = GlobalAssets.Instance.colorSetOptions[idx].cropGrown;
    logicOn.a = byte.MaxValue;
    logicOff.a = byte.MaxValue;
    cropHalted.a = byte.MaxValue;
    cropGrowing.a = byte.MaxValue;
    cropGrown.a = byte.MaxValue;
    this.colorExampleLogicOn.color = (Color) logicOn;
    this.colorExampleLogicOff.color = (Color) logicOff;
    this.colorExampleCropHalted.color = (Color) cropHalted;
    this.colorExampleCropGrowing.color = (Color) cropGrowing;
    this.colorExampleCropGrown.color = (Color) cropGrown;
  }

  private int GetResolutionIndex(Resolution resolution)
  {
    int num1 = -1;
    int num2 = -1;
    for (int index = 0; index < this.resolutions.Count; ++index)
    {
      Resolution resolution1 = this.resolutions[index];
      if (resolution1.width == resolution.width && resolution1.height == resolution.height && resolution1.refreshRate == 0)
        num2 = index;
      if (resolution1.width == resolution.width && resolution1.height == resolution.height && Math.Abs(resolution1.refreshRate - resolution.refreshRate) <= 1)
      {
        num1 = index;
        break;
      }
    }
    return num1 != -1 ? num1 : num2;
  }

  private GraphicsOptionsScreen.Settings CaptureSettings() => new GraphicsOptionsScreen.Settings()
  {
    fullscreen = Screen.fullScreen,
    resolution = new Resolution()
    {
      width = Screen.width,
      height = Screen.height,
      refreshRate = Screen.currentResolution.refreshRate
    },
    lowRes = QualitySettings.GetQualityLevel(),
    colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet)
  };

  private void OnApply()
  {
    try
    {
      GraphicsOptionsScreen.Settings new_settings = new GraphicsOptionsScreen.Settings();
      new_settings.resolution = this.resolutions[this.resolutionDropdown.value];
      new_settings.fullscreen = this.fullscreenToggle.CurrentState != 0;
      new_settings.lowRes = this.lowResToggle.CurrentState;
      new_settings.colorSetId = this.colorModeId;
      if ((UnityEngine.Object) GlobalAssets.Instance.colorSetOptions[this.colorModeId] != (UnityEngine.Object) GlobalAssets.Instance.colorSet)
        this.colorModeChanged = true;
      this.ApplyConfirmSettings(new_settings, (System.Action) (() =>
      {
        this.applyButton.isInteractable = false;
        if (this.colorModeChanged)
        {
          this.feedbackDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject).GetComponent<ConfirmDialogScreen>();
          this.feedbackDialog.PopupConfirmDialog(STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK.text, (System.Action) null, (System.Action) null, STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK_BUTTON.text, (System.Action) (() => Application.OpenURL("https://forums.kleientertainment.com/forums/topic/117325-color-blindness-feedback/")));
          this.feedbackDialog.gameObject.SetActive(true);
        }
        this.colorModeChanged = false;
        GraphicsOptionsScreen.SaveSettingsToPrefs(new_settings);
      }));
    }
    catch (Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Failed to apply graphics options!\nResolutions:");
      foreach (Resolution resolution in this.resolutions)
        stringBuilder.Append("\t" + resolution.ToString() + "\n");
      stringBuilder.Append("Selected Resolution Idx: " + this.resolutionDropdown.value.ToString());
      stringBuilder.Append("FullScreen: " + this.fullscreenToggle.CurrentState.ToString());
      Debug.LogError((object) stringBuilder.ToString());
      throw ex;
    }
  }

  public void OnDone() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void RefreshApplyButton()
  {
    GraphicsOptionsScreen.Settings settings = this.CaptureSettings();
    if (settings.fullscreen && this.fullscreenToggle.CurrentState == 0)
      this.applyButton.isInteractable = true;
    else if (!settings.fullscreen && this.fullscreenToggle.CurrentState == 1)
      this.applyButton.isInteractable = true;
    else if (settings.lowRes != this.lowResToggle.CurrentState)
      this.applyButton.isInteractable = true;
    else if (settings.colorSetId != this.colorModeId)
      this.applyButton.isInteractable = true;
    else
      this.applyButton.isInteractable = this.resolutionDropdown.value != this.GetResolutionIndex(settings.resolution);
  }

  private void OnFullscreenToggle()
  {
    this.fullscreenToggle.ChangeState(this.fullscreenToggle.CurrentState == 0 ? 1 : 0);
    this.RefreshApplyButton();
  }

  private void OnResolutionChanged(int idx) => this.RefreshApplyButton();

  private void OnColorModeChanged(int idx)
  {
    this.colorModeId = idx;
    this.RefreshApplyButton();
    this.RefreshColorExamples(this.colorModeId);
  }

  private void OnLowResToggle()
  {
    this.lowResToggle.ChangeState(this.lowResToggle.CurrentState == 0 ? 1 : 0);
    this.RefreshApplyButton();
  }

  private void ApplyConfirmSettings(GraphicsOptionsScreen.Settings new_settings, System.Action on_confirm)
  {
    GraphicsOptionsScreen.Settings current_settings = this.CaptureSettings();
    this.ApplySettings(new_settings);
    this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject).GetComponent<ConfirmDialogScreen>();
    System.Action action = (System.Action) (() => this.ApplySettings(current_settings));
    Coroutine timer = this.StartCoroutine(this.Timer(15f, action));
    this.confirmDialog.onDeactivateCB = (System.Action) (() => this.StopCoroutine(timer));
    this.confirmDialog.PopupConfirmDialog(this.colorModeChanged ? STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES_STRING_COLOR.text : STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES.text, on_confirm, action);
    this.confirmDialog.gameObject.SetActive(true);
  }

  private void ApplySettings(GraphicsOptionsScreen.Settings new_settings)
  {
    Resolution resolution = new_settings.resolution;
    Screen.SetResolution(resolution.width, resolution.height, new_settings.fullscreen, resolution.refreshRate);
    Screen.fullScreen = new_settings.fullscreen;
    int resolutionIndex = this.GetResolutionIndex(new_settings.resolution);
    if (resolutionIndex != -1)
      this.resolutionDropdown.value = resolutionIndex;
    GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[new_settings.colorSetId];
    Debug.Log((object) ("Applying low res settings " + (object) new_settings.lowRes + " / existing is " + (object) QualitySettings.GetQualityLevel()));
    if (QualitySettings.GetQualityLevel() == new_settings.lowRes)
      return;
    QualitySettings.SetQualityLevel(new_settings.lowRes, true);
  }

  private IEnumerator Timer(float time, System.Action revert)
  {
    yield return (object) new WaitForSeconds(time);
    if ((UnityEngine.Object) this.confirmDialog != (UnityEngine.Object) null)
    {
      this.confirmDialog.Deactivate();
      revert();
    }
  }

  private void Update() => Debug.developerConsoleVisible = false;

  private struct Settings
  {
    public bool fullscreen;
    public Resolution resolution;
    public int lowRes;
    public int colorSetId;
  }
}
