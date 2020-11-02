// Decompiled with JetBrains decompiler
// Type: ContainerPool`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class ContainerPool<ContainerType, PoolIdentifier> : ContainerPool where ContainerType : new()
{
  private Stack<ContainerType> freeContainers = new Stack<ContainerType>();

  public ContainerType Allocate() => this.freeContainers.Count == 0 ? new ContainerType() : this.freeContainers.Pop();

  public void Free(ContainerType container) => this.freeContainers.Push(container);

  public override string GetName() => typeof (PoolIdentifier).Name + "." + typeof (ContainerType).Name;
}
