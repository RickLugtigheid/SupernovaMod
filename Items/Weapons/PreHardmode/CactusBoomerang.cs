using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class CactusBoomerang : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cactus Boomerang");
        }
        public override void SetDefaults()
        {
            item.damage = 14;
            item.crit = 4;
            item.thrown = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.width = 30;
            item.height = 30;
            item.useTime = 14;
            item.useAnimation = 14;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 1.2f;
            item.value = Item.buyPrice(0, 1, 46, 82);
            item.rare = Rarity.Blue;
            item.shootSpeed = 6f;
            item.shoot = mod.ProjectileType("CactusBoomerangProj");
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
        }
        public override bool CanUseItem(Player player) //this make that you can shoot only 1 boomerang at once
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 34);
            recipe.AddIngredient(ItemID.AntlionMandible, 2);
            recipe.AddIngredient(ItemID.SandBlock, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}