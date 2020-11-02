// Decompiled with JetBrains decompiler
// Type: ReportScreenHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
  [SerializeField]
  private ReportScreenHeaderRow rowTemplate;
  private ReportScreenHeaderRow mainRow;

  public void SetMainEntry(ReportManager.ReportGroup reportGroup)
  {
    if ((Object) this.mainRow == (Object) null)
      this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, this.gameObject, true).GetComponent<ReportScreenHeaderRow>();
    this.mainRow.SetLine(reportGroup);
  }
}
