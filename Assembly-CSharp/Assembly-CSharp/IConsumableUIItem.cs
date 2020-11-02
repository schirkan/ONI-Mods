// Decompiled with JetBrains decompiler
// Type: IConsumableUIItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IConsumableUIItem
{
  string ConsumableId { get; }

  string ConsumableName { get; }

  int MajorOrder { get; }

  int MinorOrder { get; }

  bool Display { get; }
}
