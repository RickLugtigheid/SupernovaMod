using Microsoft.Xna.Framework;
using SupernovaMod.Api.Helpers;
using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class Kunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

            // DisplayName.SetDefault("Kunai");
            // Tooltip.SetDefault("May throw 2 or 3 for the price of 1");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999; // Makes it so the weapon stacks.
            Item.damage = 10;
            Item.ArmorPenetration = 5;
            Item.crit = 1;
            Item.knockBack = 1f;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.consumable = true; // Makes it so one is taken from stack after use.
            Item.value = Item.buyPrice(0, 0, 0, 35);
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 16;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.KunaiProj>();

			Item.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Add random spread to our projectile
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (Main.rand.NextBool())
            {
				int shootAmount = Main.rand.Next(1, 3);
				Vector2[] speeds = Mathf.RandomSpread(velocity, 4, 6);
				for (int i = 0; i < shootAmount; ++i)
				{
					Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
				}
				return false;
            }

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
