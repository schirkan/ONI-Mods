// Decompiled with JetBrains decompiler
// Type: GraphLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
  [MyCmpReq]
  protected GraphBase graph_base;

  public GraphBase graph
  {
    get
    {
      if ((Object) this.graph_base == (Object) null)
        this.graph_base = this.GetComponent<GraphBase>();
      return this.graph_base;
    }
  }
}
