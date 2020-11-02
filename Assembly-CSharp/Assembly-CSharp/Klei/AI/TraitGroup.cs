// Decompiled with JetBrains decompiler
// Type: Klei.AI.TraitGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI
{
  public class TraitGroup : ModifierGroup<Trait>
  {
    public bool IsSpawnTrait;

    public TraitGroup(string id, string name, bool is_spawn_trait)
      : base(id, name)
      => this.IsSpawnTrait = is_spawn_trait;
  }
}
