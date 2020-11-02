// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeCanvasManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework
{
  public class NodeCanvasManager
  {
    public static Dictionary<System.Type, NodeCanvasTypeData> TypeOfCanvases;
    private static Action<System.Type> _callBack;

    public static void GetAllCanvasTypes()
    {
      NodeCanvasManager.TypeOfCanvases = new Dictionary<System.Type, NodeCanvasTypeData>();
      foreach (Assembly assembly in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.Contains("Assembly"))))
      {
        foreach (System.Type key in ((IEnumerable<System.Type>) assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (T => T.IsClass && !T.IsAbstract && (uint) T.GetCustomAttributes(typeof (NodeCanvasTypeAttribute), false).Length > 0U)))
        {
          NodeCanvasTypeAttribute customAttribute = key.GetCustomAttributes(typeof (NodeCanvasTypeAttribute), false)[0] as NodeCanvasTypeAttribute;
          NodeCanvasManager.TypeOfCanvases.Add(key, new NodeCanvasTypeData()
          {
            CanvasType = key,
            DisplayString = customAttribute.Name
          });
        }
      }
    }

    private static void CreateNewCanvas(object userdata)
    {
      NodeCanvasTypeData nodeCanvasTypeData = (NodeCanvasTypeData) userdata;
      NodeCanvasManager._callBack(nodeCanvasTypeData.CanvasType);
    }

    public static void PopulateMenu(ref GenericMenu menu, Action<System.Type> newNodeCanvas)
    {
      NodeCanvasManager._callBack = newNodeCanvas;
      foreach (KeyValuePair<System.Type, NodeCanvasTypeData> typeOfCanvase in NodeCanvasManager.TypeOfCanvases)
        menu.AddItem(new GUIContent(typeOfCanvase.Value.DisplayString), false, new PopupMenu.MenuFunctionData(NodeCanvasManager.CreateNewCanvas), (object) typeOfCanvase.Value);
    }
  }
}
