﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.PropertyInfoExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Reflection;

namespace YamlDotNet
{
  internal static class PropertyInfoExtensions
  {
    public static object ReadValue(this PropertyInfo property, object target) => property.GetGetMethod().Invoke(target, (object[]) null);
  }
}
