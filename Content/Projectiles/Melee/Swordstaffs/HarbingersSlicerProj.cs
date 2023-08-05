using Microsoft.Xna.Framework;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Melee.Swordstaffs
{
    public class HarbingersSlicerProj : SwordstaffProj
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harbingers Slicer");
		}
		public override void SetDefaults()
		{
			Projectile.scale = 1.25f;
			base.SetDefaults();
			SwingCycleTime = 70;
			Projectile.localNPCHitCooldown = 18;
		}

		protected override void ExtraAI(ref float swingCycleTime)
		{
			Vector2 position = Projectile.Center + new Vector2(Projectile.width / 2, -Projectile.height / 2).RotatedBy(Projectile.rotation);

			if (swingCycleTime % SwingCycleTime == (SwingCycleTime / 2))
			{
				if (Projectile.localAI[0] != 0)
				{
					ReleaseProjectile();
				}

				Projectile.localAI[0] = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, Vector2.Zero, ProjectileID.NebulaBlaze1, (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner);
				Main.projectile[(int)Projectile.localAI[0]].tileCollide = false;
				/*
				Main.projectile[(int)Projectile.localAI[0]].hostile = false;
				Main.projectile[(int)Projectile.localAI[0]].friendly = true;*/
			}

			if (Projectile.localAI[0] != 0)
			{
				Main.projectile[(int)Projectile.localAI[0]].position = position;
			}
		}

		private void ReleaseProjectile()
		{
			SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
			Vector2 velocity = Main.MouseWorld - Projectile.Center;
			velocity.Normalize();
			Main.projectile[(int)Projectile.localAI[0]].velocity = velocity * 5;
			Main.projectile[(int)Projectile.localAI[0]].tileCollide = true;
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.localAI[0] != 0)
			{
				ReleaseProjectile();
			}

			return base.PreKill(timeLeft);
		}
	}
}