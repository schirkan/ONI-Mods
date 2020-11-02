// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.IRegistrationLocationSelectionSyntax`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Serialization
{
  public interface IRegistrationLocationSelectionSyntax<TBaseRegistrationType>
  {
    void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

    void Before<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

    void After<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

    void OnTop();

    void OnBottom();
  }
}
