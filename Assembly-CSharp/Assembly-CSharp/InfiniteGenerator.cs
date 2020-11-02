// Decompiled with JetBrains decompiler
// Type: InfiniteGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class InfiniteGenerator : Generator
{
  public override bool IsEmpty => false;

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    this.ApplyDeltaJoules(this.WattageRating * dt);
  }
}
