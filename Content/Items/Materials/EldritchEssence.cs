using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Materials
{
	public class EldritchEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
			//ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 0, 80);
		}
	}
}
