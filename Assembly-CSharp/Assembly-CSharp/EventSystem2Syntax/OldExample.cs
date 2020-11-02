// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.OldExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace EventSystem2Syntax
{
  internal class OldExample : KMonoBehaviour2
  {
    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.Subscribe(0, new System.Action<object>(this.OnObjectDestroyed));
      this.Trigger(0, (object) false);
    }

    private void OnObjectDestroyed(object data) => Debug.Log((object) (bool) data);
  }
}
