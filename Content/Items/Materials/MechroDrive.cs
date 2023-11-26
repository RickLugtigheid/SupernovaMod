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
			Item.value = Item.buyPrice(0, 0, 3, 65);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.maxStack = 999;
        }
    }
}