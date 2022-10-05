using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
	public class TerrorBreath : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.Flames;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 70;
            Projectile.penetrate = -1;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.damage = (int)(Projectile.damage * Main.rand.NextFloat(0.7f, 1.5f));
            target.AddBuff(BuffID.ShadowFlame, 60);   //this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }

		public override void AI()
		{
			Vector2 position43 = new Vector2(Projectile.position.X + Projectile.velocity.X, Projectile.position.Y + Projectile.velocity.Y);
			int width31 = Projectile.width;
			int height31 = Projectile.height;
			float x10 = Projectile.velocity.X;
			float y8 = Projectile.velocity.Y;
			int num2361 = Dust.NewDust(position43, width31, height31, DustID.Torch, x10, y8, 100, default(Color), 3f * Projectile.scale);
			Main.dust[num2361].noGravity = true;

			ref float reference = ref Projectile.ai[0];
			reference += 0.6f;
			if (Projectile.ai[0] > 500f)
			{
				Projectile.Kill();
			}
			int num2475 = 0;
			for (int num1438 = 0; num1438 < 2; num1438 = num2475 + 1)
			{
				if (Main.rand.Next(3) != 0)
				{
					Vector2 position179 = new Vector2(Projectile.position.X, Projectile.position.Y);
					int width139 = Projectile.width;
					int height139 = Projectile.height;
					int dustID = Dust.NewDust(position179, width139, height139, 170, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustID].position = (Main.dust[dustID].position + Projectile.Center) / 2f;
					Main.dust[dustID].noGravity = true;
					Dust dust = Main.dust[dustID];
					dust.velocity *= 0.1f;
					if (num1438 == 1)
					{
						dust = Main.dust[dustID];
						dust.position += Projectile.velocity / 2f;
					}
					float num1436 = (800f - Projectile.ai[0]) / 800f;
					dust = Main.dust[dustID];
					dust.scale *= num1436 + 0.1f;
				}
				num2475 = num1438;
			}
			Projectile.velocity.Y = Projectile.velocity.Y + 0.008f;

			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			int num999;
			for (int num149 = 0; num149 < 20; num149 = num999 + 1)
			{
				int num148 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 100, default(Color), 2f * Projectile.scale);
				Main.dust[num148].noGravity = true;
				Dust dust24 = Main.dust[num148];
				dust24.velocity *= 2f;
				num148 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, (0f - Projectile.velocity.X) * 0.2f, (0f - Projectile.velocity.Y) * 0.2f, 100, default(Color), 1f * Projectile.scale);
				dust24 = Main.dust[num148];
				dust24.velocity *= 2f;
				num999 = num149;
			}
		}
	
	}
}
