using SupernovaMod.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class ManaCore : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.value = BuyPrice.RarityLightRed;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual = false)
		{
			// 400 / 100 = 4 * .06f = .24f
			player.manaCost -= .06f * (player.statLife / 100);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 4);
			recipe.AddIngredient(ItemID.ManaCrystal);
			recipe.AddIngredient(ModContent.ItemType<Materials.HelixStone>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
