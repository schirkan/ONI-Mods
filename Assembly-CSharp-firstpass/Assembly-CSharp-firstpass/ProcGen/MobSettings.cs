// Decompiled with JetBrains decompiler
// Type: ProcGen.MobSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class MobSettings : IMerge<MobSettings>
  {
    public static float AmbientMobDensity = 1f;
    private TagSet mobkeys;

    public ComposableDictionary<string, Mob> MobLookupTable { get; private set; }

    public MobSettings() => this.MobLookupTable = new ComposableDictionary<string, Mob>();

    public bool HasMob(string id) => this.MobLookupTable.ContainsKey(id);

    public Mob GetMob(string id)
    {
      Mob mob = (Mob) null;
      this.MobLookupTable.TryGetValue(id, out mob);
      return mob;
    }

    public TagSet GetMobTags()
    {
      if (this.mobkeys == null)
      {
        this.mobkeys = new TagSet();
        foreach (string key in (IEnumerable<string>) this.MobLookupTable.Keys)
          this.mobkeys.Add(new Tag(key));
      }
      return this.mobkeys;
    }

    public void Merge(MobSettings other)
    {
      this.MobLookupTable.Merge(other.MobLookupTable);
      this.mobkeys = (TagSet) null;
    }
  }
}
