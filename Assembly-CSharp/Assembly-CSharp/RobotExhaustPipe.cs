// Decompiled with JetBrains decompiler
// Type: RobotExhaustPipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
  private float CO2_RATE = 1f / 1000f;

  public void Sim4000ms(float dt)
  {
    Facing component = this.GetComponent<Facing>();
    bool flip = false;
    if ((bool) (Object) component)
      flip = component.GetFacing();
    CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(this.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
  }
}
