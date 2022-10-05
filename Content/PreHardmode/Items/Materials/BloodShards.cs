using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class BloodShards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Shard");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Green;
        }
    }
}
