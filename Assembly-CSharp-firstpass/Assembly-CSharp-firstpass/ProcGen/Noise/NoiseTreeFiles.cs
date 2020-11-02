// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.NoiseTreeFiles
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using LibNoiseDotNet.Graphics.Tools.Noise;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen.Noise
{
  public class NoiseTreeFiles
  {
    public static string NOISE_FILE = "noise";
    private Dictionary<string, Tree> trees;

    public static string GetPath() => System.IO.Path.Combine(Application.streamingAssetsPath, "worldgen/" + NoiseTreeFiles.NOISE_FILE + ".yaml");

    public static string GetTreeFilePath(string filename) => System.IO.Path.Combine(Application.streamingAssetsPath, "worldgen/noise/" + filename + ".yaml");

    public List<string> tree_files { get; set; }

    public void Clear()
    {
      this.tree_files.Clear();
      this.trees.Clear();
    }

    public NoiseTreeFiles()
    {
      this.trees = new Dictionary<string, Tree>();
      this.tree_files = new List<string>();
    }

    public void LoadAllTrees()
    {
      for (int index = 0; index < this.tree_files.Count; ++index)
      {
        Tree tree = YamlIO.LoadFile<Tree>(NoiseTreeFiles.GetTreeFilePath(this.tree_files[index]));
        if (tree != null)
          this.trees.Add(this.tree_files[index], tree);
      }
    }

    public Tree LoadTree(string name, string path)
    {
      if (name == null || name.Length <= 0)
        return (Tree) null;
      if (!this.trees.ContainsKey(name))
      {
        Tree tree = YamlIO.LoadFile<Tree>(path + name + ".yaml");
        if (tree != null)
          this.trees.Add(name, tree);
      }
      return this.trees[name];
    }

    public float GetZoomForTree(string name) => !this.trees.ContainsKey(name) ? 1f : this.trees[name].settings.zoom;

    public bool ShouldNormaliseTree(string name) => this.trees.ContainsKey(name) && this.trees[name].settings.normalise;

    public string[] GetTreeNames()
    {
      string[] strArray = new string[this.trees.Keys.Count];
      int num = 0;
      foreach (KeyValuePair<string, Tree> tree in this.trees)
        strArray[num++] = tree.Key;
      return strArray;
    }

    public Tree GetTree(string name, string path)
    {
      if (!this.trees.ContainsKey(name))
      {
        Tree tree = YamlIO.LoadFile<Tree>(path + "/" + name + ".yaml");
        if (tree == null)
          return (Tree) null;
        this.trees.Add(name, tree);
      }
      return this.trees[name];
    }

    public Tree GetTree(string name) => !this.trees.ContainsKey(name) ? (Tree) null : this.trees[name];

    public IModule3D BuildTree(string name, int globalSeed) => !this.trees.ContainsKey(name) ? (IModule3D) null : this.trees[name].BuildFinalModule(globalSeed);
  }
}
