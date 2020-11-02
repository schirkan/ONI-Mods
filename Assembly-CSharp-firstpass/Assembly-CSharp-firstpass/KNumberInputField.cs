// Decompiled with JetBrains decompiler
// Type: KNumberInputField
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KNumberInputField : KInputField
{
  public int decimalPlaces = -1;
  public float currentValue;
  public float minValue;
  public float maxValue;

  protected override void OnSpawn() => base.OnSpawn();

  public void SetAmount(float newValue)
  {
    newValue = Mathf.Clamp(newValue, this.minValue, this.maxValue);
    if (this.decimalPlaces != -1)
    {
      float num = Mathf.Pow(10f, (float) this.decimalPlaces);
      newValue = Mathf.Round(newValue * num) / num;
    }
    this.currentValue = newValue;
    this.SetDisplayValue(this.currentValue.ToString());
  }

  protected override void ProcessInput(string input)
  {
    input = input == "" ? this.minValue.ToString() : input;
    float minValue = this.minValue;
    try
    {
      this.SetAmount(float.Parse(input));
    }
    catch
    {
    }
  }
}
