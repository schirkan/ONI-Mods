// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.Helpers.ITestOutputHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Samples.Helpers
{
  public interface ITestOutputHelper
  {
    void WriteLine();

    void WriteLine(string value);

    void WriteLine(string format, params object[] args);
  }
}
