using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Weapons.Ranged
{
    public class VerglasBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Bow");
            Tooltip.SetDefault("Turns wooden arrows into frostburn arrows.");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.autoReuse = true;
            Item.crit = 5;
            Item.width = 16;
            Item.height = 24;
            Item.useTime = 53;
            Item.useAnimation = 53;
            Item.useStyle = 5; // Bow Use Style
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 9, 47, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 13f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Convert wooden arrows to frostburn arrows
            //
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ProjectileID.FrostburnArrow;
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
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
