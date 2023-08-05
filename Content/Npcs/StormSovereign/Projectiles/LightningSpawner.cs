using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.StormSovereign.Projectiles
{
	public class LightningSpawner : ModProjectile
	{
		private const int LIGTNING_SPEED = 10;
		public override string Texture => Supernova.GetTexturePath("InvisibleProjectile");
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Storm Bolt");
		}
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 0f)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					SoundEngine.PlaySound(SoundID.Thunder, Projectile.position);
					/*Vector2 velocityBase = new Vector2(0, 500);
					Vector2 velocity = Vector2.Normalize(velocityBase.RotatedByRandom(0.78539818525314331)) * LIGTNING_SPEED;
					int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(null), Projectile.Center.X, Projectile.Center.Y, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, 4, ai0: velocityBase.ToRotation(), ai1: Main.rand.Next(100));
					Main.projectile[projID].ai[0] = MathHelper.ToRadians(90);*/

					int type = ModContent.ProjectileType<Projectiles.StormCloud>();
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, 30, 1, Main.myPlayer, 4, LIGTNING_SPEED);

					//Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), base.Projectile.Center, Vector2.UnitY * 15f, ModContent.ProjectileType<LichFlareSpawn3>(), 0, 0f, base.Projectile.owner, 0f, 0f);
				}
				Projectile.ai[1] = -35f;
			}
		}
	}
}
