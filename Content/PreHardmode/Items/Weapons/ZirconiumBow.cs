using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class ZirconiumBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Bow");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.autoReuse = false;
            Item.crit = 3;
            Item.width = 16;
            Item.height = 24;
            Item.useAnimation = 15;
            Item.useTime = 5;
            Item.reuseDelay = 51;
            Item.useStyle = ItemUseStyleID.Shoot; // Bow Use Style
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 3, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 5.7f;

            Item.DamageType = DamageClass.Ranged;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
