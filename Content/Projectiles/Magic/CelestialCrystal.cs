using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class CelestialCystal : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 375;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            //this is projectile dust
            if (Main.rand.NextBool(2))
            {
                Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool() ? DustID.CrystalPulse : DustID.CrystalPulse2)
                    .noGravity = true;
            }

			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.60f;
		}

		public override void OnKill(int timeLeft)
		{
            // Spawn dust on hit
            for (int i = 0; i <= Main.rand.Next(10, 20); i++)
            {
				Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, TerrariaRandom.NextDustIDCrystalShard(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(.5f, 1f));
			}
			// Break sound
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.position);
			base.OnKill(timeLeft);

			for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(2, 2) * 3;
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, velocity, ModContent.ProjectileType<CelestialCystalShard>(), (int)(Projectile.damage * .6f), Projectile.knockBack / 2, Projectile.owner);
            }
		}
	}
}
