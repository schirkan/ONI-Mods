// Decompiled with JetBrains decompiler
// Type: MotdServerClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;

public class MotdServerClient
{
  private System.Action<MotdServerClient.MotdResponse, string> m_callback;
  private MotdServerClient.MotdResponse m_localMotd;

  private static string MotdServerUrl => "https://klei-motd.s3.amazonaws.com/oni/" + MotdServerClient.GetLocalePathSuffix();

  private static string MotdLocalPath => "motd_local/" + MotdServerClient.GetLocalePathSuffix();

  private static string MotdLocalImagePath(int imageVersion) => MotdServerClient.MotdLocalImagePath(imageVersion, Localization.GetLocale());

  private static string FallbackMotdLocalImagePath(int imageVersion) => MotdServerClient.MotdLocalImagePath(imageVersion, (Localization.Locale) null);

  private static string MotdLocalImagePath(int imageVersion, Localization.Locale locale) => "motd_local/" + MotdServerClient.GetLocalePathModifier(locale) + "image_" + (object) imageVersion;

  private static string GetLocalePathModifier() => MotdServerClient.GetLocalePathModifier(Localization.GetLocale());

  private static string GetLocalePathModifier(Localization.Locale locale)
  {
    string str = "";
    if (locale != null)
    {
      switch (locale.Lang)
      {
        case Localization.Language.Chinese:
        case Localization.Language.Korean:
        case Localization.Language.Russian:
          str = locale.Code + "/";
          break;
      }
    }
    return str;
  }

  private static string GetLocalePathSuffix() => MotdServerClient.GetLocalePathModifier() + "motd.json";

  public void GetMotd(System.Action<MotdServerClient.MotdResponse, string> cb)
  {
    this.m_callback = cb;
    MotdServerClient.MotdResponse localResponse = this.GetLocalMotd(MotdServerClient.MotdLocalPath);
    this.GetWebMotd(MotdServerClient.MotdServerUrl, localResponse, (System.Action<MotdServerClient.MotdResponse, string>) ((response, err) =>
    {
      MotdServerClient.MotdResponse response1;
      if (err == null)
      {
        Debug.Assert((UnityEngine.Object) response.image_texture != (UnityEngine.Object) null, (object) "Attempting to return response with no image texture");
        response1 = response;
      }
      else
      {
        Debug.LogWarning((object) ("Could not retrieve web motd from " + MotdServerClient.MotdServerUrl + ", falling back to local - err: " + err));
        response1 = localResponse;
      }
      if (Localization.GetSelectedLanguageType() == Localization.SelectedLanguageType.UGC)
      {
        Debug.Log((object) "Language Mod detected, MOTD strings falling back to local file");
        response1.image_header_text = (string) UI.FRONTEND.MOTD.IMAGE_HEADER;
        response1.news_header_text = (string) UI.FRONTEND.MOTD.NEWS_HEADER;
        response1.news_body_text = (string) UI.FRONTEND.MOTD.NEWS_BODY;
        response1.patch_notes_summary = (string) UI.FRONTEND.MOTD.PATCH_NOTES_SUMMARY;
        response1.update_text_override = (string) UI.FRONTEND.MOTD.UPDATE_TEXT;
      }
      this.doCallback(response1, (string) null);
    }));
  }

  private MotdServerClient.MotdResponse GetLocalMotd(string filePath)
  {
    this.m_localMotd = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(Resources.Load<TextAsset>(filePath.Replace(".json", "")).ToString());
    string path = MotdServerClient.MotdLocalImagePath(this.m_localMotd.image_version);
    this.m_localMotd.image_texture = Resources.Load<Texture2D>(path);
    if ((UnityEngine.Object) this.m_localMotd.image_texture == (UnityEngine.Object) null)
    {
      string str = MotdServerClient.FallbackMotdLocalImagePath(this.m_localMotd.image_version);
      if (str != path)
      {
        Debug.Log((object) ("Could not load " + path + ", falling back to " + str));
        path = str;
        this.m_localMotd.image_texture = Resources.Load<Texture2D>(path);
      }
    }
    Debug.Assert((UnityEngine.Object) this.m_localMotd.image_texture != (UnityEngine.Object) null, (object) ("Failed to load " + path));
    return this.m_localMotd;
  }

  private void GetWebMotd(
    string url,
    MotdServerClient.MotdResponse localMotd,
    System.Action<MotdServerClient.MotdResponse, string> cb)
  {
    System.Action<string, string> cb1 = (System.Action<string, string>) ((response, err) =>
    {
      DebugUtil.DevAssert((UnityEngine.Object) localMotd.image_texture != (UnityEngine.Object) null, "Local MOTD image_texture is no longer loaded");
      if ((UnityEngine.Object) localMotd.image_texture == (UnityEngine.Object) null)
        cb((MotdServerClient.MotdResponse) null, "Local image_texture has been unloaded since we requested the MOTD");
      else if (err != null)
      {
        cb((MotdServerClient.MotdResponse) null, err);
      }
      else
      {
        MotdServerClient.MotdResponse responseStruct = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(response, new JsonSerializerSettings()
        {
          Error = (System.EventHandler<ErrorEventArgs>) ((sender, args) => args.ErrorContext.Handled = true)
        });
        if (responseStruct == null)
          cb((MotdServerClient.MotdResponse) null, "Invalid json from server:" + response);
        else if (responseStruct.version <= localMotd.version)
        {
          Debug.Log((object) ("Using local MOTD at version: " + (object) localMotd.version + ", web version at " + (object) responseStruct.version));
          cb(localMotd, (string) null);
        }
        else
          SimpleNetworkCache.LoadFromCacheOrDownload("motd_image", responseStruct.image_url, responseStruct.image_version, new UnityWebRequest()
          {
            downloadHandler = (DownloadHandler) new DownloadHandlerTexture()
          }, (System.Action<UnityWebRequest>) (wr =>
          {
            string str = (string) null;
            if (string.IsNullOrEmpty(wr.error))
            {
              Debug.Log((object) ("Using web MOTD at version: " + (object) responseStruct.version + ", local version at " + (object) localMotd.version));
              responseStruct.image_texture = DownloadHandlerTexture.GetContent(wr);
            }
            else
              str = "SimpleNetworkCache - " + wr.error;
            cb(responseStruct, str);
            wr.Dispose();
          }));
      }
    });
    this.getAsyncRequest(url, cb1);
  }

  private void getAsyncRequest(string url, System.Action<string, string> cb)
  {
    UnityWebRequest motdRequest = UnityWebRequest.Get(url);
    motdRequest.SetRequestHeader("Content-Type", "application/json");
    motdRequest.SendWebRequest().completed += (System.Action<AsyncOperation>) (operation =>
    {
      cb(motdRequest.downloadHandler.text, motdRequest.error);
      motdRequest.Dispose();
    });
  }

  public void UnregisterCallback() => this.m_callback = (System.Action<MotdServerClient.MotdResponse, string>) null;

  private void doCallback(MotdServerClient.MotdResponse response, string error)
  {
    if (this.m_callback != null)
      this.m_callback(response, error);
    else
      Debug.Log((object) "Motd Response receieved, but callback was unregistered");
  }

  public class MotdResponse
  {
    public int version { get; set; }

    public string image_header_text { get; set; }

    public int image_version { get; set; }

    public string image_url { get; set; }

    public string image_link_url { get; set; }

    public string news_header_text { get; set; }

    public string news_body_text { get; set; }

    public string patch_notes_summary { get; set; }

    public string patch_notes_link_url { get; set; }

    public string last_update_time { get; set; }

    public string next_update_time { get; set; }

    public string update_text_override { get; set; }

    [JsonIgnore]
    public Texture2D image_texture { get; set; }
  }
}
