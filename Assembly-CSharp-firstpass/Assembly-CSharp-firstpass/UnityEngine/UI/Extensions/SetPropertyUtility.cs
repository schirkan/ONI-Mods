// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Extensions.SetPropertyUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UnityEngine.UI.Extensions
{
  internal static class SetPropertyUtility
  {
    public static bool SetColor(ref Color currentValue, Color newValue)
    {
      if ((double) currentValue.r == (double) newValue.r && (double) currentValue.g == (double) newValue.g && ((double) currentValue.b == (double) newValue.b && (double) currentValue.a == (double) newValue.a))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    {
      if (currentValue.Equals((object) newValue))
        return false;
      currentValue = newValue;
      return true;
    }

    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
    {
      if ((object) currentValue == null && (object) newValue == null || (object) currentValue != null && currentValue.Equals((object) newValue))
        return false;
      currentValue = newValue;
      return true;
    }
  }
}
