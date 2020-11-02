// Decompiled with JetBrains decompiler
// Type: Database.StateMachineCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class StateMachineCategories : ResourceSet<StateMachine.Category>
  {
    public StateMachine.Category Ai;
    public StateMachine.Category Monitor;
    public StateMachine.Category Chore;
    public StateMachine.Category Misc;

    public StateMachineCategories()
    {
      this.Ai = this.Add(new StateMachine.Category(nameof (Ai)));
      this.Monitor = this.Add(new StateMachine.Category(nameof (Monitor)));
      this.Chore = this.Add(new StateMachine.Category(nameof (Chore)));
      this.Misc = this.Add(new StateMachine.Category(nameof (Misc)));
    }
  }
}
