using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class CarnageScepter : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Scepter");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.crit = 7;
            item.magic = true;          //this make the item do magic damage
            item.width = 28;
            item.height = 34;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.HoldingOut;        //this is how the item is holded
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = Rarity.Green;
            item.mana = 7;             //mana use
            item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CarnageScepterProj");
            item.shootSpeed = 10f;    //projectile speed when shoot
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 12);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}