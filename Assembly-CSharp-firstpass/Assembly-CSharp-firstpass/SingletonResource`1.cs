// Decompiled with JetBrains decompiler
// Type: SingletonResource`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public static class SingletonResource<T> where T : ResourceFile
{
  private static T StaticInstance;

  public static T Get()
  {
    if ((Object) SingletonResource<T>.StaticInstance == (Object) null)
    {
      SingletonResource<T>.StaticInstance = Resources.Load<T>(typeof (T).Name);
      SingletonResource<T>.StaticInstance.Initialize();
    }
    return SingletonResource<T>.StaticInstance;
  }
}
