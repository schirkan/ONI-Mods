// Decompiled with JetBrains decompiler
// Type: Klei.AI.Sicknesses
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class Sicknesses : Modifications<Sickness, SicknessInstance>
  {
    public Sicknesses(GameObject go)
      : base(go, (ResourceSet<Sickness>) Db.Get().Sicknesses)
    {
    }

    public void Infect(SicknessExposureInfo exposure_info)
    {
      Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
      if (this.Has(modifier))
        return;
      this.CreateInstance(modifier).ExposureInfo = exposure_info;
    }

    public override SicknessInstance CreateInstance(Sickness sickness)
    {
      SicknessInstance instance = new SicknessInstance(this.gameObject, sickness);
      this.Add(instance);
      this.Trigger(GameHashes.SicknessAdded, (object) instance);
      ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, this.gameObject.GetProperName());
      return instance;
    }

    public bool IsInfected() => this.Count > 0;

    public bool Cure(Sickness sickness) => this.Cure(sickness.Id);

    public bool Cure(string sickness_id)
    {
      SicknessInstance instance = (SicknessInstance) null;
      foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) this)
      {
        if (sicknessInstance.modifier.Id == sickness_id)
        {
          instance = sicknessInstance;
          break;
        }
      }
      if (instance == null)
        return false;
      this.Remove(instance);
      this.Trigger(GameHashes.SicknessCured, (object) instance);
      ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, this.gameObject.GetProperName());
      return true;
    }
  }
}
