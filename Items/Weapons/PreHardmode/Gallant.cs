using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class Gallant : ModItem
    {
        int shoot;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gallant");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 40;
            item.crit = 3;
            item.height = 20;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.7f;
            item.value = Item.buyPrice(0, 4, 30, 0);
            item.autoReuse = false;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item38;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 9f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(shoot >= 6)
            {
                item.useAnimation = 84;
                item.useTime = 84;
                shoot = 0;
            }
            else
            {
                item.useAnimation = 4;
                item.useTime = 4;
            }
            shoot++;
            return true; // return true to allow tmodloader to call Projectile.NewProjectile as normal
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 8);
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.SetResult(this);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}