// Decompiled with JetBrains decompiler
// Type: NewGameFlowScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class NewGameFlowScreen : KModalScreen
{
  public event System.Action OnNavigateForward;

  public event System.Action OnNavigateBackward;

  protected void NavigateBackward() => this.OnNavigateBackward();

  protected void NavigateForward() => this.OnNavigateForward();
}
