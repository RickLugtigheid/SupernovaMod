using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SupernovaMod.Api.Drawing;
using System;

namespace SupernovaMod.Content.Npcs.StormSovereign.Projectiles
{
    public class LightningOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lightning Orb");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
			Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.scale *= 1.5f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
		}

        public override void AI()
        {
            // Check if we have created a link to an other orb
            //
            if (Projectile.ai[0] > 0)
            {
                Projectile linkedProj = Main.projectile[(int)Projectile.ai[0]];

				Vector2 direction9 = linkedProj.Center - Projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				if (Projectile.localAI[0] % 8 == 0 /*&& distance < 1000*/ && linkedProj.active)
				{
					DrawDust.Electricity(Projectile.Center + (Projectile.velocity * 5), linkedProj.Center + (linkedProj.velocity * 5), DustID.Electric, 0.8f, 60, Color.Blue);
				}
                else if (!linkedProj.active)
                {
                    Projectile.ai[0] = 0;
                }
                Projectile.localAI[0]++;
			}

			Projectile.velocity.X = Projectile.velocity.X *= .99f; // 0.99f for rolling grenade speed reduction. Try values between 0.9f and 0.99f
			Projectile.velocity.Y = Projectile.velocity.Y *= .99f;

            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.15f) / 255f, ((255 - Projectile.alpha) * 0.45f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);   //this is the light colors
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 120);   //this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.ai[0] < 0)
			{
				return base.Colliding(projHitbox, targetHitbox);
			}

			Projectile linkedProj = Main.projectile[(int)Projectile.ai[0]];

            if (!linkedProj.active)
            {
                Projectile.ai[0] = 0;
				return base.Colliding(projHitbox, targetHitbox);
			}

			Vector2 lineStart = Projectile.Center;
			float collisionpoint = 0f;
			Vector2 lineEnd = linkedProj.Center;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, Projectile.scale / 2, ref collisionpoint))
			{
				return true;
			}
			return false;
		}
    }
}
