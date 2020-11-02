// Decompiled with JetBrains decompiler
// Type: Grid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class Grid
{
  public static readonly CellOffset[] DefaultOffset = new CellOffset[1];
  public static float WidthInMeters;
  public static float HeightInMeters;
  public static int WidthInCells;
  public static int HeightInCells;
  public static float CellSizeInMeters;
  public static float InverseCellSizeInMeters;
  public static float HalfCellSizeInMeters;
  public static int CellCount;
  public static int InvalidCell = -1;
  public static int TopBorderHeight = 2;
  public static Dictionary<int, GameObject>[] ObjectLayers;
  public static System.Action<int> OnReveal;
  public static Grid.BuildFlags[] BuildMasks;
  public static Grid.BuildFlagsFoundationIndexer Foundation;
  public static Grid.BuildFlagsSolidIndexer Solid;
  public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;
  public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;
  public static Grid.BuildFlagsDupePassableIndexer DupePassable;
  public static Grid.BuildFlagsImpassableIndexer CritterImpassable;
  public static Grid.BuildFlagsDoorIndexer HasDoor;
  public static Grid.VisFlags[] VisMasks;
  public static Grid.VisFlagsRevealedIndexer Revealed;
  public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;
  public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;
  public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;
  public static Grid.NavValidatorFlags[] NavValidatorMasks;
  public static Grid.NavValidatorFlagsLadderIndexer HasLadder;
  public static Grid.NavValidatorFlagsPoleIndexer HasPole;
  public static Grid.NavValidatorFlagsTubeIndexer HasTube;
  public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;
  public static Grid.NavFlags[] NavMasks;
  public static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;
  public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;
  public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;
  public static Grid.NavFlagsReservedIndexer Reserved;
  public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;
  private static Dictionary<int, Grid.Restriction> restrictions = new Dictionary<int, Grid.Restriction>();
  private static Dictionary<int, Grid.TubeEntrance> tubeEntrances = new Dictionary<int, Grid.TubeEntrance>();
  private static Dictionary<int, Grid.SuitMarker> suitMarkers = new Dictionary<int, Grid.SuitMarker>();
  public static unsafe byte* elementIdx;
  public static unsafe float* temperature;
  public static unsafe float* mass;
  public static unsafe byte* properties;
  public static unsafe byte* strengthInfo;
  public static unsafe byte* insulation;
  public static unsafe byte* diseaseIdx;
  public static unsafe int* diseaseCount;
  public static unsafe byte* exposedToSunlight;
  public static unsafe float* AccumulatedFlowValues = (float*) null;
  public static byte[] Visible;
  public static byte[] Spawnable;
  public static float[] Damage;
  public static float[] Decor;
  public static bool[] GravitasFacility;
  public static float[] Loudness;
  public static global::Element[] Element;
  public static int[] LightCount;
  public static int[] RadiationCount;
  public static Grid.PressureIndexer Pressure;
  public static Grid.TransparentIndexer Transparent;
  public static Grid.ElementIdxIndexer ElementIdx;
  public static Grid.TemperatureIndexer Temperature;
  public static Grid.MassIndexer Mass;
  public static Grid.PropertiesIndexer Properties;
  public static Grid.ExposedToSunlightIndexer ExposedToSunlight;
  public static Grid.StrengthInfoIndexer StrengthInfo;
  public static Grid.Insulationndexer Insulation;
  public static Grid.DiseaseIdxIndexer DiseaseIdx;
  public static Grid.DiseaseCountIndexer DiseaseCount;
  public static Grid.LightIntensityIndexer LightIntensity;
  public static Grid.AccumulatedFlowIndexer AccumulatedFlow;
  public static Grid.ObjectLayerIndexer Objects;
  public static float LayerMultiplier = 1f;
  private static readonly Func<int, bool> VisibleBlockingDelegate = (Func<int, bool>) (cell => Grid.VisibleBlockingCB(cell));
  private static readonly Func<int, bool> PhysicalBlockingDelegate = (Func<int, bool>) (cell => Grid.PhysicalBlockingCB(cell));

  private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
  {
    if (state)
      Grid.BuildMasks[i] |= flag;
    else
      Grid.BuildMasks[i] &= ~flag;
  }

  public static void SetSolid(int cell, bool solid, CellSolidEvent ev) => Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);

  private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
  {
    if (state)
      Grid.VisMasks[i] |= flag;
    else
      Grid.VisMasks[i] &= ~flag;
  }

  private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
  {
    if (state)
      Grid.NavValidatorMasks[i] |= flag;
    else
      Grid.NavValidatorMasks[i] &= ~flag;
  }

  private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
  {
    if (state)
      Grid.NavMasks[i] |= flag;
    else
      Grid.NavMasks[i] &= ~flag;
  }

  public static void ResetNavMasksAndDetails()
  {
    Grid.NavMasks = (Grid.NavFlags[]) null;
    Grid.tubeEntrances.Clear();
    Grid.restrictions.Clear();
    Grid.suitMarkers.Clear();
  }

  public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation) => Grid.restrictions[cell] = new Grid.Restriction()
  {
    directionMasks = new Dictionary<int, Grid.Restriction.Directions>(),
    orientation = orientation
  };

  public static void UnregisterRestriction(int cell) => Grid.restrictions.Remove(cell);

  public static void SetRestriction(int cell, int minion, Grid.Restriction.Directions directions) => Grid.restrictions[cell].directionMasks[minion] = directions;

  public static void ClearRestriction(int cell, int minion) => Grid.restrictions[cell].directionMasks.Remove(minion);

  public static bool HasPermission(int cell, int minion, int fromCell)
  {
    DebugUtil.Assert(Grid.HasAccessDoor[cell]);
    Grid.Restriction restriction = Grid.restrictions[cell];
    Vector2I xy1 = Grid.CellToXY(cell);
    Vector2I xy2 = Grid.CellToXY(fromCell);
    Grid.Restriction.Directions directions1 = (Grid.Restriction.Directions) 0;
    switch (restriction.orientation)
    {
      case Grid.Restriction.Orientation.Vertical:
        int num1 = xy1.x - xy2.x;
        if (num1 < 0)
          directions1 |= Grid.Restriction.Directions.Left;
        if (num1 > 0)
        {
          directions1 |= Grid.Restriction.Directions.Right;
          break;
        }
        break;
      case Grid.Restriction.Orientation.Horizontal:
        int num2 = xy1.y - xy2.y;
        if (num2 > 0)
          directions1 |= Grid.Restriction.Directions.Left;
        if (num2 < 0)
        {
          directions1 |= Grid.Restriction.Directions.Right;
          break;
        }
        break;
    }
    Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions) 0;
    return !restriction.directionMasks.TryGetValue(minion, out directions2) && !restriction.directionMasks.TryGetValue(-1, out directions2) || (directions2 & directions1) == (Grid.Restriction.Directions) 0;
  }

  public static void RegisterTubeEntrance(int cell, int reservationCapacity)
  {
    DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
    Grid.HasTubeEntrance[cell] = true;
    Grid.tubeEntrances[cell] = new Grid.TubeEntrance()
    {
      reservationCapacity = reservationCapacity,
      reservations = new HashSet<int>()
    };
  }

  public static void UnregisterTubeEntrance(int cell)
  {
    DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
    Grid.HasTubeEntrance[cell] = false;
    Grid.tubeEntrances.Remove(cell);
  }

  public static bool ReserveTubeEntrance(int cell, int minion, bool reserve)
  {
    Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
    HashSet<int> reservations = tubeEntrance.reservations;
    if (!reserve)
      return reservations.Remove(minion);
    DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
    if (reservations.Count == tubeEntrance.reservationCapacity)
      return false;
    DebugUtil.Assert(reservations.Add(minion));
    return true;
  }

  public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
  {
    DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
    Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
    tubeEntrance.reservationCapacity = newReservationCapacity;
    Grid.tubeEntrances[cell] = tubeEntrance;
  }

  public static bool HasUsableTubeEntrance(int cell, int minion)
  {
    if (!Grid.HasTubeEntrance[cell])
      return false;
    Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
    if (!tubeEntrance.operational)
      return false;
    HashSet<int> reservations = tubeEntrance.reservations;
    return reservations.Count < tubeEntrance.reservationCapacity || reservations.Contains(minion);
  }

  public static bool HasReservedTubeEntrance(int cell, int minion)
  {
    DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
    return Grid.tubeEntrances[cell].reservations.Contains(minion);
  }

  public static void SetTubeEntranceOperational(int cell, bool operational)
  {
    DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
    Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
    tubeEntrance.operational = operational;
    Grid.tubeEntrances[cell] = tubeEntrance;
  }

  public static void RegisterSuitMarker(int cell)
  {
    DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
    Grid.HasSuitMarker[cell] = true;
    Grid.suitMarkers[cell] = new Grid.SuitMarker()
    {
      suitCount = 0,
      lockerCount = 0,
      flags = Grid.SuitMarker.Flags.Operational,
      suitReservations = new HashSet<int>(),
      emptyLockerReservations = new HashSet<int>()
    };
  }

  public static void UnregisterSuitMarker(int cell)
  {
    DebugUtil.Assert(Grid.HasSuitMarker[cell]);
    Grid.HasSuitMarker[cell] = false;
    Grid.suitMarkers.Remove(cell);
  }

  public static bool ReserveSuit(int cell, int minion, bool reserve)
  {
    DebugUtil.Assert(Grid.HasSuitMarker[cell]);
    Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
    HashSet<int> suitReservations = suitMarker.suitReservations;
    if (!reserve)
      return suitReservations.Remove(minion);
    if (suitReservations.Count == suitMarker.suitCount)
      return false;
    DebugUtil.Assert(suitReservations.Add(minion));
    return true;
  }

  public static bool ReserveEmptyLocker(int cell, int minion, bool reserve)
  {
    DebugUtil.Assert(Grid.HasSuitMarker[cell]);
    Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
    HashSet<int> lockerReservations = suitMarker.emptyLockerReservations;
    if (!reserve)
      return lockerReservations.Remove(minion);
    if (lockerReservations.Count == suitMarker.emptyLockerCount)
      return false;
    DebugUtil.Assert(lockerReservations.Add(minion));
    return true;
  }

  public static void UpdateSuitMarker(
    int cell,
    int fullLockerCount,
    int emptyLockerCount,
    Grid.SuitMarker.Flags flags,
    PathFinder.PotentialPath.Flags pathFlags)
  {
    DebugUtil.Assert(Grid.HasSuitMarker[cell]);
    Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
    suitMarker.suitCount = fullLockerCount;
    suitMarker.lockerCount = fullLockerCount + emptyLockerCount;
    suitMarker.flags = flags;
    suitMarker.pathFlags = pathFlags;
    Grid.suitMarkers[cell] = suitMarker;
  }

  public static bool TryGetSuitMarkerFlags(
    int cell,
    out Grid.SuitMarker.Flags flags,
    out PathFinder.PotentialPath.Flags pathFlags)
  {
    if (Grid.HasSuitMarker[cell])
    {
      flags = Grid.suitMarkers[cell].flags;
      pathFlags = Grid.suitMarkers[cell].pathFlags;
      return true;
    }
    flags = (Grid.SuitMarker.Flags) 0;
    pathFlags = PathFinder.PotentialPath.Flags.None;
    return false;
  }

  public static bool HasSuit(int cell, int minion)
  {
    if (!Grid.HasSuitMarker[cell])
      return false;
    Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
    HashSet<int> suitReservations = suitMarker.suitReservations;
    return suitReservations.Count < suitMarker.suitCount || suitReservations.Contains(minion);
  }

  public static bool HasEmptyLocker(int cell, int minion)
  {
    if (!Grid.HasSuitMarker[cell])
      return false;
    Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
    HashSet<int> lockerReservations = suitMarker.emptyLockerReservations;
    return lockerReservations.Count < suitMarker.emptyLockerCount || lockerReservations.Contains(minion);
  }

  public static unsafe void InitializeCells()
  {
    for (int index = 0; index != Grid.WidthInCells * Grid.HeightInCells; ++index)
    {
      byte num = Grid.elementIdx[index];
      global::Element element = ElementLoader.elements[(int) num];
      Grid.Element[index] = element;
      if (element.IsSolid)
        Grid.BuildMasks[index] |= Grid.BuildFlags.Solid;
      else
        Grid.BuildMasks[index] &= ~Grid.BuildFlags.Solid;
      Grid.RenderedByWorld[index] = element.substance != null && element.substance.renderedByWorld && (UnityEngine.Object) Grid.Objects[index, 9] == (UnityEngine.Object) null;
    }
  }

  public static unsafe bool IsInitialized() => (IntPtr) Grid.mass != IntPtr.Zero;

  public static int GetCellInDirection(int cell, Direction d)
  {
    switch (d)
    {
      case Direction.Up:
        return Grid.CellAbove(cell);
      case Direction.Right:
        return Grid.CellRight(cell);
      case Direction.Down:
        return Grid.CellBelow(cell);
      case Direction.Left:
        return Grid.CellLeft(cell);
      case Direction.None:
        return cell;
      default:
        return -1;
    }
  }

  public static int CellAbove(int cell) => cell + Grid.WidthInCells;

  public static int CellBelow(int cell) => cell - Grid.WidthInCells;

  public static int CellLeft(int cell) => cell % Grid.WidthInCells <= 0 ? -1 : cell - 1;

  public static int CellRight(int cell) => cell % Grid.WidthInCells >= Grid.WidthInCells - 1 ? -1 : cell + 1;

  public static CellOffset GetOffset(int cell)
  {
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    return new CellOffset(x, y);
  }

  public static int CellUpLeft(int cell)
  {
    int num = -1;
    if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
      num = cell - 1 + Grid.WidthInCells;
    return num;
  }

  public static int CellUpRight(int cell)
  {
    int num = -1;
    if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
      num = cell + 1 + Grid.WidthInCells;
    return num;
  }

  public static int CellDownLeft(int cell)
  {
    int num = -1;
    if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
      num = cell - 1 - Grid.WidthInCells;
    return num;
  }

  public static int CellDownRight(int cell)
  {
    int num = -1;
    if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
      num = cell + 1 - Grid.WidthInCells;
    return num;
  }

  public static bool IsCellLeftOf(int cell, int other_cell) => Grid.CellColumn(cell) < Grid.CellColumn(other_cell);

  public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
  {
    int length = target_offsets.Length;
    for (int index = 0; index < length; ++index)
    {
      if (cell == Grid.OffsetCell(target_cell, target_offsets[index]))
        return true;
    }
    return false;
  }

  public static bool IsCellOffsetOf(int cell, GameObject target, CellOffset[] target_offsets)
  {
    int cell1 = Grid.PosToCell(target);
    return Grid.IsCellOffsetOf(cell, cell1, target_offsets);
  }

  public static int GetCellDistance(int cell_a, int cell_b)
  {
    CellOffset offset = Grid.GetOffset(cell_a, cell_b);
    return Math.Abs(offset.x) + Math.Abs(offset.y);
  }

  public static int GetCellRange(int cell_a, int cell_b)
  {
    CellOffset offset = Grid.GetOffset(cell_a, cell_b);
    return Math.Max(Math.Abs(offset.x), Math.Abs(offset.y));
  }

  public static CellOffset GetOffset(int base_cell, int offset_cell)
  {
    int x1;
    int y1;
    Grid.CellToXY(base_cell, out x1, out y1);
    int x2;
    int y2;
    Grid.CellToXY(offset_cell, out x2, out y2);
    return new CellOffset(x2 - x1, y2 - y1);
  }

  public static int OffsetCell(int cell, CellOffset offset) => cell + offset.x + offset.y * Grid.WidthInCells;

  public static int OffsetCell(int cell, int x, int y) => cell + x + y * Grid.WidthInCells;

  public static bool IsCellOffsetValid(int cell, int x, int y)
  {
    int x1;
    int y1;
    Grid.CellToXY(cell, out x1, out y1);
    return x1 + x >= 0 && x1 + x < Grid.WidthInCells && y1 + y >= 0 && y1 + y < Grid.HeightInCells;
  }

  public static bool IsCellOffsetValid(int cell, CellOffset offset) => Grid.IsCellOffsetValid(cell, offset.x, offset.y);

  public static int PosToCell(StateMachine.Instance smi) => Grid.PosToCell(smi.transform.GetPosition());

  public static int PosToCell(GameObject go) => Grid.PosToCell(go.transform.GetPosition());

  public static int PosToCell(KMonoBehaviour cmp) => Grid.PosToCell(cmp.transform.GetPosition());

  public static bool IsValidBuildingCell(int cell) => cell >= 0 && cell < Grid.CellCount - Grid.WidthInCells * Grid.TopBorderHeight;

  public static bool IsValidCell(int cell) => cell >= 0 && cell < Grid.CellCount;

  public static bool IsCellOpenToSpace(int cell) => !Grid.IsSolidCell(cell) && !((UnityEngine.Object) Grid.Objects[cell, 2] != (UnityEngine.Object) null) && World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;

  public static int PosToCell(Vector2 pos)
  {
    float x = pos.x;
    int num1 = (int) ((double) pos.y + 0.0500000007450581);
    int num2 = (int) x;
    int widthInCells = Grid.WidthInCells;
    return num1 * widthInCells + num2;
  }

  public static int PosToCell(Vector3 pos)
  {
    float x = pos.x;
    int num1 = (int) ((double) pos.y + 0.0500000007450581);
    int num2 = (int) x;
    int widthInCells = Grid.WidthInCells;
    return num1 * widthInCells + num2;
  }

  public static void PosToXY(Vector3 pos, out int x, out int y) => Grid.CellToXY(Grid.PosToCell(pos), out x, out y);

  public static void PosToXY(Vector3 pos, out Vector2I xy) => Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);

  public static Vector2I PosToXY(Vector3 pos)
  {
    Vector2I vector2I;
    Grid.CellToXY(Grid.PosToCell(pos), out vector2I.x, out vector2I.y);
    return vector2I;
  }

  public static int XYToCell(int x, int y) => x + y * Grid.WidthInCells;

  public static void CellToXY(int cell, out int x, out int y)
  {
    x = Grid.CellColumn(cell);
    y = Grid.CellRow(cell);
  }

  public static Vector2I CellToXY(int cell) => new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));

  public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
  {
    int widthInCells = Grid.WidthInCells;
    double num1 = (double) Grid.CellSizeInMeters * (double) (cell % widthInCells);
    float num2 = Grid.CellSizeInMeters * (float) (cell / widthInCells);
    double num3 = (double) x_offset;
    return new Vector3((float) (num1 + num3), num2 + y_offset, z_offset);
  }

  public static Vector3 CellToPos(int cell)
  {
    int widthInCells = Grid.WidthInCells;
    return new Vector3(Grid.CellSizeInMeters * (float) (cell % widthInCells), Grid.CellSizeInMeters * (float) (cell / widthInCells), 0.0f);
  }

  public static Vector3 CellToPos2D(int cell)
  {
    int widthInCells = Grid.WidthInCells;
    return (Vector3) new Vector2(Grid.CellSizeInMeters * (float) (cell % widthInCells), Grid.CellSizeInMeters * (float) (cell / widthInCells));
  }

  public static int CellRow(int cell) => cell / Grid.WidthInCells;

  public static int CellColumn(int cell) => cell % Grid.WidthInCells;

  public static int ClampX(int x) => Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);

  public static int ClampY(int y) => Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);

  public static Vector2I Constrain(Vector2I val)
  {
    val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
    val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
    return val;
  }

  public static void Reveal(int cell, byte visibility = 255)
  {
    int num = Grid.Spawnable[cell] != (byte) 0 ? 0 : (visibility > (byte) 0 ? 1 : 0);
    Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
    if (!Grid.PreventFogOfWarReveal[cell])
      Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
    if (num == 0 || Grid.OnReveal == null)
      return;
    Grid.OnReveal(cell);
  }

  public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
  {
    switch (conduit_type)
    {
      case ConduitType.Gas:
        return ObjectLayer.GasConduitConnection;
      case ConduitType.Liquid:
        return ObjectLayer.LiquidConduitConnection;
      case ConduitType.Solid:
        return ObjectLayer.SolidConduitConnection;
      default:
        throw new ArgumentException("Invalid value.", nameof (conduit_type));
    }
  }

  public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
  {
    switch (alignment)
    {
      case CellAlignment.Bottom:
        return Grid.CellToPosCBC(cell, layer);
      case CellAlignment.Top:
        return Grid.CellToPosCTC(cell, layer);
      case CellAlignment.Left:
        return Grid.CellToPosLCC(cell, layer);
      case CellAlignment.Right:
        return Grid.CellToPosRCC(cell, layer);
      case CellAlignment.RandomInternal:
        Vector3 vector3 = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0.0f, 0.0f);
        return Grid.CellToPosCCC(cell, layer) + vector3;
      default:
        return Grid.CellToPosCCC(cell, layer);
    }
  }

  public static float GetLayerZ(Grid.SceneLayer layer) => (float) (-(double) Grid.HalfCellSizeInMeters - (double) Grid.CellSizeInMeters * (double) layer * (double) Grid.LayerMultiplier);

  public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));

  public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));

  public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float) layer * Grid.LayerMultiplier);

  public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));

  public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));

  public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer) => Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));

  public static bool IsSolidCell(int cell) => Grid.IsValidCell(cell) && Grid.Solid[cell];

  public static unsafe bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
  {
    if (Grid.IsValidCell(cell))
    {
      byte num = Grid.elementIdx[cell];
      if ((int) num < ElementLoader.elements.Count)
      {
        global::Element element = ElementLoader.elements[(int) num];
        if (element.IsLiquid && (double) Grid.mass[cell] >= (double) element.defaultValues.mass * (double) threshold)
          return true;
      }
    }
    return false;
  }

  public static bool IsVisiblyInLiquid(Vector2 pos)
  {
    int cell1 = Grid.PosToCell(pos);
    if (Grid.IsValidCell(cell1) && Grid.IsLiquid(cell1))
    {
      int cell2 = Grid.CellAbove(cell1);
      if (Grid.IsValidCell(cell2) && Grid.IsLiquid(cell2))
        return true;
      if ((double) Grid.Mass[cell1] / 1000.0 <= (double) ((float) (int) pos.y - pos.y))
        return true;
    }
    return false;
  }

  public static bool IsLiquid(int cell) => ElementLoader.elements[(int) Grid.ElementIdx[cell]].IsLiquid;

  public static bool IsGas(int cell) => ElementLoader.elements[(int) Grid.ElementIdx[cell]].IsGas;

  public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
  {
    Vector3 worldPoint1 = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
    Vector3 worldPoint2 = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
    min_y = (int) worldPoint2.y;
    max_y = (int) ((double) worldPoint1.y + 0.5);
    min_x = (int) worldPoint2.x;
    max_x = (int) ((double) worldPoint1.x + 0.5);
  }

  public static void GetVisibleExtents(out Vector2I min, out Vector2I max) => Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);

  public static bool IsVisible(int cell) => Grid.Visible[cell] > (byte) 0 || !PropertyTextures.IsFogOfWarEnabled;

  public static bool VisibleBlockingCB(int cell) => !Grid.Transparent[cell] && Grid.IsSolidCell(cell);

  public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false) => Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible);

  public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
  {
    int x1 = 0;
    int y1 = 0;
    Grid.CellToXY(cell, out x1, out y1);
    int x2 = 0;
    int y2 = 0;
    Grid.CellToXY(target_cell, out x2, out y2);
    return Grid.VisibilityTest(x1, y1, x2, y2, blocking_tile_visible);
  }

  public static bool PhysicalBlockingCB(int cell) => Grid.Solid[cell];

  public static bool IsPhysicallyAccessible(
    int x,
    int y,
    int x2,
    int y2,
    bool blocking_tile_visible = false)
  {
    return Grid.TestLineOfSight(x, y, x2, y2, Grid.PhysicalBlockingDelegate, blocking_tile_visible);
  }

  public static bool TestLineOfSight(
    int x,
    int y,
    int x2,
    int y2,
    Func<int, bool> blocking_cb,
    bool blocking_tile_visible = false)
  {
    int num1 = x;
    int num2 = y;
    int num3 = x2 - x;
    int num4 = y2 - y;
    int num5 = 0;
    int num6 = 0;
    int num7 = 0;
    int num8 = 0;
    if (num3 < 0)
      num5 = -1;
    else if (num3 > 0)
      num5 = 1;
    if (num4 < 0)
      num6 = -1;
    else if (num4 > 0)
      num6 = 1;
    if (num3 < 0)
      num7 = -1;
    else if (num3 > 0)
      num7 = 1;
    int num9 = Math.Abs(num3);
    int num10 = Math.Abs(num4);
    if (num9 <= num10)
    {
      num9 = Math.Abs(num4);
      num10 = Math.Abs(num3);
      if (num4 < 0)
        num8 = -1;
      else if (num4 > 0)
        num8 = 1;
      num7 = 0;
    }
    int num11 = num9 >> 1;
    for (int index = 0; index <= num9; ++index)
    {
      int cell = Grid.XYToCell(x, y);
      if (!Grid.IsValidCell(cell))
        return false;
      bool flag = blocking_cb(cell);
      if (((x != num1 ? 1 : (y != num2 ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        return blocking_tile_visible && x == x2 && y == y2;
      num11 += num10;
      if (num11 >= num9)
      {
        num11 -= num9;
        x += num5;
        y += num6;
      }
      else
      {
        x += num7;
        y += num8;
      }
    }
    return true;
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawBoxOnCell(int cell, Color color, float offset = 0.0f)
  {
    Vector3 vector3 = Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0.0f);
  }

  [System.Flags]
  public enum BuildFlags : byte
  {
    Solid = 1,
    Foundation = 2,
    Door = 4,
    FakeFloor = 8,
    DupePassable = 16, // 0x10
    DupeImpassable = 32, // 0x20
    CritterImpassable = 64, // 0x40
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsFoundationIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsSolidIndexer
  {
    public bool this[int i] => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > 0U;
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsDupeImpassableIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsFakeFloorIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.FakeFloor, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsDupePassableIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsImpassableIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildFlagsDoorIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > 0U;
      set => Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
    }
  }

  [System.Flags]
  public enum VisFlags : byte
  {
    Revealed = 1,
    PreventFogOfWarReveal = 2,
    RenderedByWorld = 4,
    AllowPathfinding = 8,
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VisFlagsRevealedIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > 0U;
      set => Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VisFlagsPreventFogOfWarRevealIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > 0U;
      set => Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VisFlagsRenderedByWorldIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > 0U;
      set => Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VisFlagsAllowPathfindingIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > 0U;
      set => Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
    }
  }

  [System.Flags]
  public enum NavValidatorFlags : byte
  {
    Ladder = 1,
    Pole = 2,
    Tube = 4,
    UnderConstruction = 8,
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavValidatorFlagsLadderIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > 0U;
      set => Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavValidatorFlagsPoleIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > 0U;
      set => Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavValidatorFlagsTubeIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > 0U;
      set => Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavValidatorFlagsUnderConstructionIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > 0U;
      set => Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
    }
  }

  [System.Flags]
  public enum NavFlags : byte
  {
    AccessDoor = 1,
    TubeEntrance = 2,
    PreventIdleTraversal = 4,
    Reserved = 8,
    SuitMarker = 16, // 0x10
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavFlagsAccessDoorIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > 0U;
      set => Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavFlagsTubeEntranceIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > 0U;
      set => Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavFlagsPreventIdleTraversalIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > 0U;
      set => Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavFlagsReservedIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > 0U;
      set => Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NavFlagsSuitMarkerIndexer
  {
    public bool this[int i]
    {
      get => (uint) (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > 0U;
      set => Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
    }
  }

  public struct Restriction
  {
    public const int DefaultID = -1;
    public Dictionary<int, Grid.Restriction.Directions> directionMasks;
    public Grid.Restriction.Orientation orientation;

    [System.Flags]
    public enum Directions : byte
    {
      Left = 1,
      Right = 2,
    }

    public enum Orientation : byte
    {
      Vertical,
      Horizontal,
    }
  }

  private struct TubeEntrance
  {
    public bool operational;
    public int reservationCapacity;
    public HashSet<int> reservations;
  }

  public struct SuitMarker
  {
    public int suitCount;
    public int lockerCount;
    public Grid.SuitMarker.Flags flags;
    public PathFinder.PotentialPath.Flags pathFlags;
    public HashSet<int> suitReservations;
    public HashSet<int> emptyLockerReservations;

    public int emptyLockerCount => this.lockerCount - this.suitCount;

    [System.Flags]
    public enum Flags : byte
    {
      OnlyTraverseIfUnequipAvailable = 1,
      Operational = 2,
      Rotated = 4,
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ObjectLayerIndexer
  {
    public GameObject this[int cell, int layer]
    {
      get
      {
        GameObject gameObject = (GameObject) null;
        Grid.ObjectLayers[layer].TryGetValue(cell, out gameObject);
        return gameObject;
      }
      set
      {
        if ((UnityEngine.Object) value == (UnityEngine.Object) null)
          Grid.ObjectLayers[layer].Remove(cell);
        else
          Grid.ObjectLayers[layer][cell] = value;
        GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], (object) value);
      }
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct PressureIndexer
  {
    public unsafe float this[int i] => Grid.mass[i] * 101.3f;
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct TransparentIndexer
  {
    public unsafe bool this[int i] => ((uint) Grid.properties[i] & 16U) > 0U;
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ElementIdxIndexer
  {
    public unsafe byte this[int i] => Grid.elementIdx[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct TemperatureIndexer
  {
    public unsafe float this[int i] => Grid.temperature[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct MassIndexer
  {
    public unsafe float this[int i] => Grid.mass[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct PropertiesIndexer
  {
    public unsafe byte this[int i] => Grid.properties[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ExposedToSunlightIndexer
  {
    public unsafe byte this[int i] => Grid.exposedToSunlight[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct StrengthInfoIndexer
  {
    public unsafe byte this[int i] => Grid.strengthInfo[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Insulationndexer
  {
    public unsafe byte this[int i] => Grid.insulation[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DiseaseIdxIndexer
  {
    public unsafe byte this[int i] => Grid.diseaseIdx[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DiseaseCountIndexer
  {
    public unsafe int this[int i] => Grid.diseaseCount[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct AccumulatedFlowIndexer
  {
    public unsafe float this[int i] => Grid.AccumulatedFlowValues[i];
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct LightIntensityIndexer
  {
    public unsafe int this[int i] => (int) ((double) Grid.exposedToSunlight[i] / (double) byte.MaxValue * (double) Game.Instance.currentSunlightIntensity) + Grid.LightCount[i];
  }

  public enum SceneLayer
  {
    NoLayer = -2, // 0xFFFFFFFE
    Background = -1, // 0xFFFFFFFF
    Backwall = 1,
    Gas = 2,
    GasConduits = 3,
    GasConduitBridges = 4,
    LiquidConduits = 5,
    LiquidConduitBridges = 6,
    SolidConduits = 7,
    SolidConduitContents = 8,
    SolidConduitBridges = 9,
    Wires = 10, // 0x0000000A
    WireBridges = 11, // 0x0000000B
    WireBridgesFront = 12, // 0x0000000C
    LogicWires = 13, // 0x0000000D
    LogicGates = 14, // 0x0000000E
    LogicGatesFront = 15, // 0x0000000F
    InteriorWall = 16, // 0x00000010
    GasFront = 17, // 0x00000011
    BuildingBack = 18, // 0x00000012
    Building = 19, // 0x00000013
    BuildingUse = 20, // 0x00000014
    BuildingFront = 21, // 0x00000015
    TransferArm = 22, // 0x00000016
    Ore = 23, // 0x00000017
    Creatures = 24, // 0x00000018
    Move = 25, // 0x00000019
    Front = 26, // 0x0000001A
    GlassTile = 27, // 0x0000001B
    Liquid = 28, // 0x0000001C
    Ground = 29, // 0x0000001D
    TileMain = 30, // 0x0000001E
    TileFront = 31, // 0x0000001F
    FXFront = 32, // 0x00000020
    FXFront2 = 33, // 0x00000021
    SceneMAX = 34, // 0x00000022
  }
}
