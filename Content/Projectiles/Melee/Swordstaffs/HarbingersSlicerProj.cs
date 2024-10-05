using Microsoft.Xna.Framework;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee.Swordstaffs
{
    public class HarbingersSlicerProj : SwordstaffProj
    {
		public override void SetDefaults()
		{
			base.SetDefaults();

			Projectile.scale = 1.24f;
			SwingCycleTime = 74;
			Projectile.localNPCHitCooldown = 16;
		}

		protected override void ExtraAI(ref float swingCycleTime)
		{
			Vector2 position = Projectile.Center + new Vector2(Projectile.width * .7f, -Projectile.height / 2).RotatedBy(Projectile.rotation);

			if (swingCycleTime % SwingCycleTime == (SwingCycleTime / 2))
			{
				Projectile.localAI[1]--;

				if (Projectile.localAI[0] == 0 && Projectile.localAI[1] < 1)
				{
					Projectile.localAI[0] = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<NebualShot>(), (int)(Projectile.damage * .7f), Projectile.knockBack, Projectile.owner);
					Main.projectile[(int)Projectile.localAI[0]].tileCollide = false;
					Projectile.localAI[1] = 2;
				}
			}


			if (Projectile.localAI[0] != 0)
			{
				if (!Main.projectile[(int)Projectile.localAI[0]].active)
				{
					Projectile.localAI[0] = 0;
					return;
				}
				Main.projectile[(int)Projectile.localAI[0]].position = position;
			}
		}

		private void ReleaseProjectile()
		{
			Projectile proj = Main.projectile[(int)Projectile.localAI[0]];
			SoundEngine.PlaySound(SoundID.Item20, proj.Center);
			Vector2 velocity = Main.MouseWorld - proj.Center;
			velocity.Normalize();
			proj.velocity = velocity * 7;
			proj.tileCollide = true;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle(hitbox.X, hitbox.Y, (int)(hitbox.Width * Projectile.scale), (int)(hitbox.Height * Projectile.scale));
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.localAI[0] != 0)
			{
				ReleaseProjectile();
			}

			return base.PreKill(timeLeft);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.Zero, ProjectileID.NebulaArcanumSubshot, 0, 0, Projectile.owner);
		}
	}
}