// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Version
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class Version
  {
    public int Major { get; private set; }

    public int Minor { get; private set; }

    public Version(int major, int minor)
    {
      this.Major = major;
      this.Minor = minor;
    }

    public override bool Equals(object obj) => obj is Version version && this.Major == version.Major && this.Minor == version.Minor;

    public override int GetHashCode()
    {
      int num = this.Major;
      int hashCode1 = num.GetHashCode();
      num = this.Minor;
      int hashCode2 = num.GetHashCode();
      return hashCode1 ^ hashCode2;
    }
  }
}
