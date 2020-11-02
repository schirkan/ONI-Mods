// Decompiled with JetBrains decompiler
// Type: Database.SkillPerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class SkillPerk : Resource
  {
    public System.Action<MinionResume> OnApply { get; protected set; }

    public System.Action<MinionResume> OnRemove { get; protected set; }

    public System.Action<MinionResume> OnMinionsChanged { get; protected set; }

    public bool affectAll { get; protected set; }

    public SkillPerk(
      string id_str,
      string description,
      System.Action<MinionResume> OnApply,
      System.Action<MinionResume> OnRemove,
      System.Action<MinionResume> OnMinionsChanged,
      bool affectAll = false)
      : base(id_str, description)
    {
      this.OnApply = OnApply;
      this.OnRemove = OnRemove;
      this.OnMinionsChanged = OnMinionsChanged;
      this.affectAll = affectAll;
    }
  }
}
