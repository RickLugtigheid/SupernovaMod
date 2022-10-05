using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
    public class TerrorTuft : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Wing");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 50, 0);
        }

        public override void PostUpdate() => Lighting.AddLight(Item.Center, Color.Purple.ToVector3() * 0.7f * Main.essScale);
    }
}
