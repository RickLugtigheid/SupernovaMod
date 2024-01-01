using System;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class ZirconProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zircon Shot");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;                       //this is the projectile penetration
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 72;
        }

        public override void AI()
        {
			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 1.2f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.5f;
				num3 = num241;
			}
			for (int num243 = 0; num243 < 2; num243 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 0.4f);
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
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
        }

		public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ZicroniumExplosion>(), (int)(Projectile.damage * .6f), Projectile.knockBack, Projectile.owner, 90);
            SoundEngine.PlaySound(SoundID.Item14);
		}
    }
}

