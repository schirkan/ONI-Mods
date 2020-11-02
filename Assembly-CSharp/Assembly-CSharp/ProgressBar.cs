﻿// Decompiled with JetBrains decompiler
// Type: ProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
  public Image bar;
  private Func<float> updatePercentFull;
  private int overlayUpdateHandle = -1;
  public bool autoHide = true;

  public Color barColor
  {
    get => this.bar.color;
    set => this.bar.color = value;
  }

  public float PercentFull
  {
    get => this.bar.fillAmount;
    set => this.bar.fillAmount = value;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.autoHide)
    {
      this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new System.Action<object>(this.OnOverlayChanged));
      if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.GetMode() != OverlayModes.None.ID)
        this.gameObject.SetActive(false);
    }
    this.enabled = this.updatePercentFull != null;
  }

  public void SetUpdateFunc(Func<float> func)
  {
    this.updatePercentFull = func;
    this.enabled = this.updatePercentFull != null;
  }

  public virtual void Update()
  {
    if (this.updatePercentFull == null)
      return;
    this.PercentFull = this.updatePercentFull();
  }

  public virtual void OnOverlayChanged(object data = null)
  {
    if (!this.autoHide)
      return;
    if ((HashedString) data == OverlayModes.None.ID)
    {
      if (this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(true);
    }
    else
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }
  }

  protected override void OnCleanUp()
  {
    if (this.overlayUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.overlayUpdateHandle);
    base.OnCleanUp();
  }

  private void OnBecameInvisible() => this.enabled = false;

  private void OnBecameVisible() => this.enabled = true;

  public static ProgressBar CreateProgressBar(
    KMonoBehaviour entity,
    Func<float> updateFunc)
  {
    ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab);
    progressBar.SetUpdateFunc(updateFunc);
    progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
    progressBar.name = ((UnityEngine.Object) entity != (UnityEngine.Object) null ? entity.name + "_" : "") + " ProgressBar";
    progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor(nameof (ProgressBar));
    progressBar.Update();
    Vector3 vector3 = entity.transform.GetPosition() + Vector3.down * 0.5f;
    Building component = entity.GetComponent<Building>();
    progressBar.transform.SetPosition(!((UnityEngine.Object) component != (UnityEngine.Object) null) ? vector3 - Vector3.right * 0.5f : vector3 - Vector3.right * 0.5f * (float) (component.Def.WidthInCells % 2) + component.Def.placementPivot);
    return progressBar;
  }
}
