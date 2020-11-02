// Decompiled with JetBrains decompiler
// Type: ProcGen.World
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ProcGen
{
  [Serializable]
  public class World
  {
    public string filePath;

    public string name { get; private set; }

    public string description { get; private set; }

    public string coordinatePrefix { get; private set; }

    public string spriteName { get; private set; }

    public int difficulty { get; private set; }

    public int tier { get; private set; }

    public bool disableWorldTraits { get; private set; }

    public string GetCoordinatePrefix()
    {
      if (string.IsNullOrEmpty(this.coordinatePrefix))
      {
        string str1 = "";
        string[] strArray = Strings.Get(this.name).String.Split(' ');
        int a = 5 - strArray.Length;
        bool flag = true;
        foreach (string input in strArray)
        {
          if (!flag)
            str1 += "-";
          string str2 = Regex.Replace(input, "(a|e|i|o|u)", "");
          str1 += str2.Substring(0, Mathf.Min(a, str2.Length)).ToUpper();
          flag = false;
        }
        this.coordinatePrefix = str1;
      }
      return this.coordinatePrefix;
    }

    public World.Skip skip { get; private set; }

    public bool noStart { get; private set; }

    public Vector2I worldsize { get; private set; }

    public DefaultSettings defaultsOverrides { get; private set; }

    public World.LayoutMethod layoutMethod { get; private set; }

    public List<WeightedName> subworldFiles { get; private set; }

    public List<World.AllowedCellsFilter> unknownCellsAllowedSubworlds { get; private set; }

    public string startSubworldName { get; private set; }

    public string startingBaseTemplate { get; set; }

    public MinMax startingBasePositionHorizontal { get; private set; }

    public MinMax startingBasePositionVertical { get; private set; }

    public Dictionary<string, int> globalFeatureTemplates { get; private set; }

    public Dictionary<string, int> globalFeatures { get; private set; }

    public World()
    {
      this.subworldFiles = new List<WeightedName>();
      this.unknownCellsAllowedSubworlds = new List<World.AllowedCellsFilter>();
      this.startingBasePositionHorizontal = new MinMax(0.5f, 0.5f);
      this.startingBasePositionVertical = new MinMax(0.5f, 0.5f);
      this.globalFeatureTemplates = new Dictionary<string, int>();
      this.globalFeatures = new Dictionary<string, int>();
    }

    public void ModStartLocation(MinMax hMod, MinMax vMod)
    {
      MinMax positionHorizontal = this.startingBasePositionHorizontal;
      MinMax positionVertical = this.startingBasePositionVertical;
      positionHorizontal.Mod(hMod);
      positionVertical.Mod(vMod);
      this.startingBasePositionHorizontal = positionHorizontal;
      this.startingBasePositionVertical = positionVertical;
    }

    public enum Skip
    {
      False = 0,
      Never = 0,
      Always = 99, // 0x00000063
      True = 99, // 0x00000063
      EditorOnly = 100, // 0x00000064
    }

    public enum LayoutMethod
    {
      Default = 0,
      VoronoiTree = 0,
      PowerTree = 1,
    }

    [Serializable]
    public class AllowedCellsFilter
    {
      public AllowedCellsFilter()
      {
        this.temperatureRanges = new List<Temperature.Range>();
        this.zoneTypes = new List<SubWorld.ZoneType>();
        this.subworldNames = new List<string>();
      }

      public World.AllowedCellsFilter.TagCommand tagcommand { get; private set; }

      public string tag { get; private set; }

      public int minDistance { get; private set; }

      public int maxDistance { get; private set; }

      public int distCmp { get; private set; }

      public World.AllowedCellsFilter.Command command { get; private set; }

      public List<Temperature.Range> temperatureRanges { get; private set; }

      public List<SubWorld.ZoneType> zoneTypes { get; private set; }

      public List<string> subworldNames { get; private set; }

      public enum TagCommand
      {
        Default,
        AtTag,
        DistanceFromTag,
      }

      public enum Command
      {
        Clear,
        Replace,
        UnionWith,
        IntersectWith,
        ExceptWith,
        SymmetricExceptWith,
      }
    }
  }
}
