using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class BlazingFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Fire");
            Tooltip.SetDefault("Creates a fire aura around you.");
        }
        public override void SetDefaults()
        {
            item.damage = 18;    //The damage stat for the Weapon.
            item.melee = true;
            item.crit = 8;
            item.scale *= 1.56f;
            item.width = 80;
            item.height = 80;
            item.useTime = 6;
            item.useAnimation = 6;
            item.channel = true;
            item.useStyle = 100;
            item.knockBack = 6f;
            item.value = Item.buyPrice(0, 5, 40, 0); 
            item.rare = Rarity.Orange;   			
            item.shoot = mod.ProjectileType("BlazingFireProj");
            item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddIngredient(ItemID.Obsidian, 7);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}