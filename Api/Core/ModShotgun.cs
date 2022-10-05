using Microsoft.Xna.Framework;
using Supernova.Common;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Api.Core
{
	public abstract class ModShotgun : ModItem
	{
		public abstract float SpreadAngle { get; }
		public abstract int MinShots { get; }
		public abstract int MaxShots { get; }
		public virtual int Shots { get; } // TODO: Make this the ammount of shots. The user can use Main.rand.Next for random ammount of shots

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2[] speeds = Mathf.RandomSpread(velocity, SpreadAngle, 6);
			for (int i = 0; i < Main.rand.Next(MinShots, MaxShots); ++i)
			{
				Projectile.NewProjectile(source, position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
