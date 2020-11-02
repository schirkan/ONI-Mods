// Decompiled with JetBrains decompiler
// Type: KMod.Label
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace KMod
{
  [JsonObject(MemberSerialization.Fields)]
  [DebuggerDisplay("{title}")]
  public struct Label
  {
    public Label.DistributionPlatform distribution_platform;
    public string id;
    public long version;
    public string title;

    [JsonIgnore]
    private string distribution_platform_name => this.distribution_platform.ToString();

    [JsonIgnore]
    public string install_path => FileSystem.Normalize(Path.Combine(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name), this.id));

    [JsonIgnore]
    public DateTime time_stamp => DateTime.FromFileTimeUtc(this.version);

    public override string ToString() => this.title;

    public bool Match(Label rhs) => this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;

    public enum DistributionPlatform
    {
      Local,
      Steam,
      Epic,
      Rail,
      Dev,
    }
  }
}
