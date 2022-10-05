using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class DiscOfTheDesert : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disc of the Desert");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.crit = 2;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 23;
            Item.height = 23;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.noUseGraphic = true;
            Item.useStyle = 1;
            Item.knockBack = 0.1f;
            Item.value = Item.buyPrice(0, 5, 60, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 7f;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.DiscOfTheDesertProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Throwing;
        }

        public override void AddRecipes() //SturdyFossil
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amber, 7);
            recipe.AddIngredient(ItemID.DesertFossil, 20);
            recipe.AddIngredient(ModContent.ItemType<CactusBoomerang>());
            recipe.AddIngredient(ItemID.SandBlock, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}