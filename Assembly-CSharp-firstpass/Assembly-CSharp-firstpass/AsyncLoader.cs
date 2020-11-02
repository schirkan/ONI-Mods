// Decompiled with JetBrains decompiler
// Type: AsyncLoader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public abstract class AsyncLoader
{
  public virtual Type[] GatherDependencies() => (Type[]) null;

  public virtual void CollectLoaders(List<AsyncLoader> loaders)
  {
  }

  public abstract void Run();
}
