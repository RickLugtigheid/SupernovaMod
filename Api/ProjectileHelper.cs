using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace SupernovaMod.Api
{
	/// <summary>
	/// A helper class for shooting projectiles.
	/// </summary>
	public static class ProjectileHelper
	{
		public static Vector2 CalculateBasicTargetPrediction(Vector2 sourcePosition, Vector2 targetPosition, Vector2 velocity)
		{
            // Basic target prediction
            // https://www.reddit.com/r/Unity3D/comments/do5ymo/comment/f5k2thx/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button
            float dist = Vector2.Distance(sourcePosition, targetPosition);
			Vector2 timeToTarget = velocity / dist;
			return targetPosition + (velocity * timeToTarget);
        }
        /// <summary>
        /// Spawns {<paramref name="projectileAmount"/>} of projectiles in a cross (X) pattern.
        /// </summary>
        /// <returns>Projectile ids</returns>
        public static int[] ShootCrossPattern(IEntitySource spawnSource, Vector2 position, int projectileAmount, float shootSpeed, int type, int damage, float knockback, int owner = -1, float ai0 = 0, float ai1 = 0, float ai2 = 0)
		{
			int[] res = new int[projectileAmount];
			for (int i = 0; i < projectileAmount; i++)
			{
				// Create velocity for angle
				Vector2 value17 = -Vector2
					// Normalize so the velocity ammount of the projectile doesn't matter
					//.Normalize(Projectile.velocity)
					//
					.One
					// Rotate by angle
					.RotatedBy(MathHelper.ToRadians(360 / projectileAmount * (i - 2)))
					// Add the shoot speed to the velocity
					* shootSpeed;
				// Create the projectile
				res[i] = Projectile.NewProjectile(spawnSource, position, value17, type, damage, knockback, owner, ai0, ai1, ai2);
			}
			return res;
		}
		/// <summary>
		/// Spawns {<paramref name="projectileAmount"/>} of projectiles in a plus (+) pattern.
		/// </summary>
		/// <param name="spawnSource"></param>
		/// <param name="position"></param>
		/// <param name="projectileAmount"></param>
		/// <param name="shootSpeed"></param>
		/// <param name="type"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="owner"></param>
		/// <param name="ai0"></param>
		/// <param name="ai1"></param>
		/// <param name="ai2"></param>
		/// <returns></returns>
		public static int[] ShootPlusPattern(IEntitySource spawnSource, Vector2 position, int projectileAmount, float shootSpeed, int type, int damage, float knockback, int owner = -1, float ai0 = 0, float ai1 = 0, float ai2 = 0)
		{
			int[] res = new int[projectileAmount];
			for (int i = 0; i < projectileAmount; i++)
			{
				// Create velocity for angle
				Vector2 value17 = -Vector2
					// Normalize so the velocity ammount of the projectile doesn't matter
					//.Normalize(Projectile.velocity)
					//
					.One
					// Rotate by angle
					.RotatedBy(MathHelper.ToRadians(45 + (360 / projectileAmount * (i - 2))))
					// Add the shoot speed to the velocity
					* shootSpeed;
				// Create the projectile
				res[i] = Projectile.NewProjectile(spawnSource, position, value17, type, damage, knockback, owner, ai0, ai1, ai2);
			}
			return res;
		}
	}
}
