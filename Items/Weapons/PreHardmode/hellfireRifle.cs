using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class hellfireRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellfire Rifle");
            Tooltip.SetDefault("Turns Wooden bullets into Molten Bullets");
        }
        public override void SetDefaults()
        {
            item.damage = 15;
            item.ranged = true;
            item.width = 40;
            item.crit = 4;
            item.height = 20;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 2.4f;
            item.value = Item.buyPrice(0, 15, 50, 0);
            item.autoReuse = true;
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 11f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("WoodenRifle"));
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 4);
            recipe.AddIngredient(ItemID.HellstoneBar, 17);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .18f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;

            if (type == mod.ProjectileType("WoodenBullet")) // or ProjectileID.WoodenArrowFriendly
            {
                type = mod.ProjectileType("MoltenBullet"); // or ProjectileID.FireArrow;
            }
            return true;
        }
    }
}