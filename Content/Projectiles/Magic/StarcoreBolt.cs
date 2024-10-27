using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class StarcoreBolt : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.InfernoHostileBolt}";

        private float _speed = 16;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;                       //this is the projectile penetration
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 460;
        }

        public override void AI()
        {
			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.YellowStarDust, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 1.2f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.5f;
				num3 = num241;
			}
			for (int num243 = 0; num243 < 2; num243 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.YellowStarfish, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 0.4f);
				if (num243 == 0)
				{
					Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center * 5f) / 6f;
				}
				else if (num243 == 1)
				{
					Main.dust[num242].position = (Main.dust[num242].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
				}
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.1f;
				Main.dust[num242].noGravity = true;
				Main.dust[num242].fadeIn = 1f;
				num3 = num243;
			}
            //this make that the projectile faces the right way
            //Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            AI_Homing();
        }

		private void AI_Homing()
		{
            if (Projectile.ai[1] >= 4)
            {
                float d = 248f;
                bool targetfound = false;
                Vector2 targetcenter = Projectile.position;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(null, false))
                    {
                        float dpt = Vector2.Distance(Projectile.Center, npc.Center);
                        if ((dpt < d && !targetfound) || dpt < d)
                        {
                            d = dpt;
                            targetfound = true;
                            targetcenter = npc.Center;
                        }
                    }
                }
                if (targetfound)
                {
                    Projectile.velocity = Vector2.Normalize(targetcenter - Projectile.Center) + Projectile.oldVelocity * 0.87f;
                    float num = Math.Abs(Projectile.velocity.X);
                    float vely = Math.Abs(Projectile.velocity.Y);
                    if (num > _speed)
                    {
                        float direction = Math.Abs(Projectile.velocity.X) / Projectile.velocity.X;
                        Projectile.velocity.X = _speed * direction;
                    }
                    if (vely > _speed)
                    {
                        float direction2 = Math.Abs(Projectile.velocity.Y) / Projectile.velocity.Y;
                        Projectile.velocity.Y = _speed * direction2;
                        return;
                    }
                }
                else
                {
                    Projectile.velocity = new Vector2(_speed, 0f).RotatedBy((double)Projectile.rotation, default(Vector2));
                }
            }
            else
            {
                Projectile.ai[1]++;
            }
        }

		public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, -70), Vector2.Zero, ModContent.ProjectileType<MagmaBlast>(), (int)(Projectile.damage * .8f), Projectile.knockBack, Projectile.owner, 90);
            SoundEngine.PlaySound(SoundID.Item14);
		}

        /*public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Vector2 origin11 = new Vector2(120f, 180f);
            Main.spriteBatch.Draw(Main.glowMaskTexture[128], Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), Projectile.getRect(), Microsoft.Xna.Framework.Color.White * 0.3f, Projectile.rotation, origin11, Projectile.scale, spriteEffects, 0f);
            return base.PreDraw(ref lightColor);
        }*/
    }
}

