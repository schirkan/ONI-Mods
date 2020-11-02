// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.ContextFillerAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.Reflection;

namespace NodeEditorFramework
{
  [AttributeUsage(AttributeTargets.Method)]
  public class ContextFillerAttribute : Attribute
  {
    public ContextType contextType { get; private set; }

    public ContextFillerAttribute(ContextType type) => this.contextType = type;

    internal static bool AssureValidity(MethodInfo method, ContextFillerAttribute attr)
    {
      if (!method.IsGenericMethod && !method.IsGenericMethodDefinition && (method.ReturnType == (Type) null || method.ReturnType == typeof (void)))
      {
        ParameterInfo[] parameters = method.GetParameters();
        if (parameters.Length == 2 && parameters[0].ParameterType == typeof (NodeEditorInputInfo) && parameters[1].ParameterType == typeof (GenericMenu))
          return true;
        Debug.LogWarning((object) ("Method " + method.Name + " has incorrect signature for ContextAttribute!"));
      }
      return false;
    }
  }
}
