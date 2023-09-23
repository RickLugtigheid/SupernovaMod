using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api.Effects;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace SupernovaMod.Content.Effects.Particles
{
	public class StarParticle : Particle
	{
		private Color starColor;
		private Color bloomColor;
		private float opacity;
		public int maxTime;
		private float rotationSpeed;

		public StarParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime, float rotationSpeed = 1f)
		{
			this.position = position;
			this.velocity = velocity;
			starColor = color;
			bloomColor = color;
			rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			this.scale = scale;
			this.maxTime = maxTime;
			this.rotationSpeed = rotationSpeed;
		}

		public StarParticle(Vector2 position, Vector2 velocity, Color starColor, Color bloomColor, float scale, int maxTime, float rotationSpeed = 1f)
		{
			this.position = position;
			this.velocity = velocity;
			this.starColor = starColor;
			this.bloomColor = bloomColor;
			rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			this.scale = scale;
			this.maxTime = maxTime;
			this.rotationSpeed = rotationSpeed;
		}

		public override void Update()
		{
			opacity = (float)Math.Sin(((float)lifeTime / maxTime) * MathHelper.Pi);
			color = bloomColor * opacity;
			Lighting.AddLight(position, color.R / 255f, color.G / 255f, color.B / 255f);
			velocity *= 0.98f;
			rotation += rotationSpeed * ((velocity.X > 0) ? 0.07f : -0.07f);

			if (lifeTime >= maxTime)
			{
				Kill();
			}
		}

		protected override bool PreDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleSystem.GetTexture(type);
			Texture2D bloomtexture = ModContent.Request<Texture2D>(Supernova.GetEffectPath("Masks/CircleGradient"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			spriteBatch.Draw(bloomtexture, position - Main.screenPosition, null, bloomColor * opacity * 0.5f, 0, bloomtexture.Size() / 2, scale / 2, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, position - Main.screenPosition, null, starColor * opacity * 0.5f, rotation * 1.5f, basetexture.Size() / 2, scale * 0.75f, SpriteEffects.None, 0);
			spriteBatch.Draw(basetexture, position - Main.screenPosition, null, starColor * opacity * 0.5f, -rotation * 1.5f, basetexture.Size() / 2, scale * 0.75f, SpriteEffects.None, 0);

			spriteBatch.Draw(basetexture, position - Main.screenPosition, null, starColor * opacity, rotation, basetexture.Size() / 2, scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
