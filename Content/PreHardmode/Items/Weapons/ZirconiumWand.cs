using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class ZirconiumWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Wand");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.crit = 4;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 37;
            Item.useAnimation = 37;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.mana = 5;
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.ZirconProj>();
            Item.shootSpeed = 13f;

            Item.DamageType = DamageClass.Magic;
        }

         public override void AddRecipes()
         {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register(); 
         }
    }
}