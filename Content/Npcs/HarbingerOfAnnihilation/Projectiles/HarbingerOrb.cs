using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles
{
    public class HarbingerOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Harbinger Orb");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 500;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 0.2f;
        }

        private Vector2 _targetPosition;
        private bool _initialized = false;
        public override void AI()
        {
            ref Player target = ref Main.player[(int)Projectile.ai[0]];

            if (!_initialized)
            {
                Projectile.scale = .01f;
                _initialized = true;
            }

            // Loop through the 5 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Projectile.scale < 1)
            {
                Projectile.scale += .01f;
                if (Projectile.scale >= 0.99f)
                {
					SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                }

				Vector2 dustPos = Projectile.Center + new Vector2((Projectile.width * Projectile.scale) + 2, 0).RotatedByRandom(MathHelper.ToRadians(360));
				Vector2 diff = Projectile.Center - dustPos;
				diff.Normalize();

				Dust.NewDustPerfect(dustPos, DustID.HallowedTorch, diff * 1, Scale: 1).noGravity = true;
				Dust.NewDustPerfect(dustPos, DustID.CrystalPulse, diff * 2, Scale: Projectile.scale + .05f).noGravity = true;
			}
            else
            {
                // Dusts
                //
                if (Main.rand.NextBool(4))
                {
                    for (int m = 0; m < 1; m++)
                    {
                        Vector2 value18 = -Vector2.UnitX.RotatedByRandom(0.19634954631328583).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                        int num306 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                        Dust obj7 = Main.dust[num306];
                        obj7.velocity *= 0.1f;
                        Main.dust[num306].position = Projectile.Center + value18 * Projectile.width / 2f;
                        Main.dust[num306].fadeIn = 0.9f;
                    }
                }
                if (Main.rand.NextBool(32))
                {
                    for (int n = 0; n < 1; n++)
                    {
                        Vector2 value19 = -Vector2.UnitX.RotatedByRandom(0.39269909262657166).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                        int num317 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 155, default, 0.8f);
                        Dust obj8 = Main.dust[num317];
                        obj8.velocity *= 0.3f;
                        Main.dust[num317].position = Projectile.Center + value19 * Projectile.width / 2f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num317].fadeIn = 1.4f;
                        }
                    }
                }
                if (Main.rand.NextBool(2))
                {
                    for (int num320 = 0; num320 < 2; num320++)
                    {
                        Vector2 value20 = -Vector2.UnitX.RotatedByRandom(0.78539818525314331).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                        int num326 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 1.2f);
                        Dust obj9 = Main.dust[num326];
                        obj9.velocity *= 0.3f;
                        Main.dust[num326].noGravity = true;
                        Main.dust[num326].position = Projectile.Center + value20 * Projectile.width / 2f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num326].fadeIn = 1.4f;
                        }
                    }
                }


                // Move
                _targetPosition = target.position;
                Vector2 distanceFromTarget = new Vector2(_targetPosition.X, _targetPosition.Y) - Projectile.Center;
                SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 3.5f, .001f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.timeLeft = 0;
            base.OnHitPlayer(target, info);
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 vector33;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int num925 = 0; num925 < 4; num925++)
            {
                int num915 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                Main.dust[num915].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
            }
            for (int num924 = 0; num924 < 30; num924++)
            {
                int num917 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 200, default, 3.7f);
                Main.dust[num917].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[num917].noGravity = true;
                Dust dust24 = Main.dust[num917];
                dust24.velocity *= 3f;
                num917 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1.5f);
                Main.dust[num917].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                dust24 = Main.dust[num917];
                dust24.velocity *= 2f;
                Main.dust[num917].noGravity = true;
                Main.dust[num917].fadeIn = 2.5f;
            }
            for (int num923 = 0; num923 < 10; num923++)
            {
                int num918 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 2.7f);
                Dust obj20 = Main.dust[num918];
                Vector2 center15 = Projectile.Center;
                Vector2 spinningpoint21 = Vector2.UnitX.RotatedByRandom(3.1415927410125732);
                double radians20 = (double)Projectile.velocity.ToRotation();
                vector33 = default;
                obj20.position = center15 + spinningpoint21.RotatedBy(radians20, vector33) * Projectile.width / 2f;
                Main.dust[num918].noGravity = true;
                Dust dust24 = Main.dust[num918];
                dust24.velocity *= 3f;
            }
            for (int num922 = 0; num922 < 10; num922++)
            {
                int num919 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                Dust obj21 = Main.dust[num919];
                Vector2 center16 = Projectile.Center;
                Vector2 spinningpoint22 = Vector2.UnitX.RotatedByRandom(3.1415927410125732);
                double radians21 = (double)Projectile.velocity.ToRotation();
                vector33 = default;
                obj21.position = center16 + spinningpoint22.RotatedBy(radians21, vector33) * Projectile.width / 2f;
                Main.dust[num919].noGravity = true;
                Dust dust24 = Main.dust[num919];
                dust24.velocity *= 3f;
            }
            for (int num921 = 0; num921 < 2; num921++)
            {
                Vector2 position16 = Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f;
                vector33 = default;
                int num920 = Gore.NewGore(Projectile.GetSource_Death(), position16, vector33, Main.rand.Next(61, 64), 1f);
                Main.gore[num920].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Gore gore2 = Main.gore[num920];
                gore2.velocity *= 0.3f;
                Gore expr_79A3_cp_0 = Main.gore[num920];
                expr_79A3_cp_0.velocity.X = expr_79A3_cp_0.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                Gore expr_79D3_cp_0 = Main.gore[num920];
                expr_79D3_cp_0.velocity.Y = expr_79D3_cp_0.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
            }
        }
    }
}
