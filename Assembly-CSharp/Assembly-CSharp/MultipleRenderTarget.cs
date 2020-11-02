// Decompiled with JetBrains decompiler
// Type: MultipleRenderTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class MultipleRenderTarget : MonoBehaviour
{
  private MultipleRenderTargetProxy renderProxy;
  private FullScreenQuad quad;
  public bool isFrontEnd;

  public event System.Action<Camera> onSetupComplete;

  private void Start() => this.StartCoroutine(this.SetupProxy());

  private IEnumerator SetupProxy()
  {
    MultipleRenderTarget multipleRenderTarget = this;
    yield return (object) null;
    Camera component = multipleRenderTarget.GetComponent<Camera>();
    Camera camera = new GameObject().AddComponent<Camera>();
    camera.CopyFrom(component);
    multipleRenderTarget.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
    camera.name = component.name + " MRT";
    camera.transform.parent = component.transform;
    camera.transform.SetLocalPosition(Vector3.zero);
    camera.depth = component.depth - 1f;
    component.cullingMask = 0;
    component.clearFlags = CameraClearFlags.Color;
    multipleRenderTarget.quad = new FullScreenQuad(nameof (MultipleRenderTarget), component, true);
    if (multipleRenderTarget.onSetupComplete != null)
      multipleRenderTarget.onSetupComplete(camera);
  }

  private void OnPreCull()
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.quad.Draw((Texture) this.renderProxy.Textures[0]);
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.renderProxy.ToggleColouredOverlayView(enabled);
  }
}
