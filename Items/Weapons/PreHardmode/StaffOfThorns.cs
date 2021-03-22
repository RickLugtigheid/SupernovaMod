using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class StaffOfThorns : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Thorns");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.crit = 4;
            item.magic = true;          //this make the item do magic damage
            item.width = 28;
            item.height = 34;
            item.useTime = 78;
            item.useAnimation = 78;
            item.useStyle = 5;        //this is how the item is holded
            item.noMelee = true;
            item.knockBack = 1.7f;
            item.value = Item.buyPrice(0, 5, 77, 0); // Another way to handle value of item.
            item.rare = 6;
            item.mana = 5;             //mana use
            item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ThornBal"); 
            item.shootSpeed = 6.75f;    //projectile speed when shoot
        }
    }
}