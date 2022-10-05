using Microsoft.Xna.Framework;
using Supernova.Common.Players;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersKnellProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omen");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.CloneDefaults(317);
            AIType = 317;

            Projectile.width = 20;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();
            if (player.dead)
            {
                modPlayer.hasMinionHairbringersKnell = false;
            }
            if (modPlayer.hasMinionHairbringersKnell)
            {
                Projectile.timeLeft = 2;
            }
            if(!player.HasBuff(ModContent.BuffType<Global.Buffs.Minion.HarbingersKnellBuff>()))
            {
                modPlayer.hasMinionHairbringersKnell = false;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)

            {
                Projectile.tileCollide = false;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.tileCollide = false;
            }
            return false;
        }
        public override void AI()
        {
            //projectile.velocity *= 1.5f;
            Projectile.rotation += (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(80);
            CheckActive();
        }
    }
}