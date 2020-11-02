// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.NewExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace EventSystem2Syntax
{
  internal class NewExample : KMonoBehaviour2
  {
    protected override void OnPrefabInit()
    {
      this.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
      this.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent()
      {
        parameter = false
      });
    }

    private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
    {
    }

    private struct ObjectDestroyedEvent : IEventData
    {
      public bool parameter;
    }
  }
}
