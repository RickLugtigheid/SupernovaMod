using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class CarnageJavalin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Javalin");
        }

        public override void SetDefaults()
        {
            item.damage = 21;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 22;
            item.useTime = 64;
            item.shootSpeed = 4.18f; // The shoot speed for the spear projectile.
            item.knockBack = 0.63f;
            item.width = 32;
            item.height = 32;
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item1;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.melee = true;
            item.autoReuse = false; // Will auto reuse the item.
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("CarnageJavalinProj");
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] < 1;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 8);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
