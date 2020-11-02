// Decompiled with JetBrains decompiler
// Type: TuningData`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class TuningData<TuningType>
{
  public static TuningType _TuningData;

  public static TuningType Get()
  {
    TuningSystem.Init();
    return TuningData<TuningType>._TuningData;
  }
}
