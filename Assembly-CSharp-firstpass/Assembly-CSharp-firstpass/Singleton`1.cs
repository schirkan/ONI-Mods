// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public abstract class Singleton<T> where T : class, new()
{
  private static T _instance;
  private static object _lock = new object();

  public static T Instance
  {
    get
    {
      lock (Singleton<T>._lock)
        return Singleton<T>._instance;
    }
  }

  public static void CreateInstance()
  {
    lock (Singleton<T>._lock)
    {
      if ((object) Singleton<T>._instance != null)
        return;
      Singleton<T>._instance = new T();
    }
  }

  public static void DestroyInstance()
  {
    lock (Singleton<T>._lock)
      Singleton<T>._instance = default (T);
  }
}
