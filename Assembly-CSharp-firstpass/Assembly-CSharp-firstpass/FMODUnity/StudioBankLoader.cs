// Decompiled with JetBrains decompiler
// Type: FMODUnity.StudioBankLoader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
  [AddComponentMenu("FMOD Studio/FMOD Studio Bank Loader")]
  public class StudioBankLoader : MonoBehaviour
  {
    public LoaderGameEvent LoadEvent;
    public LoaderGameEvent UnloadEvent;
    [BankRef]
    public List<string> Banks;
    public string CollisionTag;
    public bool PreloadSamples;
    private bool isQuitting;

    private void HandleGameEvent(LoaderGameEvent gameEvent)
    {
      if (this.LoadEvent == gameEvent)
        this.Load();
      if (this.UnloadEvent != gameEvent)
        return;
      this.Unload();
    }

    private void Start()
    {
      RuntimeUtils.EnforceLibraryOrder();
      this.HandleGameEvent(LoaderGameEvent.ObjectStart);
    }

    private void OnApplicationQuit() => this.isQuitting = true;

    private void OnDestroy()
    {
      if (this.isQuitting)
        return;
      this.HandleGameEvent(LoaderGameEvent.ObjectDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(LoaderGameEvent.TriggerEnter);
    }

    private void OnTriggerExit(Collider other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(LoaderGameEvent.TriggerExit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(LoaderGameEvent.TriggerEnter2D);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!string.IsNullOrEmpty(this.CollisionTag) && !other.CompareTag(this.CollisionTag))
        return;
      this.HandleGameEvent(LoaderGameEvent.TriggerExit2D);
    }

    public void Load()
    {
      foreach (string bank in this.Banks)
      {
        try
        {
          RuntimeManager.LoadBank(bank, this.PreloadSamples);
        }
        catch (BankLoadException ex)
        {
          Debug.LogException((Exception) ex);
        }
      }
      RuntimeManager.WaitForAllLoads();
    }

    public void Unload()
    {
      foreach (string bank in this.Banks)
        RuntimeManager.UnloadBank(bank);
    }
  }
}
