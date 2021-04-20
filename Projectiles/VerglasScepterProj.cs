using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Supernova.Projectiles
{
    public class VerglasScepterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Scepter");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 375;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;

        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(36));
            projectile.velocity = perturbedSpeed;

            // Spawn dust on hit
            for (int i = 0; i <= Main.rand.Next(4, 8); i++)
			{
                int dust = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height * 2, DustID.Ice, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 20, default(Color), Main.rand.NextFloat(.5f, 1.5f));
                Main.dust[dust].noGravity = true;
            }
            // Ice break sound
            Main.PlaySound(SoundID.Item50, projectile.position);
        }
        int bounce;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounce++;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            if (bounce >= 6) return true;
            return false; // return false because we are handling collision
        }
        public override void AI()
        {
            //this is projectile dust
            if(Main.rand.NextBool(2))
			{
                int DustID2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 92, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), Main.rand.NextFloat(.85f, 1.35f));
                Main.dust[DustID2].noGravity = true;
            }


            // Add sinewave effect
            const double amp = 5;
            const double freq = .1;
            float sineWave = (float)(Math.Cos(freq * projectile.timeLeft) * amp * freq) * projectile.ai[0];
            //projectile.position.Y += sineWave;
            projectile.velocity.Y += sineWave;
            //projectile.velocity.Y = (float)(1 * projectile.timeLeft + (Math.Cos(freq * projectile.timeLeft) * amp * freq)) * -1;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.60f;

            /*projectile.localAI[0] += 1f;

            // After x times swap the value
            projectile.ai[0]++;
            if (projectile.ai[0] >= 30)
			{
                projectile.ai[0] = 0;
                sineValue = -sineValue;
                if (sineValue == 0) sineValue = .025f;
            }
            // Rotate a small bit
            projectile.velocity = projectile.velocity.RotatedByRandom(sineValue);*/
        }
        public override void Kill(int timeLeft)
		{
            // Spawn dust on hit
            for(int i = 0; i <= Main.rand.Next(10, 20); i++)
               Dust.NewDust(projectile.position, projectile.width * 2, projectile.height * 2, DustID.Ice, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 20, default(Color), Main.rand.NextFloat(.5f, 1.5f));
            // Ice break sound
            Main.PlaySound(SoundID.Item50, projectile.position);
            base.Kill(timeLeft);
		}
	}
}
