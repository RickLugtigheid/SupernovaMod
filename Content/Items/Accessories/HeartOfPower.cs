using SupernovaMod.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class HeartOfPower : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			//DisplayName.SetDefault("Heart of Power");
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
			HandleUpdateAccessory(player);
		}

		public static void HandleUpdateAccessory(Player player)
		{
            // 400 / 100 = 4 * .04f = .16f
            player.GetDamage(DamageClass.Generic) += .04f * (player.statLife / 100);
        }


        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 4);
			recipe.AddIngredient(ItemID.LifeCrystal);
			recipe.AddIngredient(ModContent.ItemType<Materials.HelixStone>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
