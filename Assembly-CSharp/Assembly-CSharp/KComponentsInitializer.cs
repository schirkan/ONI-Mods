// Decompiled with JetBrains decompiler
// Type: KComponentsInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class KComponentsInitializer : KComponentSpawn
{
  private void Awake()
  {
    KComponentSpawn.instance = (KComponentSpawn) this;
    this.comps = (KComponents) new GameComps();
  }
}
