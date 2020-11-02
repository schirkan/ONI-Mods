// Decompiled with JetBrains decompiler
// Type: IWorkItemCollection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public interface IWorkItemCollection
{
  int Count { get; }

  void InternalDoWorkItem(int work_item_idx);
}
