// Decompiled with JetBrains decompiler
// Type: MedicineInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class MedicineInfo
{
  public string id;
  public string effect;
  public MedicineInfo.MedicineType medicineType;
  public List<string> curedSicknesses;

  public MedicineInfo(
    string id,
    string effect,
    MedicineInfo.MedicineType medicineType,
    string[] curedDiseases = null)
  {
    Debug.Assert(!string.IsNullOrEmpty(effect) || curedDiseases != null && (uint) curedDiseases.Length > 0U, (object) "Medicine should have an effect or cure diseases");
    this.id = id;
    this.effect = effect;
    this.medicineType = medicineType;
    if (curedDiseases != null)
      this.curedSicknesses = new List<string>((IEnumerable<string>) curedDiseases);
    else
      this.curedSicknesses = new List<string>();
  }

  public enum MedicineType
  {
    Booster,
    CureAny,
    CureSpecific,
  }
}
