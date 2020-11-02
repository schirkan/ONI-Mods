// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.YamlNodeIdentityEqualityComparer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace YamlDotNet.RepresentationModel
{
  public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode>
  {
    public bool Equals(YamlNode x, YamlNode y) => x == y;

    public int GetHashCode(YamlNode obj) => obj.GetHashCode();
  }
}
