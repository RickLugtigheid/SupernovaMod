using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace SupernovaMod.Content.Npcs.StormSovereign.Projectiles
{
    public class StormCloud : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.RainCloudRaining}";

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Cloud");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.RainCloudMoving);
			//Projectile.width = 14;
			//Projectile.height = 14;
			//Projectile.aiStyle = -1;
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
                SoundEngine.PlaySound(SoundID.Thunder, Projectile.position);
                Vector2 velocityBase = new Vector2(0, 500);
                Vector2 velocity = Vector2.Normalize(velocityBase.RotatedByRandom(0.78539818525314331)) * lightningSpeed;
                int projID = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, 0, ai0: velocityBase.ToRotation(), ai1: Main.rand.Next(100));
                Main.projectile[projID].ai[0] = MathHelper.ToRadians(90);

                Projectile.timeLeft = 40;
            }

			int num3 = Projectile.frameCounter + 1;
			Projectile.frameCounter = num3;
			if (num3 > 4)
			{
				Projectile.frameCounter = 0;
				num3 = Projectile.frame + 1;
				Projectile.frame = num3;
				if (num3 >= 6)
				{
					Projectile.frame = 0;
				}
			}

			Projectile.ai[0]--;
        }
    }
}
