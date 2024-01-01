using SupernovaMod.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class IronSkull : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			//Tooltip.SetDefault("The more health you have left the less mana your weapons will use.\nReduces a max of 30% mana cost when at 500 health");
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
			if (player.statLife < 100)
			{
				player.statDefense += 11;
				return;
			}
			player.statDefense += (int)(10 / (player.statLife / 100f));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
			recipe.AddIngredient(ItemID.IronBar, 4);
			recipe.AddIngredient(ItemID.Skull);
			recipe.AddIngredient(ModContent.ItemType<Materials.HelixStone>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
