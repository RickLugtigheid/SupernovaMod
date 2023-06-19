using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Api.Drawing;
using System.Collections.Generic;
using Terraria.Audio;

namespace SupernovaMod.Content.Npcs.StormSovereign.Projectiles
{
    public class ChainLightningHostile : ModProjectile
    {
		private const int MAX_RANGE = 400;
		private const int CHAIN_HIT_COOLDOWN = 6;
		public override string Texture => Supernova.GetTexturePath("InvisibleProjectile");
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.ArmorPenetration = 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;  //The amount of time the projectile is alive for
		}

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.ModProjectile != null)
                return projectile.ModProjectile.OnTileCollide(oldVelocity);
            return true;
        }

		private ModProjectile _nextTarget = null;
		private Vector2 _prevPosition = Vector2.Zero;
		public Vector2 startPosition = Vector2.Zero;

        public override void AI()
        {
			if (_nextTarget != null)
			{
				Projectile.ai[0]++;
				if (Projectile.ai[0] >= CHAIN_HIT_COOLDOWN)
				{
					Projectile.ai[0] = 0;
					Projectile.timeLeft = CHAIN_HIT_COOLDOWN;
					OnHitTarget(_nextTarget);
					DrawDust.Electricity(_prevPosition, Projectile.Center, DustID.Electric, 1.2f, 80);
				}
			}

			Projectile.velocity = Vector2.Zero;
			//Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors

			if (startPosition != Vector2.Zero && _nextTarget == null)
            {
				_prevPosition = startPosition;
				Projectile.Center = startPosition;
				SearchForTargets(out bool foundTarget, out float distFromTarget, out Vector2 targetCenter, out ModProjectile newTarget);

                if (foundTarget)
                {
					Projectile.Center = targetCenter;
					Projectile.timeLeft = CHAIN_HIT_COOLDOWN * 2;
					OnHitTarget(newTarget);
				}
			}
		}

		private List<ModProjectile> _hitProjectiles = new List<ModProjectile>();
		public void OnHitTarget(ModProjectile target)
		{
			if (target == null) return;

			Projectile.position = target.Projectile.Center;

			_hitProjectiles.Add(target);
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.position);

			// Spawn dust
			//
			for (int x = 0; x < 3; x++)
			{
				int dust = Dust.NewDust(target.Projectile.Center, 25, 25, DustID.Electric, Main.rand.Next(-2, 2), -Main.rand.Next(1, 4), 0, default, Main.rand.NextFloat(.75f, 1));
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= Main.rand.NextFloat(1, 1.2f);
			}

			// Kill the projectile
			target.Projectile.Kill();

			SearchForTargets(out bool foundTarget, out float distFromTarget, out Vector2 targetCenter, out ModProjectile newTarget);

			if (foundTarget)
			{
				_prevPosition = _nextTarget?.Projectile?.position ?? startPosition;
				_nextTarget = newTarget;
			}
			else
			{
				Projectile.Kill();
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (_prevPosition == Vector2.Zero)
			{
				return base.Colliding(projHitbox, targetHitbox);
			}

			Vector2 lineStart = Projectile.Center;
			float collisionpoint = 0f;
			Vector2 lineEnd = _prevPosition;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, Projectile.scale / 2, ref collisionpoint))
			{
				return true;
			}
			return false;
		}

		protected virtual void SearchForTargets(out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out ModProjectile target)
		{
			// Starting search distance
			distanceFromTarget = 300;
			targetCenter = Projectile.position;
			foundTarget = false;
			target = null;

			// Find a target
			//
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (proj == Projectile || _hitProjectiles.Contains(proj.ModProjectile))
				{
					continue;
				}

				/*if (!_hitNPCs.Contains(npc) && npc.CanBeChasedBy())*/
				{
					float between = Vector2.Distance(proj.Center, Projectile.Center);
					bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
					bool inRange = between < distanceFromTarget;
					if (between > MAX_RANGE)
					{
						inRange = false;
					}
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, proj.position, proj.width, proj.height);
					// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
					// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
					bool closeThroughWall = between < 100f;

					if ((closest && inRange || !foundTarget) && (lineOfSight || closeThroughWall))
					{
						distanceFromTarget = between;
						targetCenter = proj.Center;
						foundTarget = true;
						target = proj.ModProjectile;
					}
				}
			}
		}
	}
}
