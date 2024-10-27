using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
	public class MechroDrive : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 0, 6);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.maxStack = 9999;
        }
    }
}