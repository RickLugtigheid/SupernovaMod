using SubworldLibrary;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using ReLogic.Utilities;
using System;
using Terraria.DataStructures;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Common.World.Dreamlands
{
    public class DreamlandsSubworld : Subworld
    {
        public override string Name => "Dreamlands";

        public static double worldSurface;
        public static double worldSurfaceHigh;
        public static double worldSurfaceLow;
        public static double rockLayer;
        public static double rockLayerHigh;
        public static double rockLayerLow;


        public override int Width => 1600;
        public override int Height => 800;

        public override bool ShouldSave => false; // TODO: Change after testing
        public override bool NoPlayerSaving => false; // TODO: Change after testing
        public override List<GenPass> Tasks => GenerateWorld();

        private List<GenPass> GenerateWorld()
        {
            List<GenPass> genPasses = new List<GenPass>(); // TODO: Add initial array size to preform less resizes since we already know how many items the array will contain

            // Setup
            //
            Main.worldSurface = worldSurface;
            Main.rockLayer = rockLayer;

            ushort dirtTileID = (ushort)ModContent.TileType<Content.Tiles.Dreamlands.OtherworldlyStone>();
            ushort stoneTileID = (ushort)ModContent.TileType<Content.Tiles.Dreamlands.OtherworldlyStone>();
            genPasses.Add(new PassLegacy("Reset", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "";
                // TODO: Set any values for later generation
            }));

            // Generate basic terrain
            genPasses.Add(new PassLegacy("Terrain", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[0].Value;
                int num696 = 0;
                int num697 = 0;

                double height = Height * 2;

                worldSurface = ((double)height) * 0.3;
                worldSurface *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
                rockLayer = worldSurface + ((double)height) * 0.2;
                rockLayer *= (double)WorldGen.genRand.Next(90, 110) * 0.01;
                worldSurfaceLow = worldSurface;
                worldSurfaceHigh = worldSurface;
                rockLayerLow = rockLayer;
                rockLayerHigh = rockLayer;

                Main.worldSurface = worldSurface;
                Main.rockLayer = rockLayer;

                // For each tile in width
                //
                for (int x = 0; x < Width; x++)
                {
                    float progressLeft = (float)x / (float)Width;
                    progress.Set(progressLeft);

                    //
                    if (worldSurface < worldSurfaceLow)
                    {
                        worldSurfaceLow = worldSurface;
                    }
                    if (worldSurface > worldSurfaceHigh)
                    {
                        worldSurfaceHigh = worldSurface;
                    }
                    if (rockLayer < rockLayerLow)
                    {
                        rockLayerLow = rockLayer;
                    }
                    if (rockLayer > rockLayerHigh)
                    {
                        rockLayerHigh = rockLayer;
                    }
                    if (num697 <= 0)
                    {
                        num696 = WorldGen.genRand.Next(0, 5);
                        num697 = WorldGen.genRand.Next(5, 35);
                        if (num696 == 0)
                        {
                            num697 *= (int)((double)WorldGen.genRand.Next(5, 30) * 0.2);
                        }
                    }
                    num697--;
                    if ((double)x > (double)Width * 0.43 && (double)x < (double)Width * 0.57 && num696 >= 3)
                    {
                        num696 = WorldGen.genRand.Next(3);
                    }
                    if ((double)x > (double)Width * 0.47 && (double)x < (double)Width * 0.53)
                    {
                        num696 = 0;
                    }
                    switch (num696)
                    {
                        case 0:
                            while (WorldGen.genRand.Next(0, 7) == 0)
                            {
                                worldSurface += (double)WorldGen.genRand.Next(-1, 2);
                            }
                            break;
                        case 1:
                            while (WorldGen.genRand.Next(0, 4) == 0)
                            {
                                worldSurface -= 1.0;
                            }
                            while (WorldGen.genRand.Next(0, 10) == 0)
                            {
                                worldSurface += 1.0;
                            }
                            break;
                        case 2:
                            while (WorldGen.genRand.Next(0, 4) == 0)
                            {
                                worldSurface += 1.0;
                            }
                            while (WorldGen.genRand.Next(0, 10) == 0)
                            {
                                worldSurface -= 1.0;
                            }
                            break;
                        case 3:
                            while (WorldGen.genRand.Next(0, 2) == 0)
                            {
                                worldSurface -= 1.0;
                            }
                            while (WorldGen.genRand.Next(0, 6) == 0)
                            {
                                worldSurface += 1.0;
                            }
                            break;
                        case 4:
                            while (WorldGen.genRand.Next(0, 2) == 0)
                            {
                                worldSurface += 1.0;
                            }
                            while (WorldGen.genRand.Next(0, 5) == 0)
                            {
                                worldSurface -= 1.0;
                            }
                            break;
                    }
                    if (worldSurface < (double)height * 0.17)
                    {
                        worldSurface = (double)height * 0.17;
                        num697 = 0;
                    }
                    else if (worldSurface > (double)height * 0.3)
                    {
                        worldSurface = (double)height * 0.3;
                        num697 = 0;
                    }
                    if ((x < 275 || x > Width - 275) && worldSurface > (double)height * 0.25)
                    {
                        worldSurface = (double)height * 0.25;
                        num697 = 1;
                    }
                    while (WorldGen.genRand.Next(0, 3) == 0)
                    {
                        rockLayer += (double)WorldGen.genRand.Next(-2, 3);
                    }
                    if (rockLayer < worldSurface + (double)height * 0.05)
                    {
                        rockLayer += 1.0;
                    }
                    if (rockLayer > worldSurface + (double)height * 0.35)
                    {
                        rockLayer -= 1.0;
                    }
                    for (int num699 = 0; (double)num699 < worldSurface; num699++)
                    {
                        Main.tile[x, num699].ClearTile();
                        Main.tile[x, num699].TileFrameX = -1;
                        Main.tile[x, num699].TileFrameY = -1;
                    }
                    for (int num700 = (int)worldSurface; num700 < height; num700++)
                    {
                        if ((double)num700 < rockLayer)
                        {
                            //Main.tile[x, num700].HasTile = true;
                            //Main.tile[x, num700].type = 0;
                            Main.tile[x, num700].ResetToType(stoneTileID);
                            Main.tile[x, num700].TileFrameX = -1;
                            Main.tile[x, num700].TileFrameY = -1;
                        }
                        else
                        {
                            //Main.tile[x, num700].HasTile = true;
                            //Main.tile[x, num700].type = 1;
                            Main.tile[x, num700].ResetToType(dirtTileID);
                            Main.tile[x, num700].TileFrameX = -1;
                            Main.tile[x, num700].TileFrameY = -1;
                        }
                    }
                }
                worldSurface = worldSurfaceHigh + 25.0;
                rockLayer = rockLayerHigh;
                double num701 = (double)((int)((rockLayer - worldSurface) / 6.0) * 6);
                rockLayer = worldSurface + num701;
                //WorldGen.waterLine = (int)(rockLayer + (double)Height) / 2;
                //WorldGen.waterLine += WorldGen.genRand.Next(-100, 20);
                //WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next(50, 80);

            }));

            //
            genPasses.Add(new PassLegacy("Tunnels", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                for (int x = 0; x < (int)((double)Width * 0.0015); x++)
                {
                    int[] array = new int[10];
                    int[] array2 = new int[10];
                    int num692 = WorldGen.genRand.Next(450, Width - 450);
                    while ((float)num692 > (float)Width * 0.45f && (float)num692 < (float)Width * 0.55f)
                    {
                        num692 = WorldGen.genRand.Next(0, Width);
                    }
                    int num693 = 0;
                    for (int num694 = 0; num694 < 10; num694++)
                    {
                        for (num692 %= Width; !Main.tile[num692, num693].HasTile; num693++)
                        {
                        }
                        array[num694] = num692;
                        array2[num694] = num693 - WorldGen.genRand.Next(11, 16);
                        num692 += WorldGen.genRand.Next(5, 11);
                    }
                    for (int num695 = 0; num695 < 10; num695++)
                    {
                        WorldGen.TileRunner(array[num695], array2[num695], (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), dirtTileID, true, -2f, -0.3f, false, true);
                        WorldGen.TileRunner(array[num695], array2[num695], (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), dirtTileID, true, 2f, -0.3f, false, true);
                    }
                }
            }));

            //
            genPasses.Add(new PassLegacy("Mount Caves", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                int numMCaves = 0;
                progress.Message = Lang.gen[2].Value;
                List<int> mCaveX = new List<int>(5000); // FIXME: Pls fix
                List<int> mCaveY = new List<int>(5000); // FIXME: Pls fix

                for (int num671 = 0; num671 < (int)((double)Width * 0.0008); num671++)
                {
                    int num672 = 0;
                    bool flag47 = false;
                    bool flag48 = false;
                    int num673 = WorldGen.genRand.Next((int)((double)Width * 0.25), (int)((double)Width * 0.75));
                    while (!flag48)
                    {
                        flag48 = true;
                        while (num673 > Width / 2 - 100 && num673 < Width / 2 + 100)
                        {
                            num673 = WorldGen.genRand.Next((int)((double)Width * 0.25), (int)((double)Width * 0.75));
                        }
                        int num674 = 0;
                        while (num674 < numMCaves)
                        {
                            //if (num673 <= mCaveX[num674] - 50 || num673 >= mCaveX[num674] + 50)
                            //{
                            //    num674++;
                            //    continue;
                            //}
                            num672++;
                            flag48 = false;
                            break;
                        }
                        if (num672 >= 200)
                        {
                            flag47 = true;
                            break;
                        }
                    }
                    if (!flag47)
                    {
                        for (int x = 0; (double)x < worldSurface; x++)
                        {
                            if (Main.tile[num673, x].HasTile)
                            {
                                for (int num676 = num673 - 50; num676 < num673 + 50; num676++)
                                {
                                    for (int num677 = x - 25; num677 < x + 25; num677++)
                                    {
                                        if (Main.tile[num676, num677].HasTile && (Main.tile[num676, num677].TileType == 53 || Main.tile[num676, num677].TileType == 151 || Main.tile[num676, num677].TileType == 274))
                                        {
                                            flag47 = true;
                                        }
                                    }
                                }
                                if (!flag47)
                                {
                                    Mountinater(num673, x, dirtTileID);
                                    //mCaveX[numMCaves] = num673;
                                    //mCaveY[numMCaves] = x;
                                    numMCaves++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }));

            //
            genPasses.Add(new PassLegacy("Dirt Wall Backgrounds", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[3].Value;
                for (int num669 = 1; num669 < Width - 1; num669++)
                {
                    byte wallType = 2;
                    float value20 = (float)num669 / (float)Width;
                    progress.Set(value20);
                    bool flag46 = false;
                    int howFar = WorldGen.genRand.Next(-1, 2);
                    if (howFar < 0)
                    {
                        howFar = 0;
                    }
                    if (howFar > 10)
                    {
                        howFar = 10;
                    }
                    for (int num670 = 0; (double)num670 < worldSurface + 10.0 && (double)num670 <= worldSurface + (double)howFar; num670++)
                    {
                        if (Main.tile[num669, num670].HasTile)
                        {
                            wallType = (byte)((Main.tile[num669, num670].TileType != 147) ? 2 : 40);
                        }
                        if (flag46 && Main.tile[num669, num670].WallType != 64)
                        {
                            //Main.tile[num669, num670].ResetToType(wallType);
                            WorldGen.PlaceWall(num669, num670, WallID.Dirt, true);
                        }
                        if (Main.tile[num669, num670].HasTile && Main.tile[num669 - 1, num670].HasTile && Main.tile[num669 + 1, num670].HasTile && Main.tile[num669, num670 + 1].HasTile && Main.tile[num669 - 1, num670 + 1].HasTile && Main.tile[num669 + 1, num670 + 1].HasTile)
                        {
                            flag46 = true;
                        }
                    }
                }
            }));
            genPasses.Add(new PassLegacy("Rocks In Dirt", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[4].Value;
                for (int num664 = 0; num664 < (int)((double)(Width * Height) * 0.00015); num664++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next(0, (int)worldSurfaceLow + 1), (double)WorldGen.genRand.Next(4, 15), WorldGen.genRand.Next(5, 40), 1, false, 0f, 0f, false, true);
                }
                for (int num665 = 0; num665 < (int)((double)(Width * Height) * 0.0002); num665++)
                {
                    int num666 = WorldGen.genRand.Next(0, Width);
                    int num667 = WorldGen.genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh + 1);
                    if (!Main.tile[num666, num667 - 10].HasTile)
                    {
                        num667 = WorldGen.genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh + 1);
                    }
                    WorldGen.TileRunner(num666, num667, (double)WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), stoneTileID, false, 0f, 0f, false, true);
                }
                for (int num668 = 0; num668 < (int)((double)(Width * Height) * 0.0045); num668++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)worldSurfaceHigh, (int)rockLayerHigh + 1), (double)WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 23), stoneTileID, false, 0f, 0f, false, true);
                }
            }));
            genPasses.Add(new PassLegacy("Dirt In Rocks", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[5].Value;
                for (int num663 = 0; num663 < (int)((double)(Width * Height) * 0.005); num663++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)rockLayerLow, Height), (double)WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), dirtTileID, false, 0f, 0f, false, true);
                }
            }));
            int i2;
            genPasses.Add(new PassLegacy("Small Holes", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                i2 = 0;
                progress.Message = Lang.gen[7].Value;
                for (int num656 = 0; num656 < (int)((double)(Width * Height) * 0.0015); num656++)
                {
                    float value19 = (float)((double)num656 / ((double)(Width * Height) * 0.0015));
                    progress.Set(value19);
                    int type9 = -1;
                    //if (WorldGen.genRand.Next(5) == 0)
                    //{
                    //    type9 = -2; // Liquid
                    //}
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)worldSurfaceHigh, Height), (double)WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(2, 20), type9, false, 0f, 0f, false, true);
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)worldSurfaceHigh, Height), (double)WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(7, 30), type9, false, 0f, 0f, false, true);
                }
            }));
            genPasses.Add(new PassLegacy("Dirt Layer Caves", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[8].Value;
                for (int num655 = 0; num655 < (int)((double)(Width * Height) * 3E-05); num655++)
                {
                    float value18 = (float)((double)num655 / ((double)(Width * Height) * 3E-05));
                    progress.Set(value18);
                    if (rockLayerHigh <= (double)Height)
                    {
                        int type8 = -1;
                        //if (WorldGen.genRand.Next(6) == 0)
                        //{
                        //    type8 = -2; // Liquid
                        //}
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)worldSurfaceLow, (int)rockLayerHigh + 1), (double)WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(30, 200), type8, false, 0f, 0f, false, true);
                    }
                }
            }));
            genPasses.Add(new PassLegacy("Rock Layer Caves", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[9].Value;
                for (int num654 = 0; num654 < (int)((double)(Width * Height) * 0.00013); num654++)
                {
                    float value17 = (float)((double)num654 / ((double)(Width * Height) * 0.00013));
                    progress.Set(value17);
                    if (rockLayerHigh <= (double)Height)
                    {
                        int type7 = -1;
                        //if (WorldGen.genRand.Next(10) == 0)
                        //{
                        //    type7 = -2; // Liquid
                        //}
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Width), WorldGen.genRand.Next((int)rockLayerHigh, Height), (double)WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(50, 300), type7, false, 0f, 0f, false, true);
                    }
                }
            }));
            genPasses.Add(new PassLegacy("Surface Caves", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = Lang.gen[10].Value;
                for (int x = 0; x < (int)((double)Width * 0.002); x++)
                {
                    i2 = WorldGen.genRand.Next(0, Width);
                    while ((float)i2 > (float)Width * 0.45f && (float)i2 < (float)Width * 0.55f)
                    {
                        i2 = WorldGen.genRand.Next(0, Width);
                    }
                    int j2 = 0;
                    while ((double)j2 < worldSurfaceHigh)
                    {
                        if (!Main.tile[i2, j2].HasTile)
                        {
                            j2++;
                            continue;
                        }
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 1f, false, true);
                        break;
                    }
                }
                for (int x = 0; x < (int)((double)Width * 0.0007); x++)
                {
                    i2 = WorldGen.genRand.Next(0, Width);
                    while ((float)i2 > (float)Width * 0.43f && (float)i2 < (float)Width * 0.57f)
                    {
                        i2 = WorldGen.genRand.Next(0, Width);
                    }
                    int j2 = 0;
                    while ((double)j2 < worldSurfaceHigh)
                    {
                        if (!Main.tile[i2, j2].HasTile)
                        {
                            j2++;
                            continue;
                        }
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                        break;
                    }
                }
                for (int x = 0; x < (int)((double)Width * 0.0003); x++)
                {
                    i2 = WorldGen.genRand.Next(0, Width);
                    while ((float)i2 > (float)Width * 0.4f && (float)i2 < (float)Width * 0.6f)
                    {
                        i2 = WorldGen.genRand.Next(0, Width);
                    }
                    int j2 = 0;
                    while ((double)j2 < worldSurfaceHigh)
                    {
                        if (!Main.tile[i2, j2].HasTile)
                        {
                            j2++;
                            continue;
                        }
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 4f, false, true);
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f, false, true);
                        break;
                    }
                }
                for (int x = 0; x < (int)((double)Width * 0.0004); x++)
                {
                    i2 = WorldGen.genRand.Next(0, Width);
                    while ((float)i2 > (float)Width * 0.4f && (float)i2 < (float)Width * 0.6f)
                    {
                        i2 = WorldGen.genRand.Next(0, Width);
                    }
                    int j2 = 0;
                    while ((double)j2 < worldSurfaceHigh)
                    {
                        if (!Main.tile[i2, j2].HasTile)
                        {
                            j2++;
                            continue;
                        }
                        WorldGen.TileRunner(i2, j2, (double)WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, false, 0f, 1f, true, true);
                        break;
                    }
                }
                float num652 = (float)(Width / 4200);
                for (int num653 = 0; (float)num653 < 5f * num652; num653++)
                {
                    try
                    {
                        WorldGen.Caverer(WorldGen.genRand.Next(100, Width - 100), WorldGen.genRand.Next((int)rockLayer, Height - 400));
                    }
                    catch
                    {
                    }
                }
            }));

            //
            genPasses.Add(new DreamlandsSwampBiomePass(Mod));

            //void Jungle(GenerationProgress progress, GameConfiguration configuration)
            //{
            //    progress.Message = Lang.gen[11].Value;
            //    float position = (float)(Main.maxTilesX / 4200);
            //    position *= 1.5f;
            //    float randPos = (float)WorldGen.genRand.Next(15, 30) * 0.01f;
            //    int worldPosition;
            //    float dungeonSide = -1;
            //    if (dungeonSide == -1)
            //    {
            //        randPos = 1f - randPos;
            //        worldPosition = (int)((float)Main.maxTilesX * randPos);
            //    }
            //    else
            //    {
            //        worldPosition = (int)((float)Main.maxTilesX * randPos);
            //    }
            //    int num615 = (int)((double)Main.maxTilesY + Main.rockLayer) / 2;
            //    worldPosition += WorldGen.genRand.Next((int)(-100f * position), (int)(101f * position));
            //    num615 += WorldGen.genRand.Next((int)(-100f * position), (int)(101f * position));
            //    int num616 = worldPosition;
            //    int num617 = num615;

            //    // Generate mud
            //    WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next((int)(250f * position), (int)(500f * position)), WorldGen.genRand.Next(50, 150), TileID.Mud, false, (float)(dungeonSide * 3), 0f, false, true);

            //    // Generate gems???
            //    for (int num618 = 0; (float)num618 < 6f * position; num618++)
            //    {
            //        WorldGen.TileRunner(worldPosition + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), num615 + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), (double)WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(TileID.Sapphire, TileID.Emerald), false, 0f, 0f, false, true);
            //    }

            //    // Generate more mud
            //    WorldGen.mudWall = true;
            //    progress.Set(0.15f);
            //    worldPosition += WorldGen.genRand.Next((int)(-250f * position), (int)(251f * position));
            //    num615 += WorldGen.genRand.Next((int)(-150f * position), (int)(151f * position));
            //    int num619 = worldPosition;
            //    int num620 = num615;
            //    int num621 = worldPosition;
            //    int num622 = num615;
            //    WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next((int)(250f * position), (int)(500f * position)), WorldGen.genRand.Next(50, 150), TileID.Mud, false, 0f, 0f, false, true);
            //    WorldGen.mudWall = false;

            //    // Generate more gems???
            //    for (int num623 = 0; (float)num623 < 6f * position; num623++)
            //    {
            //        WorldGen.TileRunner(worldPosition + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), num615 + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), (double)WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(TileID.Emerald, TileID.Amethyst), false, 0f, 0f, false, true);
            //    }

            //    WorldGen.mudWall = true;
            //    progress.Set(0.3f);
            //    worldPosition += WorldGen.genRand.Next((int)(-400f * position), (int)(401f * position));
            //    num615 += WorldGen.genRand.Next((int)(-150f * position), (int)(151f * position));
            //    int num624 = worldPosition;
            //    int num625 = num615;

            //    // More mud?
            //    WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next((int)(250f * position), (int)(500f * position)), WorldGen.genRand.Next(50, 150), TileID.Mud, false, (float)(dungeonSide * -3), 0f, false, true);
            //    WorldGen.mudWall = false;

            //    // Generate gems and jungle stuff??
            //    for (int num626 = 0; (float)num626 < 6f * position; num626++)
            //    {
            //        WorldGen.TileRunner(worldPosition + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), num615 + WorldGen.genRand.Next(-(int)(125f * position), (int)(125f * position)), (double)WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(TileID.Amethyst, TileID.JungleThorns), false, 0f, 0f, false, true);
            //    }
            //    WorldGen.mudWall = true;
            //    progress.Set(0.45f);
            //    worldPosition = (num616 + num619 + num624) / 3;
            //    num615 = (num617 + num620 + num625) / 3;

            //    // More mud...
            //    WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next((int)(400f * position), (int)(600f * position)), 10000, TileID.Mud, false, 0f, -20f, true, true);
            //    WorldGen.JungleRunner(worldPosition, num615);
            //    progress.Set(0.6f);
            //    WorldGen.mudWall = false;

            //    // Add mud walls
            //    for (int num627 = 0; num627 < Main.maxTilesX / 4; num627++)
            //    {
            //        worldPosition = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
            //        num615 = WorldGen.genRand.Next((int)WorldGen.worldSurface + 10, Main.maxTilesY - 200);
            //        while (Main.tile[worldPosition, num615].wall != 64 && Main.tile[worldPosition, num615].wall != 15)
            //        {
            //            worldPosition = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
            //            num615 = WorldGen.genRand.Next((int)WorldGen.worldSurface + 10, Main.maxTilesY - 200);
            //        }
            //        WorldGen.MudWallRunner(worldPosition, num615);
            //    }
            //    worldPosition = num621;
            //    num615 = num622;

            //    // More mud???
            //    for (int num628 = 0; (float)num628 <= 20f * position; num628++)
            //    {
            //        progress.Set((60f + (float)num628 / position) * 0.01f);
            //        worldPosition += WorldGen.genRand.Next((int)(-5f * position), (int)(6f * position));
            //        num615 += WorldGen.genRand.Next((int)(-5f * position), (int)(6f * position));
            //        WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next(40, 100), WorldGen.genRand.Next(300, 500), TileID.Mud, false, 0f, 0f, false, true);
            //    }

            //    //
            //    for (int num629 = 0; (float)num629 <= 10f * position; num629++)
            //    {
            //        progress.Set((80f + (float)num629 / position * 2f) * 0.01f);
            //        worldPosition = num621 + WorldGen.genRand.Next((int)(-600f * position), (int)(600f * position));
            //        num615 = num622 + WorldGen.genRand.Next((int)(-200f * position), (int)(200f * position));
            //        while (true)
            //        {
            //            // 
            //            if (worldPosition >= 1 && worldPosition < Main.maxTilesX - 1 && num615 >= 1 && num615 < Main.maxTilesY - 1 && Main.tile[worldPosition, num615].TileType == TileID.Mud)
            //            {
            //                break;
            //            }
            //            worldPosition = num621 + WorldGen.genRand.Next((int)(-600f * position), (int)(600f * position));
            //            num615 = num622 + WorldGen.genRand.Next((int)(-200f * position), (int)(200f * position));
            //        }

            //        //
            //        for (int num630 = 0; (float)num630 < 8f * position; num630++)
            //        {
            //            worldPosition += WorldGen.genRand.Next(-30, 31);
            //            num615 += WorldGen.genRand.Next(-30, 31);

            //            // Random liquid??
            //            int type5 = -1;
            //            if (WorldGen.genRand.Next(7) == 0)
            //            {
            //                type5 = -2;
            //            }
            //            // Place liquid
            //            WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(30, 70), type5, false, 0f, 0f, false, true);
            //        }
            //    }

            //    // Undeground generation stuff???
            //    //
            //    for (int num631 = 0; (float)num631 <= 300f * position; num631++)
            //    {
            //        worldPosition = num621 + WorldGen.genRand.Next((int)(-600f * position), (int)(600f * position));
            //        num615 = num622 + WorldGen.genRand.Next((int)(-200f * position), (int)(200f * position));
            //        while (true)
            //        {
            //            if (worldPosition >= 1 && worldPosition < Main.maxTilesX - 1 && num615 >= 1 && num615 < Main.maxTilesY - 1 && Main.tile[worldPosition, num615].type == 59)
            //            {
            //                break;
            //            }
            //            worldPosition = num621 + WorldGen.genRand.Next((int)(-600f * position), (int)(600f * position));
            //            num615 = num622 + WorldGen.genRand.Next((int)(-200f * position), (int)(200f * position));
            //        }
            //        WorldGen.TileRunner(worldPosition, num615, (double)WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), TileID.Stone, false, 0f, 0f, false, true);
            //        if (WorldGen.genRand.Next(4) == 0)
            //        {
            //            // Generate more gems and JungleThorns
            //            int type6 = WorldGen.genRand.Next(TileID.Sapphire, TileID.JungleThorns);
            //            WorldGen.TileRunner(worldPosition + WorldGen.genRand.Next(-1, 2), num615 + WorldGen.genRand.Next(-1, 2), (double)WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(4, 8), type6, false, 0f, 0f, false, true);
            //        }
            //    }
            //}

            //
            genPasses.Add(new PassLegacy("Spawn Point", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                int num187 = 5;
                bool flag7 = true;
                while (flag7)
                {
                    int num188 = Width / 2 + WorldGen.genRand.Next(-num187, num187 + 1);
                    int num189 = 0;
                    while (num189 < Height)
                    {
                        if (!Main.tile[num188, num189].HasTile)
                        {
                            num189++;
                            continue;
                        }
                        Main.spawnTileX = num188;
                        Main.spawnTileY = num189;
                        break;
                    }
                    flag7 = false;
                    num187++;
                    if ((double)Main.spawnTileY > worldSurface)
                    {
                        flag7 = true;
                    }
                    if (Main.tile[Main.spawnTileX, Main.spawnTileY - 1].LiquidAmount > 0)
                    {
                        flag7 = true;
                    }
                }
                int num190 = 10;
                while ((double)Main.spawnTileY > worldSurface)
                {
                    int num191 = WorldGen.genRand.Next(Width / 2 - num190, Width / 2 + num190);
                    int num192 = 0;
                    while (num192 < Main.maxTilesY)
                    {
                        if (!Main.tile[num191, num192].HasTile)
                        {
                            num192++;
                            continue;
                        }
                        Main.spawnTileX = num191;
                        Main.spawnTileY = num192;
                        break;
                    }
                    num190++;
                }
            }));
            genPasses.Add(new PassLegacy("Guide", delegate (GenerationProgress progress, GameConfiguration configuration)
            {
                int num174 = NPC.NewNPC(new EntitySource_WorldGen("Spawn Guide"), Main.spawnTileX * 16, Main.spawnTileY * 16, 22, 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num174].homeTileX = Main.spawnTileX;
                Main.npc[num174].homeTileY = Main.spawnTileY;
                Main.npc[num174].direction = 1;
                Main.npc[num174].homeless = true;
            }));

            // FOR LATER
            //genPasses.Add(new PassLegacy("Grass", delegate (GenerationProgress progress, GameConfiguration configuration)
            //{
            //    for (int num632 = 0; num632 < (int)((double)(Width * Height) * 0.002); num632++)
            //    {
            //        int num633 = WorldGen.genRand.Next(1, Width - 1);
            //        int num634 = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, (int)WorldGen.worldSurfaceHigh);
            //        if (num634 >= Height)
            //        {
            //            num634 = Height - 2;
            //        }
            //        if (Main.tile[num633 - 1, num634].active() && Main.tile[num633 - 1, num634].type == 0 && Main.tile[num633 + 1, num634].active() && Main.tile[num633 + 1, num634].type == 0 && Main.tile[num633, num634 - 1].active() && Main.tile[num633, num634 - 1].type == 0 && Main.tile[num633, num634 + 1].active() && Main.tile[num633, num634 + 1].type == 0)
            //        {
            //            Main.tile[num633, num634].active(true);
            //            Main.tile[num633, num634].type = 2;
            //        }
            //        num633 = WorldGen.genRand.Next(1, Width - 1);
            //        num634 = WorldGen.genRand.Next(0, (int)WorldGen.worldSurfaceLow);
            //        if (num634 >= Height)
            //        {
            //            num634 = Height - 2;
            //        }
            //        if (Main.tile[num633 - 1, num634].active() && Main.tile[num633 - 1, num634].type == 0 && Main.tile[num633 + 1, num634].active() && Main.tile[num633 + 1, num634].type == 0 && Main.tile[num633, num634 - 1].active() && Main.tile[num633, num634 - 1].type == 0 && Main.tile[num633, num634 + 1].active() && Main.tile[num633, num634 + 1].type == 0)
            //        {
            //            Main.tile[num633, num634].active(true);
            //            Main.tile[num633, num634].type = 2;
            //        }
            //    }
            //}));

            //
            return genPasses;
        }

        public void Mountinater(int i, int j, ushort dirtTileID)
        {
            double num = WorldGen.genRand.Next(80, 120);
            double num2 = num;
            double num3 = WorldGen.genRand.Next(40, 55);
            if (WorldGen.remixWorldGen)
            {
                num2 *= 1.5;
                num3 *= 1.5;
            }

            Vector2D vector2D = default(Vector2D);
            vector2D.X = i;
            vector2D.Y = (double)j + num3 / 2.0;
            Vector2D vector2D2 = default(Vector2D);
            vector2D2.X = (double)WorldGen.genRand.Next(-10, 11) * 0.1;
            vector2D2.Y = (double)WorldGen.genRand.Next(-20, -10) * 0.1;
            while (num > 0.0 && num3 > 0.0)
            {
                num -= (double)WorldGen.genRand.Next(4);
                num3 -= 1.0;
                int num4 = (int)(vector2D.X - num * 0.5);
                int num5 = (int)(vector2D.X + num * 0.5);
                int num6 = (int)(vector2D.Y - num * 0.5);
                int num7 = (int)(vector2D.Y + num * 0.5);
                if (num4 < 0)
                    num4 = 0;

                if (num5 > Width)
                    num5 = Width;

                if (num6 < 0)
                    num6 = 0;

                if (num7 > Height)
                    num7 = Height;

                num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
                for (int k = num4; k < num5; k++)
                {
                    for (int l = num6; l < num7; l++)
                    {
                        double num8 = Math.Abs((double)k - vector2D.X);
                        double num9 = Math.Abs((double)l - vector2D.Y);
                        if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4 && !Main.tile[k, l].HasTile)
                        {
                            Main.tile[k, l].ResetToType(dirtTileID);
                        }
                    }
                }

                vector2D += vector2D2;
                vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                if (vector2D2.X > 0.5)
                    vector2D2.X = 0.5;

                if (vector2D2.X < -0.5)
                    vector2D2.X = -0.5;

                if (vector2D2.Y > -0.5)
                    vector2D2.Y = -0.5;

                if (vector2D2.Y < -1.5)
                    vector2D2.Y = -1.5;
            }
        }

        public override void OnEnter()
        {
            //Clear the background.
            Main.numClouds = 0;
            Main.numCloudsTemp = 0;
            Main.cloudBGAlpha = 0f;

            Main.cloudAlpha = 0f;
            Main.resetClouds = true;
            Main.moonPhase = 4;
        }

        public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
        {
            if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0)
            {
                color = Color.DarkGreen.ToVector3();
                color.Y = tile.LiquidAmount / 255f; // The result of integer division is rounded down, so one of the numbers must be a float 
            }
            return base.GetLight(tile, x, y, ref rand, ref color);
        }
    }
}
