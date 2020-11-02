// Decompiled with JetBrains decompiler
// Type: DivisibleTask`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
  public string name;
  public int start;
  public int end;

  public void Run(SharedData sharedData) => this.RunDivision(sharedData);

  protected DivisibleTask(string name) => this.name = name;

  protected abstract void RunDivision(SharedData sharedData);
}
