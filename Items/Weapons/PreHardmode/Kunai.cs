using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class Kunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kunai");
        }

        public override void SetDefaults()
        {       
            item.thrown = true; // Set this to true if the weapon is throwable.
            item.maxStack = 999; // Makes it so the weapon stacks.
            item.damage = 14;
            item.crit = 3;
            item.knockBack = 1f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 7;
            item.useTime = 7;
            item.width = 30;
            item.height = 30;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.consumable = true; // Makes it so one is taken from stack after use.
            item.value = Item.buyPrice(0, 0, 0, 30);
            item.rare = Rarity.Green;
            item.shootSpeed = 19f;
            item.shoot = mod.ProjectileType("KunaiProj");
        }
    }
}
