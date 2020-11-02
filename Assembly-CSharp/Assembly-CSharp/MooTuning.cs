// Decompiled with JetBrains decompiler
// Type: MooTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

public static class MooTuning
{
  public static float STANDARD_CALORIES_PER_CYCLE = 200000f;
  public static float STANDARD_STARVE_CYCLES = 6f;
  public static float STANDARD_STOMACH_SIZE = MooTuning.STANDARD_CALORIES_PER_CYCLE * MooTuning.STANDARD_STARVE_CYCLES;
  public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;
  public static float EGG_MASS = 0.5f;
}
