// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeTypes
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework
{
  public static class NodeTypes
  {
    public static Dictionary<Node, NodeData> nodes;

    public static void FetchNodes()
    {
      NodeTypes.nodes = new Dictionary<Node, NodeData>();
      foreach (Assembly assembly in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.Contains("Assembly"))))
      {
        foreach (System.Type type in ((IEnumerable<System.Type>) assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (T => T.IsClass && !T.IsAbstract && T.IsSubclassOf(typeof (Node)))))
        {
          if (!(type.GetCustomAttributes(typeof (NodeAttribute), false)[0] is NodeAttribute customAttribute) || !customAttribute.hide)
          {
            try
            {
              Node key = (ScriptableObject.CreateInstance(type.Name) as Node).Create(Vector2.zero);
              NodeTypes.nodes.Add(key, new NodeData(customAttribute == null ? key.name : customAttribute.contextText, customAttribute.typeOfNodeCanvas));
            }
            catch (Exception ex)
            {
              Debug.LogError((object) (ex.Message + " " + type.Name));
            }
          }
        }
      }
    }

    public static NodeData getNodeData(Node node) => NodeTypes.nodes[NodeTypes.getDefaultNode(node.GetID)];

    public static Node getDefaultNode(string nodeID) => NodeTypes.nodes.Keys.Single<Node>((Func<Node, bool>) (node => node.GetID == nodeID));

    public static T getDefaultNode<T>() where T : Node => NodeTypes.nodes.Keys.Single<Node>((Func<Node, bool>) (node => node.GetType() == typeof (T))) as T;

    public static List<Node> getCompatibleNodes(NodeOutput nodeOutput)
    {
      if ((UnityEngine.Object) nodeOutput == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (nodeOutput));
      List<Node> nodeList = new List<Node>();
      foreach (Node key in NodeTypes.nodes.Keys)
      {
        for (int index = 0; index < key.Inputs.Count; ++index)
        {
          NodeInput input = key.Inputs[index];
          if ((UnityEngine.Object) input == (UnityEngine.Object) null)
            throw new UnityException("Input " + (object) index + " is null!");
          if (input.typeData.Type.IsAssignableFrom(nodeOutput.typeData.Type))
          {
            nodeList.Add(key);
            break;
          }
        }
      }
      return nodeList;
    }
  }
}
