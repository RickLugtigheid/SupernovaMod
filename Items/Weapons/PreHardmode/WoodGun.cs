using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class WoodGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wood Gun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override void SetDefaults()
        {
            item.damage = 3;
            item.ranged = true;
            item.width = 40;
            item.crit = 3;
            item.height = 20;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1.7f;
            item.value = Item.buyPrice(0, 1, 70, 0); // Another way to handle value of item.
            item.autoReuse = false;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item41;
            item.shoot = 10;
            item.shootSpeed = 4;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}