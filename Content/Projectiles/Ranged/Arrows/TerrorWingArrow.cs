using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Ranged.Arrows
{
    public class TerrorWingArrow : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.ShadowFlameArrow}";

		public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

			Projectile.width = 14;
            Projectile.height = 38;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 240;
			AIType = ProjectileID.Bullet;
			Projectile.scale = .75f;
		}
		public override void AI()
        {
			Projectile.velocity *= 1.025f;

			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.TerrorDust>(), Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, num240, default(Color), 1.2f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.5f;
				num3 = num241;
			}
			for (int num243 = 0; num243 < 2; num243 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * .8f, Projectile.velocity.Y * .8f, num240, default(Color), 0.4f);
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
        }

		public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 4; i++)
			{
				Vector2 speed = new Vector2(0f, -3f).RotatedBy((double)(1.5707964f * (float)i), default(Vector2));
				Dust.NewDustPerfect(Projectile.position, ModContent.DustType<Dusts.TerrorDust>(), new Vector2?(speed), 0, default(Color), 1.4f).noGravity = true;
			}
		}
	}
}
