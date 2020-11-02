// Decompiled with JetBrains decompiler
// Type: ScenePartitionerLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ScenePartitionerLayer
{
  public HashedString name;
  public int layer;
  public System.Action<int, object> OnEvent;

  public ScenePartitionerLayer(HashedString name, int layer)
  {
    this.name = name;
    this.layer = layer;
  }
}
