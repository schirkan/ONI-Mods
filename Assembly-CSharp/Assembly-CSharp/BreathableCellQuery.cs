// Decompiled with JetBrains decompiler
// Type: BreathableCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class BreathableCellQuery : PathFinderQuery
{
  private OxygenBreather breather;

  public BreathableCellQuery Reset(Brain brain)
  {
    this.breather = brain.GetComponent<OxygenBreather>();
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost) => this.breather.IsBreathableElementAtCell(cell);
}
