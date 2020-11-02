// Decompiled with JetBrains decompiler
// Type: Insulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
  [MyCmpReq]
  private Building building;
  [SerializeField]
  public CellOffset offset = CellOffset.none;

  protected override void OnSpawn() => SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);

  protected override void OnCleanUp() => SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), 1f);
}
