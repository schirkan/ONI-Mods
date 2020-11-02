// Decompiled with JetBrains decompiler
// Type: IAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public interface IAmountDisplayer
{
  string GetValueString(Amount master, AmountInstance instance);

  string GetDescription(Amount master, AmountInstance instance);

  string GetTooltip(Amount master, AmountInstance instance);

  IAttributeFormatter Formatter { get; }
}
