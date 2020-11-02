// Decompiled with JetBrains decompiler
// Type: LightSymbolTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
  public HashedString targetSymbol;

  public void RenderEveryTick(float dt)
  {
    Vector3 zero = Vector3.zero;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    Vector3 vector3 = (component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out bool _)).MultiplyPoint(Vector3.zero) - this.transform.GetPosition();
    this.GetComponent<Light2D>().Offset = (Vector2) vector3;
  }
}
