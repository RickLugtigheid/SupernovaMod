using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class TrowingStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thrown Stone");
        }

        public override void SetDefaults()
        {   
            item.thrown = true; // Set this to true if the weapon is throwable.
            item.maxStack = 999; // Makes it so the weapon stacks.
            item.damage = 6;
            item.crit = 4;
            item.knockBack = 1f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 30;
            item.height = 30;
            item.consumable = true; // Makes it so one is taken from stack after use.
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = Rarity.Blue;
            item.shootSpeed = 8f;
            item.shoot = mod.ProjectileType("StoneProj");
            item.ammo = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }
    }
}
