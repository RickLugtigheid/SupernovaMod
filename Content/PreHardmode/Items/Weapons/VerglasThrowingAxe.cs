using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class VerglasThrowingAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Throwing Axe");
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.crit = 4;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.noUseGraphic = true;
            Item.useStyle = 1;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 12, 47, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.VerglasThrowingAxe>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}