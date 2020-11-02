// Decompiled with JetBrains decompiler
// Type: CellSelectionInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CellSelectionInstantiator : MonoBehaviour
{
  public GameObject CellSelectionPrefab;

  private void Awake()
  {
    GameObject gameObject1 = Util.KInstantiate(this.CellSelectionPrefab, name: "WorldSelectionCollider");
    GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, name: "WorldSelectionCollider");
    CellSelectionObject component1 = gameObject1.GetComponent<CellSelectionObject>();
    CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
    component1.alternateSelectionObject = component2;
    component2.alternateSelectionObject = component1;
  }
}
