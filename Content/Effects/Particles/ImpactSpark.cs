using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api.Effects;
using System;
using Terraria;

namespace SupernovaMod.Content.Effects.Particles
{
	public class ImpactSpark : Particle
	{
		private Vector2 _scaleMod;
		private int _timeLeft;

		//		public ImpactLine(Vector2 position, Vector2 velocity, Color color, Vector2 scale, int timeLeft, Entity attatchedEntity = null)

		public ImpactSpark(Vector2 position, Vector2 velocity, Color color, Vector2 scale, int timeLeft, Entity attatchedEntity = null)
		{
			this.position = position;
			this.velocity = velocity;
			this.color = color;
			_scaleMod = scale;
			_timeLeft = timeLeft;
		}

		public override void Update()
		{
			float opacity = (float)Math.Sin((lifeTime / (float)_timeLeft) * MathHelper.Pi);
			color = color * opacity;
			rotation = velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(position, color.ToVector3() / 2f);
			/*if (_ent != null)
			{
				if (!_ent.active)
				{
					Kill();
					return;
				}
				Position = _ent.Center + _offset;
				_offset += Velocity;
			}*/

			if (lifeTime > _timeLeft)
			{
				Kill();
			}
		}

		protected override bool PreDraw(SpriteBatch spriteBatch)
		{
			float progress = (float)Math.Sin((lifeTime / (float)_timeLeft) * MathHelper.Pi);
			Vector2 scale = new Vector2(0.5f, progress) * _scaleMod;
			Vector2 offset = Vector2.Zero;
			Texture2D tex = ParticleSystem.GetTexture(type);
			Vector2 origin = new Vector2(tex.Width / 2, tex.Height);

			if (lifeTime > _timeLeft / 2)
			{
				offset = Vector2.UnitX.RotatedBy(rotation - MathHelper.PiOver2) * tex.Height * scale.Y;
				origin.Y = 0;
			}

			spriteBatch.Draw(tex, position + offset - Main.screenPosition, null, color * ((progress / 5) + 0.8f), rotation, origin, scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
