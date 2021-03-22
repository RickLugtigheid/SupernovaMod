using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class ZirconiumWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Wand");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.crit = 4;
            item.magic = true;
            item.width = 28;
            item.height = 34;
            item.useTime = 37;
            item.useAnimation = 37;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = Rarity.Orange;
            item.mana = 5;
            item.UseSound = SoundID.Item21;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ZirconProj"); 
            item.shootSpeed = 13f;
        }

         public override void AddRecipes()
         {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 10);
            recipe.SetResult(this);
            recipe.AddRecipe(); 
         }
    }
}