using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class VerglasThrowingAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Throwing Axe");
        }

        public override void SetDefaults()
        {
            item.damage = 27;
            item.crit = 4;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 30;
            item.height = 30;
            item.useTime = 17;
            item.useAnimation = 17;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 1;
            item.value = Item.buyPrice(0, 12, 47, 0); // Another way to handle value of item.
            item.rare = Rarity.Orange;
            item.shootSpeed = 12f;
            item.shoot = mod.ProjectileType("VerglasThrowingAxe");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}