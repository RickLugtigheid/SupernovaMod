using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.StormSovereign.Projectiles
{
	public class StormBolt : ModProjectile
	{
		private const int LIGTNING_SPEED = 10;
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SharknadoBolt}";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Bolt");
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2DarkMageBolt);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 310;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Thunder, Projectile.position);
			Vector2 velocityBase = new Vector2(0, 500);
			Vector2 velocity = Vector2.Normalize(velocityBase.RotatedByRandom(0.78539818525314331)) * LIGTNING_SPEED;
			int projID = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y - 600, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, 0, ai0: velocityBase.ToRotation(), ai1: Main.rand.Next(100));
			Main.projectile[projID].ai[0] = MathHelper.ToRadians(90);

			Projectile.timeLeft = 40;
			base.Kill(timeLeft);
		}
	}
}
