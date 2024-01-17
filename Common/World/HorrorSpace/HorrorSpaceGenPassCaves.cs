using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace SupernovaMod.Common.World.HorrorSpace
{
	public class HorrorSpaceGenPassCaves : GenPass
	{
		public HorrorSpaceGenPassCaves() : base("Caves", 2) { }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Generating caves";
			GenerateTunnels(progress);
			GenerateDirtLayerCaves(progress);
			GenerateRockLayerCaves(progress);
			//GenerateSurfaceCaves(progress);
		}

		private void GenerateTunnels(GenerationProgress progress)
		{
			int num1032 = (int)((double)Main.maxTilesX * 0.0015);
			if (WorldGen.remixWorldGen)
			{
				num1032 = (int)((double)num1032 * 1.5);
			}

			for (int num1033 = 0; num1033 < num1032; num1033++)
			{
				if (GenVars.numTunnels >= GenVars.maxTunnels - 1)
				{
					break;
				}

				int[] array = new int[10];
				int[] array2 = new int[10];
				int num1034 = WorldGen.genRand.Next(225, Main.maxTilesX - 225);
				if (!WorldGen.remixWorldGen)
				{
					if (WorldGen.tenthAnniversaryWorldGen)
					{
						num1034 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
					}
					else
					{
						while ((double)num1034 > (double)Main.maxTilesX * 0.4 && (double)num1034 < (double)Main.maxTilesX * 0.6)
						{
							num1034 = WorldGen.genRand.Next(225, Main.maxTilesX - 225);
						}
					}
				}

				int num1035 = 0;
				bool flag61;
				do
				{
					flag61 = false;
					for (int num1036 = 0; num1036 < 10; num1036++)
					{
						for (num1034 %= Main.maxTilesX; !Main.tile[num1034, num1035].HasTile; num1035++)
						{
						}

						if (Main.tile[num1034, num1035].TileType == TileID.Sand)
						{
							flag61 = true;
						}

						array[num1036] = num1034;
						array2[num1036] = num1035 - WorldGen.genRand.Next(11, 16);
						num1034 += WorldGen.genRand.Next(5, 11);
					}
				}
				while (flag61);
				GenVars.tunnelX[GenVars.numTunnels] = array[5];
				GenVars.numTunnels++;
				for (int num1037 = 0; num1037 < 10; num1037++)
				{
					WorldGen.TileRunner(array[num1037], array2[num1037], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), 0, addTile: true, -2.0, -0.3);
					WorldGen.TileRunner(array[num1037], array2[num1037], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), 0, addTile: true, 2.0, -0.3);
				}
			}
		}
		private void GenerateDirtLayerCaves(GenerationProgress progress)
		{
			progress.Message = Lang.gen[8].Value;

			//double worldSurfaceHigh = (HorrorSpaceSubworld.worldSurface + 11);
			double worldSurfaceHigh = HorrorSpaceSubworld.worldSurface;
			int num992 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05);
			/*if (remixWorldGen)
			{
				num992 *= 2;
			}*/

			for (int num993 = 0; num993 < num992; num993++)
			{
				double value18 = num993 / num992;
				progress.Set(value18);
				if (GenVars.rockLayerHigh <= (double)Main.maxTilesY)
				{
					int type12 = -1;
					if (WorldGen.genRand.NextBool(6))
					{
						type12 = -2;
					}

					int num994 = WorldGen.genRand.Next(0, Main.maxTilesX);
					int num995 = WorldGen.genRand.Next((int)HorrorSpaceSubworld.worldSurface, (int)HorrorSpaceSubworld.rockLayer + 11);
					while (((num994 < GenVars.smallHolesBeachAvoidance || num994 > Main.maxTilesX - GenVars.smallHolesBeachAvoidance) && (double)num995 < worldSurfaceHigh) || ((double)num994 >= (double)Main.maxTilesX * 0.45 && (double)num994 <= (double)Main.maxTilesX * 0.55 && (double)num995 < Main.worldSurface))
					{
						num994 = WorldGen.genRand.Next(0, Main.maxTilesX);
						num995 = WorldGen.genRand.Next((int)HorrorSpaceSubworld.worldSurface, (int)HorrorSpaceSubworld.rockLayer + 11);
					}

					int num996 = WorldGen.genRand.Next(5, 15);
					int num997 = WorldGen.genRand.Next(30, 200);
					if (WorldGen.remixWorldGen)
					{
						num996 = (int)((double)num996 * 1.1);
						num997 = (int)((double)num997 * 1.9);
					}

					WorldGen.TileRunner(num994, num995, num996, num997, type12);
				}
			}
		}
		private void GenerateRockLayerCaves(GenerationProgress progress)
		{
			progress.Message = Lang.gen[9].Value;
			int num984 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013);
			if (WorldGen.remixWorldGen)
			{
				num984 = (int)((double)num984 * 1.1);
			}

			for (int num985 = 0; num985 < num984; num985++)
			{
				double value17 = (double)num985 / (double)num984;
				progress.Set(value17);
				if ((HorrorSpaceSubworld.rockLayer + 11) <= (double)Main.maxTilesY)
				{
					int type10 = -1;
					if (WorldGen.genRand.NextBool(10))
					{
						type10 = -2;
					}

					int num986 = WorldGen.genRand.Next(6, 20);
					int num987 = WorldGen.genRand.Next(50, 300);
					if (WorldGen.remixWorldGen)
					{
						num986 = (int)((double)num986 * 0.7);
						num987 = (int)((double)num987 * 0.7);
					}

					WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((HorrorSpaceSubworld.rockLayer + 11), Main.maxTilesY), num986, num987, type10);
				}
			}

			if (WorldGen.remixWorldGen)
			{
				num984 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013 * 0.4);
				for (int num988 = 0; num988 < num984; num988++)
				{
					if (GenVars.rockLayerHigh <= (double)Main.maxTilesY)
					{
						int type11 = -1;
						if (WorldGen.genRand.NextBool(10))
						{
							type11 = -2;
						}

						int num989 = WorldGen.genRand.Next(7, 26);
						int steps = WorldGen.genRand.Next(50, 200);
						double num990 = (double)WorldGen.genRand.Next(100, 221) * 0.1;
						double num991 = (double)WorldGen.genRand.Next(-10, 11) * 0.02;
						int i5 = WorldGen.genRand.Next(0, Main.maxTilesX);
						int j7 = WorldGen.genRand.Next((HorrorSpaceSubworld.rockLayer + 11), Main.maxTilesY);
						WorldGen.TileRunner(i5, j7, num989, steps, type11, addTile: false, num990, num991, noYChange: true);
						WorldGen.TileRunner(i5, j7, num989, steps, type11, addTile: false, 0.0 - num990, 0.0 - num991, noYChange: true);
					}
				}
			}
		}
		private void GenerateSurfaceCaves(GenerationProgress progress)
		{
			progress.Message = Lang.gen[10].Value;
			int num965 = (int)((double)Main.maxTilesX * 0.002);
			int num966 = (int)((double)Main.maxTilesX * 0.0007);
			int num967 = (int)((double)Main.maxTilesX * 0.0003);
			if (WorldGen.remixWorldGen)
			{
				num965 *= 3;
				num966 *= 3;
				num967 *= 3;
			}

			for (int num968 = 0; num968 < num965; num968++)
			{
				int num969 = WorldGen.genRand.Next(0, Main.maxTilesX);
				while (((double)num969 > (double)Main.maxTilesX * 0.45 && (double)num969 < (double)Main.maxTilesX * 0.55) || num969 < GenVars.leftBeachEnd + 20 || num969 > GenVars.rightBeachStart - 20)
				{
					num969 = WorldGen.genRand.Next(0, Main.maxTilesX);
				}

				for (int num970 = 0; (double)num970 < (HorrorSpaceSubworld.worldSurface + 11); num970++)
				{
					if (Main.tile[num969, num970].HasTile)
					{
						WorldGen.TileRunner(num969, num970, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 1.0);
						break;
					}
				}
			}

			progress.Set(0.2);
			for (int num971 = 0; num971 < num966; num971++)
			{
				int num972 = WorldGen.genRand.Next(0, Main.maxTilesX);
				while (((double)num972 > (double)Main.maxTilesX * 0.43 && (double)num972 < (double)Main.maxTilesX * 0.57000000000000006) || num972 < GenVars.leftBeachEnd + 20 || num972 > GenVars.rightBeachStart - 20)
				{
					num972 = WorldGen.genRand.Next(0, Main.maxTilesX);
				}

				for (int num973 = 0; (double)num973 < (HorrorSpaceSubworld.worldSurface + 11); num973++)
				{
					if (Main.tile[num972, num973].HasTile)
					{
						WorldGen.TileRunner(num972, num973, WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
						break;
					}
				}
			}

			progress.Set(0.4);
			for (int num974 = 0; num974 < num967; num974++)
			{
				int num975 = WorldGen.genRand.Next(0, Main.maxTilesX);
				while (((double)num975 > (double)Main.maxTilesX * 0.4 && (double)num975 < (double)Main.maxTilesX * 0.6) || num975 < GenVars.leftBeachEnd + 20 || num975 > GenVars.rightBeachStart - 20)
				{
					num975 = WorldGen.genRand.Next(0, Main.maxTilesX);
				}

				for (int num976 = 0; (double)num976 < (HorrorSpaceSubworld.worldSurface + 11); num976++)
				{
					if (Main.tile[num975, num976].HasTile)
					{
						WorldGen.TileRunner(num975, num976, WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 4.0);
						WorldGen.TileRunner(num975, num976, WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
						WorldGen.TileRunner(num975, num976, WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
						break;
					}
				}
			}

			progress.Set(0.6);
			for (int num977 = 0; num977 < (int)((double)Main.maxTilesX * 0.0004); num977++)
			{
				int num978 = WorldGen.genRand.Next(0, Main.maxTilesX);
				while (((double)num978 > (double)Main.maxTilesX * 0.4 && (double)num978 < (double)Main.maxTilesX * 0.6) || num978 < GenVars.leftBeachEnd + 20 || num978 > GenVars.rightBeachStart - 20)
				{
					num978 = WorldGen.genRand.Next(0, Main.maxTilesX);
				}

				for (int num979 = 0; (double)num979 < (HorrorSpaceSubworld.worldSurface + 11); num979++)
				{
					if (Main.tile[num978, num979].HasTile)
					{
						WorldGen.TileRunner(num978, num979, WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, addTile: false, 0.0, 1.0, noYChange: true);
						break;
					}
				}
			}

			progress.Set(0.8);
			double num980 = (double)Main.maxTilesX / 4200.0;
			for (int num981 = 0; (double)num981 < 5.0 * num980; num981++)
			{
				try
				{
					int num982 = (int)Main.rockLayer;
					int num983 = Main.maxTilesY - 400;
					if (num982 >= num983)
					{
						num982 = num983 - 1;
					}

					WorldGen.Caverer(WorldGen.genRand.Next(GenVars.surfaceCavesBeachAvoidance2, Main.maxTilesX - GenVars.surfaceCavesBeachAvoidance2), WorldGen.genRand.Next(num982, num983));
				}
				catch
				{
				}
			}
		}
	}
}
