// Decompiled with JetBrains decompiler
// Type: AsyncLoadManager`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

public static class AsyncLoadManager<AsyncLoaderType>
{
  private static Dictionary<Type, AsyncLoader> loaders = new Dictionary<Type, AsyncLoader>();

  public static void Run()
  {
    List<AsyncLoader> loaders = new List<AsyncLoader>();
    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (!type.IsAbstract && typeof (AsyncLoaderType).IsAssignableFrom(type))
        {
          AsyncLoader instance = (AsyncLoader) Activator.CreateInstance(type);
          loaders.Add(instance);
          AsyncLoadManager<AsyncLoaderType>.loaders[type] = instance;
          instance.CollectLoaders(loaders);
        }
      }
    }
    if (AsyncLoadManager<AsyncLoaderType>.loaders.Count <= 0)
      return;
    WorkItemCollection<AsyncLoadManager<AsyncLoaderType>.RunLoader, object> workItemCollection = new WorkItemCollection<AsyncLoadManager<AsyncLoaderType>.RunLoader, object>();
    workItemCollection.Reset((object) null);
    foreach (AsyncLoader asyncLoader in loaders)
      workItemCollection.Add(new AsyncLoadManager<AsyncLoaderType>.RunLoader()
      {
        loader = asyncLoader
      });
    GlobalJobManager.Run((IWorkItemCollection) workItemCollection);
  }

  public static AsyncLoader GetLoader(Type type) => AsyncLoadManager<AsyncLoaderType>.loaders[type];

  public abstract class AsyncLoader<LoaderType> : AsyncLoader where LoaderType : class
  {
    public static LoaderType Get() => AsyncLoadManager<AsyncLoaderType>.GetLoader(typeof (LoaderType)) as LoaderType;
  }

  private struct RunLoader : IWorkItem<object>
  {
    public AsyncLoader loader;

    public void Run(object shared_data) => this.loader.Run();
  }
}
