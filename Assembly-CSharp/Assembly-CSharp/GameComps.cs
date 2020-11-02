// Decompiled with JetBrains decompiler
// Type: GameComps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;

public class GameComps : KComponents
{
  public static GravityComponents Gravities;
  public static FallerComponents Fallers;
  public static InfraredVisualizerComponents InfraredVisualizers;
  public static ElementSplitterComponents ElementSplitters;
  public static OreSizeVisualizerComponents OreSizeVisualizers;
  public static StructureTemperatureComponents StructureTemperatures;
  public static DiseaseContainers DiseaseContainers;
  public static RequiresFoundation RequiresFoundations;
  public static WhiteBoard WhiteBoards;
  private static Dictionary<System.Type, IKComponentManager> kcomponentManagers = new Dictionary<System.Type, IKComponentManager>();

  public GameComps()
  {
    foreach (FieldInfo field in typeof (GameComps).GetFields())
    {
      object instance = Activator.CreateInstance(field.FieldType);
      field.SetValue((object) null, instance);
      this.Add<IComponentManager>(instance as IComponentManager);
      if (instance is IKComponentManager)
      {
        IKComponentManager inst = instance as IKComponentManager;
        GameComps.AddKComponentManager(field.FieldType, inst);
      }
    }
  }

  public new void Clear()
  {
    foreach (FieldInfo field in typeof (GameComps).GetFields())
    {
      if (field.GetValue((object) null) is IComponentManager componentManager)
        componentManager.Clear();
    }
  }

  public static void AddKComponentManager(System.Type kcomponent, IKComponentManager inst) => GameComps.kcomponentManagers[kcomponent] = inst;

  public static IKComponentManager GetKComponentManager(System.Type kcomponent_type) => GameComps.kcomponentManagers[kcomponent_type];
}
