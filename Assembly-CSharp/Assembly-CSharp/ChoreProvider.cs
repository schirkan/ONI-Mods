// Decompiled with JetBrains decompiler
// Type: ChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
  public List<Chore> chores = new List<Chore>();

  public string Name { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Name = this.name;
  }

  public virtual void AddChore(Chore chore)
  {
    chore.provider = this;
    this.chores.Add(chore);
  }

  public virtual void RemoveChore(Chore chore)
  {
    if (chore == null)
      return;
    chore.provider = (ChoreProvider) null;
    this.chores.Remove(chore);
  }

  public virtual void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    for (int index = 0; index < this.chores.Count; ++index)
      this.chores[index].CollectChores(consumer_state, succeeded, failed_contexts, false);
  }
}
