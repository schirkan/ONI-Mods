// Decompiled with JetBrains decompiler
// Type: KSelectableHealthBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class KSelectableHealthBar : KSelectable
{
  [MyCmpGet]
  private ProgressBar progressBar;
  private int scaleAmount = 100;

  public override string GetName() => string.Format("{0} {1}/{2}", (object) this.entityName, (object) (int) ((double) this.progressBar.PercentFull * (double) this.scaleAmount), (object) this.scaleAmount);
}
