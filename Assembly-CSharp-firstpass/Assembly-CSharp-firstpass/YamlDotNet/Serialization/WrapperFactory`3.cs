// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.WrapperFactory`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Serialization
{
  public delegate TComponent WrapperFactory<TArgument, TComponentBase, TComponent>(
    TComponentBase wrapped,
    TArgument argument)
    where TComponent : TComponentBase;
}
