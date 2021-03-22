using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class MagicStarBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Starblade");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }
        public override void SetDefaults()
        {
            item.damage = 31;
            item.crit = 5;
            item.magic = true;          //this make the item do magic damage
            item.width = 28;
			item.height = 34;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 1000;
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item29;            //this is the sound when you use the item
            item.autoReuse = true;
            item.shootSpeed = 15f;
            item.shoot = ProjectileID.HallowStar;
            item.useTime = 24;
            item.useAnimation = 24;
            item.mana = 7;             //mana use
            item.Size *= 0.5f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Feather, 20);
            recipe.AddIngredient(ItemID.PlatinumBar, 14);
            recipe.AddIngredient(ItemID.FallenStar, 6);
            recipe.AddIngredient(ItemID.BeeWax, 4);
            recipe.AddIngredient(ItemID.Cloud, 35);
            recipe.AddIngredient(ItemID.RainCloud, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe(); 
        }
    }
}