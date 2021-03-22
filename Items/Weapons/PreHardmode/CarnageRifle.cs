using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class CarnageRifle : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 41;
            item.ranged = true;
            item.width = 40;
            item.crit = 4;
            item.height = 20;
            item.useAnimation = 50;
            item.useTime = 50;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 6.4f;
            item.autoReuse = false;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item38;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 5);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 7);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(mod.GetItem("FirearmManual"), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}