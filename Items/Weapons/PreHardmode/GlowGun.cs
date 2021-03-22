using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class GlowGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glow Gun");
        }

        public override void SetDefaults()
        {
            item.damage = 13;
            item.width = 40;
            item.crit = 7;
            item.height = 20;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1.2f;
            item.value = Item.buyPrice(0, 2, 50, 0);
            item.autoReuse = true;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item38;
            item.shoot = mod.ProjectileType("GlowGunProj"); 
            item.shootSpeed = 8f;
            item.magic = true; 
            item.mana = 7;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddIngredient(ItemID.StickyGlowstick, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}