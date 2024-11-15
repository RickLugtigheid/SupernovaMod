using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SupernovaMod.Common.World.Dreamlands
{
    public class DreamlandsSwampBiomePass : GenPass
    {
        private const int BIOME_WIDTH = 340;

        private readonly Mod _mod;
        private GenerationProgress _progress;
        private int _biomeCenterX;
        private int _biomeCenterY;

        public DreamlandsSwampBiomePass(Mod ourMod) : base(ourMod.DisplayName + ": Dreamlands Swamp", 600)
        {
            _mod = ourMod;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            _progress = progress;
            SetProgress("Initializing...", 0);
            Setup();
            SetProgress("Finding biome spot", .01f);
            _biomeCenterX = DetermineBiomeCenter();
            _biomeCenterY = Scan_FindBiomeHeight(_biomeCenterX, (int)Main.worldSurface - 100, (int)Main.worldSurface + 100);
            _mod.Logger.Info(_progress.Message + " - Center(x:"+_biomeCenterX+", y:"+_biomeCenterY+")");

            SetProgress("Flattening terrain", .1f);
            MorphStep_0_FlattenTerrain();
            SetProgress("Calculating some data", .3f);
            MorphStep_1_CalculateData();
            SetProgress("Adding surface water", .9f);
            MorphStep_2_AddSurfaceWater();
        }

        private void Setup()
        {

        }

        private int DetermineBiomeCenter()
        {
            float randPos = (float)WorldGen.genRand.Next(15, 30) * 0.015f;
            if (WorldGen.genRand.NextBool())
            {
                randPos = 1f - randPos;
            }
            return (int)(Main.maxTilesX * randPos);
        }

        private int Scan_FindBiomeHeight(int x, int minY, int maxY)
        {
            int y;
            for (y = minY; y < maxY; y++)
            {
                // Go up until there is no tile left anymore.
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile)
                {
                    continue;
                }
            }
            // Return the biome height from the specified x position
            return y;
        }

        private void SetProgress(string step, float progress)
        {
            _progress.Message = _mod.DisplayName + ": Dreamlands Swamp - " + step;
            _mod.Logger.Info(_progress.Message);
            _progress.Set(progress);
        }

        private void MorphStep_0_FlattenTerrain()
        {
            int widthHalve = BIOME_WIDTH / 2;
            for (int i2 = _biomeCenterX - widthHalve; i2 < _biomeCenterX + widthHalve; i2++)
            {
                // Flattern terrain
                int j2 = (int)DreamlandsSubworld.worldSurfaceLow + 50;
                while ((double)j2 < DreamlandsSubworld.worldSurfaceHigh)
                {
                    if (WorldGen.CanKillTile(i2, j2))
                    {
                        Main.tile[i2, j2].ClearEverything();
                        //Tile tile = Framing.GetTileSafely(i2, j2);
                        //tile.HasTile = false;
                        //_mod.Logger.Info(_progress.Message + " - MorphStep_0_FlattenTerrain() KILLED Main.tile[" + i2 + ", " + j2 + "]");
                        //WorldGen.KillWall(i2, j2);
                    }
                    j2++;
                }

                // Fill holes
                int j3 = (int)DreamlandsSubworld.worldSurfaceLow + 100;
                while ((double)j3 < DreamlandsSubworld.worldSurfaceHigh)
                {
                    if (!Main.tile[i2, j3].HasTile)
                    {
                        Tile tile = Framing.GetTileSafely(i2, j3);
                        tile.HasTile = true;
                        tile.TileType = TileID.Mud;//(ushort)ModContent.TileType<Content.Tiles.Dreamlands.OtherworldlyStone>();
                        tile.WallType = WallID.MudUnsafe;
                        //Main.tile[i2, j3].ResetToType((ushort)ModContent.TileType<Content.Tiles.Dreamlands.OtherworldlyStone>()); // TODO: Store type to reduce ModContent.TileType method calls
                    }
                    j3++;
                }
            }
        }
        private void MorphStep_1_CalculateData()
        {

        }
        private void MorphStep_2_AddSurfaceWater()
        {
            bool placeTree = false;
            int nextWater = 0;
            int waterSize = 0;
            int widthHalve = BIOME_WIDTH / 2;
            for (int i2 = _biomeCenterX - widthHalve; i2 < _biomeCenterX + widthHalve; i2++)
            {
                if (waterSize == 0 && nextWater == 0)
                {
                    placeTree = WorldGen.genRand.NextBool();
                    nextWater = WorldGen.genRand.Next(3, 12);
                    waterSize = WorldGen.genRand.Next(8, 21);
                }
                if (nextWater > 0)
                {
                    if (placeTree && nextWater < 6)
                    {
                        placeTree = false;
                        int j = (int)DreamlandsSubworld.worldSurfaceLow - 50;
                        for (int i = (int)DreamlandsSubworld.worldSurfaceLow - 50; i < (DreamlandsSubworld.worldSurfaceHigh + 50); i++)
                        {
                            if (Main.tile[i2, j].HasTile)
                            {
                                j++;
                            }
                        }
                        WorldGen.GrowTree(i2, j);
                    }
                    nextWater--;
                    continue;
                }
                int j2 = (int)DreamlandsSubworld.worldSurfaceLow - 50;
                while ((double)j2 < DreamlandsSubworld.worldSurfaceHigh + 50)
                {
                    if (!Main.tile[i2, j2].HasTile)
                    {
                        j2++;
                        continue;
                    }
                    WorldGenSystem.CustomTileRunner(WorldGen.genRand, new Point(i2, j2), WorldGen.genRand.Next(2, 6), 0, WorldGen.genRand.Next(3, 6), Vector2.Zero, (x, y) =>
                    {
                        _mod.Logger.Info(_progress.Message + " - MorphStep_0_FlattenTerrain() - CustomTileRunner x:" + x + " y2:" + y);
                        if (WorldGen.CanKillTile(x, y))
                        {
                            Tile tile = Framing.GetTileSafely(x, y);
                            tile.HasTile = false;
                            //tile.LiquidAmount = byte.MaxValue;
                            WorldGen.PlaceLiquid(x, y, (byte)LiquidID.Water, byte.MaxValue);
                        }
                    });
                    break;
                }
                waterSize--;
            }
        }
    }
}
