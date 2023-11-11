using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SupernovaMod.Api.Effects
{
	public class Particle
	{
		public virtual string Texture { get; } = null;

		public int whoAmI;
		public int type;

		public int lifeTime;

		public Vector2 position;
		public Vector2 velocity;
		public Vector2 origin;

		public Color color;
		public int alpha = 0;
		public float rotation;
		public float scale = 1;

		public void Kill() => ParticleSystem.KillParticle(whoAmI);

		public virtual void PreUpdate()
		{

		}
		public virtual void Update()
		{

		}

		protected virtual bool PreDraw(SpriteBatch spriteBatch) => true;
		internal void Draw(SpriteBatch spriteBatch)
		{
			if (PreDraw(spriteBatch))
			{
				Color drawColor = color;
				drawColor.A = (byte)alpha;
				spriteBatch.Draw(ParticleSystem.GetTexture(type), position - Main.screenPosition, null, drawColor, rotation, origin, scale * Main.GameViewMatrix.Zoom, SpriteEffects.None, 0f);
			}
		}

		#region Static Methods
		public static int NewParticle<T>(Vector2 position, Vector2 velocity, Vector2 origin = default, float rotation = 0f, float scale = 1f, Color color = default) where T : Particle, new()
		{
			T particle = new T();
			particle.position = position;
			particle.velocity = velocity;
			particle.color = color;
			particle.origin = origin;
			particle.rotation = rotation;
			particle.scale = scale;
			particle.type = ParticleSystem.ParticleType<T>();

			return ParticleSystem.SpawnParticle(particle);
		}
		public static int NewParticle(int type, Vector2 position, Vector2 velocity, Vector2 origin = default, float rotation = 0f, float scale = 1f)
		{
			Particle particle = new Particle();
			particle.position = position;
			particle.velocity = velocity;
			particle.color = Color.White;
			particle.origin = origin;
			particle.rotation = rotation;
			particle.scale = scale;
			particle.type = type;

			return ParticleSystem.SpawnParticle(particle);
		}
		public static int NewParticle(int type, Vector2 position, Vector2 velocity)
		{
			Particle particle = new Particle();
			particle.position = position;
			particle.velocity = velocity;
			particle.color = Color.White;
			particle.origin = Vector2.Zero;
			particle.rotation = 0f;
			particle.scale = 1f;
			particle.type = type;

			return ParticleSystem.SpawnParticle(particle);
		}
		#endregion
	}
}
