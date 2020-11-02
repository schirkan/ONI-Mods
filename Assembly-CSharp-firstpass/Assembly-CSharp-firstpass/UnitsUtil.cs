// Decompiled with JetBrains decompiler
// Type: UnitsUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public static class UnitsUtil
{
  public static bool IsTimeUnit(Units unit)
  {
    switch (unit)
    {
      case Units.PerDay:
      case Units.PerSecond:
        return true;
      default:
        return false;
    }
  }

  public static string GetUnitSuffix(Units unit) => unit == Units.Kelvin ? "K" : "";
}
