using Microsoft.Xna.Framework;
using Supernova.Common;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Items.Weapons.BaseWeapons
{
    public enum GunStyle { Default, Shotgun }
    public enum GunUseStyle { Default, PumpAction }
    public abstract class SupernovaGunItem : ModItem
    {
        public GunItem Gun { get; } = new GunItem();

        /// <summary>
        /// Base SetDefaults for a gun
        /// </summary>
        public override void SetDefaults()
        {
            Gun.SetHandleDefault();

            Item.noMelee = true; // So the item's animation doesn't do damage
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.Bullet;

            Item.DamageType = DamageClass.Ranged;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Gun.style == GunStyle.Default)
            {
                // Add random spread to our projectile
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(Gun.spread));
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.MountedCenter + player.itemRotation.ToRotationVector2() * 7f;
            position.Y -= Item.height / 2;
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 30;
            MuzzleFlash(position + muzzleOffset, velocity);

            if (Gun.style == GunStyle.Shotgun)
            {
                return ShootShotgun(player, source, position, velocity, type, damage, knockback);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        protected virtual bool ShootShotgun(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int shots = Main.rand.Next(
                Gun.shotgunMinShots,
                Gun.shotgunMaxShots
            );
            Vector2[] speeds = Mathf.RandomSpread(velocity, Gun.spread, 6);
            for (int i = 0; i < shots; ++i)
            {
                Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        protected virtual void MuzzleFlash(Vector2 position, Vector2 speed)
        {
            if (!Gun.muzzleFlash)
            {
                return;
            }
            int dustId = DustID.Torch;
            int dustCount = 6;
            ModifyMuzzleFlash(ref position, ref speed, ref dustId, ref dustCount);

            for (int i = 0; i < dustCount; i++)
            {
                int dust = Dust.NewDust(position, -Item.height, Item.height, dustId, speed.X, speed.Y);
                Main.dust[dust].noGravity = true;
            }
        }
        protected virtual void ModifyMuzzleFlash(ref Vector2 position, ref Vector2 speed, ref int dustId, ref int dustCount)
        {
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.direction = Math.Sign((Main.MouseWorld - player.Center).X);
            float itemRotation = player.compositeFrontArm.rotation + 1.5707964f * player.gravDir;
            Vector2 itemPosition = player.MountedCenter + itemRotation.ToRotationVector2() * 7f;
            SupernovaUtils.CleanHoldStyle(player, itemRotation, itemPosition, new Vector2(Item.width, Item.height), new Vector2?(Gun.handlePosition), false, false, true);
            base.UseStyle(player, heldItemFrame);
        }

        public override void UseItemFrame(Player player)
        {
            player.direction = Math.Sign((Main.MouseWorld - player.Center).X);
            float animProgress = 1f - player.itemTime / (float)player.itemTimeMax;
            float rotation = (player.Center - Main.MouseWorld).ToRotation() * player.gravDir + 1.5707964f;
            if (animProgress < 0.4f)
            {
                // Aply gun recoil
                float recoil = -Gun.recoil;
                rotation += recoil * (float)Math.Pow((double)((0.4f - animProgress) / 0.4f), 2.0) * player.direction;
            }
            player.SetCompositeArmFront(true, 0, rotation);

            if (Gun.useStyle != GunUseStyle.Default && animProgress > 0.5f)
            {
                if (Gun.useStyle == GunUseStyle.PumpAction)
                {
                    float backArmRotation = rotation + 0.52f * player.direction;
                    Player.CompositeArmStretchAmount stretch = ((float)Math.Sin((double)(3.1415927f * (animProgress - 0.5f) / 0.36f))).ToStretchAmount();
                    player.SetCompositeArmBack(true, stretch, backArmRotation);
                }
            }
        }
    }

    public class GunItem
    {
        public Vector2 handlePosition = Vector2.Zero;

        /// <summary>
        /// GunStyle.Default: The random spread to aplly when shooting in Degrees.
        /// <para>GunStyle.Shotgun: The shotgun spread pattern.</para>
        /// </summary>
        public float spread;
        /// <summary>
        /// The style of the gun
        /// </summary>
        public GunStyle style = GunStyle.Default;

        public GunUseStyle useStyle = GunUseStyle.Default;

        /// <summary>
        /// The gun item recoil when shooting.
        /// <para>Default = 0.45f</para>
        /// </summary>
        public float recoil = .45f;

        /// <summary>
        /// The minimum amount of shots to shoot for GunStyle.Shotgun
        /// </summary>
        public int shotgunMinShots = 1;
        /// <summary>
        /// The maximum amount of shots to shoot for GunStyle.Shotgun
        /// </summary>
        public int shotgunMaxShots = 1;

        /// <summary>
        /// Enables muzzle flash
        /// </summary>
        public bool muzzleFlash = false;

        /// <summary>
        /// Set the handle position to handgun default position
        /// </summary>
        public void SetHandleDefault() => handlePosition = new Vector2(-15, 1);
    }
}
