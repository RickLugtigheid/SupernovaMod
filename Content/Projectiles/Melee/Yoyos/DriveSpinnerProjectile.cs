using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Api;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Melee.Yoyos
{
    public class DriveSpinnerProjectile : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.scale = 1f;
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 14;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 320;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 14f;
		}
		public override void AI()
		{
			if (Main.rand.NextBool(2))
			{
				int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width + 2, Projectile.height + 2, DustID.Electric, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 80, default(Color), 0.7f);
				Main.dust[DustID2].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextChance(.4f))
			{
				target.AddBuff(BuffID.Electrified, 180);
			}
			OnHitAny(target);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (info.PvP && Main.rand.NextChance(.4f))
			{
				target.AddBuff(BuffID.Electrified, 180);
			}
			OnHitAny(target);
		}
		private void OnHitAny(Entity target)
		{
			Projectile.ai[2]++;
			if (Projectile.ai[2] >= 2)
			{
                const float speed = 3.5f;
                Vector2 randVelocity = Main.rand.NextVector2CircularEdge(speed, speed);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, randVelocity, ModContent.ProjectileType<MicroMagnetSphere>(), (int)(Projectile.damage * .8f), 2, Projectile.owner);
				Projectile.ai[2] = 0;
            }
		}
    }
}
