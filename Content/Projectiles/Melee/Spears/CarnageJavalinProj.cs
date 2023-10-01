using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace SupernovaMod.Content.Projectiles.Melee.Spears
{
	public class CarnageJavalinProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Carnage Javalin");
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			//Projectile.hide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 240;
			AIType = ProjectileID.BoneJavelin;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			// For going through platforms and such, javelins use a tad smaller size
			width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
			return true;
		}

		public override void AI()
		{
			if (Utils.NextBool(Main.rand, 4))
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, default(Color), 1f);
			}
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 0.785f;
			if (base.Projectile.spriteDirection == -1)
			{
				base.Projectile.rotation -= 1.57f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			Vector2 velocity = Vector2.One * 10;
			velocity = velocity.RotatedBy(Projectile.rotation - MathHelper.ToRadians(90));
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, velocity, ProjectileID.NettleBurstRight, (int)(Projectile.damage * .45f), 2, Main.myPlayer, 0f, 0f);
			base.OnKill(timeLeft);
		}
	}
}

