﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class VerglasBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Bar");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 2100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.IronBar);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddIngredient(ModContent.ItemType<Rime>());
            recipe.AddIngredient(ItemID.IceBlock, 3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}
