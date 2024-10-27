using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;
using SupernovaMod.Common.Players;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

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
            float sin = 1 + (float)Math.Sin(Projectile.timeLeft * 10);
            float cos = 1 + (float)Math.Cos(Projectile.timeLeft * 10);
            Color color = new Color(0.5f + cos * 0.2f, 0.8f, 0.5f + sin * 0.2f);

            for (int i = 0; i < 12; i++)
            {
                Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(4), DustID.Dirt,
                    Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3.5f, 4.5f), 0, color, Main.rand.NextFloat(0.85f, 1.6f)).
                    customData = Main.rand.NextFloat(0.5f, 1f);

                Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(4), DustID.Stone,
                    Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(0.7f, 1.25f), 0, color, 0.5f);
            }

            // Spawn dust on hit
            //for (int i = 0; i <= Main.rand.Next(10, 20); i++)
            //{
            //	Dust.NewDust(Projectile.position, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), DustID.Dirt, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(1, 2));
            //}

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

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			float scaleMulti = 1;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
				scaleMulti -= .05f;
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * scaleMulti, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
