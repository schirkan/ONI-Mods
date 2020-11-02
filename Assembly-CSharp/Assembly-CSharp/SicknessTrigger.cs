// Decompiled with JetBrains decompiler
// Type: SicknessTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SicknessTrigger")]
public class SicknessTrigger : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public List<SicknessTrigger.TriggerInfo> triggers = new List<SicknessTrigger.TriggerInfo>();

  public void AddTrigger(
    GameHashes src_event,
    string[] sickness_ids,
    SicknessTrigger.SourceCallback source_callback)
  {
    this.triggers.Add(new SicknessTrigger.TriggerInfo()
    {
      srcEvent = src_event,
      sickness_ids = sickness_ids,
      sourceCallback = source_callback
    });
  }

  protected override void OnSpawn()
  {
    for (int index = 0; index < this.triggers.Count; ++index)
    {
      SicknessTrigger.TriggerInfo trigger = this.triggers[index];
      this.Subscribe((int) trigger.srcEvent, (System.Action<object>) (data => this.OnSicknessTrigger((GameObject) data, trigger)));
    }
  }

  private void OnSicknessTrigger(GameObject target, SicknessTrigger.TriggerInfo trigger)
  {
    int index = UnityEngine.Random.Range(0, trigger.sickness_ids.Length);
    string sicknessId = trigger.sickness_ids[index];
    Sickness sickness = (Sickness) null;
    Database.Sicknesses sicknesses = Db.Get().Sicknesses;
    for (int idx = 0; idx < sicknesses.Count; ++idx)
    {
      if (sicknesses[idx].Id == sicknessId)
      {
        sickness = sicknesses[idx];
        break;
      }
    }
    if (sickness != null)
    {
      string infection_source_info = trigger.sourceCallback(this.gameObject, target);
      SicknessExposureInfo exposure_info = new SicknessExposureInfo(sickness.Id, infection_source_info);
      target.GetComponent<MinionModifiers>().sicknesses.Infect(exposure_info);
    }
    else
      DebugUtil.DevLogErrorFormat((UnityEngine.Object) this.gameObject, "Couldn't find sickness with id [{0}]", (object) sicknessId);
  }

  public List<Descriptor> EffectDescriptors(GameObject go)
  {
    Dictionary<GameHashes, HashSet<string>> dictionary = new Dictionary<GameHashes, HashSet<string>>();
    foreach (SicknessTrigger.TriggerInfo trigger in this.triggers)
    {
      HashSet<string> stringSet = (HashSet<string>) null;
      if (!dictionary.TryGetValue(trigger.srcEvent, out stringSet))
      {
        stringSet = new HashSet<string>();
        dictionary[trigger.srcEvent] = stringSet;
      }
      foreach (string sicknessId in trigger.sickness_ids)
        stringSet.Add(sicknessId);
    }
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<string> stringList = new List<string>();
    string properName = this.GetComponent<KSelectable>().GetProperName();
    foreach (KeyValuePair<GameHashes, HashSet<string>> keyValuePair in dictionary)
    {
      HashSet<string> stringSet = keyValuePair.Value;
      stringList.Clear();
      foreach (string id in stringSet)
      {
        Sickness sickness = Db.Get().Sicknesses.TryGet(id);
        stringList.Add(sickness.Name);
      }
      string newValue = string.Join(", ", stringList.ToArray());
      string str1 = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS." + Enum.GetName(typeof (GameHashes), (object) keyValuePair.Key).ToUpper()).String;
      string str2 = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS.TOOLTIPS." + Enum.GetName(typeof (GameHashes), (object) keyValuePair.Key).ToUpper()).String;
      string txt = str1.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
      string tooltip = str2.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
      descriptorList.Add(new Descriptor(txt, tooltip));
    }
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => this.EffectDescriptors(go);

  public delegate string SourceCallback(GameObject source, GameObject target);

  [Serializable]
  public struct TriggerInfo
  {
    [HashedEnum]
    public GameHashes srcEvent;
    public string[] sickness_ids;
    public SicknessTrigger.SourceCallback sourceCallback;
  }
}
