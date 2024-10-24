using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class SoulOfSwiftness : ModItem
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
			HandleUpdateAccessory(player);
        }
        public static void HandleUpdateAccessory(Player player)
        {
            if (player.statLife < 100)
            {
                player.moveSpeed *= 1.2f;
                return;
            }
            float multi = .2f / (player.statLife / 100);
            player.moveSpeed *= 1 + multi;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SwiftnessPotion);
            recipe.AddIngredient(ItemID.Feather, 8);
            recipe.AddIngredient(ModContent.ItemType<Materials.HelixStone>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}
