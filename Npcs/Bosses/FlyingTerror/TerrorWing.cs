using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorWing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Wing");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 2;
            item.rare = Rarity.Orange;
            item.value = Item.buyPrice(0, 2, 50, 0);
        }

        public override void PostUpdate() => Lighting.AddLight(item.Center, Color.Purple.ToVector3() * 0.7f * Main.essScale);
    }
}
