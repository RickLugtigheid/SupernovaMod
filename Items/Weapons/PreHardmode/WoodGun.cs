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

        public override void SetDefaults()
        {
            item.damage = 5;
            item.ranged = true;
            item.width = 40;
            item.crit = 3;
            item.height = 20;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 3.7f;
            item.value = Item.buyPrice(0, 1, 70, 0); // Another way to handle value of item.
            item.autoReuse = false;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item41;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 7f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ConsumeAmmo(Player player) => Main.rand.NextFloat() >= .17f;
    }
}