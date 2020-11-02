using Harmony;
using ProcGenGame;
using ProcGen;
using System.Collections.Generic;
using System;

namespace Neutronium_Cave
{
    public class Patches
    {

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("Hello from Neutronium_Cave!");
            }
        }
        
        [HarmonyPatch(typeof(WorldGen))]
        [HarmonyPatch("SetupDefaultElements")]
        public class WorldGen_SetupDefaultElements_Patch
        {
            public static void Postfix()
            {
                // Debug.Log("Init world with neutronium");
                WorldGen.katairiteElement = WorldGen.unobtaniumElement;
            }
        }

        [HarmonyPatch(typeof(Border))]
        [HarmonyPatch("ConvertToMap")]
        public class Border_ConvertToMap_Patch
        {
            public static void Prefix(Border __instance)
            {
                if (!__instance.neighbors.n0.node.tags.Contains(WorldGenTags.AtSurface) && 
                    !__instance.neighbors.n1.node.tags.Contains(WorldGenTags.AtSurface))
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
            static HashSet<int> _borderCells;
            static Element unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
            static Element katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
            static byte katairiteIdx = katairiteElement.idx;
            static float katairiteTemperature  = katairiteElement.defaultValues.temperature;
            static float katairiteMass = katairiteElement.defaultValues.mass;

            public static void Prefix(
                Sim.Cell[] cells,
                Chunk world,
                SeededRandom rnd,
                HashSet<int> borderCells,
                WorldGen.OfflineCallbackFunction updateProgressFn)
            {
                _cells = cells;
                _world = world;
                _borderCells = borderCells;
            }

            public static void Postfix(WorldGen __instance)
            {
                Debug.Log("I execute after WorldGen.DrawWorldBorder!");

                foreach (TerrainCell overworldCell in __instance.OverworldCells)
                {
                    SubWorld subWorld = __instance.Settings.GetSubWorld(overworldCell.node.type);
                    if (subWorld != null)
                    {
                        //subWorld.
                        var x = (int)Math.Round(overworldCell.node.position.x);
                        var y = (int)Math.Round(overworldCell.node.position.y);
                        // Debug.Log("----");
                        Debug.Log("Node: " + overworldCell.node.type + " at " + x + "|" + y + " ("+ overworldCell.node.tags.ToString() + ")");

                        var borderTop = FindNextBorderCell(x, y, 0, 1);
                        if (overworldCell.poly.PointInPolygon(Grid.CellToPos(borderTop))) ReplaceVerticalBorderCells(borderTop, true);

                        var borderRight = FindNextBorderCell(x, y, 1, 0);
                        if (overworldCell.poly.PointInPolygon(Grid.CellToPos(borderRight))) ReplaceHorizontalBorderCells(borderRight, true);
                        /*
                        var borderBottom = FindNextBorderCell(x, y, 0, -1);
                        ReplaceVerticalBorderCells(borderBottom, false);

                        var borderLeft = FindNextBorderCell(x, y, -1, 0);
                        ReplaceHorizontalBorderCells(borderLeft, false);
                        */
                    }
                }

                // cleanup
                _cells = null;
                _world = null;
                _borderCells = null;
            }

            private static void ReplaceVerticalBorderCells(int cell, bool inc)
            {
                if (cell == -1) return;
                var position = Grid.CellToXY(cell);
                var x = position.x;
                var y = position.y;
                var replace = true;

                while (replace && Math.Abs(y - position.y) < 5)
                {
                    // center
                    replace = ReplaceBorderWithAbysalite(cell);
                    // left + right
                   // ReplaceBorderWithAbysalite(Grid.XYToCell(x - 1, y));
                    ReplaceBorderWithAbysalite(Grid.XYToCell(x + 1, y));
                    // next
                    y = inc ? y + 1 : y - 1;
                    cell = Grid.XYToCell(x, y);
                }

                // before & after
                var min = Math.Min(y, position.y);
                var max = Math.Max(y, position.y);
                if (inc) max--;
                else min++;
              //  ReplaceBorderWithAbysalite(Grid.XYToCell(x - 1, min - 1));
                ReplaceBorderWithAbysalite(Grid.XYToCell(x + 1, min - 1));
              //  ReplaceBorderWithAbysalite(Grid.XYToCell(x - 1, max + 1));
                ReplaceBorderWithAbysalite(Grid.XYToCell(x + 1, max + 1));
            }

            private static void ReplaceHorizontalBorderCells(int cell, bool inc)
            {
                if (cell == -1) return;
                var position = Grid.CellToXY(cell);
                var x = position.x;
                var y = position.y;
                var replace = true;

                while (replace && Math.Abs(x - position.x) < 5)
                {
                    // center
                    replace = ReplaceBorderWithAbysalite(cell);
                    // upper + lower
                  //  ReplaceBorderWithAbysalite(Grid.XYToCell(x, y - 1));
                    ReplaceBorderWithAbysalite(Grid.XYToCell(x, y + 1));
                    // next
                    x = inc ? x + 1 : x - 1;
                    cell = Grid.XYToCell(x, y);
                }

                // before & after
                var min = Math.Min(x, position.x);
                var max = Math.Max(x, position.x);
                if (inc) max--;
                else min++;
              //  ReplaceBorderWithAbysalite(Grid.XYToCell(min - 1, y - 1));
                ReplaceBorderWithAbysalite(Grid.XYToCell(min - 1, y + 1));
              //  ReplaceBorderWithAbysalite(Grid.XYToCell(max + 1, y - 1));
                ReplaceBorderWithAbysalite(Grid.XYToCell(max + 1, y + 1));
            }

            private static bool ReplaceBorderWithAbysalite(int cell)
            {
                try
                {
                    if (_cells[cell].elementIdx == unobtaniumElement.idx)
                    {
                        _cells[cell].SetValues(katairiteIdx, katairiteTemperature, katairiteMass);
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
