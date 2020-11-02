// Decompiled with JetBrains decompiler
// Type: RadiationGermSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/RadiationGermSpawner")]
public class RadiationGermSpawner : KMonoBehaviour
{
  private const float GERM_SCALE = 100f;
  private const int CELLS_PER_UPDATE = 1024;
  private int nextEvaluatedCell;
  private float cellRatio;
  private byte disease_idx;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.cellRatio = (float) (Grid.CellCount / 1024);
    this.disease_idx = byte.MaxValue;
  }

  private void Update()
  {
  }

  private void EvaluateRadiation()
  {
    for (int index1 = 0; index1 < 1024; ++index1)
    {
      int index2 = (this.nextEvaluatedCell + index1) % Grid.CellCount;
      if (Grid.RadiationCount[index2] >= 0)
      {
        int disease_delta1 = Mathf.RoundToInt((float) ((double) Grid.RadiationCount[index2] * 100.0 * ((double) Time.deltaTime * (double) this.cellRatio)));
        if ((int) Grid.DiseaseIdx[index2] == (int) this.disease_idx)
        {
          SimMessages.ModifyDiseaseOnCell(index2, this.disease_idx, disease_delta1);
        }
        else
        {
          int disease_delta2 = Grid.DiseaseCount[index2] - disease_delta1;
          if (disease_delta2 < 0)
            SimMessages.ModifyDiseaseOnCell(index2, this.disease_idx, disease_delta2);
          else
            SimMessages.ModifyDiseaseOnCell(index2, Grid.DiseaseIdx[index2], -disease_delta1);
        }
      }
    }
    this.nextEvaluatedCell = (this.nextEvaluatedCell + 1024) % Grid.CellCount;
  }
}
