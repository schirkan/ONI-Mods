// Decompiled with JetBrains decompiler
// Type: FogOfWarPostFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FogOfWarPostFX : MonoBehaviour
{
  [SerializeField]
  private Shader shader;
  private Material material;
  private Camera myCamera;

  private void Awake()
  {
    this.enabled = SystemInfo.supportsImageEffects;
    if (!((Object) this.shader != (Object) null))
      return;
    this.material = new Material(this.shader);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.SetupUVs();
    Graphics.Blit((Texture) source, destination, this.material, 0);
  }

  private void SetupUVs()
  {
    if ((Object) this.myCamera == (Object) null)
    {
      this.myCamera = this.GetComponent<Camera>();
      if ((Object) this.myCamera == (Object) null)
        return;
    }
    Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
    float distance1 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point1 = ray.GetPoint(distance1);
    Vector4 vector4;
    vector4.x = point1.x / Grid.WidthInMeters;
    vector4.y = point1.y / Grid.HeightInMeters;
    ray = this.myCamera.ViewportPointToRay(Vector3.one);
    float distance2 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point2 = ray.GetPoint(distance2);
    vector4.z = point2.x / Grid.WidthInMeters - vector4.x;
    vector4.w = point2.y / Grid.HeightInMeters - vector4.y;
    this.material.SetVector("_UVOffsetScale", vector4);
  }
}
