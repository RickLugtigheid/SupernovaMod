using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class GlowGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Glow Gun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-4, -2);

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item38;
            Item.shoot = ModContent.ProjectileType<Global.Projectiles.GlowGunProj>();
            Item.shootSpeed = 8f;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;

            Item.scale = .8f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddIngredient(ItemID.StickyGlowstick, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            // Rotate our projectile randomly
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
    }
}