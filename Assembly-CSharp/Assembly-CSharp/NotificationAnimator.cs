﻿// Decompiled with JetBrains decompiler
// Type: NotificationAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class NotificationAnimator : MonoBehaviour
{
  private const float START_SPEED = 1f;
  private const float ACCELERATION = 0.5f;
  private const float BOUNCE_DAMPEN = 2f;
  private const int BOUNCE_COUNT = 2;
  private const float OFFSETX = 100f;
  private float speed = 1f;
  private int bounceCount = 2;
  private LayoutElement layoutElement;

  public void Init()
  {
    this.layoutElement = this.GetComponent<LayoutElement>();
    this.layoutElement.minWidth = 100f;
  }

  private void LateUpdate()
  {
    this.layoutElement.minWidth -= this.speed;
    this.speed += 0.5f;
    if ((double) this.layoutElement.minWidth > 0.0)
      return;
    if (this.bounceCount > 0)
    {
      --this.bounceCount;
      this.speed = -this.speed / Mathf.Pow(2f, (float) (2 - this.bounceCount));
      this.layoutElement.minWidth = -this.speed;
    }
    else
    {
      this.layoutElement.minWidth = 0.0f;
      this.enabled = false;
    }
  }
}
