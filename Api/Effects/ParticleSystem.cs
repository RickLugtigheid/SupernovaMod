using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Api.Effects
{
	public static class ParticleSystem
	{
		internal static int maxParticles = ModContent.GetInstance<Common.Configs.CommonConfig>().maxParticles;

		public static Particle[] particles;

		private static Dictionary<Type, int> _particleTypeMap;
		private static Dictionary<int, Texture2D> _particleTextureMap;
		private static List<Particle> _particleInstances;

		private static int _particlePointer = 0;
		private static int _particlesActive;

		/// <summary>
		/// Loads all resources for the <see cref="ParticleSystem"/>.
		/// </summary>
		internal static void Load()
		{
			particles = new Particle[maxParticles];
			_particleTypeMap = new Dictionary<Type, int>();
			_particleTextureMap = new Dictionary<int, Texture2D>();
			_particleInstances = new List<Particle>();

			Assembly supernovaAssembly = ModContent.GetInstance<Supernova>().Code;
			Type particleType = typeof(Particle);

			foreach (Type type in supernovaAssembly.GetTypes())
			{
				//
				if (!type.IsSubclassOf(particleType) || type.IsAbstract || type == particleType)
				{
					continue;
				}

				Particle instance = (Particle)FormatterServices.GetUninitializedObject(type);

				// Map our type to the next index
				int typeIndex = _particleTypeMap.Count;
				_particleTypeMap[type] = typeIndex;

				// Load & map our texture
				//
				string texturePath = SupernovaUtils.GetNamespacePath(type);
				if (!string.IsNullOrEmpty(instance.Texture))
				{
					texturePath = instance.Texture;
				}
				_particleTextureMap[typeIndex] = ModContent.Request<Texture2D>(texturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				// Instantiate our particle
				_particleInstances.Add((Particle)FormatterServices.GetUninitializedObject(type));
			}
		}

		/// <summary>
		/// Unloads all resources for the <see cref="ParticleSystem"/>.
		/// </summary>
		internal static void Unload()
		{
			particles = null;
			_particleTypeMap = null;
			_particleTextureMap = null;
			_particleInstances = null;
		}

		public static Texture2D GetTexture(int type) => _particleTextureMap[type];
		public static int ParticleType<T>() => _particleTypeMap[typeof(T)];

		public static int SpawnParticle(Particle particle)
		{
			if (Main.gamePaused || Main.dedServ)
			{
				return -1;
			}
			// Check if we are already at max particles
			//
			if (_particlesActive >= maxParticles)
			{
                return -1;
			}

			//
			particle.whoAmI = _particlePointer;
			particle.type = _particleTypeMap[particle.GetType()];
			particles[_particlePointer] = particle;

			// Get the next pointer position
			//
			_particlePointer++;
			if (_particlePointer >= particles.Length || particles[_particlePointer] == null)
			{
				_particlePointer--;

				// Get a particle index not already in use
				//
				for (int i = 0; i < particles.Length; i++)
				{
					if (particles[i] == null)
					{
						_particlePointer = i;
						break;
					}
				}
			}

			// A new particle has become active
			_particlesActive++;

			//Main.NewText($"SpawnParticle(at: {particle.position}, origin: {particle.origin}, scale: {particle.scale}, type: {particle.type}, whoAmI: {particle.whoAmI})");

			// Return our id so the caller can get the particle object
			return particle.whoAmI;
		}

		public static void KillParticle(int index)
		{
			particles[index] = null;
			_particlesActive--;
			_particlePointer = index;
		}

		#region Update methods

		internal static void UpdateParticles()
		{
			foreach (Particle particle in particles)
			{
				// Ignore null values
				//
				if (particle == null)
				{
					continue;
				}

				particle.PreUpdate();

				particle.lifeTime++;
				particle.position += particle.velocity;

				particle.Update();
			}
		}

		internal static void DrawParticles(SpriteBatch spriteBatch)
		{
			spriteBatch.End();

			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, null, null, Main.GameViewMatrix.ZoomMatrix);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			foreach (Particle particle in particles)
			{
				// Ignore null values
				//
				if (particle == null)
				{
					continue;
				}

				// Draw the particle
				particle.Draw(spriteBatch);
			}

			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		}

		#endregion
	}
}
