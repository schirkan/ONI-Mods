// Decompiled with JetBrains decompiler
// Type: LanguageOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KMod;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageOptionsScreen : KModalScreen, SteamUGCService.IClient
{
  private static readonly string[] poFile = new string[1]
  {
    "strings.po"
  };
  public const string KPLAYER_PREFS_LANGUAGE_KEY = "InstalledLanguage";
  public const string TAG_LANGUAGE = "language";
  public KButton textButton;
  public KButton dismissButton;
  public KButton closeButton;
  public KButton workshopButton;
  public KButton uninstallButton;
  [Space]
  public GameObject languageButtonPrefab;
  public GameObject preinstalledLanguagesTitle;
  public GameObject preinstalledLanguagesContainer;
  public GameObject ugcLanguagesTitle;
  public GameObject ugcLanguagesContainer;
  private List<GameObject> buttons = new List<GameObject>();
  private PublishedFileId_t _currentLanguage = PublishedFileId_t.Invalid;
  private System.DateTime currentLastModified;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.dismissButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText((string) STRINGS.UI.FRONTEND.OPTIONS_SCREEN.BACK);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.workshopButton.onClick += (System.Action) (() => this.OnClickOpenWorkshop());
    this.uninstallButton.onClick += (System.Action) (() => this.OnClickUninstall());
    this.uninstallButton.gameObject.SetActive(false);
    this.RebuildScreen();
  }

  private void RebuildScreen()
  {
    foreach (UnityEngine.Object button in this.buttons)
      UnityEngine.Object.Destroy(button);
    this.buttons.Clear();
    this.uninstallButton.isInteractable = KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()) != Localization.SelectedLanguageType.None.ToString();
    this.RebuildPreinstalledButtons();
    this.RebuildUGCButtons();
  }

  private void RebuildPreinstalledButtons()
  {
    foreach (string preinstalledLanguage in Localization.PreinstalledLanguages)
    {
      string code = preinstalledLanguage;
      if (!(code != Localization.DEFAULT_LANGUAGE_CODE) || File.Exists(Localization.GetPreinstalledLocalizationFilePath(code)))
      {
        GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.preinstalledLanguagesContainer);
        gameObject.name = code + "_button";
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        LocText reference = component.GetReference<LocText>("Title");
        reference.text = Localization.GetPreinstalledLocalizationTitle(code);
        reference.enabled = false;
        reference.enabled = true;
        Texture2D localizationImage = Localization.GetPreinstalledLocalizationImage(code);
        if ((UnityEngine.Object) localizationImage != (UnityEngine.Object) null)
          component.GetReference<Image>("Image").sprite = Sprite.Create(localizationImage, new Rect(Vector2.zero, new Vector2((float) localizationImage.width, (float) localizationImage.height)), Vector2.one * 0.5f);
        gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.ConfirmLanguageChoiceDialog(code != Localization.DEFAULT_LANGUAGE_CODE ? code : string.Empty, PublishedFileId_t.Invalid));
        this.buttons.Add(gameObject);
      }
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    Global.Instance.modManager.Sanitize(this.gameObject);
    this.currentLanguage = LanguageOptionsScreen.GetInstalledFileID(out this.currentLastModified);
    if (!((UnityEngine.Object) SteamUGCService.Instance != (UnityEngine.Object) null))
      return;
    if (SteamUGCService.Instance.FindMod(this.currentLanguage) == null)
    {
      this.currentLanguage = PublishedFileId_t.Invalid;
      this.InstallLanguageFile(this.currentLanguage);
    }
    SteamUGCService.Instance.AddClient((SteamUGCService.IClient) this);
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    if (!((UnityEngine.Object) SteamUGCService.Instance != (UnityEngine.Object) null))
      return;
    SteamUGCService.Instance.RemoveClient((SteamUGCService.IClient) this);
  }

  private void ConfirmLanguageChoiceDialog(
    string[] lines,
    bool is_template,
    System.Action install_language)
  {
    Localization.Locale locale = Localization.GetLocale(lines);
    Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
    TMP_FontAsset font = Localization.GetFont(locale.FontName);
    ConfirmDialogScreen screen = this.GetConfirmDialog();
    HashSet<MemberInfo> excluded_members = new HashSet<MemberInfo>((IEnumerable<MemberInfo>) typeof (ConfirmDialogScreen).GetMember("cancelButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
    Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);
    string str1;
    Func<LocString, string> func = (Func<LocString, string>) (loc_string => !translated_strings.TryGetValue(loc_string.key.String, out str1) ? (string) loc_string : str1);
    ConfirmDialogScreen confirmDialogScreen = screen;
    string str2 = func(STRINGS.UI.CONFIRMDIALOG.DIALOG_HEADER);
    string text = func(STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT);
    System.Action on_confirm = (System.Action) (() =>
    {
      install_language();
      App.instance.Restart();
    });
    System.Action on_cancel = (System.Action) (() => Localization.SetFont<ConfirmDialogScreen>(screen, Localization.FontAsset, Localization.IsRightToLeft, excluded_members));
    string title_text = str2;
    string confirm_text = func(STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART);
    string cancel = (string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.CANCEL;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, title_text: title_text, confirm_text: confirm_text, cancel_text: cancel);
  }

  private void ConfirmLanguageChoiceDialog(string selected_preinstalled_translation)
  {
    Localization.SelectedLanguageType selectedLanguageType = Localization.GetSelectedLanguageType();
    if (!string.IsNullOrEmpty(selected_preinstalled_translation))
    {
      string preinstalledLanguageCode = Localization.GetSelectedPreinstalledLanguageCode();
      if (selectedLanguageType == Localization.SelectedLanguageType.Preinstalled && preinstalledLanguageCode == selected_preinstalled_translation)
        this.Deactivate();
      else
        this.ConfirmLanguageChoiceDialog(File.ReadAllLines(Localization.GetPreinstalledLocalizationFilePath(selected_preinstalled_translation), Encoding.UTF8), false, (System.Action) (() => Localization.LoadPreinstalledTranslation(selected_preinstalled_translation)));
    }
    else if (selectedLanguageType == Localization.SelectedLanguageType.None)
      this.Deactivate();
    else
      this.ConfirmLanguageChoiceDialog(File.ReadAllLines(Localization.GetDefaultLocalizationFilePath(), Encoding.UTF8), true, (System.Action) (() => Localization.ClearLanguage()));
  }

  private void ConfirmLanguageChoiceDialog(
    string selected_preinstalled_translation,
    PublishedFileId_t selected_language_pack)
  {
    if (selected_language_pack != PublishedFileId_t.Invalid)
    {
      if (Localization.GetSelectedLanguageType() == Localization.SelectedLanguageType.UGC && selected_language_pack == this.currentLanguage)
        this.Deactivate();
      else
        this.ConfirmLanguageChoiceDialog(LanguageOptionsScreen.GetLanguageFile(selected_language_pack, out System.DateTime _).Split('\n'), false, (System.Action) (() => this.SetCurrentLanguage(selected_language_pack)));
    }
    else
      this.ConfirmLanguageChoiceDialog(selected_preinstalled_translation);
  }

  private ConfirmDialogScreen GetConfirmDialog()
  {
    KScreen component = KScreenManager.AddChild(this.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
    component.Activate();
    return component.GetComponent<ConfirmDialogScreen>();
  }

  private void RebuildUGCButtons()
  {
    if ((UnityEngine.Object) SteamUGCService.Instance == (UnityEngine.Object) null)
      return;
    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
    {
      if ((mod.available_content & Content.Translation) != (Content) 0 && mod.status == KMod.Mod.Status.Installed)
      {
        GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.ugcLanguagesContainer);
        gameObject.name = mod.title + "_button";
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        PublishedFileId_t file_id = new PublishedFileId_t(ulong.Parse(mod.label.id));
        TMP_FontAsset fontForLangage = LanguageOptionsScreen.GetFontForLangage(file_id);
        LocText reference = component.GetReference<LocText>("Title");
        reference.SetText(string.Format((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.UGC_MOD_TITLE_FORMAT, (object) mod.title));
        reference.font = fontForLangage;
        Texture2D previewImage = SteamUGCService.Instance.FindMod(file_id)?.previewImage;
        if ((UnityEngine.Object) previewImage != (UnityEngine.Object) null)
          component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float) previewImage.width, (float) previewImage.height)), Vector2.one * 0.5f);
        gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.ConfirmLanguageChoiceDialog(string.Empty, file_id));
        this.buttons.Add(gameObject);
      }
    }
  }

  private void InstallLanguage(PublishedFileId_t item)
  {
    this.SetCurrentLanguage(item);
    this.GetConfirmDialog().PopupConfirmDialog((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(((KScreen) this).Deactivate));
  }

  private void Uninstall() => this.GetConfirmDialog().PopupConfirmDialog((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.ARE_YOU_SURE, (System.Action) (() =>
  {
    Localization.ClearLanguage();
    this.GetConfirmDialog().PopupConfirmDialog((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(((KScreen) this).Deactivate));
  }), (System.Action) (() => {}));

  private void OnClickUninstall() => this.Uninstall();

  private void OnClickOpenWorkshop() => Application.OpenURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=language");

  public void UpdateMods(
    IEnumerable<PublishedFileId_t> added,
    IEnumerable<PublishedFileId_t> updated,
    IEnumerable<PublishedFileId_t> removed,
    IEnumerable<SteamUGCService.Mod> loaded_previews)
  {
    PublishedFileId_t currentLanguage = (PublishedFileId_t) this.GetCurrentLanguage();
    if (removed.Contains<PublishedFileId_t>(currentLanguage))
    {
      Debug.Log((object) ("Unsubscribe detected for currently installed font [" + (object) currentLanguage + "]"));
      this.GetConfirmDialog().PopupConfirmDialog((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, (System.Action) (() =>
      {
        Localization.ClearLanguage();
        this.currentLanguage = PublishedFileId_t.Invalid;
        App.instance.Restart();
      }), (System.Action) null, confirm_text: ((string) STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART));
    }
    if (updated.Contains<PublishedFileId_t>(currentLanguage))
    {
      Debug.Log((object) ("Download complete for currently installed font [" + (object) currentLanguage + "] updating in background. Changes will happen next restart."));
      this.UpdateInstalledLanguage(currentLanguage);
    }
    this.RebuildScreen();
  }

  private ulong GetCurrentLanguage() => (ulong) KPlayerPrefs.GetInt("InstalledLanguage");

  public static void CleanUpCurrentModLanguage()
  {
    KPlayerPrefs.SetInt("InstalledLanguage", (int) PublishedFileId_t.Invalid.m_PublishedFileId);
    LanguageOptionsScreen.InstalledLanguageData.Delete();
    string localizationFilePath = Localization.GetModLocalizationFilePath();
    if (!File.Exists(localizationFilePath))
      return;
    File.Delete(localizationFilePath);
  }

  public PublishedFileId_t currentLanguage
  {
    get => this._currentLanguage;
    private set
    {
      this._currentLanguage = value;
      KPlayerPrefs.SetInt("InstalledLanguage", (int) this._currentLanguage.m_PublishedFileId);
    }
  }

  public void SetCurrentLanguage(PublishedFileId_t item) => this.InstallLanguageFile(item);

  public static bool HasInstalledLanguage() => LanguageOptionsScreen.GetInstalledFileID(out System.DateTime _) != PublishedFileId_t.Invalid;

  public static string GetInstalledLanguageCode(out PublishedFileId_t installed)
  {
    string str = "";
    string languageFilename = LanguageOptionsScreen.GetLanguageFilename(out installed, out System.DateTime _);
    if (languageFilename != null && File.Exists(languageFilename))
    {
      Localization.Locale locale = Localization.GetLocale(File.ReadAllLines(languageFilename, Encoding.UTF8));
      if (locale != null)
        str = locale.Code;
    }
    return str;
  }

  public static string GetInstalledLanguageFilename(ref PublishedFileId_t item) => LanguageOptionsScreen.GetLanguageFilename(out item, out System.DateTime _);

  public static TMP_FontAsset GetFontForLangage(PublishedFileId_t item)
  {
    string languageFile = LanguageOptionsScreen.GetLanguageFile(item, out System.DateTime _);
    if (languageFile == null || languageFile.Length <= 0)
      return (TMP_FontAsset) null;
    return Localization.GetFont(LanguageOptionsScreen.GetFontForLocalisation(languageFile.Split('\n')));
  }

  public static void LoadTranslation(ref PublishedFileId_t item) => Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.UGC, LanguageOptionsScreen.GetInstalledLanguageFilename(ref item));

  private void UpdateInstalledLanguage(PublishedFileId_t item)
  {
    string languageFile = LanguageOptionsScreen.GetLanguageFile(item, out this.currentLastModified);
    if (languageFile != null && languageFile.Length > 0)
    {
      LanguageOptionsScreen.InstalledLanguageData.Set(item, this.currentLastModified);
      File.WriteAllText(Localization.GetModLocalizationFilePath(), languageFile);
    }
    else
      Debug.LogWarning((object) ("Loc file was empty.. [" + (object) item + "]  [" + (object) this.currentLastModified + "]"));
  }

  private void InstallLanguageFile(PublishedFileId_t item, bool fromDownload = false)
  {
    LanguageOptionsScreen.CleanUpCurrentModLanguage();
    if (item != PublishedFileId_t.Invalid)
      this.UpdateInstalledLanguage(item);
    PublishedFileId_t invalid = PublishedFileId_t.Invalid;
    LanguageOptionsScreen.LoadTranslation(ref invalid);
    this.currentLanguage = item;
  }

  private static string GetFontForLocalisation(string[] lines) => Localization.GetLocale(lines).FontName;

  private static string GetLanguageFilename(out PublishedFileId_t item, out System.DateTime lastModified)
  {
    LanguageOptionsScreen.InstalledLanguageData.Get(out item, out lastModified);
    if (item != PublishedFileId_t.Invalid)
    {
      string localizationFilePath = Localization.GetModLocalizationFilePath();
      if (File.Exists(localizationFilePath))
        return localizationFilePath;
      Debug.LogWarning((object) ("GetLanguagFile [" + localizationFilePath + "] missing for [" + (object) item + "]"));
    }
    return (string) null;
  }

  private static string GetLanguageFile(PublishedFileId_t item, out System.DateTime lastModified)
  {
    lastModified = System.DateTime.MinValue;
    if ((UnityEngine.Object) Global.Instance == (UnityEngine.Object) null || Global.Instance.modManager == null)
    {
      Debug.LogFormat("Failed to load language file from local mod installation...too early in initialization flow.", (object[]) Array.Empty<object>());
      return LanguageOptionsScreen.GetLanguageFileFromSteam(item, out lastModified);
    }
    string language_id = item.ToString();
    KMod.Mod mod = Global.Instance.modManager.mods.Find((Predicate<KMod.Mod>) (candidate => candidate.label.id == language_id));
    if (string.IsNullOrEmpty(mod.label.id))
    {
      Debug.LogFormat("Failed to load language file from local mod installation...mod not found.", (object[]) Array.Empty<object>());
      return LanguageOptionsScreen.GetLanguageFileFromSteam(item, out lastModified);
    }
    lastModified = mod.label.time_stamp;
    string filename = System.IO.Path.Combine(Application.streamingAssetsPath, "strings.po");
    byte[] bytes = mod.file_source.GetFileSystem().ReadBytes(filename);
    if (bytes != null)
      return FileSystem.ConvertToText(bytes);
    Debug.LogFormat("Failed to load language file from local mod installation...couldn't find {0}", (object) filename);
    return LanguageOptionsScreen.GetLanguageFileFromSteam(item, out lastModified);
  }

  private static string GetLanguageFileFromSteam(PublishedFileId_t item, out System.DateTime lastModified)
  {
    lastModified = System.DateTime.MinValue;
    if (item == PublishedFileId_t.Invalid)
    {
      Debug.LogWarning((object) "Cant get INVALID file id from Steam");
      return (string) null;
    }
    if (SteamUGCService.Instance.FindMod(item) == null)
    {
      Debug.LogWarning((object) "Mod is not in published list");
      return (string) null;
    }
    byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, LanguageOptionsScreen.poFile, out lastModified);
    if (bytesFromZip != null && bytesFromZip.Length != 0)
      return Encoding.UTF8.GetString(bytesFromZip);
    Debug.LogWarning((object) "Failed to read from Steam mod installation");
    return (string) null;
  }

  private static PublishedFileId_t GetInstalledFileID(out System.DateTime lastModified)
  {
    PublishedFileId_t publishedFileIdT;
    LanguageOptionsScreen.InstalledLanguageData.Get(out publishedFileIdT, out lastModified);
    if (publishedFileIdT == PublishedFileId_t.Invalid)
      publishedFileIdT = new PublishedFileId_t((ulong) (uint) KPlayerPrefs.GetInt("InstalledLanguage", (int) PublishedFileId_t.Invalid.m_PublishedFileId));
    if (publishedFileIdT != PublishedFileId_t.Invalid && (UnityEngine.Object) SteamUGCService.Instance != (UnityEngine.Object) null && !SteamUGCService.Instance.IsSubscribed(publishedFileIdT))
    {
      Debug.LogWarning((object) ("It doesn't look like we are subscribed..." + (object) publishedFileIdT));
      publishedFileIdT = PublishedFileId_t.Invalid;
    }
    return publishedFileIdT;
  }

  private class InstalledLanguageData
  {
    private static readonly string FILE_NAME = "strings/mod_installed.dat";

    private static string FilePath() => System.IO.Path.Combine(Application.streamingAssetsPath, LanguageOptionsScreen.InstalledLanguageData.FILE_NAME);

    public static void Set(PublishedFileId_t item, System.DateTime lastModified) => YamlIO.SaveOrWarnUser<LanguageOptionsScreen.InstalledLanguageData>(new LanguageOptionsScreen.InstalledLanguageData()
    {
      PublishedFileId = item.m_PublishedFileId,
      LastModified = lastModified.ToFileTimeUtc()
    }, LanguageOptionsScreen.InstalledLanguageData.FilePath());

    public static void Get(out PublishedFileId_t item, out System.DateTime lastModified)
    {
      if (LanguageOptionsScreen.InstalledLanguageData.Exists())
      {
        LanguageOptionsScreen.InstalledLanguageData installedLanguageData = YamlIO.LoadFile<LanguageOptionsScreen.InstalledLanguageData>(LanguageOptionsScreen.InstalledLanguageData.FilePath());
        if (installedLanguageData != null)
        {
          lastModified = System.DateTime.FromFileTimeUtc(installedLanguageData.LastModified);
          item = new PublishedFileId_t(installedLanguageData.PublishedFileId);
          return;
        }
      }
      lastModified = System.DateTime.MinValue;
      item = PublishedFileId_t.Invalid;
    }

    public static bool Exists() => File.Exists(LanguageOptionsScreen.InstalledLanguageData.FilePath());

    public static void Delete()
    {
      if (!LanguageOptionsScreen.InstalledLanguageData.Exists())
        return;
      File.Delete(LanguageOptionsScreen.InstalledLanguageData.FilePath());
    }

    public ulong PublishedFileId { get; set; }

    public long LastModified { get; set; }
  }
}
