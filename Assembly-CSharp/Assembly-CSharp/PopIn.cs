﻿// Decompiled with JetBrains decompiler
// Type: PopIn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PopIn : MonoBehaviour
{
  private float targetScale;
  public float speed;

  private void OnEnable() => this.StartPopIn(true);

  private void Update()
  {
    float num = Mathf.Lerp(this.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
    this.transform.localScale = new Vector3(num, num, 1f);
  }

  public void StartPopIn(bool force_reset = false)
  {
    if (force_reset)
      this.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    this.targetScale = 1f;
  }

  public void StartPopOut() => this.targetScale = 0.0f;
}
