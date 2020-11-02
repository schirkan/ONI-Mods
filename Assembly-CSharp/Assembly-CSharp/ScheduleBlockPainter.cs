// Decompiled with JetBrains decompiler
// Type: ScheduleBlockPainter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour
{
  [SerializeField]
  private KButtonDrag button;
  private System.Action<float> blockPaintHandler;
  [MyCmpGet]
  private RectTransform rectTransform;

  public void Setup(System.Action<float> blockPaintHandler)
  {
    this.blockPaintHandler = blockPaintHandler;
    this.button.onPointerDown += new System.Action(this.OnPointerDown);
    this.button.onDrag += new System.Action(this.OnDrag);
  }

  private void OnPointerDown() => this.Transmit();

  private void OnDrag() => this.Transmit();

  private void Transmit() => this.blockPaintHandler((this.transform.InverseTransformPoint(Input.mousePosition).x - this.rectTransform.rect.x) / this.rectTransform.rect.width);
}
