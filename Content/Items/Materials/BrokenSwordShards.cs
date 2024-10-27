using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
	public class BrokenSwordShards : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(1);
		}
	}
}
