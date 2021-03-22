using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Supernova
{
	public abstract class RingBase : ModItem
	{
		/// <summary>
		/// Ring cooldown to aply when ring is activated
		/// </summary>
		public abstract int cooldown { get; }
		/// <summary>
		/// When the ring is activated
		/// </summary>
		/// <param name="player">Player that activated the ring</param>
		public abstract void OnRingActivate(Player player);
		/// <summary>
		/// When the ring is cooling down
		/// </summary>
		/// <param name="curentCooldown">Curent seconds left of the Cooldown</param>
		/// <param name="player">Player that activated the ring</param>
		public virtual void OnRingCooldown(int curentCooldown, Player player)
		{

		}
		public virtual bool RingCanActivate() => true;
	}
	public abstract class Boss : ModNPC
	{
		protected Player targetPlayer;

		public int attackPointer = 0;
		public string[] attacks;

		/// <summary>
		/// Handles attacking
		/// </summary>
		public void Attack()
		{
			// Get our target
			targetPlayer = Main.player[npc.target];

			// Get our method
			MethodInfo attackMethod = this.GetType().GetMethod(attacks[attackPointer]);

			// Invoke our attack
			attackMethod.Invoke(this, null);
		}
		protected float Magnitude(Vector2 mag) => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
	}
}
