using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SupernovaMod.Content.Items.Materials
{
	public class Starcore : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 5);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofSight);
			recipe.AddIngredient(ItemID.SoulofMight);
			recipe.AddIngredient(ItemID.SoulofFright);
			recipe.AddIngredient<EldritchEssence>();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
