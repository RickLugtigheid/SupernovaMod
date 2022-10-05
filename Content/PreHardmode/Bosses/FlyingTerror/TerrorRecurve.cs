using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
    public class TerrorRecurve : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Recurve");
            Tooltip.SetDefault("May shoot a unholy arrow");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.autoReuse = false;
            Item.crit = 7;
            Item.width = 16;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 13f;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.DamageType = DamageClass.Ranged;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly && Main.rand.NextBool(4))
            {
                type = ProjectileID.UnholyArrow;
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
