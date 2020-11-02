// Decompiled with JetBrains decompiler
// Type: BuildingGroupScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class BuildingGroupScreen : KScreen
{
  public static BuildingGroupScreen Instance;

  protected override void OnPrefabInit()
  {
    BuildingGroupScreen.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ConsumeMouseScroll = true;
  }
}
