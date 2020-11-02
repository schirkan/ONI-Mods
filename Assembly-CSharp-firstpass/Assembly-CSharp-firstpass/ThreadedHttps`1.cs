// Decompiled with JetBrains decompiler
// Type: ThreadedHttps`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public class ThreadedHttps<T> where T : class, new()
{
  protected string serviceName;
  protected string CLIENT_KEY;
  protected string LIVE_ENDPOINT;
  private bool certFail;
  private const int retryCount = 3;
  protected Thread updateThread;
  protected List<byte[]> packets = new List<byte[]>();
  private EventWaitHandle _waitHandle = (EventWaitHandle) new AutoResetEvent(false);
  protected bool shouldQuit;
  protected bool quitOnError;
  private object _quitLock = new object();
  protected bool singleSend;

  public static T Instance => ThreadedHttps<T>.Singleton.instance;

  public bool RemoteCertificateValidationCallback(
    object sender,
    X509Certificate certificate,
    X509Chain chain,
    SslPolicyErrors sslPolicyErrors)
  {
    if (this.certFail)
      return false;
    this.certFail = true;
    string str = "";
    switch (sslPolicyErrors)
    {
      case SslPolicyErrors.None:
        this.certFail = false;
        break;
      case SslPolicyErrors.RemoteCertificateChainErrors:
        this.certFail = false;
        for (int index = 0; index < chain.ChainStatus.Length; ++index)
        {
          str = str + "[" + (object) index + "] " + chain.ChainStatus[index].Status.ToString() + "\n";
          if (chain.ChainStatus[index].Status != X509ChainStatusFlags.RevocationStatusUnknown)
          {
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
            if (!chain.Build((X509Certificate2) certificate))
              this.certFail = true;
          }
        }
        break;
      default:
        this.certFail = true;
        break;
    }
    if (this.certFail)
    {
      X509Certificate2 x509Certificate2 = new X509Certificate2(certificate);
      Debug.LogWarning((object) (this.serviceName + ": " + sslPolicyErrors.ToString() + "\n" + str + "\n" + x509Certificate2.ToString()));
    }
    return !this.certFail;
  }

  public void Start()
  {
    if (this.updateThread != null)
      this.End();
    if (this.certFail)
      return;
    this.packets = new List<byte[]>();
    this.shouldQuit = false;
    this.updateThread = new Thread(new ThreadStart(this.SendData));
    this.updateThread.Start();
  }

  public void End()
  {
    this.Quit();
    if (this.updateThread == null)
      return;
    if (!this.updateThread.Join(TimeSpan.FromSeconds(2.0)))
      this.updateThread.Abort();
    this.updateThread = (Thread) null;
  }

  protected virtual void OnReplyRecieved(WebResponse response)
  {
  }

  protected string Send(byte[] byteArray, bool isForce = false)
  {
    ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(this.RemoteCertificateValidationCallback);
    string str = "";
    int num = 0;
    while (true)
    {
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://" + this.LIVE_ENDPOINT);
        httpWebRequest.AllowAutoRedirect = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) byteArray.Length;
        Stream requestStream;
        try
        {
          requestStream = httpWebRequest.GetRequestStream();
        }
        catch (WebException ex)
        {
          string message = ex.Message;
          str = DateTime.Now.ToLongTimeString() + " " + this.serviceName + ": Exception getting Request Stream:" + message;
          Debug.LogWarning((object) str);
          throw;
        }
        try
        {
          requestStream.Write(byteArray, 0, byteArray.Length);
        }
        catch (WebException ex)
        {
          string message = ex.Message;
          str = DateTime.Now.ToLongTimeString() + " " + this.serviceName + ": Exception writing data to Stream:" + message;
          Debug.LogWarning((object) str);
          throw;
        }
        requestStream.Close();
        WebResponse response1;
        try
        {
          response1 = httpWebRequest.GetResponse();
        }
        catch (WebException ex)
        {
          string message = ex.Message;
          WebResponse response2 = ex.Response;
          if (response2 != null)
          {
            using (Stream responseStream = response2.GetResponseStream())
              str = new StreamReader(responseStream).ReadToEnd();
          }
          else
            str = " -- we.Response is NULL";
          str = DateTime.Now.ToLongTimeString() + " " + this.serviceName + ": Exception getting response:" + message + str;
          Debug.LogWarning((object) str);
          throw;
        }
        str = ((HttpWebResponse) response1).StatusDescription;
        if (str != "OK")
        {
          Stream responseStream = response1.GetResponseStream();
          StreamReader streamReader = new StreamReader(responseStream);
          string end = streamReader.ReadToEnd();
          streamReader.Close();
          responseStream.Close();
          str = this.serviceName + ": Server Responded with Status: [" + str + "] Response: " + end;
        }
        else
          this.OnReplyRecieved(response1);
        response1.Close();
        break;
      }
      catch (Exception ex)
      {
        if (!this.shouldQuit)
        {
          if (this.certFail)
          {
            Debug.LogWarning((object) (this.serviceName + ": Cert fail, quitting"));
            try
            {
              this.OnReplyRecieved((WebResponse) null);
            }
            catch
            {
            }
            this.QuitOnError();
            break;
          }
          ++num;
          if (num > 3)
          {
            str = DateTime.Now.ToLongTimeString() + " " + this.serviceName + ": Max Retries (" + (object) 3 + ") reached. Disabling " + this.serviceName + "...";
            Debug.LogWarning((object) str);
            try
            {
              this.OnReplyRecieved((WebResponse) null);
            }
            catch
            {
            }
            this.QuitOnError();
            break;
          }
          string message = ex.Message;
          string stackTrace = ex.StackTrace;
          TimeSpan timeout = TimeSpan.FromSeconds(Math.Pow(2.0, (double) (num + 3)));
          str = DateTime.Now.ToLongTimeString() + " " + this.serviceName + ": Exception (retrying in " + (object) timeout.TotalSeconds + " seconds): " + message + "\n" + stackTrace;
          Debug.LogWarning((object) str);
          if (isForce)
          {
            Debug.LogWarning((object) ex.StackTrace);
            break;
          }
          Thread.Sleep(timeout);
        }
      }
    }
    ServicePointManager.ServerCertificateValidationCallback -= new System.Net.Security.RemoteCertificateValidationCallback(this.RemoteCertificateValidationCallback);
    return str;
  }

  protected bool ShouldQuit()
  {
    lock (this._quitLock)
      return this.shouldQuit && this.packets.Count == 0 || this.quitOnError;
  }

  protected void QuitOnError()
  {
    lock (this._quitLock)
    {
      this.quitOnError = true;
      this.shouldQuit = true;
    }
  }

  protected void Quit()
  {
    lock (this._quitLock)
      this.shouldQuit = true;
  }

  protected byte[] GetPacket()
  {
    byte[] numArray = (byte[]) null;
    lock (this.packets)
    {
      if (this.packets.Count > 0)
      {
        numArray = this.packets[0];
        this.packets.RemoveAt(0);
      }
    }
    return numArray;
  }

  protected void PutPacket(byte[] packet, bool infront = false)
  {
    lock (this.packets)
    {
      if (infront)
      {
        this.packets.Insert(0, packet);
      }
      else
      {
        this.packets.Add(packet);
        this._waitHandle.Set();
      }
    }
  }

  public void ForceSendData()
  {
    for (byte[] packet = this.GetPacket(); packet != null; packet = this.GetPacket())
    {
      if (this.Send(packet, true) != "OK")
      {
        this.PutPacket(packet, true);
        break;
      }
    }
  }

  protected void SendData()
  {
    while (!this.ShouldQuit())
    {
      byte[] packet = this.GetPacket();
      if (packet != null)
      {
        if (this.Send(packet) != "OK")
          this.PutPacket(packet, true);
      }
      else
        this._waitHandle.WaitOne();
      if (this.singleSend)
        break;
    }
  }

  private class Singleton
  {
    internal static readonly T instance = new T();
  }
}
