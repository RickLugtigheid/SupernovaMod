using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class SnowGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Glove");
        }

        public override void SetDefaults()
        {
            
            item.thrown = true; // Set this to true if the weapon is throwable.
            //item.maxStack = 1; // Makes it so the weapon stacks.
            item.damage = 8;
            item.crit = 3;
            item.knockBack = 2f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 23;
            item.useTime = 23;
            item.width = 32;
            item.height = 32;
            //item.consumable = true; // Makes it so one is taken from stack after use.
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.buyPrice(0, 4, 46, 0); // Another way to handle value of item.
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ProjectileID.SnowBallFriendly;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = Calc.RandomSpread(speedX, speedY, 4, .1f, 3);
            for (int i = 0; i < 3; ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SnowBlock, 70);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
