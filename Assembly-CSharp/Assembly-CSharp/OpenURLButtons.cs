﻿// Decompiled with JetBrains decompiler
// Type: OpenURLButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
  public GameObject buttonPrefab;
  public List<OpenURLButtons.URLButtonData> buttonData;
  [SerializeField]
  private GameObject patchNotesScreen;
  [SerializeField]
  private FeedbackScreen feedbackScreenPrefab;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    for (int index = 0; index < this.buttonData.Count; ++index)
    {
      OpenURLButtons.URLButtonData data = this.buttonData[index];
      GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.gameObject, true);
      string text = (string) Strings.Get(data.stringKey);
      gameObject.GetComponentInChildren<LocText>().SetText(text);
      switch (data.urlType)
      {
        case OpenURLButtons.URLButtonType.url:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenURL(data.url));
          break;
        case OpenURLButtons.URLButtonType.platformUrl:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenPlatformURL(data.url));
          break;
        case OpenURLButtons.URLButtonType.patchNotes:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenPatchNotes());
          break;
        case OpenURLButtons.URLButtonType.feedbackScreen:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenFeedbackScreen());
          break;
      }
    }
  }

  public void OpenPatchNotes() => this.patchNotesScreen.SetActive(true);

  public void OpenFeedbackScreen() => Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);

  public void OpenURL(string URL) => Application.OpenURL(URL);

  public void OpenPlatformURL(string URL)
  {
    if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
      DistributionPlatform.Inst.GetAuthTicket((DistributionPlatform.AuthTicketHandler) (ticket =>
      {
        string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (Converter<byte, string>) (x => x.ToString("X2"))));
        Application.OpenURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
      }));
    else
      Application.OpenURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(URL.Replace("{SteamID}", "").Replace("{SteamTicket}", ""))));
  }

  public enum URLButtonType
  {
    url,
    platformUrl,
    patchNotes,
    feedbackScreen,
  }

  [Serializable]
  public class URLButtonData
  {
    public string stringKey;
    public OpenURLButtons.URLButtonType urlType;
    public string url;
  }
}
