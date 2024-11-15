using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Biomes
{
    public class DreamlandsOverworldBiome : ModBiome
    {
        // Select all the scenery
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<DreamlandsWaterStyle>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;

        // Select Music


        // Populate the Bestiary Filter


        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive("SupernovaMod/Dreamlands");
        }


        // Declare biome priority. The default is BiomeLow so this is only necessary if it needs a higher priority.
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
    }
}
