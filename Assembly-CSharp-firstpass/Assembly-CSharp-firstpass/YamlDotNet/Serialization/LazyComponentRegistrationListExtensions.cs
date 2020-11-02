// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.LazyComponentRegistrationListExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization
{
  internal static class LazyComponentRegistrationListExtensions
  {
    public static TComponent BuildComponentChain<TComponent>(
      this LazyComponentRegistrationList<TComponent, TComponent> registrations,
      TComponent innerComponent)
    {
      return registrations.InReverseOrder.Aggregate<Func<TComponent, TComponent>, TComponent>(innerComponent, (Func<TComponent, Func<TComponent, TComponent>, TComponent>) ((inner, factory) => factory(inner)));
    }

    public static TComponent BuildComponentChain<TArgument, TComponent>(
      this LazyComponentRegistrationList<TArgument, TComponent> registrations,
      TComponent innerComponent,
      Func<TComponent, TArgument> argumentBuilder)
    {
      return registrations.InReverseOrder.Aggregate<Func<TArgument, TComponent>, TComponent>(innerComponent, (Func<TComponent, Func<TArgument, TComponent>, TComponent>) ((inner, factory) => factory(argumentBuilder(inner))));
    }

    public static List<TComponent> BuildComponentList<TComponent>(
      this LazyComponentRegistrationList<Nothing, TComponent> registrations)
    {
      return registrations.Select<Func<Nothing, TComponent>, TComponent>((Func<Func<Nothing, TComponent>, TComponent>) (factory => factory((Nothing) null))).ToList<TComponent>();
    }

    public static List<TComponent> BuildComponentList<TArgument, TComponent>(
      this LazyComponentRegistrationList<TArgument, TComponent> registrations,
      TArgument argument)
    {
      return registrations.Select<Func<TArgument, TComponent>, TComponent>((Func<Func<TArgument, TComponent>, TComponent>) (factory => factory(argument))).ToList<TComponent>();
    }
  }
}
