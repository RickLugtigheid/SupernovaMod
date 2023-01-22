using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Api
{
	public abstract class ModBossNpc : ModNPC
	{
		public Player targetPlayer;

		public int attackPointer = 0;
		public string[] attacks;

		/// <summary>
		/// Handles attacking
		/// </summary>
		public void Attack()
		{
			// Get our target
			targetPlayer = Main.player[NPC.target];

			// Reset our attack pointer when pointed to far
			//
			if (attackPointer > attacks.Length)
			{
				attackPointer = 0;
			}

			// Get our method
			MethodInfo attackMethod = GetType().GetMethod(attacks[attackPointer]);

			// Invoke our attack
			attackMethod.Invoke(this, null);
		}
		protected float Magnitude(Vector2 mag) => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
	}
}
