// Decompiled with JetBrains decompiler
// Type: Painting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Painting : Artable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.multitoolContext = (HashedString) "paint";
    this.multitoolHitEffectTag = (Tag) "fx_paint_splash";
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Paintings.Add(this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Paintings.Remove(this);
  }
}
