using SupernovaMod.Content.Tiles;
using System;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class HorrorSpaceTileCounter : ModSystem
	{
		public int horrorStoneBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			horrorStoneBlockCount = tileCounts[ModContent.TileType<HorrorStone>()];
		}
	}
}
