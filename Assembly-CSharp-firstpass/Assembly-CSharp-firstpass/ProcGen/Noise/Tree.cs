// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Tree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using System.Collections.Generic;

namespace ProcGen.Noise
{
  public class Tree
  {
    private Dictionary<string, IModule3D> primitiveLookup = new Dictionary<string, IModule3D>();
    private Dictionary<string, IModule3D> filterLookup = new Dictionary<string, IModule3D>();
    private Dictionary<string, IModule3D> modifierLookup = new Dictionary<string, IModule3D>();
    private Dictionary<string, IModule3D> selectorLookup = new Dictionary<string, IModule3D>();
    private Dictionary<string, IModule3D> transformerLookup = new Dictionary<string, IModule3D>();
    private Dictionary<string, IModule3D> combinerLookup = new Dictionary<string, IModule3D>();

    public SampleSettings settings { get; set; }

    public List<NodeLink> links { get; set; }

    public Dictionary<string, Primitive> primitives { get; set; }

    public Dictionary<string, Filter> filters { get; set; }

    public Dictionary<string, Transformer> transformers { get; set; }

    public Dictionary<string, Selector> selectors { get; set; }

    public Dictionary<string, Modifier> modifiers { get; set; }

    public Dictionary<string, Combiner> combiners { get; set; }

    public Dictionary<string, FloatList> floats { get; set; }

    public Dictionary<string, ControlPointList> controlpoints { get; set; }

    public Tree()
    {
      this.settings = new SampleSettings();
      this.links = new List<NodeLink>();
      this.primitives = new Dictionary<string, Primitive>();
      this.filters = new Dictionary<string, Filter>();
      this.transformers = new Dictionary<string, Transformer>();
      this.selectors = new Dictionary<string, Selector>();
      this.modifiers = new Dictionary<string, Modifier>();
      this.combiners = new Dictionary<string, Combiner>();
      this.floats = new Dictionary<string, FloatList>();
      this.controlpoints = new Dictionary<string, ControlPointList>();
    }

    public void ClearEmptyLists()
    {
      if (this.links.Count == 0)
        this.links = (List<NodeLink>) null;
      if (this.primitives.Count == 0)
        this.primitives = (Dictionary<string, Primitive>) null;
      if (this.filters.Count == 0)
        this.filters = (Dictionary<string, Filter>) null;
      if (this.transformers.Count == 0)
        this.transformers = (Dictionary<string, Transformer>) null;
      if (this.selectors.Count == 0)
        this.selectors = (Dictionary<string, Selector>) null;
      if (this.modifiers.Count == 0)
        this.modifiers = (Dictionary<string, Modifier>) null;
      if (this.combiners.Count == 0)
        this.combiners = (Dictionary<string, Combiner>) null;
      if (this.floats.Count == 0)
        this.floats = (Dictionary<string, FloatList>) null;
      if (this.controlpoints.Count != 0)
        return;
      this.controlpoints = (Dictionary<string, ControlPointList>) null;
    }

    private IModule3D GetModuleFromLink(Link link)
    {
      if (link == null)
        return (IModule3D) null;
      switch (link.type)
      {
        case Link.Type.Primitive:
          if (this.primitiveLookup.ContainsKey(link.name))
            return this.primitiveLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in primitives"));
          break;
        case Link.Type.Filter:
          if (this.filterLookup.ContainsKey(link.name))
            return this.filterLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in filters"));
          break;
        case Link.Type.Transformer:
          if (this.transformerLookup.ContainsKey(link.name))
            return this.transformerLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in transformers"));
          break;
        case Link.Type.Selector:
          if (this.selectorLookup.ContainsKey(link.name))
            return this.selectorLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in selectors"));
          break;
        case Link.Type.Modifier:
          if (this.modifierLookup.ContainsKey(link.name))
            return this.modifierLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in modifiers"));
          break;
        case Link.Type.Combiner:
          if (this.combinerLookup.ContainsKey(link.name))
            return this.combinerLookup[link.name];
          Debug.LogError((object) ("Couldnt find [" + link.name + "] in combiners"));
          break;
        case Link.Type.Terminator:
          return (IModule3D) null;
      }
      Debug.LogError((object) ("Couldnt find link [" + link.name + "] [" + link.type.ToString() + "]"));
      return (IModule3D) null;
    }

    public IModule3D BuildFinalModule(int globalSeed)
    {
      IModule3D module3D = (IModule3D) null;
      this.primitiveLookup.Clear();
      this.filterLookup.Clear();
      this.modifierLookup.Clear();
      this.selectorLookup.Clear();
      this.transformerLookup.Clear();
      this.combinerLookup.Clear();
      foreach (KeyValuePair<string, Primitive> primitive in this.primitives)
        this.primitiveLookup.Add(primitive.Key, primitive.Value.CreateModule(globalSeed));
      foreach (KeyValuePair<string, Filter> filter in this.filters)
        this.filterLookup.Add(filter.Key, filter.Value.CreateModule());
      foreach (KeyValuePair<string, Modifier> modifier in this.modifiers)
        this.modifierLookup.Add(modifier.Key, modifier.Value.CreateModule());
      foreach (KeyValuePair<string, Selector> selector in this.selectors)
        this.selectorLookup.Add(selector.Key, selector.Value.CreateModule());
      foreach (KeyValuePair<string, Transformer> transformer in this.transformers)
        this.transformerLookup.Add(transformer.Key, transformer.Value.CreateModule());
      foreach (KeyValuePair<string, Combiner> combiner in this.combiners)
        this.combinerLookup.Add(combiner.Key, combiner.Value.CreateModule());
      for (int index = 0; index < this.links.Count; ++index)
      {
        NodeLink link = this.links[index];
        IModule3D moduleFromLink1 = this.GetModuleFromLink(link.target);
        if (link.target.type == Link.Type.Terminator)
        {
          module3D = this.GetModuleFromLink(link.source0);
        }
        else
        {
          switch (link.target.type)
          {
            case Link.Type.Filter:
              IModule3D moduleFromLink2 = this.GetModuleFromLink(link.source0);
              this.filters[link.target.name].SetSouces(moduleFromLink1, moduleFromLink2);
              ((FilterModule) moduleFromLink1).Primitive3D = moduleFromLink2;
              continue;
            case Link.Type.Transformer:
              IModule3D moduleFromLink3 = this.GetModuleFromLink(link.source0);
              IModule3D moduleFromLink4 = this.GetModuleFromLink(link.source1);
              IModule3D moduleFromLink5 = this.GetModuleFromLink(link.source2);
              IModule3D moduleFromLink6 = this.GetModuleFromLink(link.source3);
              this.transformers[link.target.name].SetSouces(moduleFromLink1, moduleFromLink3, moduleFromLink4, moduleFromLink5, moduleFromLink6);
              continue;
            case Link.Type.Selector:
              IModule3D moduleFromLink7 = this.GetModuleFromLink(link.source0);
              IModule3D moduleFromLink8 = this.GetModuleFromLink(link.source1);
              IModule3D moduleFromLink9 = this.GetModuleFromLink(link.source2);
              this.selectors[link.target.name].SetSouces(moduleFromLink1, moduleFromLink7, moduleFromLink8, moduleFromLink9);
              continue;
            case Link.Type.Modifier:
              IModule3D moduleFromLink10 = this.GetModuleFromLink(link.source0);
              ControlPointList controlPoints = (ControlPointList) null;
              if (link.source1 != null && link.source1.type == Link.Type.ControlPoints && this.controlpoints.ContainsKey(link.source1.name))
                controlPoints = this.controlpoints[link.source1.name];
              FloatList controlFloats = (FloatList) null;
              if (link.source2 != null && link.source2.type == Link.Type.FloatPoints && this.controlpoints.ContainsKey(link.source2.name))
                controlFloats = this.floats[link.source2.name];
              this.modifiers[link.target.name].SetSouces(moduleFromLink1, moduleFromLink10, controlFloats, controlPoints);
              continue;
            case Link.Type.Combiner:
              IModule3D moduleFromLink11 = this.GetModuleFromLink(link.source0);
              IModule3D moduleFromLink12 = this.GetModuleFromLink(link.source1);
              this.combiners[link.target.name].SetSouces(moduleFromLink1, moduleFromLink11, moduleFromLink12);
              continue;
            default:
              continue;
          }
        }
      }
      Debug.Assert(module3D != null, (object) "Missing Terminus module");
      return module3D;
    }

    public string[] GetPrimitiveNames()
    {
      string[] strArray = new string[this.primitives.Keys.Count];
      int num = 0;
      foreach (KeyValuePair<string, Primitive> primitive in this.primitives)
        strArray[num++] = primitive.Key;
      return strArray;
    }

    public string[] GetFilterNames()
    {
      string[] strArray = new string[this.filters.Keys.Count];
      int num = 0;
      foreach (KeyValuePair<string, Filter> filter in this.filters)
        strArray[num++] = filter.Key;
      return strArray;
    }
  }
}
