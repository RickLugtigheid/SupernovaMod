using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class QuarionShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("QuarionShard");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
        }
    }
}
