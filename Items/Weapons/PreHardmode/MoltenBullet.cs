using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class MoltenBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Bullet");
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.ranged = true;
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 0f;
            item.value = 10;
            item.rare = Rarity.Orange;
            item.shoot = mod.ProjectileType("MoltenBullet");
            item.shootSpeed = 4f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 25);
            recipe.AddRecipe();
        }
    }
}
