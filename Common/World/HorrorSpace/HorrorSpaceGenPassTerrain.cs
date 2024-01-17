using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SupernovaMod.Common.World.HorrorSpace
{
	public class HorrorSpaceGenPassTerrain : GenPass
	{
		public HorrorSpaceGenPassTerrain() : base("Terrain", 1) { }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Generating terrain";
			Main.worldSurface = HorrorSpaceSubworld.worldSurface;	//Hides the underground layer just out of bounds
			Main.rockLayer = HorrorSpaceSubworld.rockLayer;         //Hides the cavern layer way out of bounds

			// Generate rock and dirt layers
			//
			ushort tileIdStone = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			ushort tileIdDirt  = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = HorrorSpaceSubworld.worldSurface; y < Main.maxTilesY; y++)
				{
					if ((double)y < HorrorSpaceSubworld.rockLayer)
					{
						// TODO: Horror space dirt
						Main.tile[x, y].ResetToType(tileIdDirt);
						if (y != HorrorSpaceSubworld.worldSurface)
						{
							//Main.tile[x, y].WallType = WallID.Dirt;
						}
					}
					else
					{
						Main.tile[x, y].ResetToType(tileIdStone);
						//Main.tile[x, y].WallType = WallID.Stone;
					}
				}
			}

			// Generate mounts
			//
			for (int x = 50; x < Main.maxTilesX - 50; x++)
			{
				if (!WorldGen.genRand.NextBool(10))
				{
					continue;
				}

				try
				{
					int width = WorldGen.genRand.Next(6, 40);
					int height = WorldGen.genRand.Next(1, 6);
					WorldUtils.Gen(new Microsoft.Xna.Framework.Point(x, HorrorSpaceSubworld.worldSurface), new Shapes.Mound(width / 2, height), Actions.Chain(new GenAction[]
					{
						new Actions.SetTile(tileIdDirt),
					}));
					// Skip this mounts width
					x += width;
				}
				catch { }// TODO: Log error and debug the error(s)
			}
		}
	}
}
