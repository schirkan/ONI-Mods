// Decompiled with JetBrains decompiler
// Type: LightShapePreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
  public float radius;
  public int lux;
  public LightShape shape;
  public CellOffset offset;
  private int previousCell = -1;

  private void Update()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.previousCell)
      return;
    this.previousCell = cell;
    LightGridManager.DestroyPreview();
    LightGridManager.CreatePreview(Grid.OffsetCell(cell, this.offset), this.radius, this.shape, this.lux);
  }

  protected override void OnCleanUp() => LightGridManager.DestroyPreview();
}
