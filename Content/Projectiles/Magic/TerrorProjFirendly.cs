using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class TerrorProjFirendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Blast");
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
			Projectile.timeLeft = 720;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		private float _speed = 8.5f;
		public override void AI()
        {
			Projectile.ai[0] += 1f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 3f)
			{
				//Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.TerrorDust>(), new Vector2?(new Vector2(0f, 0f)), 0, default(Color), 1f).noGravity = true;
				Projectile.ai[1] = 0f;
				int dust = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, ModContent.DustType<Dusts.TerrorDust>(), Projectile.velocity.X * 0.45f, Projectile.velocity.Y * 0.45f, 80, default);
				Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Shadowflame, Projectile.velocity.X * 0.45f, Projectile.velocity.Y * 0.45f, 80, default);
				Main.dust[dust].noGravity = true; //this make so the dust has no gravity
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[0] >= 10f)
			{
				float d = 128f;
				bool targetfound = false;
				Vector2 targetcenter = Projectile.position;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(null, false))
					{
						float dpt = Vector2.Distance(Projectile.Center, npc.Center);
						if ((dpt < d && !targetfound) || dpt < d)
						{
							d = dpt;
							targetfound = true;
							targetcenter = npc.Center;
						}
					}
				}
				if (targetfound)
				{
					Projectile.velocity = Vector2.Normalize(targetcenter - Projectile.Center) + Projectile.oldVelocity * 0.87f;
					float num = Math.Abs(Projectile.velocity.X);
					float vely = Math.Abs(Projectile.velocity.Y);
					if (num > _speed)
					{
						float direction = Math.Abs(Projectile.velocity.X) / Projectile.velocity.X;
						Projectile.velocity.X = _speed * direction;
					}
					if (vely > _speed)
					{
						float direction2 = Math.Abs(Projectile.velocity.Y) / Projectile.velocity.Y;
						Projectile.velocity.Y = _speed * direction2;
						return;
					}
				}
				else
				{
					Projectile.velocity = new Vector2(_speed, 0f).RotatedBy((double)Projectile.rotation, default(Vector2));
				}
			}
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
			Vector2 origin = source.Size() / 2f;
			Vector2 value = new Vector2((float)Projectile.width, (float)Projectile.height) / 2f;
			lightColor.A = 205;
			float a = ((float)Math.Sin((double)(Projectile.ai[0] / 15f)) + 1.15f) / 4f;
			Main.spriteBatch.Draw(texture, Projectile.position + value - Main.screenPosition, new Rectangle?(source), lightColor * a, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
		}

		public override bool? CanCutTiles() => false;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 0f;
			target.immune[Projectile.owner] = 10;
			if (target.life <= 0)
			{
				Projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 4; i++)
			{
				Vector2 speed = new Vector2(0f, -3f).RotatedBy((double)(1.5707964f * (float)i), default(Vector2));
				Dust.NewDustPerfect(Projectile.position, ModContent.DustType<Dusts.TerrorDust>(), new Vector2?(speed), 0, default(Color), 1.4f).noGravity = true;
			}
		}
    }
}

