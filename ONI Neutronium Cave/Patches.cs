using Harmony;
using ProcGenGame;
using ProcGen;
using System.Collections.Generic;
using System;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace Neutronium_Cave
{
    public class Patches
    {
        public static ModOptions Options;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                PUtil.InitLibrary(true);
                POptions.RegisterOptions(typeof(ModOptions));
            }
        }
        
        [HarmonyPatch(typeof(WorldGen))]
        [HarmonyPatch("SetupDefaultElements")]
        public class WorldGen_SetupDefaultElements_Patch
        {
            public static void Postfix(WorldGen __instance)
            {
                // init Options
                Options = POptions.ReadSettings<ModOptions>() ?? new ModOptions();
            }
        }

        [HarmonyPatch(typeof(Border))]
        [HarmonyPatch("ConvertToMap")]
        public class Border_ConvertToMap_Patch
        {
            public static void Prefix(Border __instance)
            {
                if (!Options.Enabled) return;

                if (Options.DefaultSpaceBorder && 
                    (__instance.neighbors.n0.node.tags.Contains(WorldGenTags.ErodePointToWorldTop) ||
                    __instance.neighbors.n1.node.tags.Contains(WorldGenTags.ErodePointToWorldTop)))
                {
#if DEBUG
                    Debug.Log("DefaultSpaceBorder");
#endif
                }
                else
                {
                    __instance.element = SettingsCache.borders["impenetrable"];
                }
            }
        }   

        [HarmonyPatch(typeof(WorldGen))]
        [HarmonyPatch("DrawWorldBorder")]
        public class WorldGen_DrawWorldBorder_Patch
        {
            static Sim.Cell[] _cells;
            static Chunk _world;
            static bool _useGranite;

            static Element unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
            static Element katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
            static Element graniteElement = ElementLoader.FindElementByHash(SimHashes.Granite);

            public static void Prefix(
                Sim.Cell[] cells,
                Chunk world,
                SeededRandom rnd,
                HashSet<int> borderCells)
            {
                _cells = cells;
                _world = world;
            }

            public static void Postfix(WorldGen __instance)
            {
                if (!Options.Enabled) return;

                foreach (TerrainCell overworldCell in __instance.OverworldCells)
                {
                    SubWorld subWorld = __instance.Settings.GetSubWorld(overworldCell.node.type);
                    if (subWorld != null)
                    {
                        var x = (int)Math.Round(overworldCell.node.position.x);
                        var y = (int)Math.Round(overworldCell.node.position.y);
#if DEBUG
                        Debug.Log("Node: " + overworldCell.node.type + " at " + x + "|" + y);
#endif
                        var borderTop = FindNextBorderCell(x, y, 0, 1);
                        var borderRight = FindNextBorderCell(x, y, 1, 0);
                        var borderBottom = FindNextBorderCell(x, y, 0, -1);
                        var borderLeft = FindNextBorderCell(x, y, -1, 0);

                        _useGranite = overworldCell.node.tags.Contains(WorldGenTags.StartWorld);

                        ReplaceVerticalBorderCells(overworldCell, borderTop, true);
                        ReplaceHorizontalBorderCells(overworldCell, borderRight, true);

                        if (Options.MoreEntrances)
                        {
                            ReplaceVerticalBorderCells(overworldCell, borderBottom, false);
                            ReplaceHorizontalBorderCells(overworldCell, borderLeft, false);
                        }
                    }
                }

                // cleanup
                _cells = null;
                _world = null;
            }

            private static void ReplaceVerticalBorderCells(TerrainCell overworldCell, int cell, bool inc)
            {
                if (cell == -1) return;
                if (!overworldCell.poly.PointInPolygon(Grid.CellToPos(cell))) return;

                var position = Grid.CellToXY(cell);
                var x = position.x;
                var y = position.y;
                var replace = true;

                while (replace && Math.Abs(y - position.y) < 5)
                {
                    // center
                    replace = ReplaceElement(cell);
                    // left + right
                    ReplaceElement(Grid.XYToCell(x + 1, y));
                    if (Options.GapWidth >= 3)
                    {
                        ReplaceElement(Grid.XYToCell(x - 1, y));
                    }
                    if (Options.GapWidth >= 4)
                    {
                        ReplaceElement(Grid.XYToCell(x + 2, y));
                    }
                    if (Options.GapWidth >= 5)
                    {
                        ReplaceElement(Grid.XYToCell(x - 2, y));
                    }
                    // next
                    y = inc ? y + 1 : y - 1;
                    cell = Grid.XYToCell(x, y);
                }

                // before & after
                var min = Math.Min(y, position.y);
                var max = Math.Max(y, position.y);
                if (inc) max--;
                else min++;
                ReplaceElement(Grid.XYToCell(x + 1, min - 1));
                ReplaceElement(Grid.XYToCell(x + 1, max + 1));

                if (Options.GapWidth >= 3)
                {
                    ReplaceElement(Grid.XYToCell(x - 1, min - 1));
                    ReplaceElement(Grid.XYToCell(x - 1, max + 1));
                }
                if (Options.GapWidth >= 4)
                {
                    ReplaceElement(Grid.XYToCell(x + 2, min - 1));
                    ReplaceElement(Grid.XYToCell(x + 2, max + 1));
                }
                if (Options.GapWidth >= 5)
                {
                    ReplaceElement(Grid.XYToCell(x - 2, min - 1));
                    ReplaceElement(Grid.XYToCell(x - 2, max + 1));
                }
            }

            private static void ReplaceHorizontalBorderCells(TerrainCell overworldCell, int cell, bool inc)
            {
                if (cell == -1) return;
                if (!overworldCell.poly.PointInPolygon(Grid.CellToPos(cell))) return;

                var position = Grid.CellToXY(cell);
                var x = position.x;
                var y = position.y;
                var replace = true;

                while (replace && Math.Abs(x - position.x) < 5)
                {
                    // center
                    replace = ReplaceElement(cell);
                    // upper + lower
                    ReplaceElement(Grid.XYToCell(x, y + 1));
                    if (Options.GapWidth >= 3)
                    {
                        ReplaceElement(Grid.XYToCell(x, y - 1));
                    }
                    if (Options.GapWidth >= 4)
                    {
                        ReplaceElement(Grid.XYToCell(x, y + 2));
                    }
                    if (Options.GapWidth >= 5)
                    {
                        ReplaceElement(Grid.XYToCell(x, y - 2));
                    }
                    // next
                    x = inc ? x + 1 : x - 1;
                    cell = Grid.XYToCell(x, y);
                }

                // before & after
                var min = Math.Min(x, position.x);
                var max = Math.Max(x, position.x);
                if (inc) max--;
                else min++;
                ReplaceElement(Grid.XYToCell(min - 1, y + 1));
                ReplaceElement(Grid.XYToCell(max + 1, y + 1));

                if (Options.GapWidth >= 3)
                {
                    ReplaceElement(Grid.XYToCell(min - 1, y - 1));
                    ReplaceElement(Grid.XYToCell(max + 1, y - 1));
                }
                if (Options.GapWidth >= 4)
                {
                    ReplaceElement(Grid.XYToCell(min - 1, y + 2));
                    ReplaceElement(Grid.XYToCell(max + 1, y + 2));
                }
                if (Options.GapWidth >= 5)
                {
                    ReplaceElement(Grid.XYToCell(min - 1, y - 2));
                    ReplaceElement(Grid.XYToCell(max + 1, y - 2));
                }
            }

            private static bool ReplaceElement(int cell)
            {
                try
                {
                    if (_cells[cell].elementIdx == unobtaniumElement.idx)
                    {
                        if (_useGranite && Options.GraniteStartBiomeBorder)
                        {
                            _cells[cell].SetValues(graniteElement.idx, graniteElement.defaultValues.temperature, graniteElement.defaultValues.mass);
                        }
                        else
                        {
                            _cells[cell].SetValues(katairiteElement.idx, katairiteElement.defaultValues.temperature, katairiteElement.defaultValues.mass);
                        }                        
                        return true;
                    }
                }
                catch (Exception){}
                return false;
            }

            private static int FindNextBorderCell(int x, int y, int incX, int incY)
            {
                if (x < 0 || y < 0 || x >= _world.size.x || y >= _world.size.y) return -1;
                int cell = Grid.XYToCell(x, y);
                return _cells[cell].elementIdx == unobtaniumElement.idx ? cell : FindNextBorderCell(x + incX, y + incY, incX, incY);
            }
        }
    }
}
