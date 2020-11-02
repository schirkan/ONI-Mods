﻿// Decompiled with JetBrains decompiler
// Type: GridCompositor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GridCompositor : MonoBehaviour
{
  public Material material;
  public static GridCompositor Instance;
  private bool onMajor;
  private bool onMinor;

  public static void DestroyInstance() => GridCompositor.Instance = (GridCompositor) null;

  private void Awake()
  {
    GridCompositor.Instance = this;
    this.enabled = false;
  }

  private void Start() => this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));

  private void OnRenderImage(RenderTexture src, RenderTexture dest) => Graphics.Blit((Texture) src, dest, this.material);

  public void ToggleMajor(bool on)
  {
    this.onMajor = on;
    this.Refresh();
  }

  public void ToggleMinor(bool on)
  {
    this.onMinor = on;
    this.Refresh();
  }

  private void Refresh() => this.enabled = this.onMinor || this.onMajor;
}
