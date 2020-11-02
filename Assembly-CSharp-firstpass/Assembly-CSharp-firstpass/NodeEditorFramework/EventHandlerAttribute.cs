// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.EventHandlerAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class EventHandlerAttribute : Attribute
  {
    public EventType? handledEvent { get; private set; }

    public int priority { get; private set; }

    public EventHandlerAttribute(EventType eventType, int priorityValue)
    {
      this.handledEvent = new EventType?(eventType);
      this.priority = priorityValue;
    }

    public EventHandlerAttribute(int priorityValue)
    {
      this.handledEvent = new EventType?();
      this.priority = priorityValue;
    }

    public EventHandlerAttribute(EventType eventType)
    {
      this.handledEvent = new EventType?(eventType);
      this.priority = 50;
    }

    public EventHandlerAttribute() => this.handledEvent = new EventType?();

    internal static bool AssureValidity(MethodInfo method, EventHandlerAttribute attr)
    {
      if (!method.IsGenericMethod && !method.IsGenericMethodDefinition && (method.ReturnType == (System.Type) null || method.ReturnType == typeof (void)))
      {
        ParameterInfo[] parameters = method.GetParameters();
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof (NodeEditorInputInfo))
          return true;
        Debug.LogWarning((object) ("Method " + method.Name + " has incorrect signature for EventHandlerAttribute!"));
      }
      return false;
    }
  }
}
