using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;
using Terraria.Audio;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
	public class TerrorRecurve : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.autoReuse = false;
            Item.crit = 1;
            Item.knockBack = 3;
            Item.width = 16;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = BuyPrice.RarityBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 10;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Projectiles.Ranged.Arrows.TerrorWingArrow>();
                velocity *= .75f;
            }
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<Materials.TerrorTuft>(3);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
