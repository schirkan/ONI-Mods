﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.Helpers.TestOutputHelperExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Samples.Helpers
{
  public static class TestOutputHelperExtensions
  {
    public static void WriteLine(this ITestOutputHelper output) => output.WriteLine(string.Empty);
  }
}
