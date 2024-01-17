using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace SupernovaMod.Common.Systems.Generation
{
    public class SupernovaWorldOres : ModSystem
    {
        /* World Generation */
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
			// We use FindIndex to locate the index of the vanilla world generation task called "Shinies".
			// This ensures our code runs at the correct step.
			//
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                // 5. We register our world generation pass by passing in a name and the method that will execute our world generation code.
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("SupernovaModOres", WorldStartGenOres));
            }
        }

        private static void WorldStartGenOres(GenerationProgress progress, GameConfiguration configuration)
        {
            // Tell the user that the Supernova ores will be generated
            progress.Message = "World Gen Supernova Ores";

            WorldGenZirconiumOre();
        }

        private static void WorldGenZirconiumOre()
        {
			// Here we use a for loop to run the code inside the loop many times. This for loop scales to the product of Main.maxTilesX, Main.maxTilesY, and 2E-05. 2E-05 is scientific notation and equal to 0.00002. Sometimes scientific notation is easier to read when dealing with a lot of zeros.
			// In a small world, this math results in 4200 * 1200 * 0.00002, which is about 100. This means that we'll run the code inside the for loop 100 times. This is the amount Crimtane or Demonite will spawn. Since we are scaling by both dimensions of the world size, the ammount spawned will adjust automatically to different world sizes for a consistent distribution of ores.
			//
			//for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.0001); k++)
			{
				// We randomly choose an x and y coordinate. The x coordinate is choosen from the far left to the far right coordinates. The y coordinate, however, is choosen from between WorldGen.worldSurfaceLow and the bottom of the map. We can use this technique to determine the depth that our ore should spawn at.
				//
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY - 200);

                // Finally, we do the actual world generation code. In this example, we use the WorldGen.TileRunner method. This method spawns splotches of the Tile type we provide to the method. The behavior of TileRunner is detailed in the Useful Methods section below.
                //
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(3, 6),
                    ModContent.TileType<Content.Tiles.ZirconiumOreTile>() // Ore to spawn
                );
            }
        }
    }
}
