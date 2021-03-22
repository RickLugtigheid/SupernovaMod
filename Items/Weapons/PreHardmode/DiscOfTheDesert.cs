using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class DiscOfTheDesert : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disc of the Desert");
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.crit = 2;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 23;
            item.height = 23;
            item.useTime = 8;
            item.useAnimation = 8;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 0.1f;
            item.value = Item.buyPrice(0, 5, 60, 0);
            item.rare = Rarity.Orange;
            item.shootSpeed = 7f;
            item.shoot = mod.ProjectileType("DiscOfTheDesertProj");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void AddRecipes() //SturdyFossil
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amber, 7);
            recipe.AddIngredient(ItemID.DesertFossil, 20);
            recipe.AddIngredient(mod.GetItem("CactusBoomerang"));
            recipe.AddIngredient(ItemID.SandBlock, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}