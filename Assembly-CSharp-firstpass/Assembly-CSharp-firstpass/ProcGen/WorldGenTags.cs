// Decompiled with JetBrains decompiler
// Type: ProcGen.WorldGenTags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen
{
  public class WorldGenTags
  {
    public static readonly Tag ConnectToSiblings = TagManager.Create(nameof (ConnectToSiblings));
    public static readonly Tag ConnectTypeMinSpan = TagManager.Create(nameof (ConnectTypeMinSpan));
    public static readonly Tag ConnectTypeSpan = TagManager.Create(nameof (ConnectTypeSpan));
    public static readonly Tag ConnectTypeNone = TagManager.Create(nameof (ConnectTypeNone));
    public static readonly Tag ConnectTypeFull = TagManager.Create(nameof (ConnectTypeFull));
    public static readonly Tag ConnectTypeRandom = TagManager.Create(nameof (ConnectTypeRandom));
    public static readonly Tag Cell = TagManager.Create(nameof (Cell));
    public static readonly Tag Edge = TagManager.Create(nameof (Edge));
    public static readonly Tag Corner = TagManager.Create(nameof (Corner));
    public static readonly Tag EdgeUnpassable = TagManager.Create(nameof (EdgeUnpassable));
    public static readonly Tag EdgeClosed = TagManager.Create(nameof (EdgeClosed));
    public static readonly Tag EdgeOpen = TagManager.Create(nameof (EdgeOpen));
    public static readonly Tag IgnoreCaveOverride = TagManager.Create(nameof (IgnoreCaveOverride));
    public static readonly Tag ErodePointToCentroid = TagManager.Create(nameof (ErodePointToCentroid));
    public static readonly Tag ErodePointToCentroidInv = TagManager.Create(nameof (ErodePointToCentroidInv));
    public static readonly Tag ErodePointToEdge = TagManager.Create(nameof (ErodePointToEdge));
    public static readonly Tag ErodePointToEdgeInv = TagManager.Create(nameof (ErodePointToEdgeInv));
    public static readonly Tag ErodePointToBorder = TagManager.Create(nameof (ErodePointToBorder));
    public static readonly Tag ErodePointToBorderInv = TagManager.Create(nameof (ErodePointToBorderInv));
    public static readonly Tag ErodePointToWorldTop = TagManager.Create(nameof (ErodePointToWorldTop));
    public static readonly Tag DistFunctionPointCentroid = TagManager.Create(nameof (DistFunctionPointCentroid));
    public static readonly Tag DistFunctionPointEdge = TagManager.Create(nameof (DistFunctionPointEdge));
    public static readonly Tag SplitOnParentDensity = TagManager.Create(nameof (SplitOnParentDensity));
    public static readonly Tag SplitTwice = TagManager.Create(nameof (SplitTwice));
    public static readonly Tag UltraHighDensitySplit = TagManager.Create(nameof (UltraHighDensitySplit));
    public static readonly Tag VeryHighDensitySplit = TagManager.Create(nameof (VeryHighDensitySplit));
    public static readonly Tag HighDensitySplit = TagManager.Create(nameof (HighDensitySplit));
    public static readonly Tag MediumDensitySplit = TagManager.Create(nameof (MediumDensitySplit));
    public static readonly Tag UnassignedNode = TagManager.Create(nameof (UnassignedNode));
    public static readonly Tag Feature = TagManager.Create(nameof (Feature));
    public static readonly Tag CenteralFeature = TagManager.Create(nameof (CenteralFeature));
    public static readonly Tag Overworld = TagManager.Create(nameof (Overworld));
    public static readonly Tag StartNear = TagManager.Create(nameof (StartNear));
    public static readonly Tag StartMedium = TagManager.Create(nameof (StartMedium));
    public static readonly Tag StartFar = TagManager.Create(nameof (StartFar));
    public static readonly Tag NearEdge = TagManager.Create(nameof (NearEdge));
    public static readonly Tag NearSurface = TagManager.Create(nameof (NearSurface));
    public static readonly Tag NearDepths = TagManager.Create(nameof (NearDepths));
    public static readonly Tag AtStart = TagManager.Create(nameof (AtStart));
    public static readonly Tag AtSurface = TagManager.Create(nameof (AtSurface));
    public static readonly Tag AtDepths = TagManager.Create(nameof (AtDepths));
    public static readonly Tag AtEdge = TagManager.Create(nameof (AtEdge));
    public static readonly Tag EdgeOfVoid = TagManager.Create(nameof (EdgeOfVoid));
    public static readonly Tag Dry = TagManager.Create(nameof (Dry));
    public static readonly Tag Wet = TagManager.Create(nameof (Wet));
    public static readonly Tag River = TagManager.Create(nameof (River));
    public static readonly Tag StartWorld = TagManager.Create(nameof (StartWorld));
    public static readonly Tag StartLocation = TagManager.Create(nameof (StartLocation));
    public static readonly Tag NearStartLocation = TagManager.Create(nameof (NearStartLocation));
    public static readonly Tag POI = TagManager.Create(nameof (POI));
    public static readonly Tag NoGlobalFeatureSpawning = TagManager.Create(nameof (NoGlobalFeatureSpawning));
    public static readonly Tag PreventAmbientMobsInFeature = TagManager.Create(nameof (PreventAmbientMobsInFeature));
    public static readonly Tag AllowExceedNodeBorders = TagManager.Create(nameof (AllowExceedNodeBorders));
    public static readonly Tag HighPriorityFeature = TagManager.Create(nameof (HighPriorityFeature));
    public static readonly Tag CaveVoidSliver = TagManager.Create(nameof (CaveVoidSliver));
    public static readonly Tag Geode = TagManager.Create(nameof (Geode));
    public static readonly Tag TheVoid = TagManager.Create(nameof (TheVoid));
    public static readonly Tag SprinkleOfMetal = TagManager.Create(nameof (SprinkleOfMetal));
    public static readonly Tag SprinkleOfOxyRock = TagManager.Create(nameof (SprinkleOfOxyRock));
    public static readonly Tag Infected = TagManager.Create(nameof (Infected));
    public static readonly Tag InfectedDweebcephaly = TagManager.Create("Infected:Dweebcephaly");
    public static readonly Tag InfectedLazibonitis = TagManager.Create("Infected:Lazibonitis");
    public static readonly Tag InfectedDiarrhea = TagManager.Create("Infected:Diarrhea");
    public static readonly Tag InfectedFoodPoisoning = TagManager.Create("Infected:FoodPoisoning");
    public static readonly Tag InfectedPutridOdour = TagManager.Create("Infected:PutridOdour");
    public static readonly Tag InfectedSpores = TagManager.Create("Infected:Spores");
    public static readonly Tag InfectedColdBrain = TagManager.Create("Infected:ColdBrain");
    public static readonly Tag InfectedHeatRash = TagManager.Create("Infected:HeatRash");
    public static readonly Tag InfectedSlimeLung = TagManager.Create("Infected:SlimeLung");
    public static readonly Tag DEBUG_Split = TagManager.Create(nameof (DEBUG_Split));
    public static readonly Tag DEBUG_SplitForChildCount = TagManager.Create(nameof (DEBUG_SplitForChildCount));
    public static readonly Tag DEBUG_SplitTopSite = TagManager.Create(nameof (DEBUG_SplitTopSite));
    public static readonly Tag DEBUG_SplitBottomSite = TagManager.Create(nameof (DEBUG_SplitBottomSite));
    public static readonly Tag DEBUG_SplitLargeStartingSites = TagManager.Create(nameof (DEBUG_SplitLargeStartingSites));
    public static readonly Tag DEBUG_NoSplitForChildCount = TagManager.Create(nameof (DEBUG_NoSplitForChildCount));
    public static readonly TagSet DebugTags = new TagSet(new Tag[6]
    {
      WorldGenTags.DEBUG_Split,
      WorldGenTags.DEBUG_SplitForChildCount,
      WorldGenTags.DEBUG_SplitTopSite,
      WorldGenTags.DEBUG_SplitBottomSite,
      WorldGenTags.DEBUG_SplitLargeStartingSites,
      WorldGenTags.DEBUG_NoSplitForChildCount
    });
    public static readonly TagSet MapTags = new TagSet(new Tag[6]
    {
      WorldGenTags.Cell,
      WorldGenTags.Edge,
      WorldGenTags.Corner,
      WorldGenTags.EdgeUnpassable,
      WorldGenTags.EdgeClosed,
      WorldGenTags.EdgeOpen
    });
    public static readonly TagSet CommandTags = new TagSet(new Tag[11]
    {
      WorldGenTags.IgnoreCaveOverride,
      WorldGenTags.ErodePointToCentroid,
      WorldGenTags.ErodePointToCentroidInv,
      WorldGenTags.DistFunctionPointCentroid,
      WorldGenTags.DistFunctionPointEdge,
      WorldGenTags.SplitOnParentDensity,
      WorldGenTags.SplitTwice,
      WorldGenTags.UltraHighDensitySplit,
      WorldGenTags.VeryHighDensitySplit,
      WorldGenTags.HighDensitySplit,
      WorldGenTags.MediumDensitySplit
    });
    public static readonly TagSet WorldTags = new TagSet(new Tag[12]
    {
      WorldGenTags.UnassignedNode,
      WorldGenTags.Feature,
      WorldGenTags.CenteralFeature,
      WorldGenTags.Overworld,
      WorldGenTags.NearSurface,
      WorldGenTags.NearDepths,
      WorldGenTags.AtSurface,
      WorldGenTags.AtDepths,
      WorldGenTags.AtEdge,
      WorldGenTags.AtStart,
      WorldGenTags.StartNear,
      WorldGenTags.StartMedium
    });
    public static readonly TagSet DistanceTags = new TagSet(new Tag[4]
    {
      WorldGenTags.AtSurface,
      WorldGenTags.AtDepths,
      WorldGenTags.AtEdge,
      WorldGenTags.AtStart
    });
  }
}
