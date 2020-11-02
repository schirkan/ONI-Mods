// Decompiled with JetBrains decompiler
// Type: KMod.LoadedModData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Reflection;

namespace KMod
{
  public class LoadedModData
  {
    public ICollection<Assembly> dlls;
    public ICollection<MethodBase> patched_methods;
  }
}
