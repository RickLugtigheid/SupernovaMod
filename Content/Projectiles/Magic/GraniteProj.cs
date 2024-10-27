using System;
using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class GraniteProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;                       //this is the projectile penetration
            //Main.projFrames[projectile.type] = 4;           //this is projectile frames
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
            Projectile.alpha = 180;
		}

        public override void AI()
        {
            ref float timer = ref Projectile.localAI[0];
            timer++;

            if (timer <= 30)
            {
                Projectile.alpha -= 6;
                Projectile.velocity = new Vector2(Projectile.ai[0], Projectile.ai[1]) * .15f;
                Projectile.rotation = Projectile.GetTargetLookRotation(Main.MouseWorld);
			}
            else if (timer == 32)
            {
				SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
				Vector2 velocity = Main.MouseWorld - Projectile.Center;
				velocity.Normalize();
				Projectile.velocity = velocity * 14.5f;
				Projectile.tileCollide = true;
			}
            else
            {
                // Dust bolt effect
                //
				int num3;
				for (int i = 0; i < 3; i = num3 + 1)
				{
					int num242 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Granite, Projectile.velocity.X, Projectile.velocity.Y, 120, default(Color));
					Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
					Main.dust[num242].noGravity = true;
					Dust dust2 = Main.dust[num242];
					dust2.velocity *= 0.5f;
					num3 = i;
				}
				for (int num243 = 0; num243 < 2; num243 = num3 + 1)
				{
					int num242 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Granite, Projectile.velocity.X, Projectile.velocity.Y, 140, default(Color), 0.4f);
					if (num243 == 0)
					{
						Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center * 5f) / 6f;
					}
					else if (num243 == 1)
					{
						Main.dust[num242].position = (Main.dust[num242].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
					}
					Dust dust2 = Main.dust[num242];
					dust2.velocity *= 0.23f;
					Main.dust[num242].noGravity = true;
					Main.dust[num242].fadeIn = 1f;
					num3 = num243;
				}

				//this make that the projectile faces the right way
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 9.57f;
			}
			/*Projectile.localAI[0] += 1f;

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= MAX_TICKS)
                {
                    float velXmult = 0.98f;
                    float velYmult = 0.38f;
                    Projectile.ai[1] = MAX_TICKS;
                    Projectile.velocity.X = Projectile.velocity.X * velXmult;
                    Projectile.velocity.Y = Projectile.velocity.Y + velYmult;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }*/
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
            if (Main.rand.NextBool(3))
            {
				target.AddBuff(BuffID.Confused, 120);
			}
		}

		public override void OnKill(int timeLeft)
		{
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(4), DustID.Granite,
                    Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(0.8f, 1.3f), 0, default, 0.5f);
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
		}
	}
}

