// Decompiled with JetBrains decompiler
// Type: RenderTextureDestroyer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Plugins/RenderTextureDestroyer")]
public class RenderTextureDestroyer : KMonoBehaviour
{
  public static RenderTextureDestroyer Instance;
  public List<RenderTexture> queued = new List<RenderTexture>();
  public List<RenderTexture> finished = new List<RenderTexture>();

  public static void DestroyInstance() => RenderTextureDestroyer.Instance = (RenderTextureDestroyer) null;

  protected override void OnPrefabInit() => RenderTextureDestroyer.Instance = this;

  public void Add(RenderTexture render_texture) => this.queued.Add(render_texture);

  private void LateUpdate()
  {
    foreach (Object @object in this.finished)
      Object.Destroy(@object);
    this.finished.Clear();
    this.finished.AddRange((IEnumerable<RenderTexture>) this.queued);
    this.queued.Clear();
  }
}
