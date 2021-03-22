using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Npcs.Bosses.FlyingTerror
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

            item.damage = 18;
            item.autoReuse = false;
            item.crit = 7;
            item.width = 16;
            item.height = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            item.rare = Rarity.Orange;
            item.UseSound = SoundID.Item5; // Sound for Bows
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 13f;
            item.ranged = true; // For Ranged Weapon
            item.useTime = 18;
            item.useAnimation = 18;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly && Main.rand.Next(4) == 0)
                type = ProjectileID.UnholyArrow;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("TerrorWing"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
