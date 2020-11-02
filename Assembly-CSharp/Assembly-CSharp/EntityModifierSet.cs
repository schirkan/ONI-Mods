// Decompiled with JetBrains decompiler
// Type: EntityModifierSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

public class EntityModifierSet : ModifierSet
{
  public DuplicantStatusItems DuplicantStatusItems;
  public ChoreGroups ChoreGroups;

  public override void Initialize()
  {
    base.Initialize();
    this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
    this.ChoreGroups = new ChoreGroups(this.Root);
    this.LoadTraits();
  }
}
