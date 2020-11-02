// Decompiled with JetBrains decompiler
// Type: HeatBulb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
  [SerializeField]
  private float minTemperature;
  [SerializeField]
  private float kjConsumptionRate;
  [SerializeField]
  private float lightKJConsumptionRate;
  [SerializeField]
  private Vector2I minCheckOffset;
  [SerializeField]
  private Vector2I maxCheckOffset;
  [MyCmpGet]
  private Light2D lightSource;
  [MyCmpGet]
  private KBatchedAnimController kanim;
  [Serialize]
  private float kjConsumed;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.kanim.Play((HashedString) "off");
  }

  public void Sim200ms(float dt)
  {
    double num1 = (double) this.kjConsumptionRate * (double) dt;
    Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
    double num2 = (double) (vector2I.x * vector2I.y);
    float num3 = (float) (num1 / num2);
    int x1;
    int y1;
    Grid.PosToXY(this.transform.GetPosition(), out x1, out y1);
    for (int y2 = this.minCheckOffset.y; y2 <= this.maxCheckOffset.y; ++y2)
    {
      for (int x2 = this.minCheckOffset.x; x2 <= this.maxCheckOffset.x; ++x2)
      {
        int cell = Grid.XYToCell(x1 + x2, y1 + y2);
        if (Grid.IsValidCell(cell) && (double) Grid.Temperature[cell] > (double) this.minTemperature)
        {
          this.kjConsumed += num3;
          SimMessages.ModifyEnergy(cell, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
        }
      }
    }
    float num4 = this.lightKJConsumptionRate * dt;
    if ((double) this.kjConsumed > (double) num4)
    {
      if (!this.lightSource.enabled)
      {
        this.kanim.Play((HashedString) "open");
        this.kanim.Queue((HashedString) "on");
        this.lightSource.enabled = true;
      }
      this.kjConsumed -= num4;
    }
    else
    {
      if (this.lightSource.enabled)
      {
        this.kanim.Play((HashedString) "close");
        this.kanim.Queue((HashedString) "off");
      }
      this.lightSource.enabled = false;
    }
  }
}
