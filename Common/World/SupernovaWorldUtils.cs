using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Common.World
{
	public static class SupernovaWorldUtils
	{
		/// <summary> 
		///  
		/// </summary> 
		/// <param name="placementPoint"></param> 
		/// <param name="careAboutLiquids"></param> 
		/// <returns></returns> 
		public static bool ShouldAvoidLocation(Point placementPoint, bool careAboutLiquids = true)
		{
			if (!WorldGen.InWorld(placementPoint.X, placementPoint.Y))
			{
				return false;
			}

			Tile tile = GetTileSafe(placementPoint.X, placementPoint.Y);
			return (careAboutLiquids && tile.LiquidAmount > 0)
			|| (
				Main.tileDungeon[tile.TileType]
				|| Main.wallDungeon[tile.WallType]
			)
			|| (
				tile.TileType == TileID.LihzahrdBrick
				|| tile.WallType == TileID.Pianos
			);
		}

		public static Tile GetTileSafe(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 0))
			{
				return default(Tile);
			}
			return Main.tile[x, y];
		}
	}
}
