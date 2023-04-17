/*using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Humanizer.On;

namespace SupernovaMod.Common.Systems.Generation
{
	internal class SupernovaWorldStructures : ModSystem
	{
		public static void GenerateMeteorChests(GenerationProgress progress, GameConfiguration config)
		{
			Chest chest = null;
			int attempts = 0;
			while (chest == null && attempts < 1000)
			{
				attempts++;
				int x = WorldGen.genRand.Next(MinX, MaxX);
				int y = WorldGen.genRand.Next((int)Main.worldSurface, MaxY);
				if (Main.wallDungeon[Main.tile[x, y].WallType] && !Main.tile[x, y].HasTile)
				{
					chest = MiscWorldgenRoutines.AddChestWithLoot(x, y, (ushort)ChestTypes[i], 1U, ChestStyles[i]);
				}
			}
			if (chest != null)
			{
				chest.item[0].SetDefaults(ItemTypes[i]);
				chest.item[0].Prefix(-1);
			}
		}
		private static Chest AddChestWithLoot(int i, int j, ushort type = 21, uint startingSlot = 1U, int tileStyle = 0)
		{
			int chestIndex = -1;
			while (j < Main.maxTilesY - 210)
			{
				if (WorldGen.SolidTile(i, j, false))
				{
					chestIndex = WorldGen.PlaceChest(i - 1, j - 1, type, false, tileStyle);
					break;
				}
				j++;
			}
			if (chestIndex < 0)
			{
				return null;
			}
			Chest chest = Main.chest[chestIndex];
			PlaceLootInChest(ref chest, type, startingSlot);
			return chest;
		}
		private static void PlaceLootInChest(ref Chest chest, ushort type, uint startingSlot)
		{

		}
	}
}
*/