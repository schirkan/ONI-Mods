// Decompiled with JetBrains decompiler
// Type: ResearchPointObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ResearchPointObject")]
public class ResearchPointObject : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public string TypeID = "";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Research.Instance.AddResearchPoints(this.TypeID, 1f);
    ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, this.transform);
    Util.KDestroyGameObject(this.gameObject);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
    descriptorList.Add(new Descriptor(string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, (object) researchType.name), string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, (object) researchType.description)));
    return descriptorList;
  }
}
