// Decompiled with JetBrains decompiler
// Type: NavigationTactics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class NavigationTactics
{
  public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, pathCostPenalty: 4);
  public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12);
  public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999);
}
