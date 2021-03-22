using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class BloodShards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Shard");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.rare = Rarity.Green;
        }
    }
}
