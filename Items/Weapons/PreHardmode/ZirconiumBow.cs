using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Items.Weapons.PreHardmode
{
    public class ZirconiumBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Bow");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.autoReuse = false;
            item.crit = 3;
            item.width = 16;
            item.height = 24;
            item.useAnimation = 15;
            item.useTime = 5;
            item.reuseDelay = 51;
            item.useStyle = 5; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(0, 3, 0, 0); // Another way to handle value of item.
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item5; // Sound for Bows
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = 1;
            item.shootSpeed = 5.7f;
            item.ranged = true; // For Ranged Weapon
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
