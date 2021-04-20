using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class ShockSurge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Surge");

            Tooltip.SetDefault("Shoots a bolt of lighting at the target");
        }
        public override void SetDefaults()
        {
            item.damage = 14;  //The damage stat for the Weapon.
            item.crit = 3;
            item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.noUseGraphic = false;
            item.magic = true;    //This defines if it does magic damage and if its effected by magic increasing Armor/Accessories.
            item.channel = true;                            //Channel so that you can held the weapon
            item.rare = 3;   //The color the title of your Weapon when hovering over it ingame
            item.width = 28;   //The size of the width of the hitbox in pixels.
            item.height = 30;    //The size of the height of the hitbox in pixels.
            item.UseSound = SoundID.Item20;
            item.useTime = 40;
            item.useAnimation = 40;
            item.shootSpeed = 27;
            item.mana = 12;
            item.useStyle = 5;   //The way your Weapon will be used, 5 is the Holding Out Used for: Guns, Spellbooks, Drills, Chainsaws, Flails, Spears for example
            item.value = Item.sellPrice(0, 5, 32, 64);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Lightning");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RainCloud, 40);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.AddIngredient(ItemID.Silk, 4);
            recipe.AddIngredient(ItemID.Wood, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
