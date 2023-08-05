using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class ZirconiumBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Bow");
            // Tooltip.SetDefault("Wooden arrows turn into Zirconium Arrows.\nZirconium Arrows explode on impact.");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.autoReuse = false;
			Item.crit = 1;
			Item.knockBack = 3;
            Item.width = 16;
            Item.height = 24;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.useStyle = ItemUseStyleID.Shoot; // Bow Use Style
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = Item.buyPrice(0, 3, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 5;

            Item.DamageType = DamageClass.Ranged;
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Projectiles.Ranged.Arrows.ZirconiumArrow>();
            }
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
