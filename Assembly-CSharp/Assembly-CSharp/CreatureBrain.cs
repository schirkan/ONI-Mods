// Decompiled with JetBrains decompiler
// Type: CreatureBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CreatureBrain : Brain
{
  public string symbolPrefix;
  public Tag species;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<Navigator>().SetAbilities((PathFinderAbilities) new CreaturePathFinderAbilities(this.GetComponent<Navigator>()));
  }
}
