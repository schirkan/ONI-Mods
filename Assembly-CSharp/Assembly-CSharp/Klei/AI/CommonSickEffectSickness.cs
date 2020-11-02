// Decompiled with JetBrains decompiler
// Type: Klei.AI.CommonSickEffectSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class CommonSickEffectSickness : Sickness.SicknessComponent
  {
    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      KBatchedAnimController effect = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0.0f, 0.0f, -0.1f), go.transform, true);
      effect.Play((HashedString) "fx_loop", KAnim.PlayMode.Loop);
      return (object) effect;
    }

    public override void OnCure(GameObject go, object instance_data) => ((Component) instance_data).gameObject.DeleteObject();
  }
}
