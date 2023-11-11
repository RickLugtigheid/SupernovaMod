using SupernovaMod.Api;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class MeteorBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = BuyPrice.RarityBlue;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.damage = 25;
            Item.DamageType = DamageClass.Generic;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            // TODO:
            // You would do an accessory based on the SimpleModPlayer
            // and then use the PreHurt hook in ModPlayer
            // basically you need to reimplement vanilla code a bit to detect if you received fall damage
            // read the adaption guide

			Common.Players.DashPlayer dashPlayer = player.GetModPlayer<Common.Players.DashPlayer>();
            dashPlayer.dashType = Common.Players.SupernovaDashType.Meteor;

            dashPlayer.UpdateDashAccessory(Item, hideVisual);
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
