// Decompiled with JetBrains decompiler
// Type: HealthyGameMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HealthyGameMessageScreen")]
public class HealthyGameMessageScreen : KMonoBehaviour
{
  public KButton confirmButton;
  public CanvasGroup canvasGroup;
  private float spawnTime;
  private float totalTime = 10f;
  private float fadeTime = 1.5f;
  private bool isFirstUpdate = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.confirmButton.onClick += (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject));
    this.confirmButton.gameObject.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void Update()
  {
    if (this.isFirstUpdate)
    {
      this.isFirstUpdate = false;
      this.spawnTime = Time.unscaledTime;
    }
    else
    {
      float num1 = Mathf.Min(Time.unscaledDeltaTime, 0.03333334f);
      float num2 = Time.unscaledTime - this.spawnTime;
      if ((double) num2 < (double) this.totalTime - (double) this.fadeTime)
        this.canvasGroup.alpha += num1 * (1f / this.fadeTime);
      else if ((double) num2 >= (double) this.totalTime + 0.75)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        if ((double) num2 < (double) this.totalTime - (double) this.fadeTime)
          return;
        this.canvasGroup.alpha -= num1 * (1f / this.fadeTime);
      }
    }
  }
}
