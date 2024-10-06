using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.CosmicCollective.Projectiles
{
    public class DimensionalRift : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 35;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 0.2f;
            Projectile.scale = 3;
        }

        public override void AI()
        {
            UpdateEffects();
        }
        protected virtual void UpdateEffects()
        {
            float finalPhaseDustRatio = .5f;

            int dustRings = 3;
            for (int h = 0; h < dustRings; h++)
            {
                float distanceDivisor = (float)h + 1f;
                float dustDistance = Projectile.ai[0] / distanceDivisor;
                int numDust = (int)(0.62831855f * dustDistance);
                float angleIncrement = 6.2831855f / (float)numDust;
                Vector2 dustOffset = new Vector2(dustDistance, 0f);
                dustOffset = Utils.RotatedByRandom(dustOffset, 6.2831854820251465);
                int var = (int)(dustDistance / finalPhaseDustRatio);
                float dustVelocity = 24f / distanceDivisor * finalPhaseDustRatio;
                for (int j = 0; j < numDust; j++)
                {
                    if (Utils.NextBool(Main.rand, var))
                    {
                        dustOffset = Utils.RotatedBy(dustOffset, (double)angleIncrement, default(Vector2));
                        int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Demonite, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[dust].position = Projectile.Center + dustOffset;
                        Main.dust[dust].fadeIn = 1f;
                        Main.dust[dust].velocity = Vector2.Normalize(Projectile.Center - Main.dust[dust].position) * dustVelocity;
                        Main.dust[dust].scale = 3f - (float)h;
                    }
                }
            }

            UpdatePortalEffects();
        }
        /// <summary>
        /// Effects based on <see cref="ProjectileID.VortexVortexPortal"/>
        /// </summary>
        private void UpdatePortalEffects()
        {
            if (Projectile.timeLeft < 80)
            {
                Projectile.scale -= .0125f;
            }

            Projectile.alpha = 255 - (int)(255f * Projectile.scale);
            Projectile.rotation -= 0.15707964f;
            if (Main.rand.NextBool(2))
            {
                Vector2 vector104 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust29 = Main.dust[Dust.NewDust(Projectile.Center - vector104 * 30f, 0, 0, DustID.CrimsonTorch, 0f, 0f, 0, default(Color), .9f)];
                dust29.noGravity = true;
                dust29.position = Projectile.Center - vector104 * (float)Main.rand.Next(4, 12);
                dust29.velocity = vector104.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
                dust29.scale = 0.5f + Main.rand.NextFloat();
                dust29.fadeIn = 0.5f;
                dust29.customData = Projectile.Center;
            }
            if (Main.rand.NextBool(2))
            {
                Vector2 vector105 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust30 = Main.dust[Dust.NewDust(Projectile.Center - vector105 * 30f, 0, 0, DustID.Blood, 0f, 0f, 0, default(Color), 1f)];
                dust30.noGravity = true;
                dust30.position = Projectile.Center - vector105 * 8f;
                dust30.velocity = vector105.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
                dust30.scale = 0.5f + Main.rand.NextFloat();
                dust30.fadeIn = 0.5f;
                dust30.customData = Projectile.Center;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Red;
            Projectile.localAI[0] = Projectile.type;
            Projectile.type = ProjectileID.MoonlordTurret;
            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            Projectile.type = (int)Projectile.localAI[0];
        }
    }
}
