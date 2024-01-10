using Terraria.ModLoader;
using Terraria;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Biomes
{
	public class HorrorSpace : ModBiome
	{
		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			// First, we will use the exampleBlockCount from our added ModSystem for our first custom condition
			bool b1 = ModContent.GetInstance<HorrorSpaceTileCounter>().horrorStoneBlockCount >= 40;

			// Second, we will limit this biome to the inner horizontal third of the map as our second custom condition
			//int xStart = (int)(Main.maxTilesX * 0.888f);
			//int yEnd = 137;
			//bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX * 0.888f;

			// Finally, we will limit the height at which this biome can be active to above ground (ie sky and surface). Most (if not all) surface biomes will use this condition.
			bool b3 = player.ZoneSkyHeight;
			return b1 && b3;
		}

		// Declare biome priority. The default is BiomeLow so this is only necessary if it needs a higher priority.
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
	}
}
