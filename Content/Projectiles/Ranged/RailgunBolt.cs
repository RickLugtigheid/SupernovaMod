using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SupernovaMod.Api.Effects;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Projectiles.Ranged
{
    public class RailgunBolt : ModProjectile
    {
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PulseBolt}";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.PulseBolt);

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 8;
			AIType = ProjectileID.Bullet;
		}
        public override void AI()
		{
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] % 8 == 0)
			{
				//DrawDust.Electricity(_startPosition.Value, Projectile.position, DustID.Electric, .65f, 60, default, .6f);
				DrawDust.RingScaleOutwards(Projectile.position, Vector2.One * 2, Vector2.One * 4, DustID.Electric, dustScale: .6f);
			}

			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), .5f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				num3 = num241;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextChance(.3f))
			{
				target.AddBuff(BuffID.Electrified, 120);
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Main.rand.NextChance(.3f))
			{
				target.AddBuff(BuffID.Electrified, 120);
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(2, 2);
			base.ModifyDamageHitbox(ref hitbox);
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				DrawDust.MakeExplosion(Projectile.Center, 6f, DustID.Electric, 22, 0f, 8f, 50, 120, .8f, 1.2f, true);
				DrawDust.MakeExplosion(Projectile.Center, 6f, DustID.Electric, 15, 4f, 14f, 70, 120, .7f, 1f, true);
				Projectile.CreateExplosion(96, 96, killProjectile: false);
			}
			return base.PreKill(timeLeft);
		}
	}
}
