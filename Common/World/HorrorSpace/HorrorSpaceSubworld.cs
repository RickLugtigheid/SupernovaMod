using System.Collections.Generic;
using Terraria.WorldBuilding;
using SubworldLibrary;
using Terraria;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SupernovaMod.Common.World.HorrorSpace
{
    public class HorrorSpaceSubworld : Subworld
    {
		public static int worldSurface = 200;
		public static int rockLayer = 280;

		public override int Width => 840;
        public override int Height => 420;

		public override bool ShouldSave => false;
		public override bool NoPlayerSaving => false;
		public override List<GenPass> Tasks => new List<GenPass>()
        {
			new HorrorSpaceGenPassTerrain(),
			new HorrorSpaceGenPassCaves(),
			//new HorrorSpaceGenPassDungeon(),
        };

		public override void OnLoad()
		{
			// Do things on load
			// Like set the weather
		}

		public override bool GetLight(Tile tile, int x, int y, ref FastRandom rand, ref Vector3 color)
		{
			// Gives water a green glow
			//
			if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0)
			{
				color.Y = tile.LiquidAmount / 255f; // The result of integer division is rounded down, so one of the numbers must be a float
			}
			return base.GetLight(tile, x, y, ref rand, ref color);
		}
	}
    /*public static class HorrorSpace
	{
		public static void GenerateHorrorSpace(GenerationProgress progress, GameConfiguration config)
		{
			progress.Message = "Generating Horror Space";
			WorldGenConfiguration config2 = WorldGenConfiguration.FromEmbeddedPath("Terraria.GameContent.WorldBuilding.Configuration.json");

			// Set biome bounds
			_startX = (int)(Main.maxTilesX * 0.001f);
			int biomeStart = _startX;
			int biomeEdge = biomeStart + Main.maxTilesX / 5;
			int biomeMiddle = (biomeStart + biomeEdge) / 2;
			int biomeStartY = 10;
			int biomeEndY = 137;

			//
			// Generate
			//
			PlaceIsland(biomeMiddle, biomeEndY - 10);
			// Generate: Astroids
			//
			int astroiedCount = Main.maxTilesX / 250;
			Supernova.Instance.Logger.Info("astroiedCount = " + astroiedCount);
			for (int i = 0; i < astroiedCount; i++)
			{
				int randX = WorldGen.genRand.Next(biomeStart, biomeEdge);
				int randY = WorldGen.genRand.Next(biomeStartY + 20, biomeEndY);
				int randSize = WorldGen.genRand.Next(5, 10);
				int maxTries = 100;
				while (!PlaceAstroid(randX, randY, randSize) && maxTries > 0)
				{
					maxTries--;
					randX = WorldGen.genRand.Next(biomeStart, biomeEdge);
					randY = WorldGen.genRand.Next(biomeStartY, biomeEndY);
				}
			}
		}

		public static void PlaceIsland(int x, int y)
		{
			ushort tileIdStone = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			ushort tileIdCloud = TileID.RainCloud;

			//
			// Step 1: Generate Clouds
			//
			PlaceIsland_old(x, y);
		}
		public static void PlaceIsland_old(int x, int y)
		{
			// Setup arguments
			ushort tileIdStone = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			ushort tileIdCloud = TileID.Cloud; // 189

			double sizeX = WorldGen.genRand.Next(190, 220);
			double sizeY = WorldGen.genRand.Next(20, 30);

			int num4 = x;
			int num5 = x;
			int num6 = x;
			int num7 = y;

			Vector2D position = new Vector2D(x, y);
			
			Vector2D vector2D2 = default(Vector2D);
			vector2D2.X = WorldGen.genRand.Next(-20, 21) * 0.2;
			while (vector2D2.X > -2.0 && vector2D2.X < 2.0)
			{
				vector2D2.X = WorldGen.genRand.Next(-20, 21) * 0.2;
			}
			vector2D2.Y = WorldGen.genRand.Next(-20, -10) * 0.02;

			//
			// Step 1: Generate cloud base
			//
			// Run until all size has been used
			while (sizeX > 0 && sizeY > 0)
			{
				sizeX -= WorldGen.genRand.Next(4);
				sizeY -= 1;

				int num8 = (int)(position.X - sizeX * 0.5);
				int num9 = (int)(position.X + sizeX * 0.5);
				int num10 = (int)(position.Y - sizeX * 0.5);
				int num11 = (int)(position.Y + sizeX * 0.5);

				if (num8 < 0)
				{
					num8 = 0;
				}
				if (num9 > Main.maxTilesX)
				{
					num9 = Main.maxTilesX;
				}
				if (num10 < 0)
				{
					num10 = 0;
				}
				if (num11 > Main.maxTilesY)
				{
					num11 = Main.maxTilesY;
				}

				double num2 = sizeX * WorldGen.genRand.Next(80, 120) * 0.01;
				double num12 = position.X + 1;
				for (int x2 = num8; x2 < num9; x2++)
				{
					if (WorldGen.genRand.NextBool(2))
					{
						num12 += (double)WorldGen.genRand.Next(-1, 2);
					}

					if (num12 < position.Y)
					{
						num12 = position.Y;
					}
					if (num12 > position.Y + 2)
					{
						num12 = position.Y + 2;
					}

					for (int y2 = num10; y2 < num11; y2++)
					{
						if (!(y2 > num12))
						{
							continue;
						}
						double num13 = Math.Abs((double)x2 - position.X);
						double num14 = Math.Abs((double)y2 - position.Y) * 3.0;

						if (Math.Sqrt(num13 * num13 + num14 * num14) < num2 * 0.4)
						{
							if (x2 < num4)
							{
								num4 = x2;
							}

							if (x2 > num5)
							{
								num5 = x2;
							}

							if (y2 < num6)
							{
								num6 = y2;
							}

							if (y2 > num7)
							{
								num7 = y2;
							}
							Main.tile[x2, y2].ResetToType(tileIdStone);
							WorldGen.SquareTileFrame(x2, y2);
						}
					}
				}
				position += vector2D2;
				vector2D2.X += WorldGen.genRand.Next(-20, 21) * 0.05;
				if (vector2D2.X > 1.0)
				{
					vector2D2.X = 1.0;
				}

				if (vector2D2.X < -1.0)
				{
					vector2D2.X = -1.0;
				}

				if (vector2D2.Y > 0.2)
				{
					vector2D2.Y = -0.2;
				}

				if (vector2D2.Y < -0.2)
				{
					vector2D2.Y = -0.2;
				}
			}

			//
			// Step 2: Generate stone
			//
			int num15 = num4;
			int num17;
			for (num15 += WorldGen.genRand.Next(5); num15 < num5; num15 += WorldGen.genRand.Next(num17, (int)((double)num17 * 1.5)))
			{
				int num16 = num7;
				while (!Main.tile[num15, num16].HasTile)
				{
					num16--;
				}

				num16 += WorldGen.genRand.Next(-3, 4);
				num17 = WorldGen.genRand.Next(4, 8);

				for (int m = num15 - num17; m <= num15 + num17; m++)
				{
					for (int n = num16 - num17; n <= num16 + num17; n++)
					{
						if (n > num6)
						{
							double num19 = Math.Abs(m - num15);
							double num20 = Math.Abs(n - num16) * 2;
							if (Math.Sqrt(num19 * num19 + num20 * num20) < (double)(num17 + WorldGen.genRand.Next(2)))
							{
								Main.tile[m, n].ResetToType(tileIdCloud);
								WorldGen.SquareTileFrame(m, n);
							}
						}
					}
				}
			}
		}
		public static void PlaceIsland_example(int x, int y)
		{
			int leftOffset = 102;
			int rightOffset = 102;
			int maxVerticalOffset = 24;

			ushort tileIdStone = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			ushort tileIdCloud = TileID.RainCloud;
			ushort tileIdOre = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();

			// Generate clouds
			// TODO: Remove / Replace / ???
			//
			List<Point> cloudPositions = new List<Point>();
			for (int dx = -leftOffset; dx < rightOffset; dx += WorldGen.genRand.Next(21, 26))
			{
				float completionRatio10 = Mathf.Convert01To010(Utils.GetLerpValue((float)(-(float)leftOffset) - 22f, (float)rightOffset + 22f, (float)dx, true));
				int verticalOffset = (int)(completionRatio10 * (float)maxVerticalOffset);
				Point cloudPosition = new Point(x + dx, y + verticalOffset);
				int radius = WorldGen.genRand.Next(22, 26) - (int)((1f - completionRatio10) * 15f);
				WorldUtils.Gen(cloudPosition, new CustomShapes.DistortedCircle(radius, 0.1f), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(tileIdCloud, false, true),
					new Modifiers.Blotches(4, 1.0)
				}));
				cloudPositions.Add(cloudPosition);
			}
			WorldUtils.Gen(new Point(x, y - 4), new Shapes.Circle((leftOffset + rightOffset - 30) / 2, 16), new Actions.ClearTile(false));
			foreach (Point point in cloudPositions)
			{
				Vector2 offsetCloudPosition = Utils.ToVector2(point);
				while (!SupernovaWorldUtils.GetTileSafe((int)offsetCloudPosition.X, (int)offsetCloudPosition.Y).HasTile)
				{
					offsetCloudPosition.Y += 1f;
				}
				for (int k = 0; k < 3; k++)
				{
					int radius2 = WorldGen.genRand.Next(6, 9);
					Vector2 randomCloudPosition = offsetCloudPosition + Utils.NextVector2Circular(WorldGen.genRand, (float)radius2, (float)radius2) * 0.75f;
					randomCloudPosition.Y -= (float)radius2 * 0.4f;
					randomCloudPosition.Y += 5f;
					WorldUtils.Gen(Utils.ToPoint(randomCloudPosition), new Shapes.Circle(radius2 / 2), new Actions.SetTile(tileIdCloud, false, true));
				}
			}

			//
			for (int dx2 = -leftOffset + 10; dx2 < rightOffset - 10; dx2++)
			{
				float completionRatio11 = Mathf.Convert01To010(Utils.GetLerpValue((float)(-(float)leftOffset) - 22f, (float)rightOffset + 22f, (float)dx2, true));
				int verticalOffset2 = (int)((1f - completionRatio11) * (float)maxVerticalOffset);
				for (int dy = -5; dy < maxVerticalOffset + 13 - verticalOffset2; dy++)
				{
					if (!SupernovaWorldUtils.GetTileSafe(x + dx2, y + dy).HasTile)
					{
						Main.tile[x + dx2, y + dy].TileType = tileIdStone;
						Main.tile[x + dx2, y + dy].Get<TileWallWireStateData>().HasTile = true;
					}
				}
			}

			// Generate border
			//
			List<Point> borderPoints = new List<Point>();
			for (int dx3 = -leftOffset + 14; dx3 < rightOffset - 14; dx3++)
			{
				int verticalBorder = y - 10;
				while (SupernovaWorldUtils.GetTileSafe(x + dx3, verticalBorder).TileType != 189)
				{
					verticalBorder++;
					if (verticalBorder > y + 35)
					{
						break;
					}
				}
				if (verticalBorder < y + 35)
				{
					borderPoints.Add(new Point(x + dx3, verticalBorder));
				}
			}
			for (int l = 0; l < 10; l++)
			{
				Point borderToGenerateAt = borderPoints[WorldGen.genRand.Next(borderPoints.Count)];
				Vector2 moveDirection = Utils.RotatedBy(Vector2.UnitY, (double)Utils.NextFloat(WorldGen.genRand, -0.4f, 0.4f), default(Vector2));
				for (int m = 0; m < 4; m++)
				{
					WorldUtils.Gen(Utils.ToPoint(Utils.ToVector2(borderToGenerateAt) + Utils.NextVector2Circular(Main.rand, 5f, 5f) + moveDirection * (float)m * 3f), new CustomShapes.DistortedCircle(WorldGen.genRand.Next(8, 10) - m, 0.4f), Actions.Chain(new GenAction[]
					{
						new Actions.SetTile(tileIdStone, false, true),
						new Modifiers.IsSolid(),
						new Modifiers.Blotches(5, 1.0),
						new Modifiers.OnlyTiles(new ushort[]
						{
							tileIdStone,
						})
					}));
				}
			}

			// ???
			int n = 0;
			while (n < 12)
			{
				// Sand????
				int radius3 = WorldGen.genRand.Next(6, 8);
				ushort tileType = tileIdStone;//398;
				Point blotchPosition = new Point(x + WorldGen.genRand.Next(-leftOffset + 20, rightOffset - 20), y + WorldGen.genRand.Next(-3, 12));
				if (n <= 4)
				{
					goto IL_4CD;
				}
				if (blotchPosition.Y > y + 3)
				{
					radius3 -= WorldGen.genRand.Next(3);
					tileType = 22;// 22;
					goto IL_4CD;
				}
			IL_530:
				n++;
				continue;
			IL_4CD:
				WorldUtils.Gen(blotchPosition, new CustomShapes.DistortedCircle(radius3, 0.35f), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(tileType, false, true),
					new Modifiers.IsSolid(),
					new Modifiers.Blotches(5, 1.0),
					new Modifiers.OnlyTiles(new ushort[]
					{
						tileIdStone,
					})
				}));
				goto IL_530;
			}

			// 
			List<Point> surfacePoints = new List<Point>();
			for (int dx4 = -leftOffset + 24; dx4 < rightOffset - 24; dx4++)
			{
				if (Math.Abs(dx4) >= 15)
				{
					int surface = y - 15;
					bool hitCloud = false;
					while (!SupernovaWorldUtils.GetTileSafe(x + dx4, surface).HasTile)
					{
						surface++;
						if (SupernovaWorldUtils.GetTileSafe(x + dx4, surface).TileType == 189)
						{
							hitCloud = true;
							break;
						}
						if (surface > y + 35)
						{
							break;
						}
					}
					if (!hitCloud && surface < y + 35)
					{
						surfacePoints.Add(new Point(x + dx4, surface));
					}
				}
			}
			for (int k2 = 0; k2 < 4; k2++)
			{
				Point surfaceToGenerateAt = surfacePoints[WorldGen.genRand.Next(surfacePoints.Count)];
				WorldUtils.Gen(Utils.ToPoint(Utils.ToVector2(surfaceToGenerateAt) + Vector2.UnitY * 5f), new Shapes.Circle(WorldGen.genRand.Next(8, 10)), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(SupernovaWorldUtils.GetTileSafe(surfaceToGenerateAt.X, surfaceToGenerateAt.Y).TileType, false, true),
					new Modifiers.SkipTiles(new ushort[]
					{
						22,
						204
					}),
					new Modifiers.Blotches(5, 1.0)
				}));
			}

			// Generate ore
			//
			Vector2 oreStartPosition = new Vector2((float)(x + WorldGen.genRand.Next(-leftOffset + 28, rightOffset - 28)), (float)(y + WorldGen.genRand.Next(8, 14)));
			Vector2 oreEndPosition = oreStartPosition + Utils.RotatedBy(Vector2.UnitX, (double)Utils.NextFloat(WorldGen.genRand, -0.36f, -0.14f), default(Vector2)) * (float)Utils.ToDirectionInt(Utils.NextBool(WorldGen.genRand, 2)) * Utils.NextFloat(WorldGen.genRand, 18f, 25f);
			Vector2 oreMiddlePosition = (oreStartPosition + oreEndPosition) * 0.5f + Utils.NextVector2Circular(WorldGen.genRand, 6f, 6f);
			List<Vector2> smoothOrePositions = new BezierCurve(new Vector2[]
			{
				oreStartPosition,
				oreMiddlePosition,
				oreStartPosition
			}).GetPoints(25);
			for (int k3 = 0; k3 < smoothOrePositions.Count; k3++)
			{
				float strength = MathHelper.Lerp(1f, 4f, Mathf.Convert01To010((float)k3 / (float)smoothOrePositions.Count));
				WorldUtils.Gen(Utils.ToPoint(smoothOrePositions[k3]), new Shapes.Circle((int)strength), Actions.Chain(new GenAction[]
				{
					new Actions.SetTile(22, false, true)
				}));
			}
			for (int dx5 = -leftOffset - 30; dx5 < rightOffset + 30; dx5++)
			{
				for (int dy2 = -8; dy2 < 55; dy2++)
				{
					if (SupernovaWorldUtils.GetTileSafe(x + dx5, y + dy2).TileType == 189)
					{
						WorldGen.paintTile(x + dx5, y + dy2, 10, false);
					}
				}
			}
		}

		public static bool PlaceTree(int x, int y, int tileType)
		{
			int minDistance = 5;
			int treeNearby = 0;
			for (int i = x - minDistance; i < x + minDistance; i++)
			{
				for (int j = y - minDistance; j < y + minDistance; j++)
				{
					if (Main.tile[i, j].HasTile && (int)(Main.tile[i, j].TileType) == tileType)
					{
						treeNearby++;
						if (treeNearby > 0)
						{
							return false;
						}
					}
				}
			}
			//SpineTree.Spawn(x, y, 22, 28, false);
			return true;
		}

		public static bool PlaceAstroid(int x, int y, int size)
		{
			ushort tileIdStone = (ushort)ModContent.TileType<Content.Tiles.HorrorStone>();
			Point location = new Point(x, y);
			// Check if we can generate the astroid
			//
			// ...
			if (SupernovaWorldUtils.ShouldAvoidLocation(location))
			{
				return false;
			}

			try
			{
				// Place the astroid
				//
				GenShape shapeData = new CustomShapes.DistortedCircle(size, Main.rand.NextFloat(.1f, .3f));
				WorldUtils.Gen(location, shapeData, Actions.Chain(new GenAction[]
				{
				new Actions.SetTile(tileIdStone, false, true),
				new Modifiers.Blotches(4, 1.0)
				}));
				return true;
			}
			catch (Exception e)
			{
				Supernova.Instance.Logger.Error("HorrorSpace.PlaceAstroid(" + x+", "+y+", "+size+") failed: " + e.ToString());
				return false;
			}
		}

		private static int _startX;
	}*/
}
