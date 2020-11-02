// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.ContextEntryAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

namespace NodeEditorFramework
{
  [AttributeUsage(AttributeTargets.Method)]
  public class ContextEntryAttribute : Attribute
  {
    public ContextType contextType { get; private set; }

    public string contextPath { get; private set; }

    public ContextEntryAttribute(ContextType type, string path)
    {
      this.contextType = type;
      this.contextPath = path;
    }

    internal static bool AssureValidity(MethodInfo method, ContextEntryAttribute attr)
    {
      if (!method.IsGenericMethod && !method.IsGenericMethodDefinition && (method.ReturnType == (Type) null || method.ReturnType == typeof (void)))
      {
        ParameterInfo[] parameters = method.GetParameters();
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof (NodeEditorInputInfo))
          return true;
        Debug.LogWarning((object) ("Method " + method.Name + " has incorrect signature for ContextAttribute!"));
      }
      return false;
    }
  }
}
