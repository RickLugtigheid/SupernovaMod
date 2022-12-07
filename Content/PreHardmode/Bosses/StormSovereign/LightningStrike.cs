using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Content.PreHardmode.Bosses.StormSovereign
{
    public class LightningStrike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Strike");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            //Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            //Projectile.extraUpdates = 4;
            //Projectile.timeLeft = 120 * (Projectile.extraUpdates + 1);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
            return false;
        }

		public override void AI()
		{
            ref float lightningSpeed = ref Projectile.ai[1];
            if (Projectile.ai[0] == 0)
			{
                Vector2 velocityBase = new Vector2(0, 500);
                Vector2 velocity = Vector2.Normalize(velocityBase.RotatedByRandom(0.78539818525314331)) * lightningSpeed;
                int projID = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Projectile.ai[1], Projectile.Center.Y - 900, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, 0, ai0: velocityBase.ToRotation(), ai1: Main.rand.Next(100));
                Main.projectile[projID].ai[0] = MathHelper.ToRadians(90);

                Projectile.timeLeft = 40;
            }

            Projectile.ai[0]--;
        }
    }
}
