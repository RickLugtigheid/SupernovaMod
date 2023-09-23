using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api.Effects;
using System;
using Terraria;

namespace SupernovaMod.Content.Effects.Particles
{
	public class BloomLight : Particle
	{
		public override string Texture => Supernova.GetEffectPath("Masks/CircleGradient");

		private float opacity;
		private Color bloomColor;
		public float maxTime;


		public BloomLight()
		{
			maxTime = 40;
		}

		public override void Update()
		{
			opacity = (float)Math.Sin(((float)lifeTime / maxTime) * MathHelper.Pi);
			bloomColor = color * opacity;
			Lighting.AddLight(position, color.R / 255f, color.G / 255f, color.B / 255f);
			velocity *= 0.98f;

			if (lifeTime >= maxTime)
			{
				Kill();
			}
		}

		protected override bool PreDraw(SpriteBatch spriteBatch)
		{
			Texture2D bloomtexture = ParticleSystem.GetTexture(type);
			spriteBatch.Draw(bloomtexture, position - Main.screenPosition, null, bloomColor * opacity * 0.5f, 0, bloomtexture.Size() / 2, scale / 2, SpriteEffects.None, 0);
			return false;
		}
	}
}
