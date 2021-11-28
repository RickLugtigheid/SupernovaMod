using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class CarnageTrowingKnive : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Trowing Knive");
        }

        public override void SetDefaults()
        {
            
            item.thrown = true; // Set this to true if the weapon is throwable.
            item.maxStack = 999; // Makes it so the weapon stacks.
            item.damage = 11;
            item.crit = 7;
            item.knockBack = 3f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 8;
            item.useTime = 8;
            item.width = 30;
            item.height = 30;
            item.consumable = true; // Makes it so one is taken from stack after use.
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.value = 64;
            item.rare = Rarity.Orange;
            item.shootSpeed = 11f;
            item.shoot = mod.ProjectileType("CarnageTrowingKniveProj");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 3);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }

    }
}
