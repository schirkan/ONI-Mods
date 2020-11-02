// Decompiled with JetBrains decompiler
// Type: Facing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
  [MyCmpGet]
  private KAnimControllerBase kanimController;
  private LoggerFS log;
  private bool facingLeft;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.log = new LoggerFS(nameof (Facing));
  }

  public void Face(float target_x)
  {
    float x = this.transform.GetLocalPosition().x;
    if ((double) target_x < (double) x)
    {
      this.facingLeft = true;
      this.UpdateMirror();
    }
    else
    {
      if ((double) target_x <= (double) x)
        return;
      this.facingLeft = false;
      this.UpdateMirror();
    }
  }

  public void Face(Vector3 target_pos)
  {
    int num1 = Grid.CellColumn(Grid.PosToCell(this.transform.GetLocalPosition()));
    int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
    if (num1 > num2)
    {
      this.facingLeft = true;
      this.UpdateMirror();
    }
    else
    {
      if (num2 <= num1)
        return;
      this.facingLeft = false;
      this.UpdateMirror();
    }
  }

  [ContextMenu("Flip")]
  public void SwapFacing()
  {
    this.facingLeft = !this.facingLeft;
    this.UpdateMirror();
  }

  private void UpdateMirror()
  {
    if (!((Object) this.kanimController != (Object) null) || this.kanimController.FlipX == this.facingLeft)
      return;
    this.kanimController.FlipX = this.facingLeft;
    int num = this.facingLeft ? 1 : 0;
  }

  public bool GetFacing() => this.facingLeft;

  public void SetFacing(bool mirror_x)
  {
    this.facingLeft = mirror_x;
    this.UpdateMirror();
  }

  public int GetFrontCell()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    return this.GetFacing() ? Grid.CellLeft(cell) : Grid.CellRight(cell);
  }
}
