// Decompiled with JetBrains decompiler
// Type: KMod.Steam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using STRINGS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KMod
{
  public class Steam : IDistributionPlatform, SteamUGCService.IClient
  {
    private Mod MakeMod(SteamUGCService.Mod subscribed)
    {
      if (subscribed == null)
        return (Mod) null;
      if (((int) SteamUGC.GetItemState(subscribed.fileId) & 4) == 0)
        return (Mod) null;
      string id = subscribed.fileId.m_PublishedFileId.ToString();
      Label label = new Label()
      {
        id = id,
        distribution_platform = Label.DistributionPlatform.Steam,
        version = (long) subscribed.lastUpdateTime,
        title = subscribed.title
      };
      string pchFolder;
      if (SteamUGC.GetItemInstallInfo(subscribed.fileId, out ulong _, out pchFolder, 1024U, out uint _))
        return new Mod(label, subscribed.description, (IFileSource) new ZipFile(pchFolder), UI.FRONTEND.MODS.TOOLTIPS.MANAGE_STEAM_SUBSCRIPTION, (System.Action) (() => Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=" + id)));
      Global.Instance.modManager.events.Add(new Event()
      {
        event_type = EventType.InstallInfoInaccessible,
        mod = label
      });
      return (Mod) null;
    }

    public void UpdateMods(
      IEnumerable<PublishedFileId_t> added,
      IEnumerable<PublishedFileId_t> updated,
      IEnumerable<PublishedFileId_t> removed,
      IEnumerable<SteamUGCService.Mod> loaded_previews)
    {
      foreach (PublishedFileId_t publishedFileIdT in added)
      {
        SteamUGCService.Mod mod1 = SteamUGCService.Instance.FindMod(publishedFileIdT);
        if (mod1 == null)
        {
          DebugUtil.DevAssert(false, "SteamUGCService just told us this id was valid!");
        }
        else
        {
          Mod mod2 = this.MakeMod(mod1);
          if (mod2 != null)
            Global.Instance.modManager.Subscribe(mod2, (object) this);
        }
      }
      foreach (PublishedFileId_t publishedFileIdT in updated)
      {
        SteamUGCService.Mod mod1 = SteamUGCService.Instance.FindMod(publishedFileIdT);
        if (mod1 == null)
        {
          DebugUtil.DevAssert(false, "SteamUGCService just told us this id was valid!");
        }
        else
        {
          Mod mod2 = this.MakeMod(mod1);
          if (mod2 != null)
            Global.Instance.modManager.Update(mod2, (object) this);
        }
      }
      foreach (PublishedFileId_t publishedFileIdT in removed)
        Global.Instance.modManager.Unsubscribe(new Label()
        {
          id = publishedFileIdT.m_PublishedFileId.ToString(),
          distribution_platform = Label.DistributionPlatform.Steam
        }, (object) this);
      if (added.Count<PublishedFileId_t>() != 0)
        Global.Instance.modManager.Sanitize((GameObject) null);
      else
        Global.Instance.modManager.Report((GameObject) null);
    }
  }
}
