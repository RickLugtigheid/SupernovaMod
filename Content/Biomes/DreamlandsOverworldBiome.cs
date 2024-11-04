using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Biomes
{
    public class DreamlandsOverworldBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive("SupernovaMod/Dreamlands");
        }
    }
}
