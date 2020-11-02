// Decompiled with JetBrains decompiler
// Type: SplashMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class SplashMessageScreen : KMonoBehaviour
{
  public KButton confirmButton;
  public LayoutElement bodyText;
  public bool previewInEditor;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.confirmButton.onClick += (System.Action) (() =>
    {
      this.gameObject.SetActive(false);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
    });
  }

  private void OnEnable()
  {
    LayoutElement component = this.confirmButton.GetComponent<LayoutElement>();
    LocText componentInChildren = this.confirmButton.GetComponentInChildren<LocText>();
    if (Screen.width > 2560)
    {
      component.minWidth = 720f;
      component.minHeight = 128f;
      this.bodyText.minWidth = 840f;
      componentInChildren.fontSizeMax = 24f;
    }
    else if (Screen.width > 1920)
    {
      component.minWidth = 720f;
      component.minHeight = 128f;
      this.bodyText.minWidth = 700f;
      componentInChildren.fontSizeMax = 24f;
    }
    else if (Screen.width > 1280)
    {
      component.minWidth = 440f;
      component.minHeight = 64f;
      this.bodyText.minWidth = 480f;
      componentInChildren.fontSizeMax = 18f;
    }
    else
    {
      component.minWidth = 300f;
      component.minHeight = 48f;
      this.bodyText.minWidth = 300f;
      componentInChildren.fontSizeMax = 16f;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
    this.StartCoroutine(this.ShowMessage());
  }

  private IEnumerator ShowMessage()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SplashMessageScreen splashMessageScreen = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      splashMessageScreen.GetComponentInChildren<KScreen>(true).Show();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
