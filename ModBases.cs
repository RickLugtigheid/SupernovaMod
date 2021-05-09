using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Supernova
{
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
