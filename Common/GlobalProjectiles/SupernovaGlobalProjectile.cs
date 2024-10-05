using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.GlobalProjectiles
{
	public class SupernovaGlobalProjectile : GlobalProjectile
	{
		/// <summary>
		/// A list of demonic minion projectiles.
		/// </summary>
		public static List<int> DemonicMinions { get; } = new List<int>();

		public override bool InstancePerEntity => true;

		/// <summary>
		/// Returns the <see cref="Player"/> owner of this projectile or null when not owned by player.
		/// </summary>
		public Player Owner { get; private set; } = null;
		/// <summary>
		/// Returns true when this projectile is owned by a <see cref="Player"/>.
		/// </summary>
		public bool OwnedByPlayer => Owner != null;

		public override void SetStaticDefaults()
		{
			DemonicMinions.AddRange(new int[]
			{
				ProjectileID.FlyingImp,
				ProjectileID.BatOfLight
			});
		}

		public override void SetDefaults(Projectile projectile)
		{

		}
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			base.SetDefaults(projectile);

			//
			if (projectile.owner > 0 || projectile.owner < Main.player.Length)
			{
				Owner = Main.player[projectile.owner];
			}
			if (OwnedByPlayer)
			{
				SetDefaultsForPlayerOwnedProjectile(projectile);
			}
		}
		private void SetDefaultsForPlayerOwnedProjectile(Projectile projectile)
		{
			AccessoryPlayer accessoryPlayer = Owner.GetModPlayer<AccessoryPlayer>();
			if (projectile.minion)
			{
				if (accessoryPlayer.hasDemonomicon && IsDemonicMinion(projectile))
				{
					projectile.damage = (int)(projectile.damage * 1.1f);
				}
			}
		}

		public override void PostAI(Projectile projectile)
		{
			if (OwnedByPlayer)
			{
				AccessoryPlayer accessoryPlayer = Owner.GetModPlayer<AccessoryPlayer>();

				if (projectile.minion)
				{
					if (accessoryPlayer.hasDemonomicon && IsDemonicMinion(projectile))
					{
						//for (int num925 = 0; num925 < 4; num925++)
						int num915 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width + 4, projectile.height + 4, DustID.DemonTorch, 0f, 0f, 100, default, 1.5f);
						Main.dust[num915].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
						Main.dust[num915].noGravity = true;
						if (Main.rand.NextBool(2))
						{
							int num916 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width + 4, projectile.height + 4, DustID.FlameBurst, 0f, 0f, 100, default, 1);
							Main.dust[num916].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
							Main.dust[num916].noGravity = true;
						}
					}
				}
			}
		}
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (OwnedByPlayer)
			{
				AccessoryPlayer accessoryPlayer = Owner.GetModPlayer<AccessoryPlayer>();
				accessoryPlayer.OnProjectileHitNPC(projectile, target, hit, damageDone);
			}
		}

		#region Helper methods

		public bool IsDemonicMinion(Projectile projectile)
		{
			return DemonicMinions.Contains(projectile.type);
		}

		#endregion
	}
}
