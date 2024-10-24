using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Projectiles.Magic
{
	public class EarthProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 120;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(5);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(7))
			{
				target.AddBuff(BuffID.Confused, 180);
			}
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(7))
            {
                target.AddBuff(BuffID.Confused, 180);
            }
        }

        public override bool PreKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				DrawDust.MakeExplosion(Projectile.Center, 4f, DustID.Dirt, 20, 0f, 6f, 50, 120, 1f, 1.5f, true);
				Projectile.CreateExplosion(52, 52);
			}
			return base.PreKill(timeLeft);
		}

		public override void OnKill(int timeLeft)
		{
			// Spawn dust on hit
			for (int i = 0; i <= Main.rand.Next(10, 20); i++)
			{
				Dust.NewDust(Projectile.position, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), DustID.Dirt, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(1, 2));
			}

			// Break sound
			SoundEngine.PlaySound(SoundID.Item70, Projectile.position);

            // Screen shake
            //
            EffectsPlayer effectPlayer = Main.LocalPlayer.GetModPlayer<EffectsPlayer>();
            if (effectPlayer.ScreenShakePower < 2f)
            {
                effectPlayer.ScreenShakePower = .75f;
            }
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.SandyBrown;
		}
	}
}
