// Decompiled with JetBrains decompiler
// Type: KleiAccount
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class KleiAccount : ThreadedHttps<KleiAccount>
{
  private const string TicketFieldName = "SteamTicket";
  public const string KleiAccountKey = "KleiAccount";
  private const string GameIDFieldName = "Game";
  private const string EmailFieldName = "NoEmail";
  private const string ErrorFieldName = "Error";
  private const string UserIDFieldName = "UserID";
  public static string KleiUserID;
  private KleiAccount.GetUserIDdelegate gotUserID;
  private const string AuthTicketKey = "AUTH_TICKET";
  private byte[] authTicket;

  private Dictionary<string, object> BuildLoginRequest(byte[] ticket) => new Dictionary<string, object>()
  {
    {
      "SteamTicket",
      (object) this.EncodeToAsciiHEX(ticket)
    },
    {
      "Game",
      (object) this.CLIENT_KEY
    },
    {
      "NoEmail",
      (object) true
    }
  };

  public KleiAccount()
  {
    this.CLIENT_KEY = "ONI";
    this.LIVE_ENDPOINT = "login.kleientertainment.com" + DistributionPlatform.Inst.AccountLoginEndpoint;
    this.serviceName = nameof (KleiAccount);
    this.ClearAuthTicket();
  }

  protected override void OnReplyRecieved(WebResponse response)
  {
    if (response == null)
    {
      KleiAccount.KleiUserID = (string) null;
      this.gotUserID();
    }
    else
    {
      Stream responseStream = response.GetResponseStream();
      StreamReader streamReader = new StreamReader(responseStream);
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      responseStream.Close();
      KleiAccount.AccountReply accountReply = JsonConvert.DeserializeObject<KleiAccount.AccountReply>(end);
      if (!accountReply.Error)
      {
        Debug.Log((object) ("[Account] Got login for user " + accountReply.UserID));
        KleiAccount.KleiUserID = accountReply.UserID == "" ? (string) null : accountReply.UserID;
        this.gotUserID();
      }
      else
      {
        Debug.Log((object) ("[Account] Error logging in: " + end));
        this.gotUserID();
      }
      this.End();
    }
  }

  private string EncodeToAsciiHEX(byte[] data)
  {
    string str = "";
    for (int index = 0; index < data.Length; ++index)
      str += data[index].ToString("X2");
    return str;
  }

  public string PostRawData(Dictionary<string, object> data)
  {
    this.PutPacket(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) data)));
    return "OK";
  }

  public void AuthenticateUser(KleiAccount.GetUserIDdelegate cb)
  {
    if (KleiAccount.KleiUserID == null)
    {
      Debug.Log((object) ("[Account] Requesting auth ticket from " + DistributionPlatform.Inst.Name));
      this.gotUserID = cb;
      byte[] ticket = this.AuthTicket();
      if (ticket == null || ticket.Length == 0)
      {
        if (!DistributionPlatform.Initialized)
          return;
        DistributionPlatform.Inst.GetAuthTicket(new DistributionPlatform.AuthTicketHandler(this.OnAuthTicketObtained));
      }
      else
        this.OnAuthTicketObtained(ticket);
    }
    else
      cb();
  }

  public void OnAuthTicketObtained(byte[] ticket)
  {
    if (ticket.Length != 0)
    {
      byte[] ticket1 = new byte[ticket.Length];
      Array.Copy((Array) ticket, (Array) ticket1, ticket.Length);
      this.SetAuthTicket(ticket1);
      this.Start();
      this.PostRawData(this.BuildLoginRequest(ticket1));
    }
    else
      this.gotUserID();
  }

  public byte[] AuthTicket() => this.authTicket;

  public void SetAuthTicket(byte[] ticket) => this.authTicket = ticket;

  public void ClearAuthTicket() => this.authTicket = (byte[]) null;

  private struct AccountReply
  {
    public string UserID;
    public string Token;
    public bool Error;
    public string SupplementaryData;
  }

  public delegate void GetUserIDdelegate();
}
