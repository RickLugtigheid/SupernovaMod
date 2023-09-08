using Microsoft.Xna.Framework;
using SupernovaMod.Api.Helpers;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SupernovaMod.Api
{
	[Obsolete("Use Supernova gun item instead.")]
	public abstract class ModShotgun : ModItem
	{
		public abstract float SpreadAngle { get; }

		/// <summary>
		/// Gets the amount of shots this mod shotgun should use
		/// </summary>
		/// <returns></returns>
		public abstract int GetShotAmount();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int shootAmount = GetShotAmount();
			Vector2[] speeds = Mathf.RandomSpread(velocity, SpreadAngle, 6);
			for (int i = 0; i < shootAmount; ++i)
			{
				Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
