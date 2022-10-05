using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class QuarionShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("QuarionShard");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
        }
    }
}
