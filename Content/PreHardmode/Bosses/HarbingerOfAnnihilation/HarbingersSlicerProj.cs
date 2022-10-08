using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Content.PreHardmode.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersSlicerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbingers Slicer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 30);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            return projHitbox.Intersects(targetHitbox);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i <= 10; i++)
                Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.UndergroundHallowedEnemies, -Projectile.velocity.X * Main.rand.NextFloat(.2f, .5f), -Projectile.velocity.Y * Main.rand.NextFloat(.2f, .5f));

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            var item = 0;

            if (Main.netMode == 1 && item >= 0)
                NetMessage.SendData(MessageID.KillProjectile);
        }

        // Optional Section 

        // End Optional Section 

        private const float maxTicks = 31f;
        private const int alphaReducation = 25;

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.UndergroundHallowedEnemies, Projectile.velocity.X * .5f, Projectile.velocity.Y * .5f, Scale: Main.rand.NextFloat(.5f, 1.3f));
            Main.dust[dust].noGravity = true;

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= alphaReducation;
            }

            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= maxTicks)
                {
                    float velXmult = 0.98f;
                    float velYmult = 0.35f;
                    Projectile.ai[1] = maxTicks;
                    Projectile.velocity.X = Projectile.velocity.X * velXmult;
                    Projectile.velocity.Y = Projectile.velocity.Y + velYmult;
                }

                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.80f;
            }
        }
    }
}